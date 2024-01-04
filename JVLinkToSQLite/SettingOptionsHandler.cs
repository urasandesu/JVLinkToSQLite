// JVLinkToSQLite は、JRA-VAN データラボが提供する競馬データを SQLite データベースに変換するツールです。
// 
// Copyright (C) 2023 Akira Sugiura
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// 
// Additional permission under GNU GPL version 3 section 7
// 
// If you modify this Program, or any covered work, by linking or combining it with 
// ObscUra (or a modified version of that library), containing parts covered 
// by the terms of ObscUra's license, the licensors of this Program grant you 
// additional permission to convey the resulting work.

using CommandLine;
using DryIoc;
using System;
using System.Diagnostics;
using System.Xml;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.Settings;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace JVLinkToSQLite
{
    internal class SettingOptionsHandler : OptionsHandler<SettingOptions>
    {
        private readonly ParserResult<object> _parserResult;

        public SettingOptionsHandler(ParserResult<object> parserResult)
        {
            _parserResult = parserResult;
        }

        protected override int ProcessByInvocableOption(IJVServiceOperationListener listener, SettingOptions options)
        {
            Verbose(listener, this, MessageForArgument, nameof(SettingOptions.Setting), options.Setting);
            Verbose(listener, this, MessageForArgument, nameof(SettingOptions.XPath), options.XPath);
            Verbose(listener, this, MessageForArgument, nameof(SettingOptions.Value), options.Value);
            Verbose(listener, this, MessageForArgument, nameof(SettingOptions.LogLevel), options.LogLevel);
            Verbose(listener, this, MessageForArgument, nameof(SettingOptions.Force), options.Force);

            using (var container = JVLinkToSQLiteDependency.NewDefaultContainer(listener))
            using (var childContainer = container.CreateChild())
            {
                return UpdateSetting(childContainer, options);
            }
        }

        private int UpdateSetting(IResolver resolver, SettingOptions options)
        {
            var listener = resolver.Resolve<IJVServiceOperationListener>();
            InfoStart(listener, this, args => "JVLinkToSQLite 動作設定更新");
            var stopwatch = Stopwatch.StartNew();

            var retCode = UpdateSettingCore(resolver, listener, options);

            InfoEnd(listener, this, args => $"JVLinkToSQLite 動作設定更新 {args[0]}s", stopwatch.Elapsed.TotalSeconds);
            return retCode;
        }

        private int UpdateSettingCore(IResolver resolver, IJVServiceOperationListener listener, SettingOptions options)
        {
            var xss = resolver.Resolve<IXmlSerializationService>();

            var doc = new XmlDocument();
            using (var sr = xss.NewDeserializingTextReader(options.Setting))
            {
                doc.Load(sr);
            }

            var nodes = doc.SelectNodes(options.XPath);
            if (nodes == null || nodes.Count == 0)
            {
                Warning(listener, 
                        this, 
                        args => $"指定された XPath に該当するノードが見つかりませんでした。XPath='{args[0]}'", 
                        options.XPath);
                return (int)ReturnCodes.Warning;
            }

            Console.WriteLine($"指定された値 '{options.Value}' で、以下のノードを更新します。続行しますか？（y/N）");
            foreach (XmlNode node in nodes)
            {
                Console.WriteLine(JVDataSpecSetting.GetXPath(node));
            }

            if (!options.Force)
            {
                var line = Console.ReadLine();
                if (line != "y")
                {
                    Warning(listener,
                            this,
                            args => $"動作設定の更新を中断しました。");
                    return (int)ReturnCodes.Warning;
                }
            }

            foreach (XmlNode node in nodes)
            {
                node.InnerText = options.Value;
            }

            var tempFileName = xss.GetTempFileName();
            using (var sw = xss.NewSerializingTextWriter(tempFileName))
            {
                doc.Save(sw);
            }

            try
            {
                using (var sr = xss.NewDeserializingTextReader(tempFileName))
                {
                    xss.Deserialize<JVLinkToSQLiteSetting>(sr);
                }
            }
            catch (Exception e)
            {
                Error(listener, 
                      this, 
                      args => $"動作設定として無効なファイルが生成されたため、処理を中断しました。ファイルの内容と動作設定の仕様を見比べ、見直しをお願いします。ファイル='{args[0]}', 例外='{args[1]}'", 
                      tempFileName, e);
                return (int)ReturnCodes.Error;
            }

            xss.Copy(tempFileName, options.Setting, overwrite: true);

            Info(listener, 
                 this, 
                 args => $"動作設定を更新しました。");
            return (int)ReturnCodes.Success;
        }
    }
}
