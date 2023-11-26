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

using DryIoc;
using System.Diagnostics;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.Settings;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite.Operators
{
    internal class JVRealTimeDataServiceOperator : JVServiceOperator
    {
        public class Factory
        {
            private readonly IResolver _resolver;
            private readonly IJVServiceOperationListener _listener;
            private readonly IJVLinkService _jvLinkSrv;

            public Factory(IResolver resolver, IJVServiceOperationListener listener, IJVLinkService jvLinkSrv)
            {
                _resolver = resolver;
                _listener = listener;
                _jvLinkSrv = jvLinkSrv;
            }

            public JVRealTimeDataServiceOperator New(SQLiteConnectionInfo connInfo,
                                                     JVDataSpecSetting dataSpecSetting)
            {
                return new JVRealTimeDataServiceOperator(_resolver, _listener, _jvLinkSrv, connInfo, dataSpecSetting);
            }
        }

        private readonly SQLiteConnectionInfo _connInfo;
        private readonly JVDataSpecSetting _dataSpecSetting;

        public JVRealTimeDataServiceOperator(IResolver resolver,
                                             IJVServiceOperationListener listener,
                                             IJVLinkService jvLinkSrv,
                                             SQLiteConnectionInfo connInfo,
                                             JVDataSpecSetting dataSpecSetting) :
            base(resolver, listener, jvLinkSrv)
        {
            _connInfo = connInfo;
            _dataSpecSetting = dataSpecSetting;
        }

        static string MessageForServiceOperationError(params object[] args) =>
            $"エラーが発生しました。エラー (引数)：{args[0]} ({StringMixin.JoinIfAvailable(", ", args[1])})";

        public override JVLinkServiceOperationResult Operate()
        {
            InfoStart(_listener,
                      this,
                      args => $"リアルタイム系データの取得（データ種別：{args[0]}、読み出し開始ポイント日時：{args[1]}）",
                      _dataSpecSetting.JVDataSpec,
                      _dataSpecSetting.DataSpecKey);
            var stopwatch = Stopwatch.StartNew();

            var ret = OperateCore();

            InfoEnd(_listener, this, args => $"リアルタイム系データの取得 {args[0]}ms", stopwatch.ElapsedMilliseconds);
            return ret;
        }

        private JVLinkServiceOperationResult OperateCore()
        {
            using (var openRslt = _jvLinkSrv.JVRTOpen(_dataSpecSetting.JVDataSpec, _dataSpecSetting.DataSpecKey))
            {
                if (openRslt.Interpretation.Failed)
                {
                    var oprRslt = JVLinkServiceOperationResult.From(openRslt);
                    Warning(_listener,
                            this,
                            MessageForServiceOperationError,
                            oprRslt.DebugMessage,
                            oprRslt.Arguments);
                    return oprRslt;
                }

                if (openRslt.IsEmpty)
                {
                    var oprRslt = JVLinkServiceOperationResult.From(openRslt);
                    Info(_listener,
                         this,
                         args => $"該当データがありません。メッセージ (引数)：{args[0]} ({StringMixin.JoinIfAvailable(", ", args[1])})",
                         oprRslt.DebugMessage,
                         oprRslt.Arguments);
                    return oprRslt;
                }

                using (var jvDataToSQLiteOpr = _resolver.Resolve<JVDataToSQLiteOperator.Factory>().New(_connInfo, openRslt, _dataSpecSetting.ExcludedJVRecordSpecs))
                {
                    return jvDataToSQLiteOpr.InsertOrUpdateAll();
                }
            }
        }
    }
}
