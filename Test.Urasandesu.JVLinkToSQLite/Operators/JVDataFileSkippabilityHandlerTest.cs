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
using System.Data.SQLite;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Operators;

namespace Test.Urasandesu.JVLinkToSQLite.Operators
{
    [TestFixture]
    public class JVDataFileSkippabilityHandlerTest
    {
        [Test]
        public void Initialize_should_create_table_if_table_does_not_exist()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);

                    // Act
                    handler.Initialize(DateTime.Now, new int[1]);

                    // Assert
                    var expectedCols = new[]
                    {
                        new BridgeColumn("FileName", typeof(string), false, true, null, null),
                        new BridgeColumn("DataSpec", typeof(string), false, false, null, null),
                        new BridgeColumn("RecordSpec", typeof(string), false, false, null, null),
                        new BridgeColumn("CategorySpec", typeof(string), false, false, null, null),
                        new BridgeColumn("SaveSpec", typeof(string), false, false, null, null),
                        new BridgeColumn("DatetimeKey", typeof(string), false, false, null, null),
                        new BridgeColumn("PublishedDateTime", typeof(DateTime), false, false, null, null),
                        new BridgeColumn("ProcessedAt", typeof(DateTime), false, false, null, null),
                        new BridgeColumn("ExpirationDate", typeof(DateTime), false, false, null, null),
                    };
                    cmd.AssertTableCreation("SY_PROC_FILES", expectedCols);
                }
            }
        }

        [Test]
        public void Initialize_should_delete_expired_records()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);
                    SetupTestSY_PROC_FILES(cmd, testNow, testNow.AddDays(-1), testNow.AddDays(1));

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);

                    // Act
                    handler.Initialize(testNow, new int[1]);

                    // Assert
                    cmd.AssertRecords("SY_PROC_FILES", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HYFW2023090520230905150602.jvd" },
                            { "DataSpec", "DataSpec2" },
                            { "RecordSpec", "RecordSpec2" },
                            { "CategorySpec", "CategorySpec2" },
                            { "SaveSpec", "SaveSpec2" },
                            { "DatetimeKey", "DatetimeKey2" },
                            { "PublishedDateTime", testNow },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", testNow.AddDays(1) }
                        },
                    });
                }
            }
        }

        [Test]
        public void Initialize_should_do_nothing_if_already_executed()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);
                    var initialized = new int[1];
                    handler.Initialize(testNow, initialized);

                    SetupTestSY_PROC_FILES(cmd, testNow, testNow.AddDays(-1), testNow.AddDays(1));


                    // Act
                    handler.Initialize(testNow, initialized);

                    // Assert
                    Assert.That(initialized, Is.EqualTo(new[] { 1 }));
                    cmd.AssertRecords("SY_PROC_FILES", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HYFW2023090520230905150601.jvd" },
                            { "DataSpec", "DataSpec1" },
                            { "RecordSpec", "RecordSpec1" },
                            { "CategorySpec", "CategorySpec1" },
                            { "SaveSpec", "SaveSpec1" },
                            { "DatetimeKey", "DatetimeKey1" },
                            { "PublishedDateTime", testNow },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", testNow.AddDays(-1) }
                        },
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HYFW2023090520230905150602.jvd" },
                            { "DataSpec", "DataSpec2" },
                            { "RecordSpec", "RecordSpec2" },
                            { "CategorySpec", "CategorySpec2" },
                            { "SaveSpec", "SaveSpec2" },
                            { "DatetimeKey", "DatetimeKey2" },
                            { "PublishedDateTime", testNow },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", testNow.AddDays(1) }
                        },
                    });
                }
            }
        }

        [Test]
        public void CanSkip_should_return_true_if_record_exists()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);
                    SetupTestSY_PROC_FILES(cmd, testNow, testNow.AddDays(-1), testNow.AddDays(1));

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);

                    // Act
                    var result = handler.CanSkip(JVReadResult.New(1, fileName: "HYFW2023090520230905150601.jvd"));

                    // Assert
                    Assert.That(result, Is.True);
                }
            }
        }

        [Test]
        public void CanSkip_should_return_false_if_record_does_not_exist()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);
                    SetupTestSY_PROC_FILES(cmd, testNow, testNow.AddDays(-1), testNow.AddDays(1));

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);

                    // Act
                    var result = handler.CanSkip(JVReadResult.New(1, fileName: "HYFW2023090520230905150603.jvd"));

                    // Assert
                    Assert.That(result, Is.False);
                }
            }
        }

        [Test]
        public void CanSkip_should_return_true_if_excluded_RecordSpecs_are_passed()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, new[] { JVRecordSpec.HY });

                    // Act
                    var openRslt = JVOpenResult.New(1, JVDataSpec.HOYU, JVOpenOptions.Normal);
                    var result = handler.CanSkip(JVReadResult.New(1, openRslt, fileName: "HYFW2023090520230905150601.jvd", buff: "HY..."));

                    // Assert
                    Assert.That(result, Is.True);
                }
            }
        }

        [Test]
        public void RegisterOrUpdate_should_do_nothing_if_DataSpec_is_realtime()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();
            var openRslt = new JVOpenResult();
            openRslt.SetDataSpec(JVDataSpec._0B12);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);
                    SetupTestSY_PROC_FILES(cmd, testNow, testNow.AddDays(1));

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);

                    // Act
                    handler.RegisterOrUpdate(testNow, JVReadResult.New(1, openRslt, fileName: "hogehoge.fuga"));

                    // Assert
                    cmd.AssertRecords("SY_PROC_FILES", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HYFW2023090520230905150601.jvd" },
                            { "DataSpec", "DataSpec1" },
                            { "RecordSpec", "RecordSpec1" },
                            { "CategorySpec", "CategorySpec1" },
                            { "SaveSpec", "SaveSpec1" },
                            { "DatetimeKey", "DatetimeKey1" },
                            { "PublishedDateTime", testNow },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", testNow.AddDays(1) }
                        },
                    });
                }
            }
        }

        [Test]
        public void RegisterOrUpdate_should_register_record_if_DataSpec_is_normal()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();
            var openRslt = new JVOpenResult();
            openRslt.SetDataSpec(JVDataSpec.HOYU);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);
                    SetupTestSY_PROC_FILES(cmd, testNow, testNow.AddDays(1));

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);

                    // Act
                    handler.RegisterOrUpdate(testNow, JVReadResult.New(1, openRslt, fileName: "HYFW2023090520230905150602.jvd"));

                    // Assert
                    cmd.AssertRecords("SY_PROC_FILES", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HYFW2023090520230905150601.jvd" },
                            { "DataSpec", "DataSpec1" },
                            { "RecordSpec", "RecordSpec1" },
                            { "CategorySpec", "CategorySpec1" },
                            { "SaveSpec", "SaveSpec1" },
                            { "DatetimeKey", "DatetimeKey1" },
                            { "PublishedDateTime", testNow },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", testNow.AddDays(1) }
                        },
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HYFW2023090520230905150602.jvd" },
                            { "DataSpec", "HOYU" },
                            { "RecordSpec", "HY" },
                            { "CategorySpec", "F" },
                            { "SaveSpec", "W" },
                            { "DatetimeKey", "20230905" },
                            { "PublishedDateTime", new DateTime(2023, 09, 05, 15, 06, 02) },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", testNow.AddDays(365 * 2) }
                        },
                    });
                }
            }
        }

        [Test]
        public void RegisterOrUpdate_should_update_record_if_record_exists()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();
            var openRslt = new JVOpenResult();
            openRslt.SetDataSpec(JVDataSpec.HOYU);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);
                    SetupTestSY_PROC_FILES(cmd, testNow, testNow.AddDays(1));

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);

                    // Act
                    handler.RegisterOrUpdate(testNow, JVReadResult.New(1, openRslt, fileName: "HYFW2023090520230905150601.jvd"));

                    // Assert
                    cmd.AssertRecords("SY_PROC_FILES", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HYFW2023090520230905150601.jvd" },
                            { "DataSpec", "HOYU" },
                            { "RecordSpec", "HY" },
                            { "CategorySpec", "F" },
                            { "SaveSpec", "W" },
                            { "DatetimeKey", "20230905" },
                            { "PublishedDateTime", new DateTime(2023, 09, 05, 15, 06, 01) },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", testNow.AddDays(365 * 2) }
                        },
                    });
                }
            }
        }

        [Test]
        public void RegisterOrUpdate_should_update_record_if_ProvidedDuration_is_TimeSpan_MaxValue()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();
            var openRslt = new JVOpenResult();
            openRslt.SetDataSpec(JVDataSpec.HOSE);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var testNow = new DateTime(2023, 10, 08);
                    SetupTestSY_PROC_FILES(cmd, testNow, testNow.AddDays(1));

                    var handler = new JVDataFileSkippabilityHandler(listener, cmd, null);

                    // Act
                    handler.RegisterOrUpdate(testNow, JVReadResult.New(1, openRslt, fileName: "HSFW2023090520230905150601.jvd"));

                    // Assert
                    cmd.AssertRecords("SY_PROC_FILES", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HYFW2023090520230905150601.jvd" },
                            { "DataSpec", "DataSpec1" },
                            { "RecordSpec", "RecordSpec1" },
                            { "CategorySpec", "CategorySpec1" },
                            { "SaveSpec", "SaveSpec1" },
                            { "DatetimeKey", "DatetimeKey1" },
                            { "PublishedDateTime", testNow },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", testNow.AddDays(1) }
                        },
                        new Dictionary<string, object>()
                        {
                            { "FileName", "HSFW2023090520230905150601.jvd" },
                            { "DataSpec", "HOSE" },
                            { "RecordSpec", "HS" },
                            { "CategorySpec", "F" },
                            { "SaveSpec", "W" },
                            { "DatetimeKey", "20230905" },
                            { "PublishedDateTime", new DateTime(2023, 09, 05, 15, 06, 01) },
                            { "ProcessedAt", testNow },
                            { "ExpirationDate", DBNull.Value }
                        },
                    });
                }
            }
        }


        private static void SetupTestSY_PROC_FILES(SQLiteCommand cmd, DateTime rest, params DateTime[] expirationDateVariation)
        {
            cmd.CommandText = JVDataFileSkippabilityHandler.CreateTableSql;
            cmd.ExecuteNonQuery();

            cmd.CommandText = JVDataFileSkippabilityHandler.InsertRecordSql;
            var i = 0;
            foreach (var expirationDate in expirationDateVariation)
            {
                i++;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@FileName", $"HYFW202309052023090515060{i}.jvd");
                cmd.Parameters.AddWithValue("@DataSpec", $"DataSpec{i}");
                cmd.Parameters.AddWithValue("@RecordSpec", $"RecordSpec{i}");
                cmd.Parameters.AddWithValue("@CategorySpec", $"CategorySpec{i}");
                cmd.Parameters.AddWithValue("@SaveSpec", $"SaveSpec{i}");
                cmd.Parameters.AddWithValue("@DatetimeKey", $"DatetimeKey{i}");
                cmd.Parameters.AddWithValue("@PublishedDateTime", rest);
                cmd.Parameters.AddWithValue("@ProcessedAt", rest);
                cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
