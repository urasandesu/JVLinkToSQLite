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
    public class JVDataSpecTest
    {
        [Test]
        public void GetDataFile_returns_JVDataFile_according_to_fileName()
        {
            // Arrange
            var fileName = "HYFW2023090520230905150604.jvd";

            // Act
            var dataFile = JVDataSpec.HOYU.GetDataFile(fileName);

            // Assert
            var expected = new JVDataFile(JVDataFileSpec.HOYU_HY_F_W, "HYFW2023090520230905150604.jvd", "20230905", new DateTime(2023, 09, 05, 15, 06, 04));
            Assert.That(dataFile, Is.EqualTo(expected));
        }

        [Test]
        public void GetDataFile_returns_null_in_data_spec_updating_at_real_time()
        {
            // Arrange
            var fileName = "hogehoge.fuga";

            // Act
            var dataFile = JVDataSpec._0B12.GetDataFile(fileName);

            // Assert
            Assert.That(dataFile, Is.Null);
        }

        [Test]
        public void GetDataFile_returns_null_if_fileName_is_null()
        {
            // Arrange
            var fileName = default(string);

            // Act
            var dataFile = JVDataSpec.HOYU.GetDataFile(fileName);

            // Assert
            Assert.That(dataFile, Is.Null);
        }

        [Test]
        public void GetDataFile_throws_NotSupportedException_if_unknown_fileName_is_passed()
        {
            // Arrange
            var fileName = "hogehoge.fuga";

            // Act, Assert
            Assert.Throws<NotSupportedException>(() => JVDataSpec.HOYU.GetDataFile(fileName));
        }

    }
}
