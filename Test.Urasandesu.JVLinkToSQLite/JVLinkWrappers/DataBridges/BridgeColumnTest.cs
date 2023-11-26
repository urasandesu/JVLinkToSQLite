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
using System;
using System.Collections.Generic;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;

namespace Test.Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    [TestFixture]
    public class BridgeColumnTest
    {
        [TestCaseSource(nameof(SqlColumnType_should_be_xxx_when_columnType_is_yyy_TestCases))]
        public string SqlColumnType_should_be_xxx_when_columnType_is_yyy(Type columnType)
        {
            // Arrange
            var columnName = "ColumnName";
            var isHead = false;
            var isId = false;
            var applyCompatibilityFixes = default(Func<object, object>);
            var columnComment = "ColumnComment";

            // Act
            var bridgeColumn = new BridgeColumn(columnName, columnType, isHead, isId, applyCompatibilityFixes, columnComment);

            // Assert
            return bridgeColumn.SqlColumnType;
        }

        static IEnumerable<TestCaseData> SqlColumnType_should_be_xxx_when_columnType_is_yyy_TestCases()
        {
            yield return new TestCaseData(typeof(int)).Returns("integer");
            yield return new TestCaseData(typeof(long)).Returns("integer");
            yield return new TestCaseData(typeof(short)).Returns("integer");
            yield return new TestCaseData(typeof(byte)).Returns("integer");
            yield return new TestCaseData(typeof(decimal)).Returns("numeric");
            yield return new TestCaseData(typeof(DateTime)).Returns("numeric");
            yield return new TestCaseData(typeof(double)).Returns("real");
            yield return new TestCaseData(typeof(float)).Returns("real");
            yield return new TestCaseData(typeof(string)).Returns("text");
            yield return new TestCaseData(typeof(object)).Returns("text");
        }
    }
}
