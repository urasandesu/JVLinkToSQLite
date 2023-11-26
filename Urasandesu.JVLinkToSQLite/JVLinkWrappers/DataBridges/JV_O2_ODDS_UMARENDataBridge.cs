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

using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;
using static JVData_Struct;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    public partial class JV_O2_ODDS_UMARENDataBridge : DataBridge<JV_O2_ODDS_UMAREN>
    {
        partial void InitializeIfNecessary()
        {
            if (OpenOption == JVOpenOptions.RealTime)
            {
                Columns = JVDataStructColumns.O2Conditional.Value;
            }
        }

        partial void BuildUpCreateTableCommandWithCommandTextIfNecessary(SQLitePreparedCommandCache commandCache)
        {
            if (OpenOption == JVOpenOptions.RealTime)
            {
                commandCache.Get(new SQLitePreparedCommandKey(CreateTableId, TableName),
                                 () => DropTableIfNecessary(TableName, JVDataStructCreateTableSources.O2Conditional.GetCommandText(TableName)));
            }
        }

        partial void BuildUpInsertCommandWithCommandTextIfNecessary(SQLitePreparedCommandCache commandCache)
        {
            if (OpenOption == JVOpenOptions.RealTime)
            {
                commandCache.Get(new SQLitePreparedCommandKey(InsertId, TableName),
                                 () => JVDataStructInsertSources.O2Conditional.GetCommandText(TableName));
            }
        }
    }
}
