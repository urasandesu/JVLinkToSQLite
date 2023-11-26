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
using System.Collections.Generic;
using System.Linq;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.Operators;
using Urasandesu.JVLinkToSQLite.Settings;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite.OperatorAggregates
{
    internal class JVEventDataOperatorAggregate : JVOperatorAggregate
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

            public JVEventDataOperatorAggregate New(SQLiteConnectionInfo connInfo, JVDataSpecSetting[] dataSpecSettings)
            {
                return new JVEventDataOperatorAggregate(_resolver, _listener, _jvLinkSrv, connInfo, dataSpecSettings);
            }
        }

        private readonly SQLiteConnectionInfo _connInfo;
        private readonly JVDataSpecSetting[] _dataSpecSettings;

        public JVEventDataOperatorAggregate(IResolver resolver,
                                            IJVServiceOperationListener listener,
                                            IJVLinkService jvLinkSrv,
                                            SQLiteConnectionInfo connInfo,
                                            JVDataSpecSetting[] dataSpecSettings) :
            base(resolver, listener, jvLinkSrv)
        {
            _connInfo = connInfo;
            _dataSpecSettings = dataSpecSettings;
        }

        class MyWatchEventListener : IJVWatchEventListener
        {
            private readonly JVEventDataOperatorAggregate _this;
            private readonly JVDataSpecSetting _dataSpecSetting;
            private readonly List<JVLinkServiceOperationResult> _oprRslts;

            public MyWatchEventListener(JVEventDataOperatorAggregate @this, JVDataSpecSetting dataSpecSetting, List<JVLinkServiceOperationResult> oprRslts)
            {
                _this = @this;
                _dataSpecSetting = dataSpecSetting;
                _oprRslts = oprRslts;
            }

            public void OnEvent(string bstr)
            {
                var srvOpr = default(JVServiceOperator);
                if (_dataSpecSetting.IsEnabled)
                {
                    var dataSpecSetting = _dataSpecSetting.Clone();
                    dataSpecSetting.DataSpecKey = new JVRawKey(bstr);
                    srvOpr = _this._resolver.Resolve<JVRealTimeDataServiceOperator.Factory>().New(_this._connInfo, dataSpecSetting);
                }
                else
                {
                    srvOpr = _this._resolver.Resolve<JVNullServiceOperator>();
                }
                _oprRslts.Add(srvOpr.Operate());
            }
        }

        public override JVOperationResultAggregate OperateAll()
        {
            var oprRslts = new List<JVLinkServiceOperationResult>();
            var dispatcher = new JVWatchEventDispatcher();
            foreach (var dataSpecSetting in _dataSpecSettings.Where(_ => _.JVDataSpec.CanWatchEvent))
            {
                Info(_listener,
                     this,
                     args => $"データ種別 '{((JVDataSpecSetting)args[0]).JVDataSpec}' の待ち受けを{(((JVDataSpecSetting)args[0]).IsEnabled ? "開始" : "スキップ")}．．．",
                     dataSpecSetting);
                var listener = new MyWatchEventListener(this, dataSpecSetting, oprRslts);
                dispatcher.AddListener(dataSpecSetting.JVDataSpec, listener);
            }

            try
            {
                oprRslts.Add(JVLinkServiceOperationResult.From(_jvLinkSrv.JVWatchEvent(dispatcher)));

                _listener.OnExternalNotificationRequired(this, new JVServiceMessageEventArgs(_ => "イベント待ち．．．"));
            }
            finally
            {
                oprRslts.Add(JVLinkServiceOperationResult.From(_jvLinkSrv.JVWatchEventClose()));
            }

            return new JVOperationResultAggregate(oprRslts.ToArray());
        }
    }
}

