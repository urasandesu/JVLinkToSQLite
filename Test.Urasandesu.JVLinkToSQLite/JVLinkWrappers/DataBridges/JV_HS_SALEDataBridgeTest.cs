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
    public class JV_HS_SALEDataBridgeTest
    {
        [Test]
        public void JV_HS_SALEDataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_HS_SALEDataBridge("HS1202308282022100006112000254312200667902022001002日高軽種馬農業協同組合　　　　　　　　　北海道サマーセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202308212023082510002750000\r\n",
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
                    cmd.AssertTableCreation("NL_HS_SALE", JVDataStructColumns.HS);
                }
            }
        }

        [Test]
        public void JV_HS_SALEDataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_HS_SALEDataBridge("HS1202308282022100006112000254312200667902022001002日高軽種馬農業協同組合　　　　　　　　　北海道サマーセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202308212023082510002750000\r\n",
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
        public void JV_HS_SALEDataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_HS_SALEDataBridge("HS1202308282022100006112000254312200667902022001002日高軽種馬農業協同組合　　　　　　　　　北海道サマーセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202308212023082510002750000\r\n",
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
                    cmd.AssertRecords("NL_HS_SALE", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HS" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230828" },
                            { "KettoNum", "2022100006" },
                            { "HansyokuFNum", "1120002543" },
                            { "HansyokuMNum", "1220066790" },
                            { "BirthYear", "2022" },
                            { "SaleCode", "001002" },
                            { "SaleHostName", "日高軽種馬農業協同組合" },
                            { "SaleName", "北海道サマーセール" },
                            { "FromDate", "20230821" },
                            { "ToDate", "20230825" },
                            { "Barei", "1" },
                            { "Price", "0002750000" },
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_HS_SALEDataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_HS_SALEDataBridge("HS1202308282022100006112000254312200667902022001002日高軽種馬農業協同組合　　　　　　　　　北海道サマーセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202308212023082510002750000\r\n",
                                                      JVOpenOptions.Normal);
            var dataBridge2 = NewJV_HS_SALEDataBridge("HS1202308282022100006112000254312200667902022001002日高軽種バ農業協同組合　　　　　　　　　北海道サマーセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202308212023082510002750000\r\n",
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
                    cmd.AssertRecords("NL_HS_SALE", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HS" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230828" },
                            { "KettoNum", "2022100006" },
                            { "HansyokuFNum", "1120002543" },
                            { "HansyokuMNum", "1220066790" },
                            { "BirthYear", "2022" },
                            { "SaleCode", "001002" },
                            { "SaleHostName", "日高軽種バ農業協同組合" },
                            { "SaleName", "北海道サマーセール" },
                            { "FromDate", "20230821" },
                            { "ToDate", "20230825" },
                            { "Barei", "1" },
                            { "Price", "0002750000" },
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_HS_SALE_V4802DataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_HS_SALE_V4802DataBridge("HS120230713202210091211202561122617942022013001一般社団法人日本競走馬協会　　　　　　　セレクトセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202307102023071010016500000\r\n",
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
                    cmd.AssertTableCreation("NL_HS_SALE", JVDataStructColumns.HS_V4802);
                }
            }
        }

        [Test]
        public void JV_HS_SALE_V4802DataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_HS_SALE_V4802DataBridge("HS120230713202210091211202561122617942022013001一般社団法人日本競走馬協会　　　　　　　セレクトセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202307102023071010016500000\r\n",
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
        public void JV_HS_SALE_V4802DataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_HS_SALE_V4802DataBridge("HS120230713202210091211202561122617942022013001一般社団法人日本競走馬協会　　　　　　　セレクトセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202307102023071010016500000\r\n",
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
                    cmd.AssertRecords("NL_HS_SALE", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HS" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230713" },
                            { "KettoNum", "2022100912" },
                            { "HansyokuFNum", "1120002561" },
                            { "HansyokuMNum", "1220061794" },
                            { "BirthYear", "2022" },
                            { "SaleCode", "013001" },
                            { "SaleHostName", "一般社団法人日本競走馬協会" },
                            { "SaleName", "セレクトセール" },
                            { "FromDate", "20230710" },
                            { "ToDate", "20230710" },
                            { "Barei", "1" },
                            { "Price", "0016500000" },
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_HS_SALE_V4802DataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_HS_SALE_V4802DataBridge("HS120230713202210091211202561122617942022013001一般社団法人日本競走馬協会　　　　　　　セレクトセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202307102023071010016500000\r\n",
                                                            JVOpenOptions.Normal);
            var dataBridge2 = NewJV_HS_SALE_V4802DataBridge("HS120230713202210091211202561122617942022013001一般社団法人日本競走馬協会　　　　　　　セレクトセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202307102023071010016500000\r\n",
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
                    cmd.AssertRecords("NL_HS_SALE", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HS" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230713" },
                            { "KettoNum", "2022100912" },
                            { "HansyokuFNum", "1120002561" },
                            { "HansyokuMNum", "1220061794" },
                            { "BirthYear", "2022" },
                            { "SaleCode", "013001" },
                            { "SaleHostName", "一般社団法人日本競走馬協会" },
                            { "SaleName", "セレクトセール" },
                            { "FromDate", "20230710" },
                            { "ToDate", "20230710" },
                            { "Barei", "1" },
                            { "Price", "0016500000" },
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_HS_SALE_V4802DataBridge_BuildUpInsertCommand_can_create_an_compatible_insert_command()
        {
            // Arrange
            var dataBridge1 = NewJV_HS_SALE_V4802DataBridge("HS120230713202210091211202561122617942022013001一般社団法人日本競走馬協会　　　　　　　セレクトセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202307102023071010016500000\r\n",
                                                            JVOpenOptions.Normal);
            var dataBridge2 = NewJV_HS_SALEDataBridge("HS1202308282022100006112000254312200667902022001002日高軽種馬農業協同組合　　　　　　　　　北海道サマーセール　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　202308212023082510002750000\r\n",
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
                    cmd.AssertRecords("NL_HS_SALE", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HS" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230713" },
                            { "KettoNum", "2022100912" },
                            { "HansyokuFNum", "1120002561" },
                            { "HansyokuMNum", "1220061794" },
                            { "BirthYear", "2022" },
                            { "SaleCode", "013001" },
                            { "SaleHostName", "一般社団法人日本競走馬協会" },
                            { "SaleName", "セレクトセール" },
                            { "FromDate", "20230710" },
                            { "ToDate", "20230710" },
                            { "Barei", "1" },
                            { "Price", "0016500000" },
                        },
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "HS" },
                            { "headDataKubun", "1" },
                            { "headMakeDate", "20230828" },
                            { "KettoNum", "2022100006" },
                            { "HansyokuFNum", "1120002543" },
                            { "HansyokuMNum", "1220066790" },
                            { "BirthYear", "2022" },
                            { "SaleCode", "001002" },
                            { "SaleHostName", "日高軽種馬農業協同組合" },
                            { "SaleName", "北海道サマーセール" },
                            { "FromDate", "20230821" },
                            { "ToDate", "20230825" },
                            { "Barei", "1" },
                            { "Price", "0002750000" },
                        },
                   });
                }
            }
        }

        private static JV_HS_SALEDataBridge NewJV_HS_SALEDataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_HS_SALE();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_HS_SALEDataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }

        private static JV_HS_SALE_V4802DataBridge NewJV_HS_SALE_V4802DataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_HS_SALE_V4802();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_HS_SALE_V4802DataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }
    }
}