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

namespace Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data
{
    public class SQLitePreparedCommandCache
    {
        private readonly Dictionary<SQLitePreparedCommandKey, SQLiteCommand> _cache = new Dictionary<SQLitePreparedCommandKey, SQLiteCommand>();

        internal SQLitePreparedCommandCache(SQLiteCommand command)
        {
            BaseCommand = command;
        }

        public SQLiteCommand BaseCommand { get; }

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:SQL クエリのセキュリティ脆弱性を確認")]
        public SQLitePreparedCommand Get(SQLitePreparedCommandKey key, Func<string> commandTextProvider)
        {
            if (commandTextProvider == null)
            {
                throw new ArgumentNullException(nameof(commandTextProvider));
            }

            if (_cache.TryGetValue(key, out var command))
            {
                return new SQLitePreparedCommand(command);
            }
            else
            {
                command = (SQLiteCommand)BaseCommand.Clone();
                command.CommandText = commandTextProvider();
                _cache.Add(key, command);
                return new SQLitePreparedCommand(command);
            }
        }

        public void Commit()
        {
            BaseCommand.Transaction.Commit();
        }

        public void CommitAndNewTransaction()
        {
            BaseCommand.Transaction.Commit();
            BaseCommand.Transaction = BaseCommand.Connection.BeginTransaction();
        }

        public void Rollback()
        {
            BaseCommand.Transaction.Rollback();
        }
    }
}
