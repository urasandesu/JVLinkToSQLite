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
using System.Reflection;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;

namespace Test.Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    [TestFixture]
    public class SIDBuilderTest
    {
        [Test]
        public void SoftwareName_should_be_set_to_the_name_when_assembly_is_specified()
        {
            // Arrange
            var asmName = new AssemblyName("xxx");
            var asmMock = new AssemblyMock();
            asmMock.GetNameProvider = () => asmName;
            var sidBldr = new SIDBuilder("authorId", "softwareId", asmMock);

            // Act
            var softwareName = sidBldr.SoftwareName;

            // Assert
            Assert.That(softwareName, Is.EqualTo("xxx"));
        }

        [Test]
        public void Version_should_be_set_to_the_version_when_assembly_is_specified()
        {
            // Arrange
            var asmName = new AssemblyName("xxx");
            asmName.Version = new Version(3, 3, 0, 4);
            var asmMock = new AssemblyMock();
            asmMock.GetNameProvider = () => asmName;
            var sidBldr = new SIDBuilder("authorId", "softwareId", asmMock);

            // Act
            var version = sidBldr.Version;

            // Assert
            Assert.That(version, Is.EqualTo(new Version(3, 3, 0, 4)));
        }

        [TestCaseSource(nameof(SID_should_be_xxx_when_yyy_is_specified_TestCases))]
        public string SID_should_be_xxx_when_yyy_is_specified(string authorId, string softwareId, string softwareName, Version version)
        {
            // Arrange
            var asmName = new AssemblyName(softwareName);
            asmName.Version = version;
            var asmMock = new AssemblyMock();
            asmMock.GetNameProvider = () => asmName;
            var sidBldr = new SIDBuilder(authorId, softwareId, asmMock);

            // Act
            var sid = sidBldr.SID;

            // Assert
            return sid;
        }

        static IEnumerable<TestCaseData> SID_should_be_xxx_when_yyy_is_specified_TestCases()
        {
            yield return new TestCaseData(null, "softwareId", "softwareName", new Version(3, 3, 0, 4)).Returns("UNKNOWN");
            yield return new TestCaseData("", "softwareId", "softwareName", new Version(3, 3, 0, 4)).Returns("UNKNOWN");
            yield return new TestCaseData(ObfuscatedResources.AuthorId.Default, "softwareId", "softwareName", new Version(3, 3, 0, 4)).Returns("UNKNOWN");
            yield return new TestCaseData("authorId", null, "softwareName", new Version(3, 3, 0, 4)).Returns("UNKNOWN");
            yield return new TestCaseData("authorId", "", "softwareName", new Version(3, 3, 0, 4)).Returns("UNKNOWN");
            yield return new TestCaseData("authorId", ObfuscatedResources.SoftwareId.Default, "softwareName", new Version(3, 3, 0, 4)).Returns("UNKNOWN");
            yield return new TestCaseData("authorId", "softwareId", "softwareName", new Version(3, 3, 0, 4)).Returns("authorId/softwareId/softwareName/Ver3.3.0.4");
        }

        class AssemblyMock : Assembly
        {
            public Func<AssemblyName> GetNameProvider { get; set; }
            public override AssemblyName GetName()
            {
                return GetNameProvider?.Invoke();
            }
        }
    }
}
