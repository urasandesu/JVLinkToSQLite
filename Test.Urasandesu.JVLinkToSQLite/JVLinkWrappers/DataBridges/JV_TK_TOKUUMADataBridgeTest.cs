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
using System.Data.SQLite;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using static JVData_Struct;

namespace Test.Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    [TestFixture]
    public class JV_TK_TOKUUMADataBridgeTest
    {
        [Test]
        public void JV_TK_TOKUUMADataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_TK_TOKUUMADataBridge("TK220230828202309020102070910000知床特別　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　SHIRETOKO TOKUBETSU                                                                                                                                                                                                                                                                                                                                                     知床特別　　　　　　知床特別　　知床特0000E13A034000010010010010120017C 000000000220012020101031アスクエピソード　　　　　　　　　　001201055藤原英昭56000022020104954ウメムスビ　　　　　　　　　　　　　001201172新谷功一56000032017102809ガリレイ　　　　　　　　　　　　　　003201136高橋亮　58000042020100056クールムーア　　　　　　　　　　　　001101108矢野英一56000052018103296サニーオーシャン　　　　　　　　　　003101010中野栄治58000062019103136シゲルファンノユメ　　　　　　　　　001101023伊藤圭三58000072019101262ショウナンアメリア　　　　　　　　　002101145奥村武　56000082020104942シルフィードレーヴ　　　　　　　　　002201028西園正都54000092018103485ジャガード　　　　　　　　　　　　　001101143和田雄二58000102018100097スマートルシーダ　　　　　　　　　　001201002音無秀孝58000112019101819センタースリール　　　　　　　　　　002101056本間忍　56000122017104895ダノンカオス　　　　　　　　　　　　053200429佐々木晶58000132019104361テーオースパロー　　　　　　　　　　001201110清水久詞58000142019100170デルマカミーラ　　　　　　　　　　　002201123牧田和弥56000152020101764トーホウキザン　　　　　　　　　　　001201135高橋康之56000162019102456ナバロン　　　　　　　　　　　　　　001201178杉山佳明58000172019103897ハピネスアゲン　　　　　　　　　　　002101024萱野浩二56000182020104674バレリーナ　　　　　　　　　　　　　002201110清水久詞54000192019107075ブランデーロック　　　　　　　　　　001101005小桧山悟58000202018105873ミキノバスドラム　　　　　　　　　　051100437南田美知58000212018106552メイショウフンケイ　　　　　　　　　001201136高橋亮　58000222020106330ラウラーナ　　　　　　　　　　　　　002100420宗像義忠5400                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    \r\n",
                                                        JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertTableCreation("NL_TK_TOKUUMA", JVDataStructColumns.TK);
                    cmd.AssertTableCreation("NL_TK_TokuUmaInfo", JVDataStructColumns.TK_TokuUmaInfo);
                }
            }
        }

        [Test]
        public void JV_TK_TOKUUMADataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_TK_TOKUUMADataBridge("TK220230828202309020102070910000知床特別　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　SHIRETOKO TOKUBETSU                                                                                                                                                                                                                                                                                                                                                     知床特別　　　　　　知床特別　　知床特0000E13A034000010010010010120017C 000000000220012020101031アスクエピソード　　　　　　　　　　001201055藤原英昭56000022020104954ウメムスビ　　　　　　　　　　　　　001201172新谷功一56000032017102809ガリレイ　　　　　　　　　　　　　　003201136高橋亮　58000042020100056クールムーア　　　　　　　　　　　　001101108矢野英一56000052018103296サニーオーシャン　　　　　　　　　　003101010中野栄治58000062019103136シゲルファンノユメ　　　　　　　　　001101023伊藤圭三58000072019101262ショウナンアメリア　　　　　　　　　002101145奥村武　56000082020104942シルフィードレーヴ　　　　　　　　　002201028西園正都54000092018103485ジャガード　　　　　　　　　　　　　001101143和田雄二58000102018100097スマートルシーダ　　　　　　　　　　001201002音無秀孝58000112019101819センタースリール　　　　　　　　　　002101056本間忍　56000122017104895ダノンカオス　　　　　　　　　　　　053200429佐々木晶58000132019104361テーオースパロー　　　　　　　　　　001201110清水久詞58000142019100170デルマカミーラ　　　　　　　　　　　002201123牧田和弥56000152020101764トーホウキザン　　　　　　　　　　　001201135高橋康之56000162019102456ナバロン　　　　　　　　　　　　　　001201178杉山佳明58000172019103897ハピネスアゲン　　　　　　　　　　　002101024萱野浩二56000182020104674バレリーナ　　　　　　　　　　　　　002201110清水久詞54000192019107075ブランデーロック　　　　　　　　　　001101005小桧山悟58000202018105873ミキノバスドラム　　　　　　　　　　051100437南田美知58000212018106552メイショウフンケイ　　　　　　　　　001201136高橋亮　58000222020106330ラウラーナ　　　　　　　　　　　　　002100420宗像義忠5400                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    \r\n",
                                                        JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    var bltCmds = new List<SQLitePreparedCommand>();
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                        bltCmds.Add(bltCmd);
                    }

                    // Assert
                    foreach (var bltCmd in bltCmds)
                    {
                        Assert.DoesNotThrow(() => bltCmd.ExecuteNonQuery(), bltCmd.GetLoggingQuery());
                    }
                }
            }
        }

        [Test]
        public void JV_TK_TOKUUMADataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_TK_TOKUUMADataBridge("TK220230828202309020102070910000知床特別　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　SHIRETOKO TOKUBETSU                                                                                                                                                                                                                                                                                                                                                     知床特別　　　　　　知床特別　　知床特0000E13A034000010010010010120017C 000000000220012020101031アスクエピソード　　　　　　　　　　001201055藤原英昭56000022020104954ウメムスビ　　　　　　　　　　　　　001201172新谷功一56000032017102809ガリレイ　　　　　　　　　　　　　　003201136高橋亮　58000042020100056クールムーア　　　　　　　　　　　　001101108矢野英一56000052018103296サニーオーシャン　　　　　　　　　　003101010中野栄治58000062019103136シゲルファンノユメ　　　　　　　　　001101023伊藤圭三58000072019101262ショウナンアメリア　　　　　　　　　002101145奥村武　56000082020104942シルフィードレーヴ　　　　　　　　　002201028西園正都54000092018103485ジャガード　　　　　　　　　　　　　001101143和田雄二58000102018100097スマートルシーダ　　　　　　　　　　001201002音無秀孝58000112019101819センタースリール　　　　　　　　　　002101056本間忍　56000122017104895ダノンカオス　　　　　　　　　　　　053200429佐々木晶58000132019104361テーオースパロー　　　　　　　　　　001201110清水久詞58000142019100170デルマカミーラ　　　　　　　　　　　002201123牧田和弥56000152020101764トーホウキザン　　　　　　　　　　　001201135高橋康之56000162019102456ナバロン　　　　　　　　　　　　　　001201178杉山佳明58000172019103897ハピネスアゲン　　　　　　　　　　　002101024萱野浩二56000182020104674バレリーナ　　　　　　　　　　　　　002201110清水久詞54000192019107075ブランデーロック　　　　　　　　　　001101005小桧山悟58000202018105873ミキノバスドラム　　　　　　　　　　051100437南田美知58000212018106552メイショウフンケイ　　　　　　　　　001201136高橋亮　58000222020106330ラウラーナ　　　　　　　　　　　　　002100420宗像義忠5400                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    \r\n",
                                                        JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecords("NL_TK_TOKUUMA", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "TK" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230828" },
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "RaceInfoYoubiCD", "1" },
                            { "RaceInfoTokuNum", "0000" },
                            { "RaceInfoHondai", "知床特別" },
                            { "RaceInfoFukudai", "" },
                            { "RaceInfoKakko", "" },
                            { "RaceInfoHondaiEng", "SHIRETOKO TOKUBETSU" },
                            { "RaceInfoFukudaiEng", "" },
                            { "RaceInfoKakkoEng", "" },
                            { "RaceInfoRyakusyo10", "知床特別" },
                            { "RaceInfoRyakusyo6", "知床特別" },
                            { "RaceInfoRyakusyo3", "知床特" },
                            { "RaceInfoKubun", "0" },
                            { "RaceInfoNkai", "000" },
                            { "GradeCD", "E" },
                            { "JyokenInfoSyubetuCD", "13" },
                            { "JyokenInfoKigoCD", "A03" },
                            { "JyokenInfoJyuryoCD", "4" },
                            { "JyokenInfoJyokenCD0", "000" },
                            { "JyokenInfoJyokenCD1", "010" },
                            { "JyokenInfoJyokenCD2", "010" },
                            { "JyokenInfoJyokenCD3", "010" },
                            { "JyokenInfoJyokenCD4", "010" },
                            { "Kyori", "1200" },
                            { "TrackCD", "17" },
                            { "CourseKubunCD", "C" },
                            { "HandiDate", "00000000" },
                            { "TorokuTosu", "022" }
                        },
                    });
                    cmd.AssertRecords("NL_TK_TokuUmaInfo", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "000" },
                            { "TokuUmaInfoNum", "001" },
                            { "TokuUmaInfoKettoNum", "2020101031" },
                            { "TokuUmaInfoBamei", "アスクエピソード" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01055" },
                            { "TokuUmaInfoChokyosiRyakusyo", "藤原英昭" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "001" },
                            { "TokuUmaInfoNum", "002" },
                            { "TokuUmaInfoKettoNum", "2020104954" },
                            { "TokuUmaInfoBamei", "ウメムスビ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01172" },
                            { "TokuUmaInfoChokyosiRyakusyo", "新谷功一" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "002" },
                            { "TokuUmaInfoNum", "003" },
                            { "TokuUmaInfoKettoNum", "2017102809" },
                            { "TokuUmaInfoBamei", "ガリレイ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "3" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01136" },
                            { "TokuUmaInfoChokyosiRyakusyo", "高橋亮" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "003" },
                            { "TokuUmaInfoNum", "004" },
                            { "TokuUmaInfoKettoNum", "2020100056" },
                            { "TokuUmaInfoBamei", "クールムーア" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01108" },
                            { "TokuUmaInfoChokyosiRyakusyo", "矢野英一" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "004" },
                            { "TokuUmaInfoNum", "005" },
                            { "TokuUmaInfoKettoNum", "2018103296" },
                            { "TokuUmaInfoBamei", "サニーオーシャン" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "3" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01010" },
                            { "TokuUmaInfoChokyosiRyakusyo", "中野栄治" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "005" },
                            { "TokuUmaInfoNum", "006" },
                            { "TokuUmaInfoKettoNum", "2019103136" },
                            { "TokuUmaInfoBamei", "シゲルファンノユメ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01023" },
                            { "TokuUmaInfoChokyosiRyakusyo", "伊藤圭三" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "006" },
                            { "TokuUmaInfoNum", "007" },
                            { "TokuUmaInfoKettoNum", "2019101262" },
                            { "TokuUmaInfoBamei", "ショウナンアメリア" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01145" },
                            { "TokuUmaInfoChokyosiRyakusyo", "奥村武" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "007" },
                            { "TokuUmaInfoNum", "008" },
                            { "TokuUmaInfoKettoNum", "2020104942" },
                            { "TokuUmaInfoBamei", "シルフィードレーヴ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01028" },
                            { "TokuUmaInfoChokyosiRyakusyo", "西園正都" },
                            { "TokuUmaInfoFutan", "540" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "008" },
                            { "TokuUmaInfoNum", "009" },
                            { "TokuUmaInfoKettoNum", "2018103485" },
                            { "TokuUmaInfoBamei", "ジャガード" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01143" },
                            { "TokuUmaInfoChokyosiRyakusyo", "和田雄二" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "009" },
                            { "TokuUmaInfoNum", "010" },
                            { "TokuUmaInfoKettoNum", "2018100097" },
                            { "TokuUmaInfoBamei", "スマートルシーダ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01002" },
                            { "TokuUmaInfoChokyosiRyakusyo", "音無秀孝" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "010" },
                            { "TokuUmaInfoNum", "011" },
                            { "TokuUmaInfoKettoNum", "2019101819" },
                            { "TokuUmaInfoBamei", "センタースリール" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01056" },
                            { "TokuUmaInfoChokyosiRyakusyo", "本間忍" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "011" },
                            { "TokuUmaInfoNum", "012" },
                            { "TokuUmaInfoKettoNum", "2017104895" },
                            { "TokuUmaInfoBamei", "ダノンカオス" },
                            { "TokuUmaInfoUmaKigoCD", "05" },
                            { "TokuUmaInfoSexCD", "3" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "00429" },
                            { "TokuUmaInfoChokyosiRyakusyo", "佐々木晶" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "012" },
                            { "TokuUmaInfoNum", "013" },
                            { "TokuUmaInfoKettoNum", "2019104361" },
                            { "TokuUmaInfoBamei", "テーオースパロー" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01110" },
                            { "TokuUmaInfoChokyosiRyakusyo", "清水久詞" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "013" },
                            { "TokuUmaInfoNum", "014" },
                            { "TokuUmaInfoKettoNum", "2019100170" },
                            { "TokuUmaInfoBamei", "デルマカミーラ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01123" },
                            { "TokuUmaInfoChokyosiRyakusyo", "牧田和弥" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "014" },
                            { "TokuUmaInfoNum", "015" },
                            { "TokuUmaInfoKettoNum", "2020101764" },
                            { "TokuUmaInfoBamei", "トーホウキザン" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01135" },
                            { "TokuUmaInfoChokyosiRyakusyo", "高橋康之" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "015" },
                            { "TokuUmaInfoNum", "016" },
                            { "TokuUmaInfoKettoNum", "2019102456" },
                            { "TokuUmaInfoBamei", "ナバロン" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01178" },
                            { "TokuUmaInfoChokyosiRyakusyo", "杉山佳明" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "016" },
                            { "TokuUmaInfoNum", "017" },
                            { "TokuUmaInfoKettoNum", "2019103897" },
                            { "TokuUmaInfoBamei", "ハピネスアゲン" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01024" },
                            { "TokuUmaInfoChokyosiRyakusyo", "萱野浩二" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "017" },
                            { "TokuUmaInfoNum", "018" },
                            { "TokuUmaInfoKettoNum", "2020104674" },
                            { "TokuUmaInfoBamei", "バレリーナ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01110" },
                            { "TokuUmaInfoChokyosiRyakusyo", "清水久詞" },
                            { "TokuUmaInfoFutan", "540" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "018" },
                            { "TokuUmaInfoNum", "019" },
                            { "TokuUmaInfoKettoNum", "2019107075" },
                            { "TokuUmaInfoBamei", "ブランデーロック" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01005" },
                            { "TokuUmaInfoChokyosiRyakusyo", "小桧山悟" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "019" },
                            { "TokuUmaInfoNum", "020" },
                            { "TokuUmaInfoKettoNum", "2018105873" },
                            { "TokuUmaInfoBamei", "ミキノバスドラム" },
                            { "TokuUmaInfoUmaKigoCD", "05" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "00437" },
                            { "TokuUmaInfoChokyosiRyakusyo", "南田美知" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "020" },
                            { "TokuUmaInfoNum", "021" },
                            { "TokuUmaInfoKettoNum", "2018106552" },
                            { "TokuUmaInfoBamei", "メイショウフンケイ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01136" },
                            { "TokuUmaInfoChokyosiRyakusyo", "高橋亮" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "021" },
                            { "TokuUmaInfoNum", "022" },
                            { "TokuUmaInfoKettoNum", "2020106330" },
                            { "TokuUmaInfoBamei", "ラウラーナ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "00420" },
                            { "TokuUmaInfoChokyosiRyakusyo", "宗像義忠" },
                            { "TokuUmaInfoFutan", "540" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_TK_TOKUUMADataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_TK_TOKUUMADataBridge("TK220230828202309020102070910000知床特別　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　SHIRETOKO TOKUBETSU                                                                                                                                                                                                                                                                                                                                                     知床特別　　　　　　知床特別　　知床特0000E13A034000010010010010120017C 000000000220012020101031アスクエピソード　　　　　　　　　　001201055藤原英昭56000022020104954ウメムスビ　　　　　　　　　　　　　001201172新谷功一56000032017102809ガリレイ　　　　　　　　　　　　　　003201136高橋亮　58000042020100056クールムーア　　　　　　　　　　　　001101108矢野英一56000052018103296サニーオーシャン　　　　　　　　　　003101010中野栄治58000062019103136シゲルファンノユメ　　　　　　　　　001101023伊藤圭三58000072019101262ショウナンアメリア　　　　　　　　　002101145奥村武　56000082020104942シルフィードレーヴ　　　　　　　　　002201028西園正都54000092018103485ジャガード　　　　　　　　　　　　　001101143和田雄二58000102018100097スマートルシーダ　　　　　　　　　　001201002音無秀孝58000112019101819センタースリール　　　　　　　　　　002101056本間忍　56000122017104895ダノンカオス　　　　　　　　　　　　053200429佐々木晶58000132019104361テーオースパロー　　　　　　　　　　001201110清水久詞58000142019100170デルマカミーラ　　　　　　　　　　　002201123牧田和弥56000152020101764トーホウキザン　　　　　　　　　　　001201135高橋康之56000162019102456ナバロン　　　　　　　　　　　　　　001201178杉山佳明58000172019103897ハピネスアゲン　　　　　　　　　　　002101024萱野浩二56000182020104674バレリーナ　　　　　　　　　　　　　002201110清水久詞54000192019107075ブランデーロック　　　　　　　　　　001101005小桧山悟58000202018105873ミキノバスドラム　　　　　　　　　　051100437南田美知58000212018106552メイショウフンケイ　　　　　　　　　001201136高橋亮　58000222020106330ラウラーナ　　　　　　　　　　　　　002100420宗像義忠5400                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    \r\n",
                                                         JVOpenOptions.Normal);
            var dataBridge2 = NewJV_TK_TOKUUMADataBridge("TK220230828202309020102070910000知常特別　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　SHIRETOKO TOKUBETSU                                                                                                                                                                                                                                                                                                                                                     知常特別　　　　　　知常特別　　知床特0000E13A034000010010010010120017C 000000000220012020101031アスクエピソード　　　　　　　　　　001201055藤原英昭56000022020104954ウメムスビ　　　　　　　　　　　　　001201172新谷功一56000032017102809ガリレオ　　　　　　　　　　　　　　003201136高橋亮　58000042020100056クールムーア　　　　　　　　　　　　001101108矢野英一56000052018103296サニーオーシャン　　　　　　　　　　003101010中野栄治58000062019103136シゲルファンノユメ　　　　　　　　　001101023伊藤圭三58000072019101262ショウナンアメリア　　　　　　　　　002101145奥村武　56000082020104942シルフィードレーヴ　　　　　　　　　002201028西園正都54000092018103485ジャガード　　　　　　　　　　　　　001101143和田雄二58000102018100097スマートルシーダ　　　　　　　　　　001201002音無秀孝58000112019101819センタースリール　　　　　　　　　　002101056本間忍　56000122017104895ダノンカオス　　　　　　　　　　　　053200429佐々木晶58000132019104361テーオースパロー　　　　　　　　　　001201110清水久詞58000142019100170デルマカミーラ　　　　　　　　　　　002201123牧田和弥56000152020101764トーホウキザン　　　　　　　　　　　001201135高橋康之56000162019102456ナバロン　　　　　　　　　　　　　　001201178杉山佳明58000172019103897ハピネスアゲン　　　　　　　　　　　002101024萱野浩二56000182020104674バレリーナ　　　　　　　　　　　　　002201110清水久詞54000192019107075ブランデーロック　　　　　　　　　　001101005小桧山悟58000202018105873ミキノバスドラム　　　　　　　　　　051100437南田美知58000212018106552メイショウフンケイ　　　　　　　　　001201136高橋亮　58000222020106330ラウラーナ　　　　　　　　　　　　　002100420宗像義忠5400                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    \r\n",
                                                         JVOpenOptions.Normal);

            using (var conn = new SQLiteConnection("Data Source=:memory:"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    foreach (var bltCmd in dataBridge1.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge1.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }
                    foreach (var bltCmd in dataBridge2.BuildUpInsertCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecords("NL_TK_TOKUUMA", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "TK" },
                            { "headDataKubun", "2" },
                            { "headMakeDate", "20230828" },
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "RaceInfoYoubiCD", "1" },
                            { "RaceInfoTokuNum", "0000" },
                            { "RaceInfoHondai", "知常特別" },
                            { "RaceInfoFukudai", "" },
                            { "RaceInfoKakko", "" },
                            { "RaceInfoHondaiEng", "SHIRETOKO TOKUBETSU" },
                            { "RaceInfoFukudaiEng", "" },
                            { "RaceInfoKakkoEng", "" },
                            { "RaceInfoRyakusyo10", "知常特別" },
                            { "RaceInfoRyakusyo6", "知常特別" },
                            { "RaceInfoRyakusyo3", "知床特" },
                            { "RaceInfoKubun", "0" },
                            { "RaceInfoNkai", "000" },
                            { "GradeCD", "E" },
                            { "JyokenInfoSyubetuCD", "13" },
                            { "JyokenInfoKigoCD", "A03" },
                            { "JyokenInfoJyuryoCD", "4" },
                            { "JyokenInfoJyokenCD0", "000" },
                            { "JyokenInfoJyokenCD1", "010" },
                            { "JyokenInfoJyokenCD2", "010" },
                            { "JyokenInfoJyokenCD3", "010" },
                            { "JyokenInfoJyokenCD4", "010" },
                            { "Kyori", "1200" },
                            { "TrackCD", "17" },
                            { "CourseKubunCD", "C" },
                            { "HandiDate", "00000000" },
                            { "TorokuTosu", "022" }
                        },
                    });
                    cmd.AssertRecords("NL_TK_TokuUmaInfo", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "000" },
                            { "TokuUmaInfoNum", "001" },
                            { "TokuUmaInfoKettoNum", "2020101031" },
                            { "TokuUmaInfoBamei", "アスクエピソード" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01055" },
                            { "TokuUmaInfoChokyosiRyakusyo", "藤原英昭" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "001" },
                            { "TokuUmaInfoNum", "002" },
                            { "TokuUmaInfoKettoNum", "2020104954" },
                            { "TokuUmaInfoBamei", "ウメムスビ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01172" },
                            { "TokuUmaInfoChokyosiRyakusyo", "新谷功一" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "002" },
                            { "TokuUmaInfoNum", "003" },
                            { "TokuUmaInfoKettoNum", "2017102809" },
                            { "TokuUmaInfoBamei", "ガリレオ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "3" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01136" },
                            { "TokuUmaInfoChokyosiRyakusyo", "高橋亮" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "003" },
                            { "TokuUmaInfoNum", "004" },
                            { "TokuUmaInfoKettoNum", "2020100056" },
                            { "TokuUmaInfoBamei", "クールムーア" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01108" },
                            { "TokuUmaInfoChokyosiRyakusyo", "矢野英一" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "004" },
                            { "TokuUmaInfoNum", "005" },
                            { "TokuUmaInfoKettoNum", "2018103296" },
                            { "TokuUmaInfoBamei", "サニーオーシャン" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "3" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01010" },
                            { "TokuUmaInfoChokyosiRyakusyo", "中野栄治" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "005" },
                            { "TokuUmaInfoNum", "006" },
                            { "TokuUmaInfoKettoNum", "2019103136" },
                            { "TokuUmaInfoBamei", "シゲルファンノユメ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01023" },
                            { "TokuUmaInfoChokyosiRyakusyo", "伊藤圭三" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "006" },
                            { "TokuUmaInfoNum", "007" },
                            { "TokuUmaInfoKettoNum", "2019101262" },
                            { "TokuUmaInfoBamei", "ショウナンアメリア" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01145" },
                            { "TokuUmaInfoChokyosiRyakusyo", "奥村武" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "007" },
                            { "TokuUmaInfoNum", "008" },
                            { "TokuUmaInfoKettoNum", "2020104942" },
                            { "TokuUmaInfoBamei", "シルフィードレーヴ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01028" },
                            { "TokuUmaInfoChokyosiRyakusyo", "西園正都" },
                            { "TokuUmaInfoFutan", "540" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "008" },
                            { "TokuUmaInfoNum", "009" },
                            { "TokuUmaInfoKettoNum", "2018103485" },
                            { "TokuUmaInfoBamei", "ジャガード" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01143" },
                            { "TokuUmaInfoChokyosiRyakusyo", "和田雄二" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "009" },
                            { "TokuUmaInfoNum", "010" },
                            { "TokuUmaInfoKettoNum", "2018100097" },
                            { "TokuUmaInfoBamei", "スマートルシーダ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01002" },
                            { "TokuUmaInfoChokyosiRyakusyo", "音無秀孝" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "010" },
                            { "TokuUmaInfoNum", "011" },
                            { "TokuUmaInfoKettoNum", "2019101819" },
                            { "TokuUmaInfoBamei", "センタースリール" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01056" },
                            { "TokuUmaInfoChokyosiRyakusyo", "本間忍" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "011" },
                            { "TokuUmaInfoNum", "012" },
                            { "TokuUmaInfoKettoNum", "2017104895" },
                            { "TokuUmaInfoBamei", "ダノンカオス" },
                            { "TokuUmaInfoUmaKigoCD", "05" },
                            { "TokuUmaInfoSexCD", "3" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "00429" },
                            { "TokuUmaInfoChokyosiRyakusyo", "佐々木晶" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "012" },
                            { "TokuUmaInfoNum", "013" },
                            { "TokuUmaInfoKettoNum", "2019104361" },
                            { "TokuUmaInfoBamei", "テーオースパロー" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01110" },
                            { "TokuUmaInfoChokyosiRyakusyo", "清水久詞" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "013" },
                            { "TokuUmaInfoNum", "014" },
                            { "TokuUmaInfoKettoNum", "2019100170" },
                            { "TokuUmaInfoBamei", "デルマカミーラ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01123" },
                            { "TokuUmaInfoChokyosiRyakusyo", "牧田和弥" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "014" },
                            { "TokuUmaInfoNum", "015" },
                            { "TokuUmaInfoKettoNum", "2020101764" },
                            { "TokuUmaInfoBamei", "トーホウキザン" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01135" },
                            { "TokuUmaInfoChokyosiRyakusyo", "高橋康之" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "015" },
                            { "TokuUmaInfoNum", "016" },
                            { "TokuUmaInfoKettoNum", "2019102456" },
                            { "TokuUmaInfoBamei", "ナバロン" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01178" },
                            { "TokuUmaInfoChokyosiRyakusyo", "杉山佳明" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "016" },
                            { "TokuUmaInfoNum", "017" },
                            { "TokuUmaInfoKettoNum", "2019103897" },
                            { "TokuUmaInfoBamei", "ハピネスアゲン" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01024" },
                            { "TokuUmaInfoChokyosiRyakusyo", "萱野浩二" },
                            { "TokuUmaInfoFutan", "560" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "017" },
                            { "TokuUmaInfoNum", "018" },
                            { "TokuUmaInfoKettoNum", "2020104674" },
                            { "TokuUmaInfoBamei", "バレリーナ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01110" },
                            { "TokuUmaInfoChokyosiRyakusyo", "清水久詞" },
                            { "TokuUmaInfoFutan", "540" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "018" },
                            { "TokuUmaInfoNum", "019" },
                            { "TokuUmaInfoKettoNum", "2019107075" },
                            { "TokuUmaInfoBamei", "ブランデーロック" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "01005" },
                            { "TokuUmaInfoChokyosiRyakusyo", "小桧山悟" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "019" },
                            { "TokuUmaInfoNum", "020" },
                            { "TokuUmaInfoKettoNum", "2018105873" },
                            { "TokuUmaInfoBamei", "ミキノバスドラム" },
                            { "TokuUmaInfoUmaKigoCD", "05" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "00437" },
                            { "TokuUmaInfoChokyosiRyakusyo", "南田美知" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "020" },
                            { "TokuUmaInfoNum", "021" },
                            { "TokuUmaInfoKettoNum", "2018106552" },
                            { "TokuUmaInfoBamei", "メイショウフンケイ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "1" },
                            { "TokuUmaInfoTozaiCD", "2" },
                            { "TokuUmaInfoChokyosiCode", "01136" },
                            { "TokuUmaInfoChokyosiRyakusyo", "高橋亮" },
                            { "TokuUmaInfoFutan", "580" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "idYear", "2023" },
                            { "idMonthDay", "0902" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "07" },
                            { "idRaceNum", "09" },
                            { "TokuUmaInfoIdx", "021" },
                            { "TokuUmaInfoNum", "022" },
                            { "TokuUmaInfoKettoNum", "2020106330" },
                            { "TokuUmaInfoBamei", "ラウラーナ" },
                            { "TokuUmaInfoUmaKigoCD", "00" },
                            { "TokuUmaInfoSexCD", "2" },
                            { "TokuUmaInfoTozaiCD", "1" },
                            { "TokuUmaInfoChokyosiCode", "00420" },
                            { "TokuUmaInfoChokyosiRyakusyo", "宗像義忠" },
                            { "TokuUmaInfoFutan", "540" },
                            { "TokuUmaInfoKoryu", "0" }
                        },
                    });
                }
            }
        }

        private static JV_TK_TOKUUMADataBridge NewJV_TK_TOKUUMADataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_TK_TOKUUMA();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_TK_TOKUUMADataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }
    }
}