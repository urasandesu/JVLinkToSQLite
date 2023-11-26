﻿// JVLinkToSQLite は、JRA-VAN データラボが提供する競馬データを SQLite データベースに変換するツールです。
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

namespace Urasandesu.JVLinkToSQLite.OperatorAggregates
{
    internal class JVImmediateDataOperatorAggregate : JVOperatorAggregate
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

            public JVImmediateDataOperatorAggregate New(SQLiteConnectionInfo connInfo, JVDataSpecSetting[] dataSpecSettings)
            {
                return new JVImmediateDataOperatorAggregate(_resolver, _listener, _jvLinkSrv, connInfo, dataSpecSettings);
            }
        }

        private readonly SQLiteConnectionInfo _connInfo;
        private readonly JVDataSpecSetting[] _dataSpecSettings;

        public JVImmediateDataOperatorAggregate(IResolver resolver,
                                                IJVServiceOperationListener listener,
                                                IJVLinkService jvLinkSrv,
                                                SQLiteConnectionInfo connInfo,
                                                JVDataSpecSetting[] dataSpecSettings) :
            base(resolver, listener, jvLinkSrv)
        {
            _connInfo = connInfo;
            _dataSpecSettings = dataSpecSettings;
        }

        public override JVOperationResultAggregate OperateAll()
        {
            var oprRslts = new List<JVLinkServiceOperationResult>();
            foreach (var dataSpecSetting in _dataSpecSettings.Where(_ => _.JVDataSpec.IsImmediatelyExecutable))
            {
                var srvOpr = NewServiceOperator(_connInfo, dataSpecSetting);
                var oprRslt = srvOpr.Operate();
                oprRslts.Add(oprRslt);
            }
            return new JVOperationResultAggregate(oprRslts.ToArray());
        }

        private JVServiceOperator NewServiceOperator(SQLiteConnectionInfo connInfo,
                                                     JVDataSpecSetting dataSpecSetting)
        {
            if (dataSpecSetting.IsEnabled)
            {
                return _resolver.Resolve<JVRealTimeDataServiceOperator.Factory>().New(connInfo, dataSpecSetting);
            }
            else
            {
                return _resolver.Resolve<JVNullServiceOperator>();
            }
        }
    }
}

