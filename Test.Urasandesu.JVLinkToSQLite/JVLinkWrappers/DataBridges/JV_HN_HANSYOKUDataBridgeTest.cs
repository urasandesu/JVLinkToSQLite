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
using System.Data.SQLite;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using static JVData_Struct;

namespace Test.Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    [TestFixture]
    public class JV_HN_HANSYOKUDataBridgeTest
    {
        [Test]
        public void JV_HN_HANSYOKUDataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_HN_HANSYOKUDataBridge("HN12023083112200717330000000020161056440マーヴェラスクイン　　　　　　　　　ﾏｰｳﾞｪﾗｽｸｲﾝ                              Marvelous Queen                                                                 2016210300000新ひだか町　　　　　11200023201220056099\r\n",
                                                         JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertTableCreation("NL_HN_HANSYOKU", JVDataStructColumns.HN);
                }
            }
        }

        [Test]
        public void JV_HN_HANSYOKUDataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_HN_HANSYOKUDataBridge("HN12023083112200717330000000020161056440マーヴェラスクイン　　　　　　　　　ﾏｰｳﾞｪﾗｽｸｲﾝ                              Marvelous Queen                                                                 2016210300000新ひだか町　　　　　11200023201220056099\r\n",
                                                         JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    var bltCmds = new List<SQLitePreparedCommand>();
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                        bltCmds.Add(bltCmd);
                    }

                    // Assert
                    foreach (var bltCmd in bltCmds)
                    {
                        Assert.DoesNotThrow(() => bltCmd.ExecuteNonQuery(), bltCmd.GetLoggingQuery());
                    }
                }
            }
        }

        [Test]
        public void JV_HN_HANSYOKUDataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_HN_HANSYOKUDataBridge("HN12023083112200717330000000020161056440マーヴェラスクイン　　　　　　　　　ﾏｰｳﾞｪﾗｽｸｲﾝ                              Marvelous Queen                                                                 2016210300000新ひだか町　　　　　11200023201220056099\r\n",
                                                         JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecords("NL_HN_HANSYOKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HN" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230831" },
                            { "HansyokuNum", "1220071733" },
                            { "reserved", "00000000" },
                            { "KettoNum", "2016105644" },
                            { "DelKubun", "0" },
                            { "Bamei", "マーヴェラスクイン" },
                            { "BameiKana", "ﾏｰｳﾞｪﾗｽｸｲﾝ" },
                            { "BameiEng", "Marvelous Queen" },
                            { "BirthYear", "2016" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "03" },
                            { "HansyokuMochiKubun", "0" },
                            { "ImportYear", "0000" },
                            { "SanchiName", "新ひだか町" },
                            { "HansyokuFNum", "1120002320" },
                            { "HansyokuMNum", "1220056099" },
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_HN_HANSYOKUDataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_HN_HANSYOKUDataBridge("HN12023083112200717330000000020161056440マーヴェラスクイン　　　　　　　　　ﾏｰｳﾞｪﾗｽｸｲﾝ                              Marvelous Queen                                                                 2016210300000新ひだか町　　　　　11200023201220056099\r\n",
                                                          JVOpenOptions.Normal);
            var dataBridge2 = NewJV_HN_HANSYOKUDataBridge("HN12023083112200717330000000020161056440マアヴェラスクイン　　　　　　　　　ﾏｱｳﾞｪﾗｽｸｲﾝ                              Marvelous Queen                                                                 2016210300000新ひだか町　　　　　11200023201220056099\r\n",
                                                          JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge1.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge1.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge2.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecords("NL_HN_HANSYOKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HN" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230831" },
                            { "HansyokuNum", "1220071733" },
                            { "reserved", "00000000" },
                            { "KettoNum", "2016105644" },
                            { "DelKubun", "0" },
                            { "Bamei", "マアヴェラスクイン" },
                            { "BameiKana", "ﾏｱｳﾞｪﾗｽｸｲﾝ" },
                            { "BameiEng", "Marvelous Queen" },
                            { "BirthYear", "2016" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "03" },
                            { "HansyokuMochiKubun", "0" },
                            { "ImportYear", "0000" },
                            { "SanchiName", "新ひだか町" },
                            { "HansyokuFNum", "1120002320" },
                            { "HansyokuMNum", "1220056099" },
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_HN_HANSYOKU_V4802DataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_HN_HANSYOKU_V4802DataBridge("HN120220929124334060000000000000000000Bugle                                                                       Bugle                                                                           2012210090000　　　　　　　　　　1140638612426970\r\n",
                                                               JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertTableCreation("NL_HN_HANSYOKU", JVDataStructColumns.HN_V4802);
                }
            }
        }

        [Test]
        public void JV_HN_HANSYOKU_V4802DataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_HN_HANSYOKU_V4802DataBridge("HN120220929124334060000000000000000000Bugle                                                                       Bugle                                                                           2012210090000　　　　　　　　　　1140638612426970\r\n",
                                                               JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    var bltCmds = new List<SQLitePreparedCommand>();
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                        bltCmds.Add(bltCmd);
                    }

                    // Assert
                    foreach (var bltCmd in bltCmds)
                    {
                        Assert.DoesNotThrow(() => bltCmd.ExecuteNonQuery(), bltCmd.GetLoggingQuery());
                    }
                }
            }
        }

        [Test]
        public void JV_HN_HANSYOKU_V4802DataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_HN_HANSYOKU_V4802DataBridge("HN120220929124334060000000000000000000Bugle                                                                       Bugle                                                                           2012210090000　　　　　　　　　　1140638612426970\r\n",
                                                               JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecords("NL_HN_HANSYOKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HN" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20220929" },
                            { "HansyokuNum", "1240033406" },
                            { "reserved", "00000000" },
                            { "KettoNum", "0000000000" },
                            { "DelKubun", "0" },
                            { "Bamei", "Bugle" },
                            { "BameiKana", "" },
                            { "BameiEng", "Bugle" },
                            { "BirthYear", "2012" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "00" },
                            { "HansyokuMochiKubun", "9" },
                            { "ImportYear", "0000" },
                            { "SanchiName", "" },
                            { "HansyokuFNum", "1140006386" },
                            { "HansyokuMNum", "1240026970" },
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_HN_HANSYOKU_V4802DataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_HN_HANSYOKU_V4802DataBridge("HN120220929124334060000000000000000000Bugle                                                                       Bugle                                                                           2012210090000　　　　　　　　　　1140638612426970\r\n",
                                                                JVOpenOptions.Normal);
            var dataBridge2 = NewJV_HN_HANSYOKU_V4802DataBridge("HN120220929124334060000000000000000000Vugle                                                                       Bugle                                                                           2012210090000　　　　　　　　　　1140638612426970\r\n",
                                                                JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge1.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge1.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge2.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecords("NL_HN_HANSYOKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HN" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20220929" },
                            { "HansyokuNum", "1240033406" },
                            { "reserved", "00000000" },
                            { "KettoNum", "0000000000" },
                            { "DelKubun", "0" },
                            { "Bamei", "Vugle" },
                            { "BameiKana", "" },
                            { "BameiEng", "Bugle" },
                            { "BirthYear", "2012" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "00" },
                            { "HansyokuMochiKubun", "9" },
                            { "ImportYear", "0000" },
                            { "SanchiName", "" },
                            { "HansyokuFNum", "1140006386" },
                            { "HansyokuMNum", "1240026970" },
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_HN_HANSYOKU_V4802DataBridge_BuildUpInsertCommand_can_create_an_compatible_insert_command()
        {
            // Arrange
            var dataBridge1 = NewJV_HN_HANSYOKU_V4802DataBridge("HN120220929124334060000000000000000000Bugle                                                                       Bugle                                                                           2012210090000　　　　　　　　　　1140638612426970\r\n",
                                                                JVOpenOptions.Normal);
            var dataBridge2 = NewJV_HN_HANSYOKUDataBridge("HN12023083112200717330000000020161056440マーヴェラスクイン　　　　　　　　　ﾏｰｳﾞｪﾗｽｸｲﾝ                              Marvelous Queen                                                                 2016210300000新ひだか町　　　　　11200023201220056099\r\n",
                                                          JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge1.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge1.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge2.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecords("NL_HN_HANSYOKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HN" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20220929" },
                            { "HansyokuNum", "1240033406" },
                            { "reserved", "00000000" },
                            { "KettoNum", "0000000000" },
                            { "DelKubun", "0" },
                            { "Bamei", "Bugle" },
                            { "BameiKana", "" },
                            { "BameiEng", "Bugle" },
                            { "BirthYear", "2012" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "00" },
                            { "HansyokuMochiKubun", "9" },
                            { "ImportYear", "0000" },
                            { "SanchiName", "" },
                            { "HansyokuFNum", "1140006386" },
                            { "HansyokuMNum", "1240026970" },
                        },
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HN" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230831" },
                            { "HansyokuNum", "1220071733" },
                            { "reserved", "00000000" },
                            { "KettoNum", "2016105644" },
                            { "DelKubun", "0" },
                            { "Bamei", "マーヴェラスクイン" },
                            { "BameiKana", "ﾏｰｳﾞｪﾗｽｸｲﾝ" },
                            { "BameiEng", "Marvelous Queen" },
                            { "BirthYear", "2016" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "03" },
                            { "HansyokuMochiKubun", "0" },
                            { "ImportYear", "0000" },
                            { "SanchiName", "新ひだか町" },
                            { "HansyokuFNum", "1120002320" },
                            { "HansyokuMNum", "1220056099" },
                        },
                    });
                }
            }
        }

        private static JV_HN_HANSYOKUDataBridge NewJV_HN_HANSYOKUDataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_HN_HANSYOKU();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_HN_HANSYOKUDataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }

        private static JV_HN_HANSYOKU_V4802DataBridge NewJV_HN_HANSYOKU_V4802DataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_HN_HANSYOKU_V4802();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_HN_HANSYOKU_V4802DataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }
    }
}