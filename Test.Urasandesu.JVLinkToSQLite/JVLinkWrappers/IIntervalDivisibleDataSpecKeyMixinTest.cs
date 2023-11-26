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
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;

namespace Test.Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    [TestFixture]
    public class IIntervalDivisibleDataSpecKeyMixinTest
    {
        [Test]
        public void DivideSpecKeysInInterval_should_return_specified_interval_divided_spec_keys()
        {
            // Arrange
            var interval = TimeSpan.FromMinutes(30);
            var specKey = new JVKaisaiDateTimeRangeKey(new DateTime(1986, 1, 1), new DateTime(1986, 1, 1, 0, 59, 0));
            var expectedSpecKeys = new[]
            {
                new JVKaisaiDateTimeRangeKey(new DateTime(1986, 1, 1), new DateTime(1986, 1, 1, 0, 30, 0)),
                new JVKaisaiDateTimeRangeKey(new DateTime(1986, 1, 1, 0, 30, 0), new DateTime(1986, 1, 1, 0, 59, 0))
            };

            // Act
            var actualSpecKeys = specKey.DivideSpecKeysInInterval(interval);

            // Assert
            Assert.That(actualSpecKeys, Is.EqualTo(expectedSpecKeys));
        }

        [Test]
        public void DivideSpecKeysInInterval_should_return_same_spec_key_when_zero_is_specified()
        {
            // Arrange
            var interval = TimeSpan.Zero;
            var specKey = new JVKaisaiDateTimeKey(new DateTime(1986, 1, 1));
            var expectedSpecKeys = new[]
            {
                specKey
            };

            // Act
            var actualSpecKeys = specKey.DivideSpecKeysInInterval(interval);

            // Assert
            Assert.That(actualSpecKeys, Is.EqualTo(expectedSpecKeys));
        }
    }
}
