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

using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;

namespace Test.Urasandesu.JVLinkToSQLite
{
    public static class DbCommandAssert
    {
        public static void AssertTableCreation(this DbCommand @this, string tableName, JVDataStructColumns columns, bool isDebug = false)
        {
            @this.AssertTableCreation(tableName, columns.Value, isDebug);
        }

        public static void AssertTableCreation(this DbCommand @this, string tableName, IReadOnlyList<BridgeColumn> columns, bool isDebug = false)
        {
            @this.CommandText = $"select * from {tableName}";
            @this.Parameters.Clear();
            var dt = new DataTable();
            dt.Load(@this.ExecuteReader());
            if (isDebug)
            {
                using (var sw = new StreamWriter($"{tableName}.xml", false, Encoding.UTF8))
                {
                    dt.WriteXmlSchema(sw);
                }
            }

            Assert.That(dt.PrimaryKey, Is.EquivalentTo(columns.Where(_ => _.IsId).Select(_ => dt.Columns[_.ColumnName])));
            Assert.That(dt.Columns, Is.EquivalentTo(columns.Select(_ => dt.Columns[_.ColumnName])));
        }

        public static void AssertTableNotExists(this DbCommand @this, string tableName)
        {
            @this.CommandText = $"create table {tableName} (Id integer); drop table {tableName}";
            @this.Parameters.Clear();
            @this.ExecuteNonQuery();
        }

        public static void AssertRecords(this DbCommand cmd, string tableName, List<Dictionary<string, object>> expected, bool isDebug = false)
        {
            cmd.CommandText = $"select * from {tableName}";
            cmd.Parameters.Clear();

            var dt = new DataTable();
            dt.BeginLoadData();
            dt.Load(cmd.ExecuteReader(), LoadOption.Upsert);
            dt.EndLoadData();
            if (isDebug)
            {
                using (var sw = new StreamWriter($"{tableName}.xml", false, Encoding.UTF8))
                {
                    dt.WriteXml(sw);
                }
            }

            Assert.That(dt.Rows.Count, Is.EqualTo(expected.Count), $"{tableName} rows count equality");

            var idx = 0;
            foreach (var item in dt.AsEnumerable().Zip(expected, (_1, _2) => new { ActualRow = _1, ExpectedRow = _2 }))
            {
                if (idx == 0)
                {
                    Assert.That(dt.Columns.Count, Is.EqualTo(item.ExpectedRow.Count), $"{tableName} columns count equality");
                }

                foreach (DataColumn col in dt.Columns)
                {
                    if (!item.ExpectedRow.TryGetValue(col.ColumnName, out var expectedValue))
                    {
                        Assert.Fail($"{tableName} には、カラム '{col.ColumnName}' は存在すべきではない。row={idx}");
                    }
                    var actualValue = item.ActualRow[col.ColumnName];
                    Assert.That(actualValue, Is.EqualTo(expectedValue), $"row={idx}, column={col.ColumnName}");
                }
                idx++;
            }
        }

        public static void AssertRecordsAreEmpty(this DbCommand cmd, string tableName, bool isDebug = false)
        {
            cmd.CommandText = $"select * from {tableName}";
            cmd.Parameters.Clear();

            var dt = new DataTable();
            dt.BeginLoadData();
            dt.Load(cmd.ExecuteReader(), LoadOption.Upsert);
            dt.EndLoadData();
            if (isDebug)
            {
                using (var sw = new StreamWriter($"{tableName}.xml", false, Encoding.UTF8))
                {
                    dt.WriteXml(sw);
                }
            }

            Assert.That(dt.Rows.Count, Is.EqualTo(0), $"{tableName} rows count equality");
        }

        public static void AssertRecordsAreNotEmpty(this DbCommand cmd, string tableName, bool isDebug = false)
        {
            cmd.CommandText = $"select * from {tableName}";
            cmd.Parameters.Clear();

            var dt = new DataTable();
            dt.BeginLoadData();
            dt.Load(cmd.ExecuteReader(), LoadOption.Upsert);
            dt.EndLoadData();
            if (isDebug)
            {
                using (var sw = new StreamWriter($"{tableName}.xml", false, Encoding.UTF8))
                {
                    dt.WriteXml(sw);
                }
            }

            Assert.That(dt.Rows.Count, Is.Not.EqualTo(0), $"{tableName} rows count equality");
        }
    }
}
