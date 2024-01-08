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
using CommandLine.Text;
using DryIoc;
using JVLinkToSQLite.Mixins.CommandLine;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace JVLinkToSQLite
{
    internal class MainOptionsHandler : OptionsHandler<MainOptions>
    {
        private readonly ParserResult<object> _parserResult;

        public MainOptionsHandler(ParserResult<object> parserResult)
        {
            _parserResult = parserResult;
        }

        private static string MessageForWarningCanContinue(object[] args) =>
            $"エラーが発生しましたが、可能な限り続行します。最初のエラー (引数) [場所]：{args[0]} ({StringMixin.JoinIfAvailable(", ", args[1])}) [{args[2]}#L{args[3]}]";

        protected override bool TryProcessByUninvocableOption(MainOptions options, out int returnCode)
        {
            if (options.Mode == Modes.None)
            {
                var helpText = HelpText.AutoBuild(_parserResult, _ => _, _ => _, maxDisplayWidth: ParserSettingsMixin.MaximumDisplayWidth);
                Console.WriteLine(helpText);
                returnCode = (int)ReturnCodes.OptionNotParsed;
                return true;
            }
            else if (options.Mode == Modes.About)
            {
                returnCode = ShowNotice();
                return true;
            }
            returnCode = (int)ReturnCodes.Success;
            return false;
        }

        protected override int ProcessByInvocableOption(IJVServiceOperationListener listener, MainOptions options)
        {
            Verbose(listener, this, MessageForArgument, nameof(MainOptions.Mode), options.Mode);
            Verbose(listener, this, MessageForArgument, nameof(MainOptions.DataSource), options.DataSource);
            Verbose(listener, this, MessageForArgument, nameof(MainOptions.ThrottleSize), options.ThrottleSize);
            Verbose(listener, this, MessageForArgument, nameof(MainOptions.Setting), options.Setting);
            Verbose(listener, this, MessageForArgument, nameof(MainOptions.LogLevel), options.LogLevel);
            Verbose(listener, this, MessageForArgument, nameof(MainOptions.SkipsLastModifiedUpdate), options.SkipsLastModifiedUpdate);

            using (var container = JVLinkToSQLiteDependency.NewDefaultContainer(listener))
            using (var childContainer = container.CreateChild())
            {
                switch (options.Mode)
                {
                    case Modes.Init:
                        childContainer.Register<SetupForm>(setup: Setup.With(allowDisposableTransient: true));
                        return Initialize(childContainer);
                    case Modes.DefaultSetting:
                        return CreateDefaultSetting(childContainer, options);
                    case Modes.Event:
                        return WatchEvent(childContainer, options);
                    case Modes.Exec:
                        return Execute(childContainer, options);
                    default:
                        throw new NotImplementedException($"モード '{options.Mode}' は未実装です。");
                }
            }
        }

        private static int ShowNotice()
        {
            Console.WriteLine($@"
JVLinkToSQLite は、JRA-VAN データラボが提供する競馬データを SQLite データベースに変換するツールです。

バージョン {Assembly.GetEntryAssembly().GetName().Version}, Copyright (C) 2023 Akira Sugiura

このプログラムはフリーソフトウェアです。あなたはこれを、フリーソフトウェア財団によって発行された GNU 一般公衆利用許諾契約書（バージョン 3 か、希望によってはそれ以降のバージョンのうちどれか）の定める条件の下で再頒布または改変することができます。

このプログラムは有用であることを願って頒布されますが、**全くの無保証** です。商業可能性の保証や特定の目的への適合性は、言外に示されたものも含め全く存在しません。詳しくは <https://github.com/urasandesu/JVLinkToSQLite/blob/main/LICENSE> をご覧ください。
 
あなたはこのプログラムと共に、GNU 一般公衆利用許諾契約書の複製物を一部受け取ったはずです。もし受け取っていなければ、<https://www.gnu.org/licenses/> をご確認ください。
");
            return (int)ReturnCodes.Success;
        }

        private static int Initialize(IResolver resolver)
        {
            var listener = resolver.Resolve<IJVServiceOperationListener>();
            InfoStart(listener, typeof(MainOptionsHandler), args => "JVLinkToSQLite 初期化モード");
            var stopwatch = Stopwatch.StartNew();

            var retCode = InitializeCore(resolver, listener);

            InfoEnd(listener, typeof(MainOptionsHandler), args => $"JVLinkToSQLite 初期化モード {args[0]}s", stopwatch.Elapsed.TotalSeconds);
            return retCode;
        }

        private static int InitializeCore(IResolver resolver, IJVServiceOperationListener listener)
        {
            var retCode = ReturnCodes.Success;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (sender, e) =>
            {
                Error(listener, typeof(MainOptionsHandler), MessageForException, e.Exception);
                retCode = ReturnCodes.Error;
            };
            using (var setupForm = resolver.Resolve<SetupForm>())
            {
                Application.Run(setupForm);
            }
            return (int)retCode;
        }

        private static int CreateDefaultSetting(IResolver resolver, MainOptions options)
        {
            var listener = resolver.Resolve<IJVServiceOperationListener>();
            InfoStart(listener, typeof(MainOptionsHandler), args => "JVLinkToSQLite デフォルト設定生成モード");
            var stopwatch = Stopwatch.StartNew();

            var retCode = CreateDefaultSettingCore(resolver, options);

            InfoEnd(listener, typeof(MainOptionsHandler), args => $"JVLinkToSQLite デフォルト設定生成モード {args[0]}s", stopwatch.Elapsed.TotalSeconds);
            return retCode;
        }

        private static int CreateDefaultSettingCore(IResolver resolver, MainOptions options)
        {
            var bootstrap = resolver.Resolve<JVLinkToSQLiteBootstrap>();
            var xss = resolver.Resolve<IXmlSerializationService>();
            if (xss.ExistsXmlFile(options.Setting))
            {
                Console.WriteLine($"指定された XML ファイル '{options.Setting}' は既に存在します。上書きしますか？（y/N）");
                var line = Console.ReadLine();
                if (line != "y")
                {
                    return (int)ReturnCodes.Warning;
                }
                else
                {
                    xss.DeleteXmlFile(options.Setting);
                }
            }
            var setting = bootstrap.LoadSettingOrDefault(options.ToLoadSettingParameter());
            bootstrap.SaveSetting(options.Setting, setting);
            return (int)ReturnCodes.Success;
        }

        private static int WatchEvent(IResolver resolver, MainOptions options)
        {
            var listener = resolver.Resolve<IJVServiceOperationListener>();
            InfoStart(listener, typeof(MainOptionsHandler), args => "JVLinkToSQLite イベント待ちモード");
            var stopwatch = Stopwatch.StartNew();

            var retCode = WatchEventCore(resolver, listener, options);

            InfoEnd(listener, typeof(MainOptionsHandler), args => $"JVLinkToSQLite イベント待ちモード {args[0]}s", stopwatch.Elapsed.TotalSeconds);
            return retCode;
        }

        private static int WatchEventCore(IResolver resolver, IJVServiceOperationListener listener, MainOptions options)
        {
            var retCode = (int)ReturnCodes.Success;
            var bootstrap = resolver.Resolve<JVLinkToSQLiteBootstrap>();
            var setting = bootstrap.LoadSettingOrDefault(options.ToLoadSettingParameter());
            foreach (var settingDetail in setting.Details)
            {
                var oprAgg = settingDetail.NewOperatorAggregate(resolver, isImmediate: false);
                var rsltAgg = oprAgg.OperateAll();
                if (!rsltAgg.AreAllSucceeded)
                {
                    retCode = rsltAgg.FirstReturnCode;
                    Warning(listener,
                            typeof(MainOptionsHandler),
                            MessageForWarningCanContinue,
                            rsltAgg.FirstDebugMessage,
                            rsltAgg.FirstArguments,
                            rsltAgg.FirstCallerFilePath,
                            rsltAgg.FirstCallerLineNumber);
                }
            }

            return retCode;
        }

        private static int Execute(IResolver resolver, MainOptions options)
        {
            var listener = resolver.Resolve<IJVServiceOperationListener>();
            InfoStart(listener, typeof(MainOptionsHandler), args => "JVLinkToSQLite 実行モード");
            var stopwatch = Stopwatch.StartNew();

            var retCode = ExecuteCore(resolver, listener, options);

            InfoEnd(listener, typeof(MainOptionsHandler), args => $"JVLinkToSQLite 実行モード {args[0]}s", stopwatch.Elapsed.TotalSeconds);
            return retCode;
        }

        private static int ExecuteCore(IResolver resolver, IJVServiceOperationListener listener, MainOptions options)
        {
            var retCode = (int)ReturnCodes.Success;
            var bootstrap = resolver.Resolve<JVLinkToSQLiteBootstrap>();
            var setting = bootstrap.LoadSettingOrDefault(options.ToLoadSettingParameter());
            foreach (var settingDetail in setting.Details)
            {
                var oprAgg = settingDetail.NewOperatorAggregate(resolver, isImmediate: true);
                var rsltAgg = oprAgg.OperateAll();
                if (!rsltAgg.AreAllSucceeded)
                {
                    retCode = rsltAgg.FirstReturnCode;
                    Warning(listener,
                            typeof(MainOptionsHandler),
                            MessageForWarningCanContinue,
                            rsltAgg.FirstDebugMessage,
                            rsltAgg.FirstArguments,
                            rsltAgg.FirstCallerFilePath,
                            rsltAgg.FirstCallerLineNumber);
                }
            }

            if (retCode == (int)ReturnCodes.Success && !options.SkipsLastModifiedUpdate)
            {
                bootstrap.SaveSetting(options.Setting, setting);
            }

            return retCode;
        }
    }
}
