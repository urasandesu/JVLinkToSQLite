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

using DryIoc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Operators;
using Urasandesu.JVLinkToSQLite.Settings;

namespace Test.Urasandesu.JVLinkToSQLite.Operators
{
    [TestFixture]
    public class JVDataToSQLiteOperatorTest
    {
        [Test]
        public void InsertOrUpdateAll_should_execute_all_sql()
        {
            // Arrange
            var container = new Container();
            RegisterDummies(container, new[]
            {
                TestDatum(JVReadResult.New(1, "1"), new DataBridgeMock("Test1"), (m, dataBridgeFactory) => m.Returns(dataBridgeFactory), readRslt => readRslt),
                TestDatum(JVReadResult.New(1, "2"), new DataBridgeMock("Test2"), (m, dataBridgeFactory) => m.Returns(dataBridgeFactory), readRslt => readRslt),
                TestDatum(JVReadResult.New(-1), null, (m, dataBridgeFactory) => m.Throws(new InvalidOperationException()), readRslt => readRslt),
            });
            using (var opr = new JVDataToSQLiteOperatorMock(container))
            {
                // Act
                var oprRslt = opr.InsertOrUpdateAll();

                // Assert
                Assert.That(oprRslt.Interpretation.Succeeded, Is.True);
                opr.Command.AssertRecords("Test1", new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "Id", 1 },
                        { "Name", "Test1" },
                    }
                });
                opr.Command.AssertRecords("Test2", new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "Id", 1 },
                        { "Name", "Test2" },
                    }
                });
            }
        }

        [Test]
        public void InsertOrUpdateAll_should_not_create_duplicate_table()
        {
            // Arrange
            var container = new Container();
            RegisterDummies(container, new[]
            {
                TestDatum(JVReadResult.New(1, "1"), new DataBridgeMock("Test1"), (m, dataBridgeFactory) => m.Returns(dataBridgeFactory), readRslt => readRslt),
                TestDatum(JVReadResult.New(1, "2"), new DataBridgeMock("Test1"), (m, dataBridgeFactory) => m.Returns(dataBridgeFactory), readRslt => readRslt),
                TestDatum(JVReadResult.New(-1), null, (m, dataBridgeFactory) => m.Throws(new InvalidOperationException()), readRslt => readRslt),
            });
            using (var opr = new JVDataToSQLiteOperatorMock(container))
            {
                // Act
                var oprRslt = opr.InsertOrUpdateAll();

                // Assert
                Assert.That(oprRslt.Interpretation.Succeeded, Is.True);
                opr.Command.AssertRecords("Test1", new List<Dictionary<string, object>>()
                {
                    new Dictionary<string, object>()
                    {
                        { "Id", 1 },
                        { "Name", "Test1" },
                    },
                    new Dictionary<string, object>()
                    {
                        { "Id", 2 },
                        { "Name", "Test1" },
                    },
                });
            }
        }

        [Test]
        public void InsertOrUpdateAll_should_rollback_when_JVLinkException_is_occurred()
        {
            // Arrange
            var container = new Container();
            RegisterDummies(container, new[]
            {
                TestDatum(JVReadResult.New(1, "1"), new DataBridgeMock("Test1"), (m, dataBridgeFactory) => m.Returns(dataBridgeFactory), readRslt => readRslt),
                TestDatum(JVReadResult.New(-202), null, (m, dataBridgeFactory) => m.Returns(dataBridgeFactory), readRslt => throw new JVLinkException(readRslt)),
                TestDatum(JVReadResult.New(-1), null, (m, dataBridgeFactory) => m.Throws(new InvalidOperationException()), readRslt => readRslt),
            });
            using (var opr = new JVDataToSQLiteOperatorMock(container))
            {
                // Act
                var oprRslt = opr.InsertOrUpdateAll();

                // Assert
                Assert.That(oprRslt.Interpretation.Failed, Is.True);
                opr.Command.AssertTableNotExists("Test1");
            }
        }

        [Test]
        public void InsertOrUpdateAll_should_rollback_and_rethrow_when_another_Exception_is_occurred()
        {
            // Arrange
            var container = new Container();
            RegisterDummies(container, new[]
            {
                TestDatum(JVReadResult.New(1, "1"), new DataBridgeMock("Test1"), (m, dataBridgeFactory) => m.Returns(dataBridgeFactory), readRslt => readRslt),
                TestDatum(JVReadResult.New(1, "2"), new DataBridgeMock("Test2"), (m, dataBridgeFactory) => m.Returns(dataBridgeFactory), readRslt => readRslt),
                TestDatum(JVReadResult.New(-1), null, (m, dataBridgeFactory) => m.Throws(new InvalidOperationException()), readRslt => readRslt),
            });
            using (var opr = new JVDataToSQLiteOperatorMock(container)
            {
                CreateTableProvider = (commandCache, dataBridge) =>
                {
                    var command = commandCache.Get(new SQLitePreparedCommandKey(1, dataBridge.TableName), () => $"hoge");
                    command.ExecuteNonQuery();
                },
            })
            {
                // Act, Assert
                Assert.Throws<SQLiteException>(() => opr.InsertOrUpdateAll());
            }
        }

        private class JVOpenResultReaderFactoryMock : JVOpenResultReader.Factory
        {
            private readonly List<Func<JVReadResult>> _readRslts;

            public JVOpenResultReaderFactoryMock(List<Func<JVReadResult>> readRslts) :
                base(null, null)
            {
                _readRslts = readRslts;
            }

            public override JVOpenResultReader New(JVOpenResult openRslt, JVDataFileSkippabilityHandler jvdfSkippabilityHandler)
            {
                return new JVOpenResultReaderMock(_readRslts, openRslt, jvdfSkippabilityHandler);
            }
        }

        private class JVOpenResultReaderMock : JVOpenResultReader
        {
            private List<Func<JVReadResult>> _readRslts;

            public JVOpenResultReaderMock(JVOpenResult openRslt,
                                          JVDataFileSkippabilityHandler jvdfSkippabilityHandler) :
                base(null, null, openRslt, jvdfSkippabilityHandler)
            {
            }

            public JVOpenResultReaderMock(List<Func<JVReadResult>> readRslts, JVOpenResult openRslt, JVDataFileSkippabilityHandler jvdfSkippabilityHandler) :
                base(null, null, openRslt, jvdfSkippabilityHandler)
            {
                _readRslts = readRslts;
            }

            public override IEnumerator<JVReadResult> GetEnumerator()
            {
                return _readRslts.Select(_ => _()).GetEnumerator();
            }
        }

        private class JVDataToSQLiteOperatorMock : JVDataToSQLiteOperator
        {
            public JVDataToSQLiteOperatorMock(IResolver resolver) :
                base(resolver, null, new SQLiteConnectionInfo(":memory:"), null, null)
            {
            }

            public Action<SQLitePreparedCommandCache, DataBridge> CreateTableProvider { get; set; }
            protected override void CreateTable(SQLitePreparedCommandCache commandCache, DataBridge dataBridge)
            {
                var provider = CreateTableProvider;
                if (provider == null)
                {
                    var command = commandCache.Get(new SQLitePreparedCommandKey(1, dataBridge.TableName),
                                                   () => $"create table {dataBridge.TableName} (Id integer primary key autoincrement, Name text)");
                    command.ExecuteNonQuery();
                }
                else
                {
                    provider(commandCache, dataBridge);
                }
            }

            public Action<SQLitePreparedCommandCache, DataBridge> InsertProvider { get; set; }
            protected override void Insert(SQLitePreparedCommandCache commandCache, DataBridge dataBridge)
            {
                var provider = InsertProvider;
                if (provider == null)
                {
                    var command = commandCache.Get(new SQLitePreparedCommandKey(2, dataBridge.TableName),
                                                   () => $"insert into {dataBridge.TableName} (Name) values ('{dataBridge.TableName}')");
                    command.ExecuteNonQuery();
                }
                else
                {
                    provider(commandCache, dataBridge);
                }
            }
        }

        private class DataBridgeMock : DataBridge
        {
            public DataBridgeMock(string tableName)
            {
                TableName = tableName;
            }
        }

        private static Tuple<JVReadResult, DataBridge, Action<DataBridgeFactory, DataBridgeFactory>, Func<JVReadResult, JVReadResult>> TestDatum(
            JVReadResult readRslt, DataBridge dataBridge, Action<DataBridgeFactory, DataBridgeFactory> dataBridgeFactorySetup, Func<JVReadResult, JVReadResult> readRsltGetter)
        {
            return Tuple.Create(readRslt, dataBridge, dataBridgeFactorySetup, readRsltGetter);
        }

        private static void RegisterDummies(
            IContainer container,
            Tuple<JVReadResult, DataBridge, Action<DataBridgeFactory, DataBridgeFactory>, Func<JVReadResult, JVReadResult>>[] testData)
        {
            var listener = Substitute.For<IJVServiceOperationListener>();
            var dataBridgeFactoryFactory = Substitute.For<DataBridgeFactory.Factory>(listener);
            var readRslts = new List<Func<JVReadResult>>();
            foreach (var testDatum in testData)
            {
                var (readRslt, dataBridge, dataBridgeFactorySetup, readRsltGetter) = testDatum;
                var dataBridgeFactory = Substitute.For<DataBridgeFactory>(listener, readRslt);
                dataBridgeFactory.NewDataBridge().Returns(dataBridge);
                dataBridgeFactorySetup(dataBridgeFactoryFactory.New(readRslt), dataBridgeFactory);
                readRslts.Add(() => readRsltGetter(readRslt));
            }
            container.RegisterInstance<DataBridgeFactory.Factory>(dataBridgeFactoryFactory);
            container.RegisterInstance<JVOpenResultReader.Factory>(new JVOpenResultReaderFactoryMock(readRslts));
            container.RegisterInstance<JVDataFileSkippabilityHandler.Factory>(Substitute.For<JVDataFileSkippabilityHandler.Factory>(listener));
        }
    }
}
