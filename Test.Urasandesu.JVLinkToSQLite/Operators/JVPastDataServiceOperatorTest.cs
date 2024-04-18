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
using NUnit.Framework;
using System;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.Operators;
using Urasandesu.JVLinkToSQLite.Settings;
using Arg = NSubstitute.Arg;

namespace Test.Urasandesu.JVLinkToSQLite.Operators
{
    [TestFixture]
    public class JVPastDataServiceOperatorTest
    {
        [Test]
        public void Operate_should_return_Failed_when_JVOpen_failed()
        {
            // Arrange
            var container = new Container();
            var listener = Substitute.For<IJVServiceOperationListener>();
            var jvLinkSrv = Substitute.For<IJVLinkService>();
            {
                var openRslt = JVOpenResult.New(-111, JVDataSpec.RACE, JVOpenOptions.Normal);
                jvLinkSrv.JVOpen(Arg.Any<JVDataSpec>(), Arg.Any<JVDataSpecKey>(), Arg.Any<JVOpenOptions>()).Returns(openRslt);
            }
            var connInfo = new SQLiteConnectionInfo(":memory:");
            var dataSpecSetting = new JVDataSpecSetting("RACE");
            var opr = new JVPastDataServiceOperator(container, listener, jvLinkSrv, connInfo, dataSpecSetting, JVOpenOptions.Normal);


            // Act
            var oprRslt = opr.Operate();


            // Assert
            Assert.That(oprRslt.Interpretation.Failed, Is.True);
        }

        [Test]
        public void Operate_should_continue_processing_when_JVOpen_is_empty()
        {
            // Arrange
            var container = new Container();
            var listener = Substitute.For<IJVServiceOperationListener>();
            var jvLinkSrv = Substitute.For<IJVLinkService>();
            {
                var openRslt = JVOpenResult.New(-1, JVDataSpec.RACE, JVOpenOptions.Normal);
                var dataSpecKey = new JVKaisaiDateTimeRangeKey(new DateTime(1986, 01, 01, 00, 00, 00), new DateTime(1986, 05, 02, 18, 00, 00));
                jvLinkSrv.JVOpen(Arg.Any<JVDataSpec>(), dataSpecKey, Arg.Any<JVOpenOptions>()).Returns(openRslt);
            }
            {
                var openRslt = JVOpenResult.New(-111, JVDataSpec.RACE, JVOpenOptions.Normal);
                var dataSpecKey = new JVKaisaiDateTimeRangeKey(new DateTime(1986, 05, 02, 18, 00, 00), new DateTime(1986, 09, 01, 12, 00, 00));
                jvLinkSrv.JVOpen(Arg.Any<JVDataSpec>(), dataSpecKey, Arg.Any<JVOpenOptions>()).Returns(openRslt);
            }
            var connInfo = new SQLiteConnectionInfo(":memory:");
            var dataSpecSetting = new JVDataSpecSetting("RACE");
            var opr = new JVPastDataServiceOperator(container, listener, jvLinkSrv, connInfo, dataSpecSetting, JVOpenOptions.Normal);


            // Act
            var oprRslt = opr.Operate();


            // Assert
            Assert.That(oprRslt.Interpretation.Failed, Is.True);
        }

        [Test]
        public void Operate_should_return_Failed_when_JVStatusTimer_failed()
        {
            // Arrange
            var container = new Container();
            var listener = Substitute.For<IJVServiceOperationListener>();
            var jvLinkSrv = Substitute.For<IJVLinkService>();
            var openRslt = JVOpenResult.New(0, JVDataSpec.RACE, JVOpenOptions.Normal, readCount: 1, downloadCount: 1);
            {
                jvLinkSrv.JVOpen(Arg.Any<JVDataSpec>(), Arg.Any<JVDataSpecKey>(), Arg.Any<JVOpenOptions>()).Returns(openRslt);
            }
            var connInfo = new SQLiteConnectionInfo(":memory:");
            var dataSpecSetting = new JVDataSpecSetting("RACE");
            {
                var timer = Substitute.For<JVStatusTimer>(listener, jvLinkSrv, openRslt);
                timer.StartAndWait().Returns(JVStatusResult.New(-201));
                var factory = Substitute.For<JVStatusTimer.Factory>(listener, jvLinkSrv);
                factory.New(Arg.Any<JVOpenResult>()).Returns(timer);
                container.RegisterInstance<JVStatusTimer.Factory>(factory);
            }
            var opr = new JVPastDataServiceOperator(container, listener, jvLinkSrv, connInfo, dataSpecSetting, JVOpenOptions.Normal);


            // Act
            var oprRslt = opr.Operate();


            // Assert
            Assert.That(oprRslt.Interpretation.Failed, Is.True);
        }

        [Test]
        public void Operate_should_return_Failed_when_JVDataToSQLiteOperator_failed()
        {
            // Arrange
            var container = new Container();
            var listener = Substitute.For<IJVServiceOperationListener>();
            var jvLinkSrv = Substitute.For<IJVLinkService>();
            var openRslt = JVOpenResult.New(0, JVDataSpec.RACE, JVOpenOptions.Normal, readCount: 1);
            {
                jvLinkSrv.JVOpen(Arg.Any<JVDataSpec>(), Arg.Any<JVDataSpecKey>(), Arg.Any<JVOpenOptions>()).Returns(openRslt);
            }
            var connInfo = new SQLiteConnectionInfo(":memory:");
            var dataSpecSetting = new JVDataSpecSetting("RACE");
            {
                var jvDataToSQLiteOpr = Substitute.For<JVDataToSQLiteOperator>(container, listener, connInfo, openRslt, new JVRecordSpec[0]);
                jvDataToSQLiteOpr.InsertOrUpdateAll().Returns(JVLinkServiceOperationResult.From(JVReadResult.New(-201)));
                var factory = Substitute.For<JVDataToSQLiteOperator.Factory>(container, listener);
                factory.New(Arg.Any<SQLiteConnectionInfo>(), Arg.Any<JVOpenResult>(), Arg.Any<JVRecordSpec[]>()).Returns(jvDataToSQLiteOpr);
                container.RegisterInstance<JVDataToSQLiteOperator.Factory>(factory);
            }
            var opr = new JVPastDataServiceOperator(container, listener, jvLinkSrv, connInfo, dataSpecSetting, JVOpenOptions.Normal);


            // Act
            var oprRslt = opr.Operate();


            // Assert
            Assert.That(oprRslt.Interpretation.Failed, Is.True);
        }
    }
}
