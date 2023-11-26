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

using System;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    public partial class DataBridgeFactory
    {
        public class Factory
        {
            private readonly IJVServiceOperationListener _listener;

            public Factory(IJVServiceOperationListener listener)
            {
                _listener = listener;
            }

            public virtual DataBridgeFactory New(JVReadResult readRslt)
            {
                return new DataBridgeFactory(_listener, readRslt);
            }
        }

        private readonly IJVServiceOperationListener _listener;
        private readonly JVReadResult _readRslt;

        public DataBridgeFactory(IJVServiceOperationListener listener, JVReadResult readRslt)
        {
            _listener = listener;
            _readRslt = readRslt;
        }

        public virtual DataBridge NewDataBridge()
        {
            var recordSpec = _readRslt.GetRecordSpec();
            return NewDataBridgeCore(recordSpec);
        }

        private DataBridge NewDataBridge<TJVDataStruct, TDataBridge>(RefAction<TJVDataStruct, string> setDataBAction, TDataBridge dataBridge)
            where TJVDataStruct : struct
            where TDataBridge : DataBridge<TJVDataStruct>
        {
            try
            {
                var buf = _readRslt.Buffer;
                Debug(_listener, this, args => $"Buffer：{(args[0] + "").Replace("\r\n", "\\r\\n")}", buf);
                var dataStruct = new TJVDataStruct();
                setDataBAction(ref dataStruct, ref buf);
                dataBridge.SetProperties(dataStruct, _readRslt.OpenOption);
                return dataBridge;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException($"{typeof(TJVDataStruct).Name} の初期化中に例外が発生しました。サポートされていないデータ形式の可能性があります。" +
                                                "JV-Link の仕様変更に JVLinkToSQLite が追従できていないかもしれません。バージョンアップの有無をご確認ください。", ex);
            }
        }

        private delegate void RefAction<T1, T2>(ref T1 arg1, ref T2 arg2);
    }
}
