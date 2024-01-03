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
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;

namespace Test.Urasandesu.JVLinkToSQLite.Basis
{
#if DEBUG
    /// <summary>
    /// 一時的な動作確認用のテスト クラスです。
    /// </summary>
    [TestFixture]
    public class Playground
    {
        [Test]
        public void Hoge()
        {
            var fileName = "HYFW2023090520230905150604.jvd";
            var dataFile = JVDataSpec.HOYU.GetDataFile(fileName);
            Assert.That(dataFile, Is.EqualTo(new JVDataFile(JVDataFileSpec.HOYU_HY_F_W, "HYFW2023090520230905150604.jvd", "20230905", new DateTime(2023, 09, 05, 15, 06, 04))));
        }

        [Test]
        public void Fuga()
        {
            var key = new SQLitePreparedCommandKey(1, "NL_CK_CHAKU");
            Console.WriteLine(key);
        }
    }
#endif
}
