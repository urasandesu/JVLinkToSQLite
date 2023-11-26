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
using System.Collections.Generic;
using System.Data.SQLite;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data.Common;

namespace Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data
{
    public class SQLitePreparedCommand
    {
        private readonly SQLiteCommand _command;
        private readonly bool _isPrepared;
        private int _paramIdx;

        internal SQLitePreparedCommand(SQLiteCommand command)
        {
            _command = command;
            _isPrepared = 0 < _command.Parameters.Count;
        }

        public void PrepareParameter(string parameterName, object value)
        {
            if (_isPrepared)
            {
                _command.Parameters[_paramIdx++].Value = value;
            }
            else
            {
                _command.Parameters.AddSlim(parameterName, value);
            }
        }

        public void PrepareParameterRange(IEnumerable<SQLiteParameter> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (_isPrepared)
            {
                foreach (var parameter in parameters)
                {
                    _command.Parameters[_paramIdx++].Value = parameter.Value;
                }
            }
            else
            {
                _command.Parameters.AddRangeSlim(parameters);
            }
        }

        public int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }

        private SQLitePreparedParameterCollection _parameters;
        public SQLitePreparedParameterCollection Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new SQLitePreparedParameterCollection(_command.Parameters);
                }
                return _parameters;
            }
        }

        public string GetLoggingQuery()
        {
            return _command.GetLoggingQuery();
        }
    }
}
