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

using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Test.Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Operators;

namespace Test.Urasandesu.JVLinkToSQLite.DocGen
{
    [TestFixture]
    public class TableSpecGenerator
    {
        [Test]
        public void GenerateAll()
        {
            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());

                    var testCases = DataBridgeIntegrationTest.TestCaseSourceOfBuffer_can_be_interpreted_if_condition_is_.TestCases;
                    var publishedTables = new HashSet<string>();
                    foreach (TestCaseData testCase in testCases)
                    {
                        var openOption = (JVOpenOptions)testCase.Arguments[0];
                        var dataSpec = (JVDataSpec)testCase.Arguments[1];
                        var buffer = (string)testCase.Arguments[2];
                        var assertion = (Action<DataBridge>)testCase.Arguments[3];

                        var listener = Substitute.For<IJVServiceOperationListener>();
                        var readRslt = new JVReadResult();
                        readRslt.SetOpenOption(openOption);
                        readRslt.SetDataSpec(dataSpec);
                        readRslt.SetBuffer(buffer);
                        var factory = new DataBridgeFactory(listener, readRslt);
                        var dataBridge = factory.NewDataBridge();
                        if (publishedTables.Add(dataBridge.TableName))
                        {
                            foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                            {
                                bltCmd.ExecuteNonQuery();
                            }

                            {
                                var tableName = dataBridge.TableName;
                                WriteTableDocument(cmd, tableName);
                            }

                            if (dataBridge.ChildTableNameList != null)
                            {
                                foreach (var tableName in dataBridge.ChildTableNameList)
                                {
                                    WriteTableDocument(cmd, tableName);
                                }
                            }
                        }
                    }

                    cmd.CommandText = JVDataFileSkippabilityHandler.CreateTableSql;
                    cmd.Parameters.Clear();
                    cmd.ExecuteNonQuery();
                    WriteTableDocument(cmd, JVDataFileSkippabilityHandler.TableName);
                }
            }
        }

        private static void WriteTableDocument(SQLiteCommand cmd, string tableName)
        {
            cmd.CommandText = $"select * from sqlite_master where name='{tableName}';";
            cmd.Parameters.Clear();
            var dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            var fileName = tableName + ".md";
            Console.WriteLine($"  * [{tableName} テーブル](https://github.com/urasandesu/JVLinkToSQLite/wiki/{tableName})");
            using (var sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine("※コメント内の「コード表」は、[JRA-VAN Data Lab. SDK](https://jra-van.jp/dlb/sdv/sdk.html)に同梱されている「JV-Data 仕様書」の「コード表」を指していますので、そちらをご参照ください。");
                sw.WriteLine("```sql");
                sw.WriteLine(dt.Rows[0]["sql"]);
                sw.WriteLine("```");
            }
        }
    }
}
