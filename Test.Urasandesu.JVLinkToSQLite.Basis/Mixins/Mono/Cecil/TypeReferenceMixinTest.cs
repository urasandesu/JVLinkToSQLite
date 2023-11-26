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

using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil;

namespace Test.Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil
{
    [TestFixture]
    public class TypeReferenceMixinTest
    {
        [TestCaseSource(nameof(IsPrimitiveType_should_return_true_when_type_is_primitive_type_TestCases))]
        public bool IsPrimitiveType_should_return_true_when_type_is_primitive_type(TypeReference type)
        {
            // Arrange
            // Act
            var result = TypeReferenceMixin.IsPrimitiveType(type);

            // Assert
            return result;
        }

        static IEnumerable<TestCaseData> IsPrimitiveType_should_return_true_when_type_is_primitive_type_TestCases()
        {
            yield return new TestCaseData(new TypeReference("System", "Int32", null, null, false)).Returns(true);
            yield return new TestCaseData(new TypeReference("System", "Int64", null, null, false)).Returns(true);
            yield return new TestCaseData(new TypeReference("System", "Int16", null, null, false)).Returns(true);
            yield return new TestCaseData(new TypeReference("System", "Byte", null, null, false)).Returns(true);
            yield return new TestCaseData(new TypeReference("System", "Decimal", null, null, false)).Returns(true);
            yield return new TestCaseData(new TypeReference("System", "DateTime", null, null, false)).Returns(true);
            yield return new TestCaseData(new TypeReference("System", "String", null, null, false)).Returns(true);
            yield return new TestCaseData(new TypeReference("System", "Object", null, null, false)).Returns(false);
        }
    }
}
