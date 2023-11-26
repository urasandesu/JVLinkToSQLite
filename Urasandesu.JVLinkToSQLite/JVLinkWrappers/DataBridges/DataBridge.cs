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
using System.Linq;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    public abstract class DataBridge
    {
        public JVOpenOptions OpenOption { get; private set; }
        public string TableName { get; protected set; }
        public IReadOnlyList<BridgeColumn> Columns { get; protected set; }
        public Func<object[]> BaseGetter { get; protected set; }
        public string Prefix { get; private set; }
        public IReadOnlyList<string> ChildTableNameList { get; protected set; }
        public IReadOnlyList<int> ChildRowCountList { get; protected set; }
        public IReadOnlyList<IReadOnlyList<BridgeColumn>> ChildPureColumnsList { get; protected set; }
        public Func<Array[]> ChildGetterList { get; protected set; }
        public IReadOnlyList<JVDataStructCreateTableSources> ChildCreateTableSourcesList { get; protected set; }
        public IReadOnlyList<JVDataStructInsertSources> ChildInsertSourcesList { get; protected set; }

        protected static readonly int CreateTableId = 1;
        protected static readonly int InsertId = 2;

        public void SetProperties(JVOpenOptions openOption, string nameOfJVDataStruct)
        {
            OpenOption = openOption;
            SetTableName(nameOfJVDataStruct);
            SetPropertiesCore();
        }

        private void SetTableName(string nameOfJVDataStruct)
        {
            Prefix = "NL_";
            if (OpenOption == JVOpenOptions.RealTime)
            {
                Prefix = "RT_";
            }
            SetTableNameCore(Prefix, nameOfJVDataStruct);
        }

        protected virtual void SetTableNameCore(string prefix, string nameOfJVDataStruct)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentException($"'{nameof(prefix)}' は null または空です。", nameof(prefix));
            }
            if (string.IsNullOrEmpty(nameOfJVDataStruct))
            {
                throw new ArgumentException($"'{nameof(nameOfJVDataStruct)}' は null または空です。", nameof(nameOfJVDataStruct));
            }
            TableName = prefix + nameOfJVDataStruct.Substring(3);
        }

        protected virtual void SetPropertiesCore()
        {
            throw new NotImplementedException($"'{GetType()}' では未実装です。");
        }

        protected string DropTableIfNecessary(string tableName, string createTableCommandText)
        {
            if (OpenOption== JVOpenOptions.RealTime)
            {
                return $"drop table if exists {tableName}; {createTableCommandText}";
            }
            else
            {
                return createTableCommandText;
            }
        }

        public IEnumerable<SQLitePreparedCommand> BuildUpCreateTableCommand(SQLitePreparedCommandCache commandCache)
        {
            var builtCommand = BuildUpCreateTableCommandWithCommandText(commandCache);
            yield return builtCommand;

            if (ChildTableNameList != null)
            {
                for (var i = 0; i < ChildTableNameList.Count; i++)
                {
                    var childTableName = ChildTableNameList[i];
                    var childBuiltCommand = commandCache.Get(new SQLitePreparedCommandKey(CreateTableId, childTableName),
                                                             () => DropTableIfNecessary(childTableName, ChildCreateTableSourcesList[i].GetCommandText(childTableName)));
                    yield return childBuiltCommand;
                }
            }
        }

        protected virtual SQLitePreparedCommand BuildUpCreateTableCommandWithCommandText(SQLitePreparedCommandCache commandCache)
        {
            throw new NotImplementedException($"'{GetType()}' では未実装です。");
        }

        public IEnumerable<SQLitePreparedCommand> BuildUpInsertCommand(SQLitePreparedCommandCache commandCache)
        {
            var builtCommand = BuildUpInsertCommandWithCommandText(commandCache);
            foreach (var item in Columns.Zip(BaseGetter().SelectMany(JVDataStructGetters.ExpandPropertyValues), (_1, _2) => new { Column = _1, Value = _2 }))
            {
                var column = item.Column;
                var value = StringMixin.TrimIfAvailable(item.Value);
                builtCommand.PrepareParameter(column.ParameterName, column.GetCompatibilityFixedValue(value));
            }
            yield return builtCommand;


            if (ChildTableNameList != null)
            {
                var parentKeyParams = Columns.Where(_ => _.IsId).Select(_ => builtCommand.Parameters[_.ParameterName]).ToArray();
                for (var i = 0; i < ChildTableNameList.Count; i++)
                {
                    var childTableName = ChildTableNameList[i];
                    var childRowCount = ChildRowCountList[i];
                    var pureColumns = ChildPureColumnsList[i];
                    var values = ChildGetterList()[i];

                    var idxLength = values.Length.ToString().Length;    // 数が上下しても揃えられるよう、子レコードの最大数で桁数を決める
                    for (var j = 0; j < childRowCount; j++)
                    {
                        var childBuiltCommand = commandCache.Get(new SQLitePreparedCommandKey(InsertId, childTableName),
                                                                 () => ChildInsertSourcesList[i].GetCommandText(childTableName));
                        childBuiltCommand.PrepareParameterRange(parentKeyParams);

                        var idx = j.ToString().PadLeft(idxLength, '0');
                        var childRow = new object[] { idx }.Concat(JVDataStructGetters.ExpandPropertyValues(values.GetValue(j)));
                        foreach (var item in pureColumns.Zip(childRow, (_1, _2) => new { Column = _1, Value = _2 }))
                        {
                            var column = item.Column;
                            var value = StringMixin.TrimIfAvailable(item.Value);
                            childBuiltCommand.PrepareParameter(column.ParameterName, column.GetCompatibilityFixedValue(value));
                        }
                        yield return childBuiltCommand;
                    }
                }
            }
        }

        protected virtual SQLitePreparedCommand BuildUpInsertCommandWithCommandText(SQLitePreparedCommandCache commandCache)
        {
            throw new NotImplementedException($"'{GetType()}' では未実装です。");
        }
    }
}
