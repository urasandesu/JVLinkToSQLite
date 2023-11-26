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
using System.Linq;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.Operators;

namespace Test.Urasandesu.JVLinkToSQLite.Operators
{
    [TestFixture]
    public class JVOpenResultReaderTest
    {
        [Test]
        public void It_can_enumerate()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();
            var readRsltsList = new List<List<Func<JVReadResult>>>();
            {
                var readRslts = new List<Func<JVReadResult>>
                {
                    () => JVReadResult.New(1, "1"),
                    () => JVReadResult.New(1, "2"),
                };
                readRsltsList.Add(readRslts);
            }
            {
                var readRslts = new List<Func<JVReadResult>>
                {
                    () => JVReadResult.New(1, "3"),
                    () => JVReadResult.New(1, "4"),
                };
                readRsltsList.Add(readRslts);
            }
            var jvLinkSrv = new JVLinkServiceMock(readRsltsList);
            var openRslt = new JVOpenResult();
            var jvdfSkippabilityHandler = Substitute.For<JVDataFileSkippabilityHandler>(listener, default(SQLiteCommand), default(JVRecordSpec[]));
            var reader = new JVOpenResultReader(listener, jvLinkSrv, openRslt, jvdfSkippabilityHandler);

            // Act
            var results = reader.ToArray();

            // Assert
            var expecteds = readRsltsList.SelectMany(xx => xx.Select(x => x())).ToList();
            expecteds.Insert(2, JVReadResult.New(-1));
            expecteds.Add(JVReadResult.New(-1));
            Assert.That(results, Is.EqualTo(expecteds));
        }

        [Test]
        public void It_should_skip_by_handler_status()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();
            var readRsltsList = new List<List<Func<JVReadResult>>>();
            {
                var readRslts = new List<Func<JVReadResult>>
                {
                    () => JVReadResult.New(1, "1", "skip"),
                    () => JVReadResult.New(1, "2"),
                };
                readRsltsList.Add(readRslts);
            }
            {
                var readRslts = new List<Func<JVReadResult>>
                {
                    () => JVReadResult.New(1, "3"),
                    () => JVReadResult.New(1, "4"),
                };
                readRsltsList.Add(readRslts);
            }
            var jvLinkSrv = new JVLinkServiceMock(readRsltsList);
            var openRslt = new JVOpenResult();
            var jvdfSkippabilityHandler = Substitute.For<JVDataFileSkippabilityHandler>(listener, default(SQLiteCommand), default(JVRecordSpec[]));
            jvdfSkippabilityHandler.CanSkip(readRsltsList[0][0]()).Returns(true);
            var reader = new JVOpenResultReader(listener, jvLinkSrv, openRslt, jvdfSkippabilityHandler);

            // Act
            var results = reader.ToArray();

            // Assert
            var expecteds = readRsltsList.Skip(1).SelectMany(xx => xx.Select(x => x())).ToList();
            {
                var expected = readRsltsList[0][0]();
                expected.SetReturnCode(-1);
                expecteds.Insert(0, expected);
            }
            expecteds.Add(JVReadResult.New(-1));
            Assert.That(results, Is.EqualTo(expecteds));
        }

        [Test]
        public void It_should_throw_JVLinkException_if_JVLink_returns_other_error()
        {
            // Arrange
            var listener = Substitute.For<IJVServiceOperationListener>();
            var readRsltsList = new List<List<Func<JVReadResult>>>();
            {
                var readRslts = new List<Func<JVReadResult>>
                {
                    () => JVReadResult.New(1, "1"),
                    () => JVReadResult.New(1, "2"),
                };
                readRsltsList.Add(readRslts);
            }
            {
                var readRslts = new List<Func<JVReadResult>>
                {
                    () => JVReadResult.New(1, "3"),
                    () => JVReadResult.New(-201, "4"),
                };
                readRsltsList.Add(readRslts);
            }
            var jvLinkSrv = new JVLinkServiceMock(readRsltsList);
            var openRslt = new JVOpenResult();
            var jvdfSkippabilityHandler = Substitute.For<JVDataFileSkippabilityHandler>(listener, default(SQLiteCommand), default(JVRecordSpec[]));
            var reader = new JVOpenResultReader(listener, jvLinkSrv, openRslt, jvdfSkippabilityHandler);

            // Act, Assert
            var ex = Assert.Throws<JVLinkException>(() => reader.ToArray());
            Assert.That(ex.JVLinkResult.Interpretation.Failed, Is.True);
        }

        private class JVLinkServiceMock : IJVLinkService
        {
            private readonly Queue<Queue<Func<JVReadResult>>> _readRsltsQueue;

            public JVLinkServiceMock(List<List<Func<JVReadResult>>> readRsltsList)
            {
                _readRsltsQueue = new Queue<Queue<Func<JVReadResult>>>();
                foreach (var readRslts in readRsltsList)
                {
                    var readRsltQueue = new Queue<Func<JVReadResult>>();
                    foreach (var readRslt in readRslts)
                    {
                        readRsltQueue.Enqueue(readRslt);
                    }
                    _readRsltsQueue.Enqueue(readRsltQueue);
                }
            }

            public void JVCancel(JVOpenResult openRslt)
            {
                throw new NotImplementedException();
            }

            public JVOpenResult JVOpen(JVDataSpec dataSpec, JVDataSpecKey dataSpecKey, JVOpenOptions openOption)
            {
                throw new NotImplementedException();
            }

            public JVReadResult JVRead(JVOpenResult openRslt)
            {
                if (_readRsltsQueue.Count == 0)
                {
                    var readRslt = new JVReadResult();
                    readRslt.SetReturnCode(0);
                    return readRslt;
                }
                else if (_readRsltsQueue.Peek().Count == 0)
                {
                    _readRsltsQueue.Dequeue();
                    var readRslt = new JVReadResult();
                    readRslt.SetReturnCode(-1);
                    return readRslt;
                }
                else
                {
                    return _readRsltsQueue.Peek().Dequeue()();
                }
            }

            public JVReadResult JVGets(JVOpenResult openRslt)
            {
                return JVRead(openRslt);
            }

            public JVOpenResult JVRTOpen(JVDataSpec dataSpec, JVDataSpecKey dataSpecKey)
            {
                throw new NotImplementedException();
            }

            public JVSetUIPropertiesResult JVSetUIProperties()
            {
                throw new NotImplementedException();
            }

            public void JVSkip(JVOpenResult openRslt)
            {
                if (0 < _readRsltsQueue.Count)
                {
                    _readRsltsQueue.Dequeue();
                }
            }

            public JVStatusResult JVStatus(JVOpenResult openRslt)
            {
                throw new NotImplementedException();
            }

            public JVWatchEventResult JVWatchEvent(JVWatchEventDispatcher dispatcher)
            {
                throw new NotImplementedException();
            }

            public JVWatchEventCloseResult JVWatchEventClose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
