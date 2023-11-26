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
using System.Collections.Generic;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;

namespace Test.Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    [TestFixture]
    public class JVInitResultTest
    {
        [TestCaseSource(nameof(SetReturnCode_should_set_Interpretation_to_xxx_when_yyy_is_specified_TestCases))]
        public JVResultInterpretation SetReturnCode_should_set_Interpretation_to_xxx_when_yyy_is_specified(int returnCode)
        {
            // Arrange
            var initRslt = new JVInitResult();

            // Act
            initRslt.SetReturnCode(returnCode);

            // Assert
            return initRslt.Interpretation;
        }

        static IEnumerable<TestCaseData> SetReturnCode_should_set_Interpretation_to_xxx_when_yyy_is_specified_TestCases()
        {
            yield return new TestCaseData(0).Returns(JVResultInterpretation.SuccessTrue);
            yield return new TestCaseData(-101).Returns(JVResultInterpretation.Error);
            yield return new TestCaseData(-102).Returns(JVResultInterpretation.Error);
            yield return new TestCaseData(-103).Returns(JVResultInterpretation.Error);
            yield return new TestCaseData(-1000).Returns(JVResultInterpretation.Error);
        }
    }
}
