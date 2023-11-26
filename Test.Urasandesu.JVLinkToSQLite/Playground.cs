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

using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Settings;
using static JVData_Struct;

namespace Test.Urasandesu.JVLinkToSQLite
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
            var buf = "RA220230901202309020102070110000　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　                                                                                                                                                                                                                                                                                                                                                                        　　　　　　　　　　　　　　　　　　　0000  11A033703000000000703　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　120000001700C   000550000002200000014000000083000000550000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000009500000080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000                                                                      00                                                                      00                                                                      00                                                                      0\r\n";
            var options = JVOpenOptions.Normal;
            var dataStruct = new JV_RA_RACE();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_RA_RACEDataBridge();
            dataBridge.SetProperties(dataStruct, options);

            Assert.That(dataBridge.Columns.Count, Is.EqualTo(dataBridge.BaseGetter().SelectMany(JVDataStructGetters.ExpandPropertyValues).Count()));
            foreach (var item in dataBridge.Columns)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("---");
            foreach (var item in dataBridge.BaseGetter().SelectMany(JVDataStructGetters.ExpandPropertyValues))
            {
                Console.WriteLine(item);
            }

        }

        [Test]
        public void Fuga()
        {
            var dataSpecSetting = new JVDataSpecSetting("RACE");
            var baseDataSpecKey = dataSpecSetting.DataSpecKey.Clone();
            foreach (var dataSpecKey in baseDataSpecKey.GetSpecKeysInInterval(dataSpecSetting.TimeIntervalUnit).Take(2))
            {
                Console.WriteLine(dataSpecKey);
            }
        }

        [Test]
        public void Piyo()
        {
            Console.WriteLine(JVDataStructCreateTableSources.CC.GetCommandText("NL_CC_INFO"));
        }
    }
#endif
}
