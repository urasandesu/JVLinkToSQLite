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

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    public class BridgeColumn
    {
        public BridgeColumn(string columnName, Type columnType, bool isHead, bool isId, Func<object, object> applyCompatibilityFixes, string columnComment)
        {
            ColumnName = columnName;
            ParameterName = "@" + columnName;
            ColumnType = columnType;
            SqlColumnType = ToSqlColumnType(ColumnType);
            IsHead = isHead;
            IsId = isId;
            ApplyCompatibilityFixes = applyCompatibilityFixes;
            ColumnComment = columnComment;
        }

        private static string ToSqlColumnType(Type columnType)
        {
            var strColumnType = columnType.ToString();
            switch (strColumnType)
            {
                case "System.Int32":
                case "System.Int64":
                case "System.Int16":
                case "System.Byte":
                    return "integer";
                case "System.Decimal":
                case "System.DateTime":
                    return "numeric";
                case "System.Single":
                case "System.Double":
                    return "real";
                case "System.String":
                default:
                    return "text";
            }
        }

        public string ColumnName { get; }
        public string ParameterName { get; }
        public Type ColumnType { get; }
        public string SqlColumnType { get; }
        public bool IsHead { get; }
        public bool IsId { get; }
        public Func<object, object> ApplyCompatibilityFixes { get; }
        public string ColumnComment { get; }

        public object GetCompatibilityFixedValue(object value)
        {
            return ApplyCompatibilityFixes == null ? value : ApplyCompatibilityFixes(value);
        }

        public override string ToString()
        {
            return $"{ColumnName} {SqlColumnType}";
        }
    }
}
