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
using System;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;

namespace Test.Urasandesu.JVLinkToSQLite.JVData
{
#if DEBUG
    /// <summary>
    /// 一時的な動作確認用のテスト クラスです。
    /// </summary>
    [TestFixture]
    public class Playground
    {
        [Test]
        public void TestMethod2()
        {
            var MSCorLibAsmDef = AssemblyDefinition.ReadAssembly(typeof(int).Assembly.Location);
            Assert.That(MSCorLibAsmDef, Is.Not.Null);
            Console.WriteLine(MSCorLibAsmDef);
        }

        [Test]
        public void TestMethod4()
        {
            Assert.That(JVDataStructTypes.IsJVDataStructTypeId("UM"), Is.True);
            Assert.That(JVDataStructTypes.IsJVDataStructTypeId("UM_V4802"), Is.True);
            Assert.That(JVDataStructTypes.IsJVDataStructTypeId("O5_V4802_OddsSanrenInfo_ODDS_SANREN_INFO"), Is.False);
            Assert.That(JVDataStructTypes.IsJVDataStructTypeId("O5_OddsSanrenInfo_ODDS_SANREN_INFO"), Is.False);
        }

        [Test]
        public void TestMethod5()
        {
            Assert.That(JVDataStructTypes.ExtractJVDataStructTypeId("O5_V4802_OddsSanrenInfo_ODDS_SANREN_INFO"), Is.EqualTo("O5_V4802"));
            Assert.That(JVDataStructTypes.ExtractJVDataStructTypeId("O5_OddsSanrenInfo_ODDS_SANREN_INFO"), Is.EqualTo("O5"));
        }

        [Test]
        public void TestMethod6()
        {
            foreach (var jvDataStructType in JVDataStructTypes.GetList("Urasandesu.JVLinkToSQLite.JVData.dll"))
            {
                Console.WriteLine(JVDataStructTypes.ExtractJVDataStructTypeId(jvDataStructType));
            }
        }

        [Test]
        public void TestMethod7()
        {
            Console.WriteLine(new Version(4, 8, 0, 2).ToString());
            Console.WriteLine(new EmbeddableVersion(4, 8, 0, 2).ToString());
        }
    }
#endif
}
