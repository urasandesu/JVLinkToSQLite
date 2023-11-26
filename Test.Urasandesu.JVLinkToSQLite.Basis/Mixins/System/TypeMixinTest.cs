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
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;

namespace Test.Urasandesu.JVLinkToSQLite.Basis.Mixins.System
{
    [TestFixture]
    public class TypeMixinTest
    {
        [TestCaseSource(nameof(IsPrimitiveType_should_return_true_when_type_is_primitive_type_TestCases))]
        public bool IsPrimitiveType_should_return_true_when_type_is_primitive_type(Type type)
        {
            // Arrange
            // Act
            var result = TypeMixin.IsPrimitiveType(type);

            // Assert
            return result;
        }

        static IEnumerable<TestCaseData> IsPrimitiveType_should_return_true_when_type_is_primitive_type_TestCases()
        {
            yield return new TestCaseData(typeof(int)).Returns(true);
            yield return new TestCaseData(typeof(long)).Returns(true);
            yield return new TestCaseData(typeof(short)).Returns(true);
            yield return new TestCaseData(typeof(byte)).Returns(true);
            yield return new TestCaseData(typeof(decimal)).Returns(true);
            yield return new TestCaseData(typeof(DateTime)).Returns(true);
            yield return new TestCaseData(typeof(string)).Returns(true);
            yield return new TestCaseData(typeof(object)).Returns(false);
        }
    }
}

