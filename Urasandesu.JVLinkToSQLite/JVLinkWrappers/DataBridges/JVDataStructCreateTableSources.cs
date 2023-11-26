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
using System.Linq;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    public sealed partial class JVDataStructCreateTableSources : JVDataStructCommandTextSources
    {
        private static string ToTextTemplate(JVDataStructColumns c, string typeDescription = null)
        {
            var tableComment = default(string);
            if (typeDescription != null)
            {
                var lines = typeDescription.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                tableComment = $"  -- {string.Join("\r\n  -- ", lines.Take(lines.Length - 1))}\r\n"; // 最後の要素は「レコード区切」なので除去
            }
            return $"create table if not exists {TableNamePlaceHolder} (\r\n" +
                   tableComment + 
                   $"  {string.Join("\r\n  , ", c.Value.Select(_ => _.ColumnName + " " + _.SqlColumnType + (_.IsId ? " not null" : string.Empty) + $" -- {_.ColumnComment}"))}\r\n" +
                   $"  {(c.Value.Any(_ => _.IsId) ? ", primary key (" + string.Join(",", c.Value.Where(_ => _.IsId).Select(_ => _.ColumnName)) + ")" : string.Empty)}" +
                   $")";
        }
        private JVDataStructCreateTableSources() { }
    }
}
