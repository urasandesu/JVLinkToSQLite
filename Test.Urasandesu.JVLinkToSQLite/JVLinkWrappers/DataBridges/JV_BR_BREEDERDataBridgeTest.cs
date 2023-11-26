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
    public class JV_BR_BREEDERDataBridgeTest
    {
        [Test]
        public void JV_BR_BREEDERDataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_BR_BREEDERDataBridge("BR22023082800001400辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                             Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300049134500000044030000025000026000029000026000025000186000001484921000001178316000760000768000729000734000725005824\r\n",
                                                        JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    // Act
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertTableCreation("NL_BR_BREEDER", JVDataStructColumns.BR);
                }
            }
        }

        [Test]
        public void JV_BR_BREEDERDataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_BR_BREEDERDataBridge("BR22023082800001400辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                             Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300049134500000044030000025000026000029000026000025000186000001484921000001178316000760000768000729000734000725005824\r\n",
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
        public void JV_BR_BREEDERDataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_BR_BREEDERDataBridge("BR22023082800001400辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                             Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300049134500000044030000025000026000029000026000025000186000001484921000001178316000760000768000729000734000725005824\r\n",
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
                    cmd.AssertRecords("NL_BR_BREEDER", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "BR" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230828" },
                            { "BreederCode", "00001400" },
                            { "BreederName_Co", "辻　牧場" },
                            { "BreederName", "辻　牧場" },
                            { "BreederNameKana", "ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ" },
                            { "BreederNameEng", "Tsuji Bokujo" },
                            { "Address", "浦河郡" },
                            { "HonRuikei0SetYear", "2023" },
                            { "HonRuikei0HonSyokinTotal", "0004913450" },
                            { "HonRuikei0FukaSyokin", "0000044030" },
                            { "HonRuikei0ChakuKaisu0", "000025" },
                            { "HonRuikei0ChakuKaisu1", "000026" },
                            { "HonRuikei0ChakuKaisu2", "000029" },
                            { "HonRuikei0ChakuKaisu3", "000026" },
                            { "HonRuikei0ChakuKaisu4", "000025" },
                            { "HonRuikei0ChakuKaisu5", "000186" },
                            { "HonRuikei1SetYear", "0000" },
                            { "HonRuikei1HonSyokinTotal", "0148492100" },
                            { "HonRuikei1FukaSyokin", "0001178316" },
                            { "HonRuikei1ChakuKaisu0", "000760" },
                            { "HonRuikei1ChakuKaisu1", "000768" },
                            { "HonRuikei1ChakuKaisu2", "000729" },
                            { "HonRuikei1ChakuKaisu3", "000734" },
                            { "HonRuikei1ChakuKaisu4", "000725" },
                            { "HonRuikei1ChakuKaisu5", "005824" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_BR_BREEDERDataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_BR_BREEDERDataBridge("BR22023082800001400辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                             Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300049134500000044030000025000026000029000026000025000186000001484921000001178316000760000768000729000734000725005824\r\n",
                                                         JVOpenOptions.Normal);
            var dataBridge2 = NewJV_BR_BREEDERDataBridge("BR22023082800001400つじ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　つじ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                             Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300049134500000044030000025000026000029000026000025000186000001484921000001178316000760000768000729000734000725005824\r\n",
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
                    cmd.AssertRecords("NL_BR_BREEDER", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "BR" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230828" },
                            { "BreederCode", "00001400" },
                            { "BreederName_Co", "つじ牧場" },
                            { "BreederName", "つじ牧場" },
                            { "BreederNameKana", "ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ" },
                            { "BreederNameEng", "Tsuji Bokujo" },
                            { "Address", "浦河郡" },
                            { "HonRuikei0SetYear", "2023" },
                            { "HonRuikei0HonSyokinTotal", "0004913450" },
                            { "HonRuikei0FukaSyokin", "0000044030" },
                            { "HonRuikei0ChakuKaisu0", "000025" },
                            { "HonRuikei0ChakuKaisu1", "000026" },
                            { "HonRuikei0ChakuKaisu2", "000029" },
                            { "HonRuikei0ChakuKaisu3", "000026" },
                            { "HonRuikei0ChakuKaisu4", "000025" },
                            { "HonRuikei0ChakuKaisu5", "000186" },
                            { "HonRuikei1SetYear", "0000" },
                            { "HonRuikei1HonSyokinTotal", "0148492100" },
                            { "HonRuikei1FukaSyokin", "0001178316" },
                            { "HonRuikei1ChakuKaisu0", "000760" },
                            { "HonRuikei1ChakuKaisu1", "000768" },
                            { "HonRuikei1ChakuKaisu2", "000729" },
                            { "HonRuikei1ChakuKaisu3", "000734" },
                            { "HonRuikei1ChakuKaisu4", "000725" },
                            { "HonRuikei1ChakuKaisu5", "005824" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_BR_BREEDER_V4802DataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_BR_BREEDER_V4802DataBridge("BR220230807000014辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                           Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300043154500000038350000021000024000028000024000022000168000001478941000001172636000756000766000728000732000722005806\r\n",
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
                    cmd.AssertTableCreation("NL_BR_BREEDER", JVDataStructColumns.BR_V4802);
                }
            }
        }

        [Test]
        public void JV_BR_BREEDER_V4802DataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_BR_BREEDER_V4802DataBridge("BR220230807000014辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                           Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300043154500000038350000021000024000028000024000022000168000001478941000001172636000756000766000728000732000722005806\r\n",
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
        public void JV_BR_BREEDER_V4802DataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_BR_BREEDER_V4802DataBridge("BR220230807000014辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                           Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300043154500000038350000021000024000028000024000022000168000001478941000001172636000756000766000728000732000722005806\r\n",
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
                    cmd.AssertRecords("NL_BR_BREEDER", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "BR" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230807" },
                            { "BreederCode", "00001400" },
                            { "BreederName_Co", "辻　牧場" },
                            { "BreederName", "辻　牧場" },
                            { "BreederNameKana", "ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ" },
                            { "BreederNameEng", "Tsuji Bokujo" },
                            { "Address", "浦河郡" },
                            { "HonRuikei0SetYear", "2023" },
                            { "HonRuikei0HonSyokinTotal", "0004315450" },
                            { "HonRuikei0FukaSyokin", "0000038350" },
                            { "HonRuikei0ChakuKaisu0", "000021" },
                            { "HonRuikei0ChakuKaisu1", "000024" },
                            { "HonRuikei0ChakuKaisu2", "000028" },
                            { "HonRuikei0ChakuKaisu3", "000024" },
                            { "HonRuikei0ChakuKaisu4", "000022" },
                            { "HonRuikei0ChakuKaisu5", "000168" },
                            { "HonRuikei1SetYear", "0000" },
                            { "HonRuikei1HonSyokinTotal", "0147894100" },
                            { "HonRuikei1FukaSyokin", "0001172636" },
                            { "HonRuikei1ChakuKaisu0", "000756" },
                            { "HonRuikei1ChakuKaisu1", "000766" },
                            { "HonRuikei1ChakuKaisu2", "000728" },
                            { "HonRuikei1ChakuKaisu3", "000732" },
                            { "HonRuikei1ChakuKaisu4", "000722" },
                            { "HonRuikei1ChakuKaisu5", "005806" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_BR_BREEDER_V4802DataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_BR_BREEDER_V4802DataBridge("BR220230807000014辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                           Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300043154500000038350000021000024000028000024000022000168000001478941000001172636000756000766000728000732000722005806\r\n",
                                                               JVOpenOptions.Normal);
            var dataBridge2 = NewJV_BR_BREEDER_V4802DataBridge("BR220230807000014つじ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　つじ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                           Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300043154500000038350000021000024000028000024000022000168000001478941000001172636000756000766000728000732000722005806\r\n",
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
                    cmd.AssertRecords("NL_BR_BREEDER", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "BR" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230807" },
                            { "BreederCode", "00001400" },
                            { "BreederName_Co", "つじ牧場" },
                            { "BreederName", "つじ牧場" },
                            { "BreederNameKana", "ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ" },
                            { "BreederNameEng", "Tsuji Bokujo" },
                            { "Address", "浦河郡" },
                            { "HonRuikei0SetYear", "2023" },
                            { "HonRuikei0HonSyokinTotal", "0004315450" },
                            { "HonRuikei0FukaSyokin", "0000038350" },
                            { "HonRuikei0ChakuKaisu0", "000021" },
                            { "HonRuikei0ChakuKaisu1", "000024" },
                            { "HonRuikei0ChakuKaisu2", "000028" },
                            { "HonRuikei0ChakuKaisu3", "000024" },
                            { "HonRuikei0ChakuKaisu4", "000022" },
                            { "HonRuikei0ChakuKaisu5", "000168" },
                            { "HonRuikei1SetYear", "0000" },
                            { "HonRuikei1HonSyokinTotal", "0147894100" },
                            { "HonRuikei1FukaSyokin", "0001172636" },
                            { "HonRuikei1ChakuKaisu0", "000756" },
                            { "HonRuikei1ChakuKaisu1", "000766" },
                            { "HonRuikei1ChakuKaisu2", "000728" },
                            { "HonRuikei1ChakuKaisu3", "000732" },
                            { "HonRuikei1ChakuKaisu4", "000722" },
                            { "HonRuikei1ChakuKaisu5", "005806" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_BR_BREEDER_V4802DataBridge_BuildUpInsertCommand_can_create_an_compatible_insert_command()
        {
            // Arrange
            var dataBridge1 = NewJV_BR_BREEDER_V4802DataBridge("BR220230807000032木戸口牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　木戸口牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ｷﾄﾞｸﾞﾁ ﾎﾞｸｼﾞﾖｳ                                                        Kidoguchi Bokujo                                                                                                                                                        浦河郡　　　　　　　202300000830000000000000000000000000000003000002000000000013000000104947000000060372000055000050000088000083000073000772\r\n",
                                                               JVOpenOptions.Normal);
            var dataBridge2 = NewJV_BR_BREEDERDataBridge("BR22023082800001400辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　辻　牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ                                                             Tsuji Bokujo                                                                                                                                                            浦河郡　　　　　　　202300049134500000044030000025000026000029000026000025000186000001484921000001178316000760000768000729000734000725005824\r\n",
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
                    cmd.AssertRecords("NL_BR_BREEDER", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "BR" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230807" },
                            { "BreederCode", "00003200" },
                            { "BreederName_Co", "木戸口牧場" },
                            { "BreederName", "木戸口牧場" },
                            { "BreederNameKana", "ｷﾄﾞｸﾞﾁ ﾎﾞｸｼﾞﾖｳ" },
                            { "BreederNameEng", "Kidoguchi Bokujo" },
                            { "Address", "浦河郡" },
                            { "HonRuikei0SetYear", "2023" },
                            { "HonRuikei0HonSyokinTotal", "0000083000" },
                            { "HonRuikei0FukaSyokin", "0000000000" },
                            { "HonRuikei0ChakuKaisu0", "000000" },
                            { "HonRuikei0ChakuKaisu1", "000000" },
                            { "HonRuikei0ChakuKaisu2", "000003" },
                            { "HonRuikei0ChakuKaisu3", "000002" },
                            { "HonRuikei0ChakuKaisu4", "000000" },
                            { "HonRuikei0ChakuKaisu5", "000013" },
                            { "HonRuikei1SetYear", "0000" },
                            { "HonRuikei1HonSyokinTotal", "0010494700" },
                            { "HonRuikei1FukaSyokin", "0000060372" },
                            { "HonRuikei1ChakuKaisu0", "000055" },
                            { "HonRuikei1ChakuKaisu1", "000050" },
                            { "HonRuikei1ChakuKaisu2", "000088" },
                            { "HonRuikei1ChakuKaisu3", "000083" },
                            { "HonRuikei1ChakuKaisu4", "000073" },
                            { "HonRuikei1ChakuKaisu5", "000772" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "BR" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230828" },
                            { "BreederCode", "00001400" },
                            { "BreederName_Co", "辻　牧場" },
                            { "BreederName", "辻　牧場" },
                            { "BreederNameKana", "ﾂｼﾞ ﾎﾞｸｼﾞﾖｳ" },
                            { "BreederNameEng", "Tsuji Bokujo" },
                            { "Address", "浦河郡" },
                            { "HonRuikei0SetYear", "2023" },
                            { "HonRuikei0HonSyokinTotal", "0004913450" },
                            { "HonRuikei0FukaSyokin", "0000044030" },
                            { "HonRuikei0ChakuKaisu0", "000025" },
                            { "HonRuikei0ChakuKaisu1", "000026" },
                            { "HonRuikei0ChakuKaisu2", "000029" },
                            { "HonRuikei0ChakuKaisu3", "000026" },
                            { "HonRuikei0ChakuKaisu4", "000025" },
                            { "HonRuikei0ChakuKaisu5", "000186" },
                            { "HonRuikei1SetYear", "0000" },
                            { "HonRuikei1HonSyokinTotal", "0148492100" },
                            { "HonRuikei1FukaSyokin", "0001178316" },
                            { "HonRuikei1ChakuKaisu0", "000760" },
                            { "HonRuikei1ChakuKaisu1", "000768" },
                            { "HonRuikei1ChakuKaisu2", "000729" },
                            { "HonRuikei1ChakuKaisu3", "000734" },
                            { "HonRuikei1ChakuKaisu4", "000725" },
                            { "HonRuikei1ChakuKaisu5", "005824" }
                        },
                    });
                }
            }
        }

        private static JV_BR_BREEDERDataBridge NewJV_BR_BREEDERDataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_BR_BREEDER();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_BR_BREEDERDataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }

        private static JV_BR_BREEDER_V4802DataBridge NewJV_BR_BREEDER_V4802DataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_BR_BREEDER_V4802();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_BR_BREEDER_V4802DataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }
    }
}