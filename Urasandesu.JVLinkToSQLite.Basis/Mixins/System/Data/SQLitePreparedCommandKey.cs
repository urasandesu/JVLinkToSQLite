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
using System.Diagnostics;

namespace Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data
{
    [DebuggerDisplay(nameof(Id) + "={" + nameof(Id) + "}, " + nameof(TableName) + "={" + nameof(TableName) + "}")]
    public struct SQLitePreparedCommandKey : IEquatable<SQLitePreparedCommandKey>
    {
        public SQLitePreparedCommandKey(int id, string tableName)
        {
            Id = id;
            TableName = tableName;
        }

        public int Id { get; }
        public string TableName { get; }

        public override bool Equals(object obj)
        {
            return obj is SQLitePreparedCommandKey key && Equals(key);
        }

        public bool Equals(SQLitePreparedCommandKey other)
        {
            return Id == other.Id &&
                   TableName == other.TableName;
        }

        public override int GetHashCode()
        {
            int hashCode = -1377217700;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TableName);
            return hashCode;
        }

        public static bool operator ==(SQLitePreparedCommandKey left, SQLitePreparedCommandKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SQLitePreparedCommandKey left, SQLitePreparedCommandKey right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{nameof(Id)}={Id}, {nameof(TableName)}={TableName}";
        }
    }
}
