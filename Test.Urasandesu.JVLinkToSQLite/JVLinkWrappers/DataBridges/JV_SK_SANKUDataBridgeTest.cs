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
    public class JV_SK_SANKUDataBridgeTest
    {
        [Test]
        public void JV_SK_SANKUDataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_SK_SANKUDataBridge("SK22023083120211062262021022211010000051033200新冠町　　　　　　　11200024621220067259114000642312400274111120002208122006018411400029241240027410114000408112400161821140003863124002180811200018471220043955\r\n",
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
                    cmd.AssertTableCreation("NL_SK_SANKU", JVDataStructColumns.SK);
                }
            }
        }

        [Test]
        public void JV_SK_SANKUDataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_SK_SANKUDataBridge("SK22023083120211062262021022211010000051033200新冠町　　　　　　　11200024621220067259114000642312400274111120002208122006018411400029241240027410114000408112400161821140003863124002180811200018471220043955\r\n",
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
        public void JV_SK_SANKUDataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_SK_SANKUDataBridge("SK22023083120211062262021022211010000051033200新冠町　　　　　　　11200024621220067259114000642312400274111120002208122006018411400029241240027410114000408112400161821140003863124002180811200018471220043955\r\n",
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
                    cmd.AssertRecords("NL_SK_SANKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "SK" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230831" },
                            { "KettoNum", "2021106226" },
                            { "BirthDate", "20210222" },
                            { "SexCD", "1" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "01" },
                            { "SankuMochiKubun", "0" },
                            { "ImportYear", "0000" },
                            { "BreederCode", "51033200" },
                            { "SanchiName", "新冠町" },
                            { "HansyokuNum0", "1120002462" },
                            { "HansyokuNum1", "1220067259" },
                            { "HansyokuNum2", "1140006423" },
                            { "HansyokuNum3", "1240027411" },
                            { "HansyokuNum4", "1120002208" },
                            { "HansyokuNum5", "1220060184" },
                            { "HansyokuNum6", "1140002924" },
                            { "HansyokuNum7", "1240027410" },
                            { "HansyokuNum8", "1140004081" },
                            { "HansyokuNum9", "1240016182" },
                            { "HansyokuNum10", "1140003863" },
                            { "HansyokuNum11", "1240021808" },
                            { "HansyokuNum12", "1120001847" },
                            { "HansyokuNum13", "1220043955" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_SK_SANKUDataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_SK_SANKUDataBridge("SK22023083120211062262021022211010000051033200新冠町　　　　　　　11200024621220067259114000642312400274111120002208122006018411400029241240027410114000408112400161821140003863124002180811200018471220043955\r\n",
                                                       JVOpenOptions.Normal);
            var dataBridge2 = NewJV_SK_SANKUDataBridge("SK22023083120211062262021022211010000051033200新杯町　　　　　　　11200024621220067259114000642312400274111120002208122006018411400029241240027410114000408112400161821140003863124002180811200018471220043955\r\n",
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
                    cmd.AssertRecords("NL_SK_SANKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "SK" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230831" },
                            { "KettoNum", "2021106226" },
                            { "BirthDate", "20210222" },
                            { "SexCD", "1" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "01" },
                            { "SankuMochiKubun", "0" },
                            { "ImportYear", "0000" },
                            { "BreederCode", "51033200" },
                            { "SanchiName", "新杯町" },
                            { "HansyokuNum0", "1120002462" },
                            { "HansyokuNum1", "1220067259" },
                            { "HansyokuNum2", "1140006423" },
                            { "HansyokuNum3", "1240027411" },
                            { "HansyokuNum4", "1120002208" },
                            { "HansyokuNum5", "1220060184" },
                            { "HansyokuNum6", "1140002924" },
                            { "HansyokuNum7", "1240027410" },
                            { "HansyokuNum8", "1140004081" },
                            { "HansyokuNum9", "1240016182" },
                            { "HansyokuNum10", "1140003863" },
                            { "HansyokuNum11", "1240021808" },
                            { "HansyokuNum12", "1120001847" },
                            { "HansyokuNum13", "1220043955" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_SK_SANKU_V4802DataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_SK_SANKU_V4802DataBridge("SK120220920202111001720210228210732022800708愛　　　　　　　　　1140682012430817114066521242969911403620124308161140537812428623114026061242969811401591124096651140194812406412\r\n",
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
                    cmd.AssertTableCreation("NL_SK_SANKU", JVDataStructColumns.SK_V4802);
                }
            }
        }

        [Test]
        public void JV_SK_SANKU_V4802DataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_SK_SANKU_V4802DataBridge("SK120220920202111001720210228210732022800708愛　　　　　　　　　1140682012430817114066521242969911403620124308161140537812428623114026061242969811401591124096651140194812406412\r\n",
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
        public void JV_SK_SANKU_V4802DataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_SK_SANKU_V4802DataBridge("SK120220920202111001720210228210732022800708愛　　　　　　　　　1140682012430817114066521242969911403620124308161140537812428623114026061242969811401591124096651140194812406412\r\n",
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
                    cmd.AssertRecords("NL_SK_SANKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "SK" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20220920" },
                            { "KettoNum", "2021110017" },
                            { "BirthDate", "20210228" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "07" },
                            { "SankuMochiKubun", "3" },
                            { "ImportYear", "2022" },
                            { "BreederCode", "80070800" },
                            { "SanchiName", "愛" },
                            { "HansyokuNum0", "1140006820" },
                            { "HansyokuNum1", "1240030817" },
                            { "HansyokuNum2", "1140006652" },
                            { "HansyokuNum3", "1240029699" },
                            { "HansyokuNum4", "1140003620" },
                            { "HansyokuNum5", "1240030816" },
                            { "HansyokuNum6", "1140005378" },
                            { "HansyokuNum7", "1240028623" },
                            { "HansyokuNum8", "1140002606" },
                            { "HansyokuNum9", "1240029698" },
                            { "HansyokuNum10", "1140001591" },
                            { "HansyokuNum11", "1240009665" },
                            { "HansyokuNum12", "1140001948" },
                            { "HansyokuNum13", "1240006412" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_SK_SANKU_V4802DataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_SK_SANKU_V4802DataBridge("SK120220920202111001720210228210732022800708愛　　　　　　　　　1140682012430817114066521242969911403620124308161140537812428623114026061242969811401591124096651140194812406412\r\n",
                                                             JVOpenOptions.Normal);
            var dataBridge2 = NewJV_SK_SANKU_V4802DataBridge("SK120220920202111001720210228210732022800708藍　　　　　　　　　1140682012430817114066521242969911403620124308161140537812428623114026061242969811401591124096651140194812406412\r\n",
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
                    cmd.AssertRecords("NL_SK_SANKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "SK" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20220920" },
                            { "KettoNum", "2021110017" },
                            { "BirthDate", "20210228" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "07" },
                            { "SankuMochiKubun", "3" },
                            { "ImportYear", "2022" },
                            { "BreederCode", "80070800" },
                            { "SanchiName", "藍" },
                            { "HansyokuNum0", "1140006820" },
                            { "HansyokuNum1", "1240030817" },
                            { "HansyokuNum2", "1140006652" },
                            { "HansyokuNum3", "1240029699" },
                            { "HansyokuNum4", "1140003620" },
                            { "HansyokuNum5", "1240030816" },
                            { "HansyokuNum6", "1140005378" },
                            { "HansyokuNum7", "1240028623" },
                            { "HansyokuNum8", "1140002606" },
                            { "HansyokuNum9", "1240029698" },
                            { "HansyokuNum10", "1140001591" },
                            { "HansyokuNum11", "1240009665" },
                            { "HansyokuNum12", "1140001948" },
                            { "HansyokuNum13", "1240006412" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_SK_SANKU_V4802DataBridge_BuildUpInsertCommand_can_create_an_compatible_insert_command()
        {
            // Arrange
            var dataBridge1 = NewJV_SK_SANKU_V4802DataBridge("SK120220920202111001720210228210732022800708愛　　　　　　　　　1140682012430817114066521242969911403620124308161140537812428623114026061242969811401591124096651140194812406412\r\n",
                                                             JVOpenOptions.Normal);
            var dataBridge2 = NewJV_SK_SANKUDataBridge("SK22023083120211062262021022211010000051033200新冠町　　　　　　　11200024621220067259114000642312400274111120002208122006018411400029241240027410114000408112400161821140003863124002180811200018471220043955\r\n",
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
                    cmd.AssertRecords("NL_SK_SANKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "SK" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20220920" },
                            { "KettoNum", "2021110017" },
                            { "BirthDate", "20210228" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "07" },
                            { "SankuMochiKubun", "3" },
                            { "ImportYear", "2022" },
                            { "BreederCode", "80070800" },
                            { "SanchiName", "愛" },
                            { "HansyokuNum0", "1140006820" },
                            { "HansyokuNum1", "1240030817" },
                            { "HansyokuNum2", "1140006652" },
                            { "HansyokuNum3", "1240029699" },
                            { "HansyokuNum4", "1140003620" },
                            { "HansyokuNum5", "1240030816" },
                            { "HansyokuNum6", "1140005378" },
                            { "HansyokuNum7", "1240028623" },
                            { "HansyokuNum8", "1140002606" },
                            { "HansyokuNum9", "1240029698" },
                            { "HansyokuNum10", "1140001591" },
                            { "HansyokuNum11", "1240009665" },
                            { "HansyokuNum12", "1140001948" },
                            { "HansyokuNum13", "1240006412" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "SK" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230831" },
                            { "KettoNum", "2021106226" },
                            { "BirthDate", "20210222" },
                            { "SexCD", "1" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "01" },
                            { "SankuMochiKubun", "0" },
                            { "ImportYear", "0000" },
                            { "BreederCode", "51033200" },
                            { "SanchiName", "新冠町" },
                            { "HansyokuNum0", "1120002462" },
                            { "HansyokuNum1", "1220067259" },
                            { "HansyokuNum2", "1140006423" },
                            { "HansyokuNum3", "1240027411" },
                            { "HansyokuNum4", "1120002208" },
                            { "HansyokuNum5", "1220060184" },
                            { "HansyokuNum6", "1140002924" },
                            { "HansyokuNum7", "1240027410" },
                            { "HansyokuNum8", "1140004081" },
                            { "HansyokuNum9", "1240016182" },
                            { "HansyokuNum10", "1140003863" },
                            { "HansyokuNum11", "1240021808" },
                            { "HansyokuNum12", "1120001847" },
                            { "HansyokuNum13", "1220043955" }
                        },
                    });
                }
            }
        }

        private static JV_SK_SANKUDataBridge NewJV_SK_SANKUDataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_SK_SANKU();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_SK_SANKUDataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }

        private static JV_SK_SANKU_V4802DataBridge NewJV_SK_SANKU_V4802DataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_SK_SANKU_V4802();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_SK_SANKU_V4802DataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }
    }
}