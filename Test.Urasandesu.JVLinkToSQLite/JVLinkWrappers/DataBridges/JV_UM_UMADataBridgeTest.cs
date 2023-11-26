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
    public class JV_UM_UMADataBridgeTest
    {
        [Test]
        public void JV_UM_UMADataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_UM_UMADataBridge("UM42023082820121015471201407312016090720120402シゲルシチフクジン　　　　　　　　　ｼｹﾞﾙｼﾁﾌｸｼﾞﾝ                         Shigerusitifukujin(JPN)                                     0                   0021041120002127ストーミングホーム　　　　　　　　　1220057322シンバル２　　　　　　　　　　　　　1140002606Machiavellian                       1240023756Try to Catch Me                     1140004069Singspiel                           1240026596Valdara                             1140000948Mr. Prospector                      1240009419Coup de Folie                       1140002171Shareef Dancer                      1240006932It's in the Air                     1140002807In The Wings                        1240005908Glorious Song                       1140001945Darshaan                            1240018715Valverda                            100383伊藤正徳　　　　　　　　　　81154000ダーレー・ジャパン・ファーム　　　　　　　　　　　　　　　　　　　　　　日高町　　　　　　　674004森中　蕃　　　　　　　　　　　　　　　　　　　　　　　　　　　　000095500000000000000000000000000000000020000000000000002000002003003034001000001002002018000000000000000001000000001001002012001000000001000003000000000000000000000000000000000000000000000000000002000000000000000000001000001002002013000000000000000002000000000000000000000000000000000001000000000000000002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001008001000001002001008000000000000000000000000000000000000000000000000000002000000000000000000000001009014044\r\n",
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
                    cmd.AssertTableCreation("NL_UM_UMA", JVDataStructColumns.UM);
                }
            }
        }

        [Test]
        public void JV_UM_UMADataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_UM_UMADataBridge("UM42023082820121015471201407312016090720120402シゲルシチフクジン　　　　　　　　　ｼｹﾞﾙｼﾁﾌｸｼﾞﾝ                         Shigerusitifukujin(JPN)                                     0                   0021041120002127ストーミングホーム　　　　　　　　　1220057322シンバル２　　　　　　　　　　　　　1140002606Machiavellian                       1240023756Try to Catch Me                     1140004069Singspiel                           1240026596Valdara                             1140000948Mr. Prospector                      1240009419Coup de Folie                       1140002171Shareef Dancer                      1240006932It's in the Air                     1140002807In The Wings                        1240005908Glorious Song                       1140001945Darshaan                            1240018715Valverda                            100383伊藤正徳　　　　　　　　　　81154000ダーレー・ジャパン・ファーム　　　　　　　　　　　　　　　　　　　　　　日高町　　　　　　　674004森中　蕃　　　　　　　　　　　　　　　　　　　　　　　　　　　　000095500000000000000000000000000000000020000000000000002000002003003034001000001002002018000000000000000001000000001001002012001000000001000003000000000000000000000000000000000000000000000000000002000000000000000000001000001002002013000000000000000002000000000000000000000000000000000001000000000000000002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001008001000001002001008000000000000000000000000000000000000000000000000000002000000000000000000000001009014044\r\n",
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
        public void JV_UM_UMADataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_UM_UMADataBridge("UM42023082820121015471201407312016090720120402シゲルシチフクジン　　　　　　　　　ｼｹﾞﾙｼﾁﾌｸｼﾞﾝ                         Shigerusitifukujin(JPN)                                     0                   0021041120002127ストーミングホーム　　　　　　　　　1220057322シンバル２　　　　　　　　　　　　　1140002606Machiavellian                       1240023756Try to Catch Me                     1140004069Singspiel                           1240026596Valdara                             1140000948Mr. Prospector                      1240009419Coup de Folie                       1140002171Shareef Dancer                      1240006932It's in the Air                     1140002807In The Wings                        1240005908Glorious Song                       1140001945Darshaan                            1240018715Valverda                            100383伊藤正徳　　　　　　　　　　81154000ダーレー・ジャパン・ファーム　　　　　　　　　　　　　　　　　　　　　　日高町　　　　　　　674004森中　蕃　　　　　　　　　　　　　　　　　　　　　　　　　　　　000095500000000000000000000000000000000020000000000000002000002003003034001000001002002018000000000000000001000000001001002012001000000001000003000000000000000000000000000000000000000000000000000002000000000000000000001000001002002013000000000000000002000000000000000000000000000000000001000000000000000002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001008001000001002001008000000000000000000000000000000000000000000000000000002000000000000000000000001009014044\r\n",
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
                    cmd.AssertRecords("NL_UM_UMA", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "UM" },
                            { "headDataKubun", "4" },
                            { "headMakeDate", "20230828" },
                            { "KettoNum", "2012101547" },
                            { "DelKubun", "1" },
                            { "RegDate", "20140731" },
                            { "DelDate", "20160907" },
                            { "BirthDate", "20120402" },
                            { "Bamei", "シゲルシチフクジン" },
                            { "BameiKana", "ｼｹﾞﾙｼﾁﾌｸｼﾞﾝ" },
                            { "BameiEng", "Shigerusitifukujin(JPN)" },
                            { "ZaikyuFlag", "0" },
                            { "Reserved", "" },
                            { "UmaKigoCD", "00" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "04" },
                            { "Ketto3Info0HansyokuNum", "1120002127" },
                            { "Ketto3Info0Bamei", "ストーミングホーム" },
                            { "Ketto3Info1HansyokuNum", "1220057322" },
                            { "Ketto3Info1Bamei", "シンバル２" },
                            { "Ketto3Info2HansyokuNum", "1140002606" },
                            { "Ketto3Info2Bamei", "Machiavellian" },
                            { "Ketto3Info3HansyokuNum", "1240023756" },
                            { "Ketto3Info3Bamei", "Try to Catch Me" },
                            { "Ketto3Info4HansyokuNum", "1140004069" },
                            { "Ketto3Info4Bamei", "Singspiel" },
                            { "Ketto3Info5HansyokuNum", "1240026596" },
                            { "Ketto3Info5Bamei", "Valdara" },
                            { "Ketto3Info6HansyokuNum", "1140000948" },
                            { "Ketto3Info6Bamei", "Mr. Prospector" },
                            { "Ketto3Info7HansyokuNum", "1240009419" },
                            { "Ketto3Info7Bamei", "Coup de Folie" },
                            { "Ketto3Info8HansyokuNum", "1140002171" },
                            { "Ketto3Info8Bamei", "Shareef Dancer" },
                            { "Ketto3Info9HansyokuNum", "1240006932" },
                            { "Ketto3Info9Bamei", "It's in the Air" },
                            { "Ketto3Info10HansyokuNum", "1140002807" },
                            { "Ketto3Info10Bamei", "In The Wings" },
                            { "Ketto3Info11HansyokuNum", "1240005908" },
                            { "Ketto3Info11Bamei", "Glorious Song" },
                            { "Ketto3Info12HansyokuNum", "1140001945" },
                            { "Ketto3Info12Bamei", "Darshaan" },
                            { "Ketto3Info13HansyokuNum", "1240018715" },
                            { "Ketto3Info13Bamei", "Valverda" },
                            { "TozaiCD", "1" },
                            { "ChokyosiCode", "00383" },
                            { "ChokyosiRyakusyo", "伊藤正徳" },
                            { "Syotai", "" },
                            { "BreederCode", "81154000" },
                            { "BreederName", "ダーレー・ジャパン・ファーム" },
                            { "SanchiName", "日高町" },
                            { "BanusiCode", "674004" },
                            { "BanusiName", "森中　蕃" },
                            { "RuikeiHonsyoHeiti", "000095500" },
                            { "RuikeiHonsyoSyogai", "000000000" },
                            { "RuikeiFukaHeichi", "000000000" },
                            { "RuikeiFukaSyogai", "000000000" },
                            { "RuikeiSyutokuHeichi", "000020000" },
                            { "RuikeiSyutokuSyogai", "000000000" },
                            { "ChakuSogoChakuKaisu0", "002" },
                            { "ChakuSogoChakuKaisu1", "000" },
                            { "ChakuSogoChakuKaisu2", "002" },
                            { "ChakuSogoChakuKaisu3", "003" },
                            { "ChakuSogoChakuKaisu4", "003" },
                            { "ChakuSogoChakuKaisu5", "034" },
                            { "ChakuChuoChakuKaisu0", "001" },
                            { "ChakuChuoChakuKaisu1", "000" },
                            { "ChakuChuoChakuKaisu2", "001" },
                            { "ChakuChuoChakuKaisu3", "002" },
                            { "ChakuChuoChakuKaisu4", "002" },
                            { "ChakuChuoChakuKaisu5", "018" },
                            { "ChakuKaisuBa0ChakuKaisu0", "000" },
                            { "ChakuKaisuBa0ChakuKaisu1", "000" },
                            { "ChakuKaisuBa0ChakuKaisu2", "000" },
                            { "ChakuKaisuBa0ChakuKaisu3", "000" },
                            { "ChakuKaisuBa0ChakuKaisu4", "000" },
                            { "ChakuKaisuBa0ChakuKaisu5", "001" },
                            { "ChakuKaisuBa1ChakuKaisu0", "000" },
                            { "ChakuKaisuBa1ChakuKaisu1", "000" },
                            { "ChakuKaisuBa1ChakuKaisu2", "001" },
                            { "ChakuKaisuBa1ChakuKaisu3", "001" },
                            { "ChakuKaisuBa1ChakuKaisu4", "002" },
                            { "ChakuKaisuBa1ChakuKaisu5", "012" },
                            { "ChakuKaisuBa2ChakuKaisu0", "001" },
                            { "ChakuKaisuBa2ChakuKaisu1", "000" },
                            { "ChakuKaisuBa2ChakuKaisu2", "000" },
                            { "ChakuKaisuBa2ChakuKaisu3", "001" },
                            { "ChakuKaisuBa2ChakuKaisu4", "000" },
                            { "ChakuKaisuBa2ChakuKaisu5", "003" },
                            { "ChakuKaisuBa3ChakuKaisu0", "000" },
                            { "ChakuKaisuBa3ChakuKaisu1", "000" },
                            { "ChakuKaisuBa3ChakuKaisu2", "000" },
                            { "ChakuKaisuBa3ChakuKaisu3", "000" },
                            { "ChakuKaisuBa3ChakuKaisu4", "000" },
                            { "ChakuKaisuBa3ChakuKaisu5", "000" },
                            { "ChakuKaisuBa4ChakuKaisu0", "000" },
                            { "ChakuKaisuBa4ChakuKaisu1", "000" },
                            { "ChakuKaisuBa4ChakuKaisu2", "000" },
                            { "ChakuKaisuBa4ChakuKaisu3", "000" },
                            { "ChakuKaisuBa4ChakuKaisu4", "000" },
                            { "ChakuKaisuBa4ChakuKaisu5", "000" },
                            { "ChakuKaisuBa5ChakuKaisu0", "000" },
                            { "ChakuKaisuBa5ChakuKaisu1", "000" },
                            { "ChakuKaisuBa5ChakuKaisu2", "000" },
                            { "ChakuKaisuBa5ChakuKaisu3", "000" },
                            { "ChakuKaisuBa5ChakuKaisu4", "000" },
                            { "ChakuKaisuBa5ChakuKaisu5", "002" },
                            { "ChakuKaisuBa6ChakuKaisu0", "000" },
                            { "ChakuKaisuBa6ChakuKaisu1", "000" },
                            { "ChakuKaisuBa6ChakuKaisu2", "000" },
                            { "ChakuKaisuBa6ChakuKaisu3", "000" },
                            { "ChakuKaisuBa6ChakuKaisu4", "000" },
                            { "ChakuKaisuBa6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu0", "001" },
                            { "ChakuKaisuJyotai0ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu2", "001" },
                            { "ChakuKaisuJyotai0ChakuKaisu3", "002" },
                            { "ChakuKaisuJyotai0ChakuKaisu4", "002" },
                            { "ChakuKaisuJyotai0ChakuKaisu5", "013" },
                            { "ChakuKaisuJyotai1ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai2ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai5ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori0ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori1ChakuKaisu0", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu2", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu3", "002" },
                            { "ChakuKaisuKyori1ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori2ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu5", "002" },
                            { "ChakuKaisuKyori5ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu5", "000" },
                            { "Kyakusitu0", "000" },
                            { "Kyakusitu1", "001" },
                            { "Kyakusitu2", "009" },
                            { "Kyakusitu3", "014" },
                            { "RaceCount", "044" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_UM_UMADataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_UM_UMADataBridge("UM42023082820121015471201407312016090720120402シゲルシチフクジン　　　　　　　　　ｼｹﾞﾙｼﾁﾌｸｼﾞﾝ                         Shigerusitifukujin(JPN)                                     0                   0021041120002127ストーミングホーム　　　　　　　　　1220057322シンバル２　　　　　　　　　　　　　1140002606Machiavellian                       1240023756Try to Catch Me                     1140004069Singspiel                           1240026596Valdara                             1140000948Mr. Prospector                      1240009419Coup de Folie                       1140002171Shareef Dancer                      1240006932It's in the Air                     1140002807In The Wings                        1240005908Glorious Song                       1140001945Darshaan                            1240018715Valverda                            100383伊藤正徳　　　　　　　　　　81154000ダーレー・ジャパン・ファーム　　　　　　　　　　　　　　　　　　　　　　日高町　　　　　　　674004森中　蕃　　　　　　　　　　　　　　　　　　　　　　　　　　　　000095500000000000000000000000000000000020000000000000002000002003003034001000001002002018000000000000000001000000001001002012001000000001000003000000000000000000000000000000000000000000000000000002000000000000000000001000001002002013000000000000000002000000000000000000000000000000000001000000000000000002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001008001000001002001008000000000000000000000000000000000000000000000000000002000000000000000000000001009014044\r\n",
                                                     JVOpenOptions.Normal);
            var dataBridge2 = NewJV_UM_UMADataBridge("UM42023082820121015471201407312016090720120402スゲルシチフクジン　　　　　　　　　ｽｹﾞﾙｼﾁﾌｸｼﾞﾝ                         Shugerusitifukujin(JPN)                                     0                   0021041120002127ストーミングホーム　　　　　　　　　1220057322シンバル２　　　　　　　　　　　　　1140002606Machiavellian                       1240023756Try to Catch Me                     1140004069Singspiel                           1240026596Valdara                             1140000948Mr. Prospector                      1240009419Coup de Folie                       1140002171Shareef Dancer                      1240006932It's in the Air                     1140002807In The Wings                        1240005908Glorious Song                       1140001945Darshaan                            1240018715Valverda                            100383伊藤正徳　　　　　　　　　　81154000ダーレー・ジャパン・ファーム　　　　　　　　　　　　　　　　　　　　　　日高町　　　　　　　674004森中　蕃　　　　　　　　　　　　　　　　　　　　　　　　　　　　000095500000000000000000000000000000000020000000000000002000002003003034001000001002002018000000000000000001000000001001002012001000000001000003000000000000000000000000000000000000000000000000000002000000000000000000001000001002002013000000000000000002000000000000000000000000000000000001000000000000000002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001008001000001002001008000000000000000000000000000000000000000000000000000002000000000000000000000001009014044\r\n",
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
                    cmd.AssertRecords("NL_UM_UMA", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "UM" },
                            { "headDataKubun", "4" },
                            { "headMakeDate", "20230828" },
                            { "KettoNum", "2012101547" },
                            { "DelKubun", "1" },
                            { "RegDate", "20140731" },
                            { "DelDate", "20160907" },
                            { "BirthDate", "20120402" },
                            { "Bamei", "スゲルシチフクジン" },
                            { "BameiKana", "ｽｹﾞﾙｼﾁﾌｸｼﾞﾝ" },
                            { "BameiEng", "Shugerusitifukujin(JPN)" },
                            { "ZaikyuFlag", "0" },
                            { "Reserved", "" },
                            { "UmaKigoCD", "00" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "04" },
                            { "Ketto3Info0HansyokuNum", "1120002127" },
                            { "Ketto3Info0Bamei", "ストーミングホーム" },
                            { "Ketto3Info1HansyokuNum", "1220057322" },
                            { "Ketto3Info1Bamei", "シンバル２" },
                            { "Ketto3Info2HansyokuNum", "1140002606" },
                            { "Ketto3Info2Bamei", "Machiavellian" },
                            { "Ketto3Info3HansyokuNum", "1240023756" },
                            { "Ketto3Info3Bamei", "Try to Catch Me" },
                            { "Ketto3Info4HansyokuNum", "1140004069" },
                            { "Ketto3Info4Bamei", "Singspiel" },
                            { "Ketto3Info5HansyokuNum", "1240026596" },
                            { "Ketto3Info5Bamei", "Valdara" },
                            { "Ketto3Info6HansyokuNum", "1140000948" },
                            { "Ketto3Info6Bamei", "Mr. Prospector" },
                            { "Ketto3Info7HansyokuNum", "1240009419" },
                            { "Ketto3Info7Bamei", "Coup de Folie" },
                            { "Ketto3Info8HansyokuNum", "1140002171" },
                            { "Ketto3Info8Bamei", "Shareef Dancer" },
                            { "Ketto3Info9HansyokuNum", "1240006932" },
                            { "Ketto3Info9Bamei", "It's in the Air" },
                            { "Ketto3Info10HansyokuNum", "1140002807" },
                            { "Ketto3Info10Bamei", "In The Wings" },
                            { "Ketto3Info11HansyokuNum", "1240005908" },
                            { "Ketto3Info11Bamei", "Glorious Song" },
                            { "Ketto3Info12HansyokuNum", "1140001945" },
                            { "Ketto3Info12Bamei", "Darshaan" },
                            { "Ketto3Info13HansyokuNum", "1240018715" },
                            { "Ketto3Info13Bamei", "Valverda" },
                            { "TozaiCD", "1" },
                            { "ChokyosiCode", "00383" },
                            { "ChokyosiRyakusyo", "伊藤正徳" },
                            { "Syotai", "" },
                            { "BreederCode", "81154000" },
                            { "BreederName", "ダーレー・ジャパン・ファーム" },
                            { "SanchiName", "日高町" },
                            { "BanusiCode", "674004" },
                            { "BanusiName", "森中　蕃" },
                            { "RuikeiHonsyoHeiti", "000095500" },
                            { "RuikeiHonsyoSyogai", "000000000" },
                            { "RuikeiFukaHeichi", "000000000" },
                            { "RuikeiFukaSyogai", "000000000" },
                            { "RuikeiSyutokuHeichi", "000020000" },
                            { "RuikeiSyutokuSyogai", "000000000" },
                            { "ChakuSogoChakuKaisu0", "002" },
                            { "ChakuSogoChakuKaisu1", "000" },
                            { "ChakuSogoChakuKaisu2", "002" },
                            { "ChakuSogoChakuKaisu3", "003" },
                            { "ChakuSogoChakuKaisu4", "003" },
                            { "ChakuSogoChakuKaisu5", "034" },
                            { "ChakuChuoChakuKaisu0", "001" },
                            { "ChakuChuoChakuKaisu1", "000" },
                            { "ChakuChuoChakuKaisu2", "001" },
                            { "ChakuChuoChakuKaisu3", "002" },
                            { "ChakuChuoChakuKaisu4", "002" },
                            { "ChakuChuoChakuKaisu5", "018" },
                            { "ChakuKaisuBa0ChakuKaisu0", "000" },
                            { "ChakuKaisuBa0ChakuKaisu1", "000" },
                            { "ChakuKaisuBa0ChakuKaisu2", "000" },
                            { "ChakuKaisuBa0ChakuKaisu3", "000" },
                            { "ChakuKaisuBa0ChakuKaisu4", "000" },
                            { "ChakuKaisuBa0ChakuKaisu5", "001" },
                            { "ChakuKaisuBa1ChakuKaisu0", "000" },
                            { "ChakuKaisuBa1ChakuKaisu1", "000" },
                            { "ChakuKaisuBa1ChakuKaisu2", "001" },
                            { "ChakuKaisuBa1ChakuKaisu3", "001" },
                            { "ChakuKaisuBa1ChakuKaisu4", "002" },
                            { "ChakuKaisuBa1ChakuKaisu5", "012" },
                            { "ChakuKaisuBa2ChakuKaisu0", "001" },
                            { "ChakuKaisuBa2ChakuKaisu1", "000" },
                            { "ChakuKaisuBa2ChakuKaisu2", "000" },
                            { "ChakuKaisuBa2ChakuKaisu3", "001" },
                            { "ChakuKaisuBa2ChakuKaisu4", "000" },
                            { "ChakuKaisuBa2ChakuKaisu5", "003" },
                            { "ChakuKaisuBa3ChakuKaisu0", "000" },
                            { "ChakuKaisuBa3ChakuKaisu1", "000" },
                            { "ChakuKaisuBa3ChakuKaisu2", "000" },
                            { "ChakuKaisuBa3ChakuKaisu3", "000" },
                            { "ChakuKaisuBa3ChakuKaisu4", "000" },
                            { "ChakuKaisuBa3ChakuKaisu5", "000" },
                            { "ChakuKaisuBa4ChakuKaisu0", "000" },
                            { "ChakuKaisuBa4ChakuKaisu1", "000" },
                            { "ChakuKaisuBa4ChakuKaisu2", "000" },
                            { "ChakuKaisuBa4ChakuKaisu3", "000" },
                            { "ChakuKaisuBa4ChakuKaisu4", "000" },
                            { "ChakuKaisuBa4ChakuKaisu5", "000" },
                            { "ChakuKaisuBa5ChakuKaisu0", "000" },
                            { "ChakuKaisuBa5ChakuKaisu1", "000" },
                            { "ChakuKaisuBa5ChakuKaisu2", "000" },
                            { "ChakuKaisuBa5ChakuKaisu3", "000" },
                            { "ChakuKaisuBa5ChakuKaisu4", "000" },
                            { "ChakuKaisuBa5ChakuKaisu5", "002" },
                            { "ChakuKaisuBa6ChakuKaisu0", "000" },
                            { "ChakuKaisuBa6ChakuKaisu1", "000" },
                            { "ChakuKaisuBa6ChakuKaisu2", "000" },
                            { "ChakuKaisuBa6ChakuKaisu3", "000" },
                            { "ChakuKaisuBa6ChakuKaisu4", "000" },
                            { "ChakuKaisuBa6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu0", "001" },
                            { "ChakuKaisuJyotai0ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu2", "001" },
                            { "ChakuKaisuJyotai0ChakuKaisu3", "002" },
                            { "ChakuKaisuJyotai0ChakuKaisu4", "002" },
                            { "ChakuKaisuJyotai0ChakuKaisu5", "013" },
                            { "ChakuKaisuJyotai1ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai2ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai5ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori0ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori1ChakuKaisu0", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu2", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu3", "002" },
                            { "ChakuKaisuKyori1ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori2ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu5", "002" },
                            { "ChakuKaisuKyori5ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu5", "000" },
                            { "Kyakusitu0", "000" },
                            { "Kyakusitu1", "001" },
                            { "Kyakusitu2", "009" },
                            { "Kyakusitu3", "014" },
                            { "RaceCount", "044" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_UM_UMA_V4802DataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_UM_UMA_V4802DataBridge("UM42023080720131012461201511122019121120130508スケールアップ　　　　　　　　　　　ｽｹｰﾙｱｯﾌﾟ                            Scale Up(JPN)                                               0                   00110411202216カネヒキリ　　　　　　　　　　　　　12251079ダイコーマリーン　　　　　　　　　　11201542フジキセキ　　　　　　　　　　　　　12242898ライフアウトゼア　　　　　　　　　　11201450シャンハイ　　　　　　　　　　　　　12240748ミヤラビ　　　　　　　　　　　　　　11201232サンデーサイレンス　　　　　　　　　12229223ミルレーサー　　　　　　　　　　　　11401570Deputy Minister                     12413953Silver Valley                       11402134Procida                             12409334Korveya                             11200556モガミ　　　　　　　　　　　　　　　12226425ライラツクレデイ　　　　　　　　　　101080清水英克　　　　　　　　　　600330アサヒ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　新冠町　　　　　　　330031高岡　浩行　　　　　　　　　　　　　　　　　　　　　　　　　　　000248200000000000000000000000000000000020000000000000001005002001003023001005001001003012000000000000000000000000000000000001000000000000000000000000000000000000001002000000001003000003001001002008000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000005001001002004000000000000000004001000000000001002000000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000003001001001008001002000000002003000000000000000000008009003003035\r\n",
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
                    cmd.AssertTableCreation("NL_UM_UMA", JVDataStructColumns.UM_V4802);
                }
            }
        }

        [Test]
        public void JV_UM_UMA_V4802DataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_UM_UMA_V4802DataBridge("UM42023080720131012461201511122019121120130508スケールアップ　　　　　　　　　　　ｽｹｰﾙｱｯﾌﾟ                            Scale Up(JPN)                                               0                   00110411202216カネヒキリ　　　　　　　　　　　　　12251079ダイコーマリーン　　　　　　　　　　11201542フジキセキ　　　　　　　　　　　　　12242898ライフアウトゼア　　　　　　　　　　11201450シャンハイ　　　　　　　　　　　　　12240748ミヤラビ　　　　　　　　　　　　　　11201232サンデーサイレンス　　　　　　　　　12229223ミルレーサー　　　　　　　　　　　　11401570Deputy Minister                     12413953Silver Valley                       11402134Procida                             12409334Korveya                             11200556モガミ　　　　　　　　　　　　　　　12226425ライラツクレデイ　　　　　　　　　　101080清水英克　　　　　　　　　　600330アサヒ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　新冠町　　　　　　　330031高岡　浩行　　　　　　　　　　　　　　　　　　　　　　　　　　　000248200000000000000000000000000000000020000000000000001005002001003023001005001001003012000000000000000000000000000000000001000000000000000000000000000000000000001002000000001003000003001001002008000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000005001001002004000000000000000004001000000000001002000000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000003001001001008001002000000002003000000000000000000008009003003035\r\n",
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
        public void JV_UM_UMA_V4802DataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_UM_UMA_V4802DataBridge("UM42023080720131012461201511122019121120130508スケールアップ　　　　　　　　　　　ｽｹｰﾙｱｯﾌﾟ                            Scale Up(JPN)                                               0                   00110411202216カネヒキリ　　　　　　　　　　　　　12251079ダイコーマリーン　　　　　　　　　　11201542フジキセキ　　　　　　　　　　　　　12242898ライフアウトゼア　　　　　　　　　　11201450シャンハイ　　　　　　　　　　　　　12240748ミヤラビ　　　　　　　　　　　　　　11201232サンデーサイレンス　　　　　　　　　12229223ミルレーサー　　　　　　　　　　　　11401570Deputy Minister                     12413953Silver Valley                       11402134Procida                             12409334Korveya                             11200556モガミ　　　　　　　　　　　　　　　12226425ライラツクレデイ　　　　　　　　　　101080清水英克　　　　　　　　　　600330アサヒ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　新冠町　　　　　　　330031高岡　浩行　　　　　　　　　　　　　　　　　　　　　　　　　　　000248200000000000000000000000000000000020000000000000001005002001003023001005001001003012000000000000000000000000000000000001000000000000000000000000000000000000001002000000001003000003001001002008000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000005001001002004000000000000000004001000000000001002000000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000003001001001008001002000000002003000000000000000000008009003003035\r\n",
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
                    cmd.AssertRecords("NL_UM_UMA", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "UM" },
                            { "headDataKubun", "4" },
                            { "headMakeDate", "20230807" },
                            { "KettoNum", "2013101246" },
                            { "DelKubun", "1" },
                            { "RegDate", "20151112" },
                            { "DelDate", "20191211" },
                            { "BirthDate", "20130508" },
                            { "Bamei", "スケールアップ" },
                            { "BameiKana", "ｽｹｰﾙｱｯﾌﾟ" },
                            { "BameiEng", "Scale Up(JPN)" },
                            { "ZaikyuFlag", "0" },
                            { "Reserved", "" },
                            { "UmaKigoCD", "00" },
                            { "SexCD", "1" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "04" },
                            { "Ketto3Info0HansyokuNum", "1120002216" },
                            { "Ketto3Info0Bamei", "カネヒキリ" },
                            { "Ketto3Info1HansyokuNum", "1220051079" },
                            { "Ketto3Info1Bamei", "ダイコーマリーン" },
                            { "Ketto3Info2HansyokuNum", "1120001542" },
                            { "Ketto3Info2Bamei", "フジキセキ" },
                            { "Ketto3Info3HansyokuNum", "1220042898" },
                            { "Ketto3Info3Bamei", "ライフアウトゼア" },
                            { "Ketto3Info4HansyokuNum", "1120001450" },
                            { "Ketto3Info4Bamei", "シャンハイ" },
                            { "Ketto3Info5HansyokuNum", "1220040748" },
                            { "Ketto3Info5Bamei", "ミヤラビ" },
                            { "Ketto3Info6HansyokuNum", "1120001232" },
                            { "Ketto3Info6Bamei", "サンデーサイレンス" },
                            { "Ketto3Info7HansyokuNum", "1220029223" },
                            { "Ketto3Info7Bamei", "ミルレーサー" },
                            { "Ketto3Info8HansyokuNum", "1140001570" },
                            { "Ketto3Info8Bamei", "Deputy Minister" },
                            { "Ketto3Info9HansyokuNum", "1240013953" },
                            { "Ketto3Info9Bamei", "Silver Valley" },
                            { "Ketto3Info10HansyokuNum", "1140002134" },
                            { "Ketto3Info10Bamei", "Procida" },
                            { "Ketto3Info11HansyokuNum", "1240009334" },
                            { "Ketto3Info11Bamei", "Korveya" },
                            { "Ketto3Info12HansyokuNum", "1120000556" },
                            { "Ketto3Info12Bamei", "モガミ" },
                            { "Ketto3Info13HansyokuNum", "1220026425" },
                            { "Ketto3Info13Bamei", "ライラツクレデイ" },
                            { "TozaiCD", "1" },
                            { "ChokyosiCode", "01080" },
                            { "ChokyosiRyakusyo", "清水英克" },
                            { "Syotai", "" },
                            { "BreederCode", "60033000" },
                            { "BreederName", "アサヒ牧場" },
                            { "SanchiName", "新冠町" },
                            { "BanusiCode", "330031" },
                            { "BanusiName", "高岡　浩行" },
                            { "RuikeiHonsyoHeiti", "000248200" },
                            { "RuikeiHonsyoSyogai", "000000000" },
                            { "RuikeiFukaHeichi", "000000000" },
                            { "RuikeiFukaSyogai", "000000000" },
                            { "RuikeiSyutokuHeichi", "000020000" },
                            { "RuikeiSyutokuSyogai", "000000000" },
                            { "ChakuSogoChakuKaisu0", "001" },
                            { "ChakuSogoChakuKaisu1", "005" },
                            { "ChakuSogoChakuKaisu2", "002" },
                            { "ChakuSogoChakuKaisu3", "001" },
                            { "ChakuSogoChakuKaisu4", "003" },
                            { "ChakuSogoChakuKaisu5", "023" },
                            { "ChakuChuoChakuKaisu0", "001" },
                            { "ChakuChuoChakuKaisu1", "005" },
                            { "ChakuChuoChakuKaisu2", "001" },
                            { "ChakuChuoChakuKaisu3", "001" },
                            { "ChakuChuoChakuKaisu4", "003" },
                            { "ChakuChuoChakuKaisu5", "012" },
                            { "ChakuKaisuBa0ChakuKaisu0", "000" },
                            { "ChakuKaisuBa0ChakuKaisu1", "000" },
                            { "ChakuKaisuBa0ChakuKaisu2", "000" },
                            { "ChakuKaisuBa0ChakuKaisu3", "000" },
                            { "ChakuKaisuBa0ChakuKaisu4", "000" },
                            { "ChakuKaisuBa0ChakuKaisu5", "000" },
                            { "ChakuKaisuBa1ChakuKaisu0", "000" },
                            { "ChakuKaisuBa1ChakuKaisu1", "000" },
                            { "ChakuKaisuBa1ChakuKaisu2", "000" },
                            { "ChakuKaisuBa1ChakuKaisu3", "000" },
                            { "ChakuKaisuBa1ChakuKaisu4", "000" },
                            { "ChakuKaisuBa1ChakuKaisu5", "001" },
                            { "ChakuKaisuBa2ChakuKaisu0", "000" },
                            { "ChakuKaisuBa2ChakuKaisu1", "000" },
                            { "ChakuKaisuBa2ChakuKaisu2", "000" },
                            { "ChakuKaisuBa2ChakuKaisu3", "000" },
                            { "ChakuKaisuBa2ChakuKaisu4", "000" },
                            { "ChakuKaisuBa2ChakuKaisu5", "000" },
                            { "ChakuKaisuBa3ChakuKaisu0", "000" },
                            { "ChakuKaisuBa3ChakuKaisu1", "000" },
                            { "ChakuKaisuBa3ChakuKaisu2", "000" },
                            { "ChakuKaisuBa3ChakuKaisu3", "000" },
                            { "ChakuKaisuBa3ChakuKaisu4", "000" },
                            { "ChakuKaisuBa3ChakuKaisu5", "000" },
                            { "ChakuKaisuBa4ChakuKaisu0", "001" },
                            { "ChakuKaisuBa4ChakuKaisu1", "002" },
                            { "ChakuKaisuBa4ChakuKaisu2", "000" },
                            { "ChakuKaisuBa4ChakuKaisu3", "000" },
                            { "ChakuKaisuBa4ChakuKaisu4", "001" },
                            { "ChakuKaisuBa4ChakuKaisu5", "003" },
                            { "ChakuKaisuBa5ChakuKaisu0", "000" },
                            { "ChakuKaisuBa5ChakuKaisu1", "003" },
                            { "ChakuKaisuBa5ChakuKaisu2", "001" },
                            { "ChakuKaisuBa5ChakuKaisu3", "001" },
                            { "ChakuKaisuBa5ChakuKaisu4", "002" },
                            { "ChakuKaisuBa5ChakuKaisu5", "008" },
                            { "ChakuKaisuBa6ChakuKaisu0", "000" },
                            { "ChakuKaisuBa6ChakuKaisu1", "000" },
                            { "ChakuKaisuBa6ChakuKaisu2", "000" },
                            { "ChakuKaisuBa6ChakuKaisu3", "000" },
                            { "ChakuKaisuBa6ChakuKaisu4", "000" },
                            { "ChakuKaisuBa6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai1ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu1", "005" },
                            { "ChakuKaisuJyotai4ChakuKaisu2", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu3", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu4", "002" },
                            { "ChakuKaisuJyotai4ChakuKaisu5", "004" },
                            { "ChakuKaisuJyotai5ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu5", "004" },
                            { "ChakuKaisuJyotai6ChakuKaisu0", "001" },
                            { "ChakuKaisuJyotai6ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu4", "001" },
                            { "ChakuKaisuJyotai6ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai7ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai8ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu5", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu1", "003" },
                            { "ChakuKaisuKyori3ChakuKaisu2", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu3", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori4ChakuKaisu0", "001" },
                            { "ChakuKaisuKyori4ChakuKaisu1", "002" },
                            { "ChakuKaisuKyori4ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu4", "002" },
                            { "ChakuKaisuKyori4ChakuKaisu5", "003" },
                            { "ChakuKaisuKyori5ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu5", "000" },
                            { "Kyakusitu0", "008" },
                            { "Kyakusitu1", "009" },
                            { "Kyakusitu2", "003" },
                            { "Kyakusitu3", "003" },
                            { "RaceCount", "035" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_UM_UMA_V4802DataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_UM_UMA_V4802DataBridge("UM42023080720131012461201511122019121120130508スケールアップ　　　　　　　　　　　ｽｹｰﾙｱｯﾌﾟ                            Scale Up(JPN)                                               0                   00110411202216カネヒキリ　　　　　　　　　　　　　12251079ダイコーマリーン　　　　　　　　　　11201542フジキセキ　　　　　　　　　　　　　12242898ライフアウトゼア　　　　　　　　　　11201450シャンハイ　　　　　　　　　　　　　12240748ミヤラビ　　　　　　　　　　　　　　11201232サンデーサイレンス　　　　　　　　　12229223ミルレーサー　　　　　　　　　　　　11401570Deputy Minister                     12413953Silver Valley                       11402134Procida                             12409334Korveya                             11200556モガミ　　　　　　　　　　　　　　　12226425ライラツクレデイ　　　　　　　　　　101080清水英克　　　　　　　　　　600330アサヒ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　新冠町　　　　　　　330031高岡　浩行　　　　　　　　　　　　　　　　　　　　　　　　　　　000248200000000000000000000000000000000020000000000000001005002001003023001005001001003012000000000000000000000000000000000001000000000000000000000000000000000000001002000000001003000003001001002008000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000005001001002004000000000000000004001000000000001002000000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000003001001001008001002000000002003000000000000000000008009003003035\r\n",
                                                           JVOpenOptions.Normal);
            var dataBridge2 = NewJV_UM_UMA_V4802DataBridge("UM42023080720131012461201511122019121120130508セケールアップ　　　　　　　　　　　ｾｹｰﾙｱｯﾌﾟ                            Sacle Up(JPN)                                               0                   00110411202216カネヒキリ　　　　　　　　　　　　　12251079ダイコーマリーン　　　　　　　　　　11201542フジキセキ　　　　　　　　　　　　　12242898ライフアウトゼア　　　　　　　　　　11201450シャンハイ　　　　　　　　　　　　　12240748ミヤラビ　　　　　　　　　　　　　　11201232サンデーサイレンス　　　　　　　　　12229223ミルレーサー　　　　　　　　　　　　11401570Deputy Minister                     12413953Silver Valley                       11402134Procida                             12409334Korveya                             11200556モガミ　　　　　　　　　　　　　　　12226425ライラツクレデイ　　　　　　　　　　101080清水英克　　　　　　　　　　600330アサヒ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　新冠町　　　　　　　330031高岡　浩行　　　　　　　　　　　　　　　　　　　　　　　　　　　000248200000000000000000000000000000000020000000000000001005002001003023001005001001003012000000000000000000000000000000000001000000000000000000000000000000000000001002000000001003000003001001002008000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000005001001002004000000000000000004001000000000001002000000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000003001001001008001002000000002003000000000000000000008009003003035\r\n",
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
                    cmd.AssertRecords("NL_UM_UMA", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "UM" },
                            { "headDataKubun", "4" },
                            { "headMakeDate", "20230807" },
                            { "KettoNum", "2013101246" },
                            { "DelKubun", "1" },
                            { "RegDate", "20151112" },
                            { "DelDate", "20191211" },
                            { "BirthDate", "20130508" },
                            { "Bamei", "セケールアップ" },
                            { "BameiKana", "ｾｹｰﾙｱｯﾌﾟ" },
                            { "BameiEng", "Sacle Up(JPN)" },
                            { "ZaikyuFlag", "0" },
                            { "Reserved", "" },
                            { "UmaKigoCD", "00" },
                            { "SexCD", "1" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "04" },
                            { "Ketto3Info0HansyokuNum", "1120002216" },
                            { "Ketto3Info0Bamei", "カネヒキリ" },
                            { "Ketto3Info1HansyokuNum", "1220051079" },
                            { "Ketto3Info1Bamei", "ダイコーマリーン" },
                            { "Ketto3Info2HansyokuNum", "1120001542" },
                            { "Ketto3Info2Bamei", "フジキセキ" },
                            { "Ketto3Info3HansyokuNum", "1220042898" },
                            { "Ketto3Info3Bamei", "ライフアウトゼア" },
                            { "Ketto3Info4HansyokuNum", "1120001450" },
                            { "Ketto3Info4Bamei", "シャンハイ" },
                            { "Ketto3Info5HansyokuNum", "1220040748" },
                            { "Ketto3Info5Bamei", "ミヤラビ" },
                            { "Ketto3Info6HansyokuNum", "1120001232" },
                            { "Ketto3Info6Bamei", "サンデーサイレンス" },
                            { "Ketto3Info7HansyokuNum", "1220029223" },
                            { "Ketto3Info7Bamei", "ミルレーサー" },
                            { "Ketto3Info8HansyokuNum", "1140001570" },
                            { "Ketto3Info8Bamei", "Deputy Minister" },
                            { "Ketto3Info9HansyokuNum", "1240013953" },
                            { "Ketto3Info9Bamei", "Silver Valley" },
                            { "Ketto3Info10HansyokuNum", "1140002134" },
                            { "Ketto3Info10Bamei", "Procida" },
                            { "Ketto3Info11HansyokuNum", "1240009334" },
                            { "Ketto3Info11Bamei", "Korveya" },
                            { "Ketto3Info12HansyokuNum", "1120000556" },
                            { "Ketto3Info12Bamei", "モガミ" },
                            { "Ketto3Info13HansyokuNum", "1220026425" },
                            { "Ketto3Info13Bamei", "ライラツクレデイ" },
                            { "TozaiCD", "1" },
                            { "ChokyosiCode", "01080" },
                            { "ChokyosiRyakusyo", "清水英克" },
                            { "Syotai", "" },
                            { "BreederCode", "60033000" },
                            { "BreederName", "アサヒ牧場" },
                            { "SanchiName", "新冠町" },
                            { "BanusiCode", "330031" },
                            { "BanusiName", "高岡　浩行" },
                            { "RuikeiHonsyoHeiti", "000248200" },
                            { "RuikeiHonsyoSyogai", "000000000" },
                            { "RuikeiFukaHeichi", "000000000" },
                            { "RuikeiFukaSyogai", "000000000" },
                            { "RuikeiSyutokuHeichi", "000020000" },
                            { "RuikeiSyutokuSyogai", "000000000" },
                            { "ChakuSogoChakuKaisu0", "001" },
                            { "ChakuSogoChakuKaisu1", "005" },
                            { "ChakuSogoChakuKaisu2", "002" },
                            { "ChakuSogoChakuKaisu3", "001" },
                            { "ChakuSogoChakuKaisu4", "003" },
                            { "ChakuSogoChakuKaisu5", "023" },
                            { "ChakuChuoChakuKaisu0", "001" },
                            { "ChakuChuoChakuKaisu1", "005" },
                            { "ChakuChuoChakuKaisu2", "001" },
                            { "ChakuChuoChakuKaisu3", "001" },
                            { "ChakuChuoChakuKaisu4", "003" },
                            { "ChakuChuoChakuKaisu5", "012" },
                            { "ChakuKaisuBa0ChakuKaisu0", "000" },
                            { "ChakuKaisuBa0ChakuKaisu1", "000" },
                            { "ChakuKaisuBa0ChakuKaisu2", "000" },
                            { "ChakuKaisuBa0ChakuKaisu3", "000" },
                            { "ChakuKaisuBa0ChakuKaisu4", "000" },
                            { "ChakuKaisuBa0ChakuKaisu5", "000" },
                            { "ChakuKaisuBa1ChakuKaisu0", "000" },
                            { "ChakuKaisuBa1ChakuKaisu1", "000" },
                            { "ChakuKaisuBa1ChakuKaisu2", "000" },
                            { "ChakuKaisuBa1ChakuKaisu3", "000" },
                            { "ChakuKaisuBa1ChakuKaisu4", "000" },
                            { "ChakuKaisuBa1ChakuKaisu5", "001" },
                            { "ChakuKaisuBa2ChakuKaisu0", "000" },
                            { "ChakuKaisuBa2ChakuKaisu1", "000" },
                            { "ChakuKaisuBa2ChakuKaisu2", "000" },
                            { "ChakuKaisuBa2ChakuKaisu3", "000" },
                            { "ChakuKaisuBa2ChakuKaisu4", "000" },
                            { "ChakuKaisuBa2ChakuKaisu5", "000" },
                            { "ChakuKaisuBa3ChakuKaisu0", "000" },
                            { "ChakuKaisuBa3ChakuKaisu1", "000" },
                            { "ChakuKaisuBa3ChakuKaisu2", "000" },
                            { "ChakuKaisuBa3ChakuKaisu3", "000" },
                            { "ChakuKaisuBa3ChakuKaisu4", "000" },
                            { "ChakuKaisuBa3ChakuKaisu5", "000" },
                            { "ChakuKaisuBa4ChakuKaisu0", "001" },
                            { "ChakuKaisuBa4ChakuKaisu1", "002" },
                            { "ChakuKaisuBa4ChakuKaisu2", "000" },
                            { "ChakuKaisuBa4ChakuKaisu3", "000" },
                            { "ChakuKaisuBa4ChakuKaisu4", "001" },
                            { "ChakuKaisuBa4ChakuKaisu5", "003" },
                            { "ChakuKaisuBa5ChakuKaisu0", "000" },
                            { "ChakuKaisuBa5ChakuKaisu1", "003" },
                            { "ChakuKaisuBa5ChakuKaisu2", "001" },
                            { "ChakuKaisuBa5ChakuKaisu3", "001" },
                            { "ChakuKaisuBa5ChakuKaisu4", "002" },
                            { "ChakuKaisuBa5ChakuKaisu5", "008" },
                            { "ChakuKaisuBa6ChakuKaisu0", "000" },
                            { "ChakuKaisuBa6ChakuKaisu1", "000" },
                            { "ChakuKaisuBa6ChakuKaisu2", "000" },
                            { "ChakuKaisuBa6ChakuKaisu3", "000" },
                            { "ChakuKaisuBa6ChakuKaisu4", "000" },
                            { "ChakuKaisuBa6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai1ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu1", "005" },
                            { "ChakuKaisuJyotai4ChakuKaisu2", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu3", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu4", "002" },
                            { "ChakuKaisuJyotai4ChakuKaisu5", "004" },
                            { "ChakuKaisuJyotai5ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu5", "004" },
                            { "ChakuKaisuJyotai6ChakuKaisu0", "001" },
                            { "ChakuKaisuJyotai6ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu4", "001" },
                            { "ChakuKaisuJyotai6ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai7ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai8ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu5", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu1", "003" },
                            { "ChakuKaisuKyori3ChakuKaisu2", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu3", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori4ChakuKaisu0", "001" },
                            { "ChakuKaisuKyori4ChakuKaisu1", "002" },
                            { "ChakuKaisuKyori4ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu4", "002" },
                            { "ChakuKaisuKyori4ChakuKaisu5", "003" },
                            { "ChakuKaisuKyori5ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu5", "000" },
                            { "Kyakusitu0", "008" },
                            { "Kyakusitu1", "009" },
                            { "Kyakusitu2", "003" },
                            { "Kyakusitu3", "003" },
                            { "RaceCount", "035" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_UM_UMA_V4802DataBridge_BuildUpInsertCommand_can_create_an_compatible_insert_command()
        {
            // Arrange
            var dataBridge1 = NewJV_UM_UMA_V4802DataBridge("UM42023080720131012461201511122019121120130508スケールアップ　　　　　　　　　　　ｽｹｰﾙｱｯﾌﾟ                            Scale Up(JPN)                                               0                   00110411202216カネヒキリ　　　　　　　　　　　　　12251079ダイコーマリーン　　　　　　　　　　11201542フジキセキ　　　　　　　　　　　　　12242898ライフアウトゼア　　　　　　　　　　11201450シャンハイ　　　　　　　　　　　　　12240748ミヤラビ　　　　　　　　　　　　　　11201232サンデーサイレンス　　　　　　　　　12229223ミルレーサー　　　　　　　　　　　　11401570Deputy Minister                     12413953Silver Valley                       11402134Procida                             12409334Korveya                             11200556モガミ　　　　　　　　　　　　　　　12226425ライラツクレデイ　　　　　　　　　　101080清水英克　　　　　　　　　　600330アサヒ牧場　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　新冠町　　　　　　　330031高岡　浩行　　　　　　　　　　　　　　　　　　　　　　　　　　　000248200000000000000000000000000000000020000000000000001005002001003023001005001001003012000000000000000000000000000000000001000000000000000000000000000000000000001002000000001003000003001001002008000000000000000000000000000000000001000000000000000000000000000000000000000000000000000000000005001001002004000000000000000004001000000000001002000000000000000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001000000000000000000000000000000000000000003001001001008001002000000002003000000000000000000008009003003035\r\n",
                                                           JVOpenOptions.Normal);
            var dataBridge2 = NewJV_UM_UMADataBridge("UM42023082820121015471201407312016090720120402シゲルシチフクジン　　　　　　　　　ｼｹﾞﾙｼﾁﾌｸｼﾞﾝ                         Shigerusitifukujin(JPN)                                     0                   0021041120002127ストーミングホーム　　　　　　　　　1220057322シンバル２　　　　　　　　　　　　　1140002606Machiavellian                       1240023756Try to Catch Me                     1140004069Singspiel                           1240026596Valdara                             1140000948Mr. Prospector                      1240009419Coup de Folie                       1140002171Shareef Dancer                      1240006932It's in the Air                     1140002807In The Wings                        1240005908Glorious Song                       1140001945Darshaan                            1240018715Valverda                            100383伊藤正徳　　　　　　　　　　81154000ダーレー・ジャパン・ファーム　　　　　　　　　　　　　　　　　　　　　　日高町　　　　　　　674004森中　蕃　　　　　　　　　　　　　　　　　　　　　　　　　　　　000095500000000000000000000000000000000020000000000000002000002003003034001000001002002018000000000000000001000000001001002012001000000001000003000000000000000000000000000000000000000000000000000002000000000000000000001000001002002013000000000000000002000000000000000000000000000000000001000000000000000002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001008001000001002001008000000000000000000000000000000000000000000000000000002000000000000000000000001009014044\r\n",
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
                    cmd.AssertRecords("NL_UM_UMA", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "UM" },
                            { "headDataKubun", "4" },
                            { "headMakeDate", "20230807" },
                            { "KettoNum", "2013101246" },
                            { "DelKubun", "1" },
                            { "RegDate", "20151112" },
                            { "DelDate", "20191211" },
                            { "BirthDate", "20130508" },
                            { "Bamei", "スケールアップ" },
                            { "BameiKana", "ｽｹｰﾙｱｯﾌﾟ" },
                            { "BameiEng", "Scale Up(JPN)" },
                            { "ZaikyuFlag", "0" },
                            { "Reserved", "" },
                            { "UmaKigoCD", "00" },
                            { "SexCD", "1" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "04" },
                            { "Ketto3Info0HansyokuNum", "1120002216" },
                            { "Ketto3Info0Bamei", "カネヒキリ" },
                            { "Ketto3Info1HansyokuNum", "1220051079" },
                            { "Ketto3Info1Bamei", "ダイコーマリーン" },
                            { "Ketto3Info2HansyokuNum", "1120001542" },
                            { "Ketto3Info2Bamei", "フジキセキ" },
                            { "Ketto3Info3HansyokuNum", "1220042898" },
                            { "Ketto3Info3Bamei", "ライフアウトゼア" },
                            { "Ketto3Info4HansyokuNum", "1120001450" },
                            { "Ketto3Info4Bamei", "シャンハイ" },
                            { "Ketto3Info5HansyokuNum", "1220040748" },
                            { "Ketto3Info5Bamei", "ミヤラビ" },
                            { "Ketto3Info6HansyokuNum", "1120001232" },
                            { "Ketto3Info6Bamei", "サンデーサイレンス" },
                            { "Ketto3Info7HansyokuNum", "1220029223" },
                            { "Ketto3Info7Bamei", "ミルレーサー" },
                            { "Ketto3Info8HansyokuNum", "1140001570" },
                            { "Ketto3Info8Bamei", "Deputy Minister" },
                            { "Ketto3Info9HansyokuNum", "1240013953" },
                            { "Ketto3Info9Bamei", "Silver Valley" },
                            { "Ketto3Info10HansyokuNum", "1140002134" },
                            { "Ketto3Info10Bamei", "Procida" },
                            { "Ketto3Info11HansyokuNum", "1240009334" },
                            { "Ketto3Info11Bamei", "Korveya" },
                            { "Ketto3Info12HansyokuNum", "1120000556" },
                            { "Ketto3Info12Bamei", "モガミ" },
                            { "Ketto3Info13HansyokuNum", "1220026425" },
                            { "Ketto3Info13Bamei", "ライラツクレデイ" },
                            { "TozaiCD", "1" },
                            { "ChokyosiCode", "01080" },
                            { "ChokyosiRyakusyo", "清水英克" },
                            { "Syotai", "" },
                            { "BreederCode", "60033000" },
                            { "BreederName", "アサヒ牧場" },
                            { "SanchiName", "新冠町" },
                            { "BanusiCode", "330031" },
                            { "BanusiName", "高岡　浩行" },
                            { "RuikeiHonsyoHeiti", "000248200" },
                            { "RuikeiHonsyoSyogai", "000000000" },
                            { "RuikeiFukaHeichi", "000000000" },
                            { "RuikeiFukaSyogai", "000000000" },
                            { "RuikeiSyutokuHeichi", "000020000" },
                            { "RuikeiSyutokuSyogai", "000000000" },
                            { "ChakuSogoChakuKaisu0", "001" },
                            { "ChakuSogoChakuKaisu1", "005" },
                            { "ChakuSogoChakuKaisu2", "002" },
                            { "ChakuSogoChakuKaisu3", "001" },
                            { "ChakuSogoChakuKaisu4", "003" },
                            { "ChakuSogoChakuKaisu5", "023" },
                            { "ChakuChuoChakuKaisu0", "001" },
                            { "ChakuChuoChakuKaisu1", "005" },
                            { "ChakuChuoChakuKaisu2", "001" },
                            { "ChakuChuoChakuKaisu3", "001" },
                            { "ChakuChuoChakuKaisu4", "003" },
                            { "ChakuChuoChakuKaisu5", "012" },
                            { "ChakuKaisuBa0ChakuKaisu0", "000" },
                            { "ChakuKaisuBa0ChakuKaisu1", "000" },
                            { "ChakuKaisuBa0ChakuKaisu2", "000" },
                            { "ChakuKaisuBa0ChakuKaisu3", "000" },
                            { "ChakuKaisuBa0ChakuKaisu4", "000" },
                            { "ChakuKaisuBa0ChakuKaisu5", "000" },
                            { "ChakuKaisuBa1ChakuKaisu0", "000" },
                            { "ChakuKaisuBa1ChakuKaisu1", "000" },
                            { "ChakuKaisuBa1ChakuKaisu2", "000" },
                            { "ChakuKaisuBa1ChakuKaisu3", "000" },
                            { "ChakuKaisuBa1ChakuKaisu4", "000" },
                            { "ChakuKaisuBa1ChakuKaisu5", "001" },
                            { "ChakuKaisuBa2ChakuKaisu0", "000" },
                            { "ChakuKaisuBa2ChakuKaisu1", "000" },
                            { "ChakuKaisuBa2ChakuKaisu2", "000" },
                            { "ChakuKaisuBa2ChakuKaisu3", "000" },
                            { "ChakuKaisuBa2ChakuKaisu4", "000" },
                            { "ChakuKaisuBa2ChakuKaisu5", "000" },
                            { "ChakuKaisuBa3ChakuKaisu0", "000" },
                            { "ChakuKaisuBa3ChakuKaisu1", "000" },
                            { "ChakuKaisuBa3ChakuKaisu2", "000" },
                            { "ChakuKaisuBa3ChakuKaisu3", "000" },
                            { "ChakuKaisuBa3ChakuKaisu4", "000" },
                            { "ChakuKaisuBa3ChakuKaisu5", "000" },
                            { "ChakuKaisuBa4ChakuKaisu0", "001" },
                            { "ChakuKaisuBa4ChakuKaisu1", "002" },
                            { "ChakuKaisuBa4ChakuKaisu2", "000" },
                            { "ChakuKaisuBa4ChakuKaisu3", "000" },
                            { "ChakuKaisuBa4ChakuKaisu4", "001" },
                            { "ChakuKaisuBa4ChakuKaisu5", "003" },
                            { "ChakuKaisuBa5ChakuKaisu0", "000" },
                            { "ChakuKaisuBa5ChakuKaisu1", "003" },
                            { "ChakuKaisuBa5ChakuKaisu2", "001" },
                            { "ChakuKaisuBa5ChakuKaisu3", "001" },
                            { "ChakuKaisuBa5ChakuKaisu4", "002" },
                            { "ChakuKaisuBa5ChakuKaisu5", "008" },
                            { "ChakuKaisuBa6ChakuKaisu0", "000" },
                            { "ChakuKaisuBa6ChakuKaisu1", "000" },
                            { "ChakuKaisuBa6ChakuKaisu2", "000" },
                            { "ChakuKaisuBa6ChakuKaisu3", "000" },
                            { "ChakuKaisuBa6ChakuKaisu4", "000" },
                            { "ChakuKaisuBa6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai1ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu1", "005" },
                            { "ChakuKaisuJyotai4ChakuKaisu2", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu3", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu4", "002" },
                            { "ChakuKaisuJyotai4ChakuKaisu5", "004" },
                            { "ChakuKaisuJyotai5ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu5", "004" },
                            { "ChakuKaisuJyotai6ChakuKaisu0", "001" },
                            { "ChakuKaisuJyotai6ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu4", "001" },
                            { "ChakuKaisuJyotai6ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai7ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai8ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu5", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu1", "003" },
                            { "ChakuKaisuKyori3ChakuKaisu2", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu3", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori3ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori4ChakuKaisu0", "001" },
                            { "ChakuKaisuKyori4ChakuKaisu1", "002" },
                            { "ChakuKaisuKyori4ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu4", "002" },
                            { "ChakuKaisuKyori4ChakuKaisu5", "003" },
                            { "ChakuKaisuKyori5ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu5", "000" },
                            { "Kyakusitu0", "008" },
                            { "Kyakusitu1", "009" },
                            { "Kyakusitu2", "003" },
                            { "Kyakusitu3", "003" },
                            { "RaceCount", "035" }
                        },
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "UM" },
                            { "headDataKubun", "4" },
                            { "headMakeDate", "20230828" },
                            { "KettoNum", "2012101547" },
                            { "DelKubun", "1" },
                            { "RegDate", "20140731" },
                            { "DelDate", "20160907" },
                            { "BirthDate", "20120402" },
                            { "Bamei", "シゲルシチフクジン" },
                            { "BameiKana", "ｼｹﾞﾙｼﾁﾌｸｼﾞﾝ" },
                            { "BameiEng", "Shigerusitifukujin(JPN)" },
                            { "ZaikyuFlag", "0" },
                            { "Reserved", "" },
                            { "UmaKigoCD", "00" },
                            { "SexCD", "2" },
                            { "HinsyuCD", "1" },
                            { "KeiroCD", "04" },
                            { "Ketto3Info0HansyokuNum", "1120002127" },
                            { "Ketto3Info0Bamei", "ストーミングホーム" },
                            { "Ketto3Info1HansyokuNum", "1220057322" },
                            { "Ketto3Info1Bamei", "シンバル２" },
                            { "Ketto3Info2HansyokuNum", "1140002606" },
                            { "Ketto3Info2Bamei", "Machiavellian" },
                            { "Ketto3Info3HansyokuNum", "1240023756" },
                            { "Ketto3Info3Bamei", "Try to Catch Me" },
                            { "Ketto3Info4HansyokuNum", "1140004069" },
                            { "Ketto3Info4Bamei", "Singspiel" },
                            { "Ketto3Info5HansyokuNum", "1240026596" },
                            { "Ketto3Info5Bamei", "Valdara" },
                            { "Ketto3Info6HansyokuNum", "1140000948" },
                            { "Ketto3Info6Bamei", "Mr. Prospector" },
                            { "Ketto3Info7HansyokuNum", "1240009419" },
                            { "Ketto3Info7Bamei", "Coup de Folie" },
                            { "Ketto3Info8HansyokuNum", "1140002171" },
                            { "Ketto3Info8Bamei", "Shareef Dancer" },
                            { "Ketto3Info9HansyokuNum", "1240006932" },
                            { "Ketto3Info9Bamei", "It's in the Air" },
                            { "Ketto3Info10HansyokuNum", "1140002807" },
                            { "Ketto3Info10Bamei", "In The Wings" },
                            { "Ketto3Info11HansyokuNum", "1240005908" },
                            { "Ketto3Info11Bamei", "Glorious Song" },
                            { "Ketto3Info12HansyokuNum", "1140001945" },
                            { "Ketto3Info12Bamei", "Darshaan" },
                            { "Ketto3Info13HansyokuNum", "1240018715" },
                            { "Ketto3Info13Bamei", "Valverda" },
                            { "TozaiCD", "1" },
                            { "ChokyosiCode", "00383" },
                            { "ChokyosiRyakusyo", "伊藤正徳" },
                            { "Syotai", "" },
                            { "BreederCode", "81154000" },
                            { "BreederName", "ダーレー・ジャパン・ファーム" },
                            { "SanchiName", "日高町" },
                            { "BanusiCode", "674004" },
                            { "BanusiName", "森中　蕃" },
                            { "RuikeiHonsyoHeiti", "000095500" },
                            { "RuikeiHonsyoSyogai", "000000000" },
                            { "RuikeiFukaHeichi", "000000000" },
                            { "RuikeiFukaSyogai", "000000000" },
                            { "RuikeiSyutokuHeichi", "000020000" },
                            { "RuikeiSyutokuSyogai", "000000000" },
                            { "ChakuSogoChakuKaisu0", "002" },
                            { "ChakuSogoChakuKaisu1", "000" },
                            { "ChakuSogoChakuKaisu2", "002" },
                            { "ChakuSogoChakuKaisu3", "003" },
                            { "ChakuSogoChakuKaisu4", "003" },
                            { "ChakuSogoChakuKaisu5", "034" },
                            { "ChakuChuoChakuKaisu0", "001" },
                            { "ChakuChuoChakuKaisu1", "000" },
                            { "ChakuChuoChakuKaisu2", "001" },
                            { "ChakuChuoChakuKaisu3", "002" },
                            { "ChakuChuoChakuKaisu4", "002" },
                            { "ChakuChuoChakuKaisu5", "018" },
                            { "ChakuKaisuBa0ChakuKaisu0", "000" },
                            { "ChakuKaisuBa0ChakuKaisu1", "000" },
                            { "ChakuKaisuBa0ChakuKaisu2", "000" },
                            { "ChakuKaisuBa0ChakuKaisu3", "000" },
                            { "ChakuKaisuBa0ChakuKaisu4", "000" },
                            { "ChakuKaisuBa0ChakuKaisu5", "001" },
                            { "ChakuKaisuBa1ChakuKaisu0", "000" },
                            { "ChakuKaisuBa1ChakuKaisu1", "000" },
                            { "ChakuKaisuBa1ChakuKaisu2", "001" },
                            { "ChakuKaisuBa1ChakuKaisu3", "001" },
                            { "ChakuKaisuBa1ChakuKaisu4", "002" },
                            { "ChakuKaisuBa1ChakuKaisu5", "012" },
                            { "ChakuKaisuBa2ChakuKaisu0", "001" },
                            { "ChakuKaisuBa2ChakuKaisu1", "000" },
                            { "ChakuKaisuBa2ChakuKaisu2", "000" },
                            { "ChakuKaisuBa2ChakuKaisu3", "001" },
                            { "ChakuKaisuBa2ChakuKaisu4", "000" },
                            { "ChakuKaisuBa2ChakuKaisu5", "003" },
                            { "ChakuKaisuBa3ChakuKaisu0", "000" },
                            { "ChakuKaisuBa3ChakuKaisu1", "000" },
                            { "ChakuKaisuBa3ChakuKaisu2", "000" },
                            { "ChakuKaisuBa3ChakuKaisu3", "000" },
                            { "ChakuKaisuBa3ChakuKaisu4", "000" },
                            { "ChakuKaisuBa3ChakuKaisu5", "000" },
                            { "ChakuKaisuBa4ChakuKaisu0", "000" },
                            { "ChakuKaisuBa4ChakuKaisu1", "000" },
                            { "ChakuKaisuBa4ChakuKaisu2", "000" },
                            { "ChakuKaisuBa4ChakuKaisu3", "000" },
                            { "ChakuKaisuBa4ChakuKaisu4", "000" },
                            { "ChakuKaisuBa4ChakuKaisu5", "000" },
                            { "ChakuKaisuBa5ChakuKaisu0", "000" },
                            { "ChakuKaisuBa5ChakuKaisu1", "000" },
                            { "ChakuKaisuBa5ChakuKaisu2", "000" },
                            { "ChakuKaisuBa5ChakuKaisu3", "000" },
                            { "ChakuKaisuBa5ChakuKaisu4", "000" },
                            { "ChakuKaisuBa5ChakuKaisu5", "002" },
                            { "ChakuKaisuBa6ChakuKaisu0", "000" },
                            { "ChakuKaisuBa6ChakuKaisu1", "000" },
                            { "ChakuKaisuBa6ChakuKaisu2", "000" },
                            { "ChakuKaisuBa6ChakuKaisu3", "000" },
                            { "ChakuKaisuBa6ChakuKaisu4", "000" },
                            { "ChakuKaisuBa6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu0", "001" },
                            { "ChakuKaisuJyotai0ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai0ChakuKaisu2", "001" },
                            { "ChakuKaisuJyotai0ChakuKaisu3", "002" },
                            { "ChakuKaisuJyotai0ChakuKaisu4", "002" },
                            { "ChakuKaisuJyotai0ChakuKaisu5", "013" },
                            { "ChakuKaisuJyotai1ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai1ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai2ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai2ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai3ChakuKaisu5", "001" },
                            { "ChakuKaisuJyotai4ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai4ChakuKaisu5", "002" },
                            { "ChakuKaisuJyotai5ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai5ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai6ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai7ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai8ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai9ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai10ChakuKaisu5", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu0", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu1", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu2", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu3", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu4", "000" },
                            { "ChakuKaisuJyotai11ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori0ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori0ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori1ChakuKaisu0", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori1ChakuKaisu2", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu3", "002" },
                            { "ChakuKaisuKyori1ChakuKaisu4", "001" },
                            { "ChakuKaisuKyori1ChakuKaisu5", "008" },
                            { "ChakuKaisuKyori2ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori2ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori3ChakuKaisu5", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori4ChakuKaisu5", "002" },
                            { "ChakuKaisuKyori5ChakuKaisu0", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu1", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu2", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu3", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu4", "000" },
                            { "ChakuKaisuKyori5ChakuKaisu5", "000" },
                            { "Kyakusitu0", "000" },
                            { "Kyakusitu1", "001" },
                            { "Kyakusitu2", "009" },
                            { "Kyakusitu3", "014" },
                            { "RaceCount", "044" }
                        },
                    });
                }
            }
        }

        private static JV_UM_UMADataBridge NewJV_UM_UMADataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_UM_UMA();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_UM_UMADataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }

        private static JV_UM_UMA_V4802DataBridge NewJV_UM_UMA_V4802DataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_UM_UMA_V4802();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_UM_UMA_V4802DataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }
    }
}