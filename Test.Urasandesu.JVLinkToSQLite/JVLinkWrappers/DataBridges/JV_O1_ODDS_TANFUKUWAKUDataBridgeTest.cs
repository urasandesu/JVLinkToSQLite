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
    public class JV_O1_ODDS_TANFUKUWAKUDataBridgeTest
    {
        [Test]
        public void JV_O1_ODDS_TANFUKUWAKUDataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
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
                    cmd.AssertTableCreation("NL_O1_ODDS_TANFUKUWAKU", JVDataStructColumns.O1);
                }
            }
        }

        [Test]
        public void JV_O1_ODDS_TANFUKUWAKUDataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
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
        public void JV_O1_ODDS_TANFUKUWAKUDataBridge_BuildUpCreateTableCommand_should_not_drop_table_when_normal_execution()
        {
            // Arrange
            var dataBridge = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
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
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecordsAreNotEmpty("NL_O1_ODDS_TANFUKUWAKU");
                }
            }
        }

        [Test]
        public void JV_O1_ODDS_TANFUKUWAKUDataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
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
                    cmd.AssertRecords("NL_O1_ODDS_TANFUKUWAKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "O1" },
                            { "headDataKubun", "5" },
                            { "headMakeDate", "20230828" },
                            { "idYear", "2023" },
                            { "idMonthDay", "0826" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "05" },
                            { "idRaceNum", "01" },
                            { "HappyoTime", "00000000" },
                            { "TorokuTosu", "11" },
                            { "SyussoTosu", "11" },
                            { "TansyoFlag", "7" },
                            { "FukusyoFlag", "7" },
                            { "WakurenFlag", "7" },
                            { "FukuChakuBaraiKey", "3" },
                            { "OddsTansyoInfo0Umaban", "01" },
                            { "OddsTansyoInfo0Odds", "0789" },
                            { "OddsTansyoInfo0Ninki", "07" },
                            { "OddsTansyoInfo1Umaban", "02" },
                            { "OddsTansyoInfo1Odds", "1079" },
                            { "OddsTansyoInfo1Ninki", "09" },
                            { "OddsTansyoInfo2Umaban", "03" },
                            { "OddsTansyoInfo2Odds", "0585" },
                            { "OddsTansyoInfo2Ninki", "06" },
                            { "OddsTansyoInfo3Umaban", "04" },
                            { "OddsTansyoInfo3Odds", "0012" },
                            { "OddsTansyoInfo3Ninki", "01" },
                            { "OddsTansyoInfo4Umaban", "05" },
                            { "OddsTansyoInfo4Odds", "0176" },
                            { "OddsTansyoInfo4Ninki", "04" },
                            { "OddsTansyoInfo5Umaban", "06" },
                            { "OddsTansyoInfo5Odds", "0056" },
                            { "OddsTansyoInfo5Ninki", "02" },
                            { "OddsTansyoInfo6Umaban", "07" },
                            { "OddsTansyoInfo6Odds", "0091" },
                            { "OddsTansyoInfo6Ninki", "03" },
                            { "OddsTansyoInfo7Umaban", "08" },
                            { "OddsTansyoInfo7Odds", "4608" },
                            { "OddsTansyoInfo7Ninki", "11" },
                            { "OddsTansyoInfo8Umaban", "09" },
                            { "OddsTansyoInfo8Odds", "0190" },
                            { "OddsTansyoInfo8Ninki", "05" },
                            { "OddsTansyoInfo9Umaban", "10" },
                            { "OddsTansyoInfo9Odds", "2835" },
                            { "OddsTansyoInfo9Ninki", "10" },
                            { "OddsTansyoInfo10Umaban", "11" },
                            { "OddsTansyoInfo10Odds", "0897" },
                            { "OddsTansyoInfo10Ninki", "08" },
                            { "OddsTansyoInfo11Umaban", "" },
                            { "OddsTansyoInfo11Odds", "" },
                            { "OddsTansyoInfo11Ninki", "" },
                            { "OddsTansyoInfo12Umaban", "" },
                            { "OddsTansyoInfo12Odds", "" },
                            { "OddsTansyoInfo12Ninki", "" },
                            { "OddsTansyoInfo13Umaban", "" },
                            { "OddsTansyoInfo13Odds", "" },
                            { "OddsTansyoInfo13Ninki", "" },
                            { "OddsTansyoInfo14Umaban", "" },
                            { "OddsTansyoInfo14Odds", "" },
                            { "OddsTansyoInfo14Ninki", "" },
                            { "OddsTansyoInfo15Umaban", "" },
                            { "OddsTansyoInfo15Odds", "" },
                            { "OddsTansyoInfo15Ninki", "" },
                            { "OddsTansyoInfo16Umaban", "" },
                            { "OddsTansyoInfo16Odds", "" },
                            { "OddsTansyoInfo16Ninki", "" },
                            { "OddsTansyoInfo17Umaban", "" },
                            { "OddsTansyoInfo17Odds", "" },
                            { "OddsTansyoInfo17Ninki", "" },
                            { "OddsTansyoInfo18Umaban", "" },
                            { "OddsTansyoInfo18Odds", "" },
                            { "OddsTansyoInfo18Ninki", "" },
                            { "OddsTansyoInfo19Umaban", "" },
                            { "OddsTansyoInfo19Odds", "" },
                            { "OddsTansyoInfo19Ninki", "" },
                            { "OddsTansyoInfo20Umaban", "" },
                            { "OddsTansyoInfo20Odds", "" },
                            { "OddsTansyoInfo20Ninki", "" },
                            { "OddsTansyoInfo21Umaban", "" },
                            { "OddsTansyoInfo21Odds", "" },
                            { "OddsTansyoInfo21Ninki", "" },
                            { "OddsTansyoInfo22Umaban", "" },
                            { "OddsTansyoInfo22Odds", "" },
                            { "OddsTansyoInfo22Ninki", "" },
                            { "OddsTansyoInfo23Umaban", "" },
                            { "OddsTansyoInfo23Odds", "" },
                            { "OddsTansyoInfo23Ninki", "" },
                            { "OddsTansyoInfo24Umaban", "" },
                            { "OddsTansyoInfo24Odds", "" },
                            { "OddsTansyoInfo24Ninki", "" },
                            { "OddsTansyoInfo25Umaban", "" },
                            { "OddsTansyoInfo25Odds", "" },
                            { "OddsTansyoInfo25Ninki", "" },
                            { "OddsTansyoInfo26Umaban", "" },
                            { "OddsTansyoInfo26Odds", "" },
                            { "OddsTansyoInfo26Ninki", "" },
                            { "OddsTansyoInfo27Umaban", "" },
                            { "OddsTansyoInfo27Odds", "" },
                            { "OddsTansyoInfo27Ninki", "" },
                            { "OddsFukusyoInfo0Umaban", "01" },
                            { "OddsFukusyoInfo0OddsLow", "0032" },
                            { "OddsFukusyoInfo0OddsHigh", "0233" },
                            { "OddsFukusyoInfo0Ninki", "06" },
                            { "OddsFukusyoInfo1Umaban", "02" },
                            { "OddsFukusyoInfo1OddsLow", "0050" },
                            { "OddsFukusyoInfo1OddsHigh", "0383" },
                            { "OddsFukusyoInfo1Ninki", "08" },
                            { "OddsFukusyoInfo2Umaban", "03" },
                            { "OddsFukusyoInfo2OddsLow", "0046" },
                            { "OddsFukusyoInfo2OddsHigh", "0347" },
                            { "OddsFukusyoInfo2Ninki", "07" },
                            { "OddsFukusyoInfo3Umaban", "04" },
                            { "OddsFukusyoInfo3OddsLow", "0010" },
                            { "OddsFukusyoInfo3OddsHigh", "0011" },
                            { "OddsFukusyoInfo3Ninki", "01" },
                            { "OddsFukusyoInfo4Umaban", "05" },
                            { "OddsFukusyoInfo4OddsLow", "0016" },
                            { "OddsFukusyoInfo4OddsHigh", "0096" },
                            { "OddsFukusyoInfo4Ninki", "04" },
                            { "OddsFukusyoInfo5Umaban", "06" },
                            { "OddsFukusyoInfo5OddsLow", "0011" },
                            { "OddsFukusyoInfo5OddsHigh", "0045" },
                            { "OddsFukusyoInfo5Ninki", "02" },
                            { "OddsFukusyoInfo6Umaban", "07" },
                            { "OddsFukusyoInfo6OddsLow", "0014" },
                            { "OddsFukusyoInfo6OddsHigh", "0082" },
                            { "OddsFukusyoInfo6Ninki", "03" },
                            { "OddsFukusyoInfo7Umaban", "08" },
                            { "OddsFukusyoInfo7OddsLow", "0294" },
                            { "OddsFukusyoInfo7OddsHigh", "2406" },
                            { "OddsFukusyoInfo7Ninki", "11" },
                            { "OddsFukusyoInfo8Umaban", "09" },
                            { "OddsFukusyoInfo8OddsLow", "0019" },
                            { "OddsFukusyoInfo8OddsHigh", "0123" },
                            { "OddsFukusyoInfo8Ninki", "05" },
                            { "OddsFukusyoInfo9Umaban", "10" },
                            { "OddsFukusyoInfo9OddsLow", "0184" },
                            { "OddsFukusyoInfo9OddsHigh", "1495" },
                            { "OddsFukusyoInfo9Ninki", "10" },
                            { "OddsFukusyoInfo10Umaban", "11" },
                            { "OddsFukusyoInfo10OddsLow", "0081" },
                            { "OddsFukusyoInfo10OddsHigh", "0641" },
                            { "OddsFukusyoInfo10Ninki", "09" },
                            { "OddsFukusyoInfo11Umaban", "" },
                            { "OddsFukusyoInfo11OddsLow", "" },
                            { "OddsFukusyoInfo11OddsHigh", "" },
                            { "OddsFukusyoInfo11Ninki", "" },
                            { "OddsFukusyoInfo12Umaban", "" },
                            { "OddsFukusyoInfo12OddsLow", "" },
                            { "OddsFukusyoInfo12OddsHigh", "" },
                            { "OddsFukusyoInfo12Ninki", "" },
                            { "OddsFukusyoInfo13Umaban", "" },
                            { "OddsFukusyoInfo13OddsLow", "" },
                            { "OddsFukusyoInfo13OddsHigh", "" },
                            { "OddsFukusyoInfo13Ninki", "" },
                            { "OddsFukusyoInfo14Umaban", "" },
                            { "OddsFukusyoInfo14OddsLow", "" },
                            { "OddsFukusyoInfo14OddsHigh", "" },
                            { "OddsFukusyoInfo14Ninki", "" },
                            { "OddsFukusyoInfo15Umaban", "" },
                            { "OddsFukusyoInfo15OddsLow", "" },
                            { "OddsFukusyoInfo15OddsHigh", "" },
                            { "OddsFukusyoInfo15Ninki", "" },
                            { "OddsFukusyoInfo16Umaban", "" },
                            { "OddsFukusyoInfo16OddsLow", "" },
                            { "OddsFukusyoInfo16OddsHigh", "" },
                            { "OddsFukusyoInfo16Ninki", "" },
                            { "OddsFukusyoInfo17Umaban", "" },
                            { "OddsFukusyoInfo17OddsLow", "" },
                            { "OddsFukusyoInfo17OddsHigh", "" },
                            { "OddsFukusyoInfo17Ninki", "" },
                            { "OddsFukusyoInfo18Umaban", "" },
                            { "OddsFukusyoInfo18OddsLow", "" },
                            { "OddsFukusyoInfo18OddsHigh", "" },
                            { "OddsFukusyoInfo18Ninki", "" },
                            { "OddsFukusyoInfo19Umaban", "" },
                            { "OddsFukusyoInfo19OddsLow", "" },
                            { "OddsFukusyoInfo19OddsHigh", "" },
                            { "OddsFukusyoInfo19Ninki", "" },
                            { "OddsFukusyoInfo20Umaban", "" },
                            { "OddsFukusyoInfo20OddsLow", "" },
                            { "OddsFukusyoInfo20OddsHigh", "" },
                            { "OddsFukusyoInfo20Ninki", "" },
                            { "OddsFukusyoInfo21Umaban", "" },
                            { "OddsFukusyoInfo21OddsLow", "" },
                            { "OddsFukusyoInfo21OddsHigh", "" },
                            { "OddsFukusyoInfo21Ninki", "" },
                            { "OddsFukusyoInfo22Umaban", "" },
                            { "OddsFukusyoInfo22OddsLow", "" },
                            { "OddsFukusyoInfo22OddsHigh", "" },
                            { "OddsFukusyoInfo22Ninki", "" },
                            { "OddsFukusyoInfo23Umaban", "" },
                            { "OddsFukusyoInfo23OddsLow", "" },
                            { "OddsFukusyoInfo23OddsHigh", "" },
                            { "OddsFukusyoInfo23Ninki", "" },
                            { "OddsFukusyoInfo24Umaban", "" },
                            { "OddsFukusyoInfo24OddsLow", "" },
                            { "OddsFukusyoInfo24OddsHigh", "" },
                            { "OddsFukusyoInfo24Ninki", "" },
                            { "OddsFukusyoInfo25Umaban", "" },
                            { "OddsFukusyoInfo25OddsLow", "" },
                            { "OddsFukusyoInfo25OddsHigh", "" },
                            { "OddsFukusyoInfo25Ninki", "" },
                            { "OddsFukusyoInfo26Umaban", "" },
                            { "OddsFukusyoInfo26OddsLow", "" },
                            { "OddsFukusyoInfo26OddsHigh", "" },
                            { "OddsFukusyoInfo26Ninki", "" },
                            { "OddsFukusyoInfo27Umaban", "" },
                            { "OddsFukusyoInfo27OddsLow", "" },
                            { "OddsFukusyoInfo27OddsHigh", "" },
                            { "OddsFukusyoInfo27Ninki", "" },
                            { "OddsWakurenInfo0Kumi", "" },
                            { "OddsWakurenInfo0Odds", "" },
                            { "OddsWakurenInfo0Ninki", "" },
                            { "OddsWakurenInfo1Kumi", "12" },
                            { "OddsWakurenInfo1Odds", "03595" },
                            { "OddsWakurenInfo1Ninki", "27" },
                            { "OddsWakurenInfo2Kumi", "13" },
                            { "OddsWakurenInfo2Odds", "04703" },
                            { "OddsWakurenInfo2Ninki", "29" },
                            { "OddsWakurenInfo3Kumi", "14" },
                            { "OddsWakurenInfo3Odds", "00350" },
                            { "OddsWakurenInfo3Ninki", "10" },
                            { "OddsWakurenInfo4Kumi", "15" },
                            { "OddsWakurenInfo4Odds", "04061" },
                            { "OddsWakurenInfo4Ninki", "28" },
                            { "OddsWakurenInfo5Kumi", "16" },
                            { "OddsWakurenInfo5Odds", "00853" },
                            { "OddsWakurenInfo5Ninki", "15" },
                            { "OddsWakurenInfo6Kumi", "17" },
                            { "OddsWakurenInfo6Odds", "03421" },
                            { "OddsWakurenInfo6Ninki", "25" },
                            { "OddsWakurenInfo7Kumi", "18" },
                            { "OddsWakurenInfo7Odds", "03002" },
                            { "OddsWakurenInfo7Ninki", "23" },
                            { "OddsWakurenInfo8Kumi", "" },
                            { "OddsWakurenInfo8Odds", "" },
                            { "OddsWakurenInfo8Ninki", "" },
                            { "OddsWakurenInfo9Kumi", "23" },
                            { "OddsWakurenInfo9Odds", "03118" },
                            { "OddsWakurenInfo9Ninki", "24" },
                            { "OddsWakurenInfo10Kumi", "24" },
                            { "OddsWakurenInfo10Odds", "00347" },
                            { "OddsWakurenInfo10Ninki", "08" },
                            { "OddsWakurenInfo11Kumi", "25" },
                            { "OddsWakurenInfo11Odds", "01913" },
                            { "OddsWakurenInfo11Ninki", "20" },
                            { "OddsWakurenInfo12Kumi", "26" },
                            { "OddsWakurenInfo12Odds", "00646" },
                            { "OddsWakurenInfo12Ninki", "12" },
                            { "OddsWakurenInfo13Kumi", "27" },
                            { "OddsWakurenInfo13Odds", "02454" },
                            { "OddsWakurenInfo13Ninki", "21" },
                            { "OddsWakurenInfo14Kumi", "28" },
                            { "OddsWakurenInfo14Odds", "03421" },
                            { "OddsWakurenInfo14Ninki", "25" },
                            { "OddsWakurenInfo15Kumi", "" },
                            { "OddsWakurenInfo15Odds", "" },
                            { "OddsWakurenInfo15Ninki", "" },
                            { "OddsWakurenInfo16Kumi", "34" },
                            { "OddsWakurenInfo16Odds", "00328" },
                            { "OddsWakurenInfo16Ninki", "07" },
                            { "OddsWakurenInfo17Kumi", "35" },
                            { "OddsWakurenInfo17Odds", "01695" },
                            { "OddsWakurenInfo17Ninki", "18" },
                            { "OddsWakurenInfo18Kumi", "36" },
                            { "OddsWakurenInfo18Odds", "00661" },
                            { "OddsWakurenInfo18Ninki", "13" },
                            { "OddsWakurenInfo19Kumi", "37" },
                            { "OddsWakurenInfo19Odds", "01485" },
                            { "OddsWakurenInfo19Ninki", "17" },
                            { "OddsWakurenInfo20Kumi", "38" },
                            { "OddsWakurenInfo20Odds", "02955" },
                            { "OddsWakurenInfo20Ninki", "22" },
                            { "OddsWakurenInfo21Kumi", "" },
                            { "OddsWakurenInfo21Odds", "" },
                            { "OddsWakurenInfo21Ninki", "" },
                            { "OddsWakurenInfo22Kumi", "45" },
                            { "OddsWakurenInfo22Odds", "00079" },
                            { "OddsWakurenInfo22Ninki", "02" },
                            { "OddsWakurenInfo23Kumi", "46" },
                            { "OddsWakurenInfo23Odds", "00015" },
                            { "OddsWakurenInfo23Ninki", "01" },
                            { "OddsWakurenInfo24Kumi", "47" },
                            { "OddsWakurenInfo24Odds", "00109" },
                            { "OddsWakurenInfo24Ninki", "03" },
                            { "OddsWakurenInfo25Kumi", "48" },
                            { "OddsWakurenInfo25Odds", "00349" },
                            { "OddsWakurenInfo25Ninki", "09" },
                            { "OddsWakurenInfo26Kumi", "" },
                            { "OddsWakurenInfo26Odds", "" },
                            { "OddsWakurenInfo26Ninki", "" },
                            { "OddsWakurenInfo27Kumi", "56" },
                            { "OddsWakurenInfo27Odds", "00168" },
                            { "OddsWakurenInfo27Ninki", "05" },
                            { "OddsWakurenInfo28Kumi", "57" },
                            { "OddsWakurenInfo28Odds", "00773" },
                            { "OddsWakurenInfo28Ninki", "14" },
                            { "OddsWakurenInfo29Kumi", "58" },
                            { "OddsWakurenInfo29Odds", "01715" },
                            { "OddsWakurenInfo29Ninki", "19" },
                            { "OddsWakurenInfo30Kumi", "66" },
                            { "OddsWakurenInfo30Odds", "00135" },
                            { "OddsWakurenInfo30Ninki", "04" },
                            { "OddsWakurenInfo31Kumi", "67" },
                            { "OddsWakurenInfo31Odds", "00244" },
                            { "OddsWakurenInfo31Ninki", "06" },
                            { "OddsWakurenInfo32Kumi", "68" },
                            { "OddsWakurenInfo32Odds", "00472" },
                            { "OddsWakurenInfo32Ninki", "11" },
                            { "OddsWakurenInfo33Kumi", "77" },
                            { "OddsWakurenInfo33Odds", "08684" },
                            { "OddsWakurenInfo33Ninki", "31" },
                            { "OddsWakurenInfo34Kumi", "78" },
                            { "OddsWakurenInfo34Odds", "00906" },
                            { "OddsWakurenInfo34Ninki", "16" },
                            { "OddsWakurenInfo35Kumi", "88" },
                            { "OddsWakurenInfo35Odds", "07330" },
                            { "OddsWakurenInfo35Ninki", "30" },
                            { "TotalHyosuTansyo", "00000394142" },
                            { "TotalHyosuFukusyo", "00001587058" },
                            { "TotalHyosuWakuren", "00000072836" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_O1_ODDS_TANFUKUWAKUDataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
                                                                  JVOpenOptions.Normal);
            var dataBridge2 = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034808250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
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
                    cmd.AssertRecords("NL_O1_ODDS_TANFUKUWAKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "O1" },
                            { "headDataKubun", "5" },
                            { "headMakeDate", "20230828" },
                            { "idYear", "2023" },
                            { "idMonthDay", "0826" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "05" },
                            { "idRaceNum", "01" },
                            { "HappyoTime", "00000000" },
                            { "TorokuTosu", "11" },
                            { "SyussoTosu", "11" },
                            { "TansyoFlag", "7" },
                            { "FukusyoFlag", "7" },
                            { "WakurenFlag", "7" },
                            { "FukuChakuBaraiKey", "3" },
                            { "OddsTansyoInfo0Umaban", "01" },
                            { "OddsTansyoInfo0Odds", "0789" },
                            { "OddsTansyoInfo0Ninki", "07" },
                            { "OddsTansyoInfo1Umaban", "02" },
                            { "OddsTansyoInfo1Odds", "1079" },
                            { "OddsTansyoInfo1Ninki", "09" },
                            { "OddsTansyoInfo2Umaban", "03" },
                            { "OddsTansyoInfo2Odds", "0585" },
                            { "OddsTansyoInfo2Ninki", "06" },
                            { "OddsTansyoInfo3Umaban", "04" },
                            { "OddsTansyoInfo3Odds", "0012" },
                            { "OddsTansyoInfo3Ninki", "01" },
                            { "OddsTansyoInfo4Umaban", "05" },
                            { "OddsTansyoInfo4Odds", "0176" },
                            { "OddsTansyoInfo4Ninki", "04" },
                            { "OddsTansyoInfo5Umaban", "06" },
                            { "OddsTansyoInfo5Odds", "0056" },
                            { "OddsTansyoInfo5Ninki", "02" },
                            { "OddsTansyoInfo6Umaban", "07" },
                            { "OddsTansyoInfo6Odds", "0091" },
                            { "OddsTansyoInfo6Ninki", "03" },
                            { "OddsTansyoInfo7Umaban", "08" },
                            { "OddsTansyoInfo7Odds", "4608" },
                            { "OddsTansyoInfo7Ninki", "11" },
                            { "OddsTansyoInfo8Umaban", "09" },
                            { "OddsTansyoInfo8Odds", "0190" },
                            { "OddsTansyoInfo8Ninki", "05" },
                            { "OddsTansyoInfo9Umaban", "10" },
                            { "OddsTansyoInfo9Odds", "2835" },
                            { "OddsTansyoInfo9Ninki", "10" },
                            { "OddsTansyoInfo10Umaban", "11" },
                            { "OddsTansyoInfo10Odds", "0897" },
                            { "OddsTansyoInfo10Ninki", "08" },
                            { "OddsTansyoInfo11Umaban", "" },
                            { "OddsTansyoInfo11Odds", "" },
                            { "OddsTansyoInfo11Ninki", "" },
                            { "OddsTansyoInfo12Umaban", "" },
                            { "OddsTansyoInfo12Odds", "" },
                            { "OddsTansyoInfo12Ninki", "" },
                            { "OddsTansyoInfo13Umaban", "" },
                            { "OddsTansyoInfo13Odds", "" },
                            { "OddsTansyoInfo13Ninki", "" },
                            { "OddsTansyoInfo14Umaban", "" },
                            { "OddsTansyoInfo14Odds", "" },
                            { "OddsTansyoInfo14Ninki", "" },
                            { "OddsTansyoInfo15Umaban", "" },
                            { "OddsTansyoInfo15Odds", "" },
                            { "OddsTansyoInfo15Ninki", "" },
                            { "OddsTansyoInfo16Umaban", "" },
                            { "OddsTansyoInfo16Odds", "" },
                            { "OddsTansyoInfo16Ninki", "" },
                            { "OddsTansyoInfo17Umaban", "" },
                            { "OddsTansyoInfo17Odds", "" },
                            { "OddsTansyoInfo17Ninki", "" },
                            { "OddsTansyoInfo18Umaban", "" },
                            { "OddsTansyoInfo18Odds", "" },
                            { "OddsTansyoInfo18Ninki", "" },
                            { "OddsTansyoInfo19Umaban", "" },
                            { "OddsTansyoInfo19Odds", "" },
                            { "OddsTansyoInfo19Ninki", "" },
                            { "OddsTansyoInfo20Umaban", "" },
                            { "OddsTansyoInfo20Odds", "" },
                            { "OddsTansyoInfo20Ninki", "" },
                            { "OddsTansyoInfo21Umaban", "" },
                            { "OddsTansyoInfo21Odds", "" },
                            { "OddsTansyoInfo21Ninki", "" },
                            { "OddsTansyoInfo22Umaban", "" },
                            { "OddsTansyoInfo22Odds", "" },
                            { "OddsTansyoInfo22Ninki", "" },
                            { "OddsTansyoInfo23Umaban", "" },
                            { "OddsTansyoInfo23Odds", "" },
                            { "OddsTansyoInfo23Ninki", "" },
                            { "OddsTansyoInfo24Umaban", "" },
                            { "OddsTansyoInfo24Odds", "" },
                            { "OddsTansyoInfo24Ninki", "" },
                            { "OddsTansyoInfo25Umaban", "" },
                            { "OddsTansyoInfo25Odds", "" },
                            { "OddsTansyoInfo25Ninki", "" },
                            { "OddsTansyoInfo26Umaban", "" },
                            { "OddsTansyoInfo26Odds", "" },
                            { "OddsTansyoInfo26Ninki", "" },
                            { "OddsTansyoInfo27Umaban", "" },
                            { "OddsTansyoInfo27Odds", "" },
                            { "OddsTansyoInfo27Ninki", "" },
                            { "OddsFukusyoInfo0Umaban", "01" },
                            { "OddsFukusyoInfo0OddsLow", "0032" },
                            { "OddsFukusyoInfo0OddsHigh", "0233" },
                            { "OddsFukusyoInfo0Ninki", "06" },
                            { "OddsFukusyoInfo1Umaban", "02" },
                            { "OddsFukusyoInfo1OddsLow", "0050" },
                            { "OddsFukusyoInfo1OddsHigh", "0383" },
                            { "OddsFukusyoInfo1Ninki", "08" },
                            { "OddsFukusyoInfo2Umaban", "03" },
                            { "OddsFukusyoInfo2OddsLow", "0046" },
                            { "OddsFukusyoInfo2OddsHigh", "0347" },
                            { "OddsFukusyoInfo2Ninki", "07" },
                            { "OddsFukusyoInfo3Umaban", "04" },
                            { "OddsFukusyoInfo3OddsLow", "0010" },
                            { "OddsFukusyoInfo3OddsHigh", "0011" },
                            { "OddsFukusyoInfo3Ninki", "01" },
                            { "OddsFukusyoInfo4Umaban", "05" },
                            { "OddsFukusyoInfo4OddsLow", "0016" },
                            { "OddsFukusyoInfo4OddsHigh", "0096" },
                            { "OddsFukusyoInfo4Ninki", "04" },
                            { "OddsFukusyoInfo5Umaban", "06" },
                            { "OddsFukusyoInfo5OddsLow", "0011" },
                            { "OddsFukusyoInfo5OddsHigh", "0045" },
                            { "OddsFukusyoInfo5Ninki", "02" },
                            { "OddsFukusyoInfo6Umaban", "07" },
                            { "OddsFukusyoInfo6OddsLow", "0014" },
                            { "OddsFukusyoInfo6OddsHigh", "0082" },
                            { "OddsFukusyoInfo6Ninki", "03" },
                            { "OddsFukusyoInfo7Umaban", "08" },
                            { "OddsFukusyoInfo7OddsLow", "0294" },
                            { "OddsFukusyoInfo7OddsHigh", "2406" },
                            { "OddsFukusyoInfo7Ninki", "11" },
                            { "OddsFukusyoInfo8Umaban", "09" },
                            { "OddsFukusyoInfo8OddsLow", "0019" },
                            { "OddsFukusyoInfo8OddsHigh", "0123" },
                            { "OddsFukusyoInfo8Ninki", "05" },
                            { "OddsFukusyoInfo9Umaban", "10" },
                            { "OddsFukusyoInfo9OddsLow", "0184" },
                            { "OddsFukusyoInfo9OddsHigh", "1495" },
                            { "OddsFukusyoInfo9Ninki", "10" },
                            { "OddsFukusyoInfo10Umaban", "11" },
                            { "OddsFukusyoInfo10OddsLow", "0081" },
                            { "OddsFukusyoInfo10OddsHigh", "0641" },
                            { "OddsFukusyoInfo10Ninki", "09" },
                            { "OddsFukusyoInfo11Umaban", "" },
                            { "OddsFukusyoInfo11OddsLow", "" },
                            { "OddsFukusyoInfo11OddsHigh", "" },
                            { "OddsFukusyoInfo11Ninki", "" },
                            { "OddsFukusyoInfo12Umaban", "" },
                            { "OddsFukusyoInfo12OddsLow", "" },
                            { "OddsFukusyoInfo12OddsHigh", "" },
                            { "OddsFukusyoInfo12Ninki", "" },
                            { "OddsFukusyoInfo13Umaban", "" },
                            { "OddsFukusyoInfo13OddsLow", "" },
                            { "OddsFukusyoInfo13OddsHigh", "" },
                            { "OddsFukusyoInfo13Ninki", "" },
                            { "OddsFukusyoInfo14Umaban", "" },
                            { "OddsFukusyoInfo14OddsLow", "" },
                            { "OddsFukusyoInfo14OddsHigh", "" },
                            { "OddsFukusyoInfo14Ninki", "" },
                            { "OddsFukusyoInfo15Umaban", "" },
                            { "OddsFukusyoInfo15OddsLow", "" },
                            { "OddsFukusyoInfo15OddsHigh", "" },
                            { "OddsFukusyoInfo15Ninki", "" },
                            { "OddsFukusyoInfo16Umaban", "" },
                            { "OddsFukusyoInfo16OddsLow", "" },
                            { "OddsFukusyoInfo16OddsHigh", "" },
                            { "OddsFukusyoInfo16Ninki", "" },
                            { "OddsFukusyoInfo17Umaban", "" },
                            { "OddsFukusyoInfo17OddsLow", "" },
                            { "OddsFukusyoInfo17OddsHigh", "" },
                            { "OddsFukusyoInfo17Ninki", "" },
                            { "OddsFukusyoInfo18Umaban", "" },
                            { "OddsFukusyoInfo18OddsLow", "" },
                            { "OddsFukusyoInfo18OddsHigh", "" },
                            { "OddsFukusyoInfo18Ninki", "" },
                            { "OddsFukusyoInfo19Umaban", "" },
                            { "OddsFukusyoInfo19OddsLow", "" },
                            { "OddsFukusyoInfo19OddsHigh", "" },
                            { "OddsFukusyoInfo19Ninki", "" },
                            { "OddsFukusyoInfo20Umaban", "" },
                            { "OddsFukusyoInfo20OddsLow", "" },
                            { "OddsFukusyoInfo20OddsHigh", "" },
                            { "OddsFukusyoInfo20Ninki", "" },
                            { "OddsFukusyoInfo21Umaban", "" },
                            { "OddsFukusyoInfo21OddsLow", "" },
                            { "OddsFukusyoInfo21OddsHigh", "" },
                            { "OddsFukusyoInfo21Ninki", "" },
                            { "OddsFukusyoInfo22Umaban", "" },
                            { "OddsFukusyoInfo22OddsLow", "" },
                            { "OddsFukusyoInfo22OddsHigh", "" },
                            { "OddsFukusyoInfo22Ninki", "" },
                            { "OddsFukusyoInfo23Umaban", "" },
                            { "OddsFukusyoInfo23OddsLow", "" },
                            { "OddsFukusyoInfo23OddsHigh", "" },
                            { "OddsFukusyoInfo23Ninki", "" },
                            { "OddsFukusyoInfo24Umaban", "" },
                            { "OddsFukusyoInfo24OddsLow", "" },
                            { "OddsFukusyoInfo24OddsHigh", "" },
                            { "OddsFukusyoInfo24Ninki", "" },
                            { "OddsFukusyoInfo25Umaban", "" },
                            { "OddsFukusyoInfo25OddsLow", "" },
                            { "OddsFukusyoInfo25OddsHigh", "" },
                            { "OddsFukusyoInfo25Ninki", "" },
                            { "OddsFukusyoInfo26Umaban", "" },
                            { "OddsFukusyoInfo26OddsLow", "" },
                            { "OddsFukusyoInfo26OddsHigh", "" },
                            { "OddsFukusyoInfo26Ninki", "" },
                            { "OddsFukusyoInfo27Umaban", "" },
                            { "OddsFukusyoInfo27OddsLow", "" },
                            { "OddsFukusyoInfo27OddsHigh", "" },
                            { "OddsFukusyoInfo27Ninki", "" },
                            { "OddsWakurenInfo0Kumi", "" },
                            { "OddsWakurenInfo0Odds", "" },
                            { "OddsWakurenInfo0Ninki", "" },
                            { "OddsWakurenInfo1Kumi", "12" },
                            { "OddsWakurenInfo1Odds", "03595" },
                            { "OddsWakurenInfo1Ninki", "27" },
                            { "OddsWakurenInfo2Kumi", "13" },
                            { "OddsWakurenInfo2Odds", "04703" },
                            { "OddsWakurenInfo2Ninki", "29" },
                            { "OddsWakurenInfo3Kumi", "14" },
                            { "OddsWakurenInfo3Odds", "00350" },
                            { "OddsWakurenInfo3Ninki", "10" },
                            { "OddsWakurenInfo4Kumi", "15" },
                            { "OddsWakurenInfo4Odds", "04061" },
                            { "OddsWakurenInfo4Ninki", "28" },
                            { "OddsWakurenInfo5Kumi", "16" },
                            { "OddsWakurenInfo5Odds", "00853" },
                            { "OddsWakurenInfo5Ninki", "15" },
                            { "OddsWakurenInfo6Kumi", "17" },
                            { "OddsWakurenInfo6Odds", "03421" },
                            { "OddsWakurenInfo6Ninki", "25" },
                            { "OddsWakurenInfo7Kumi", "18" },
                            { "OddsWakurenInfo7Odds", "03002" },
                            { "OddsWakurenInfo7Ninki", "23" },
                            { "OddsWakurenInfo8Kumi", "" },
                            { "OddsWakurenInfo8Odds", "" },
                            { "OddsWakurenInfo8Ninki", "" },
                            { "OddsWakurenInfo9Kumi", "23" },
                            { "OddsWakurenInfo9Odds", "03118" },
                            { "OddsWakurenInfo9Ninki", "24" },
                            { "OddsWakurenInfo10Kumi", "24" },
                            { "OddsWakurenInfo10Odds", "00348" },
                            { "OddsWakurenInfo10Ninki", "08" },
                            { "OddsWakurenInfo11Kumi", "25" },
                            { "OddsWakurenInfo11Odds", "01913" },
                            { "OddsWakurenInfo11Ninki", "20" },
                            { "OddsWakurenInfo12Kumi", "26" },
                            { "OddsWakurenInfo12Odds", "00646" },
                            { "OddsWakurenInfo12Ninki", "12" },
                            { "OddsWakurenInfo13Kumi", "27" },
                            { "OddsWakurenInfo13Odds", "02454" },
                            { "OddsWakurenInfo13Ninki", "21" },
                            { "OddsWakurenInfo14Kumi", "28" },
                            { "OddsWakurenInfo14Odds", "03421" },
                            { "OddsWakurenInfo14Ninki", "25" },
                            { "OddsWakurenInfo15Kumi", "" },
                            { "OddsWakurenInfo15Odds", "" },
                            { "OddsWakurenInfo15Ninki", "" },
                            { "OddsWakurenInfo16Kumi", "34" },
                            { "OddsWakurenInfo16Odds", "00328" },
                            { "OddsWakurenInfo16Ninki", "07" },
                            { "OddsWakurenInfo17Kumi", "35" },
                            { "OddsWakurenInfo17Odds", "01695" },
                            { "OddsWakurenInfo17Ninki", "18" },
                            { "OddsWakurenInfo18Kumi", "36" },
                            { "OddsWakurenInfo18Odds", "00661" },
                            { "OddsWakurenInfo18Ninki", "13" },
                            { "OddsWakurenInfo19Kumi", "37" },
                            { "OddsWakurenInfo19Odds", "01485" },
                            { "OddsWakurenInfo19Ninki", "17" },
                            { "OddsWakurenInfo20Kumi", "38" },
                            { "OddsWakurenInfo20Odds", "02955" },
                            { "OddsWakurenInfo20Ninki", "22" },
                            { "OddsWakurenInfo21Kumi", "" },
                            { "OddsWakurenInfo21Odds", "" },
                            { "OddsWakurenInfo21Ninki", "" },
                            { "OddsWakurenInfo22Kumi", "45" },
                            { "OddsWakurenInfo22Odds", "00079" },
                            { "OddsWakurenInfo22Ninki", "02" },
                            { "OddsWakurenInfo23Kumi", "46" },
                            { "OddsWakurenInfo23Odds", "00015" },
                            { "OddsWakurenInfo23Ninki", "01" },
                            { "OddsWakurenInfo24Kumi", "47" },
                            { "OddsWakurenInfo24Odds", "00109" },
                            { "OddsWakurenInfo24Ninki", "03" },
                            { "OddsWakurenInfo25Kumi", "48" },
                            { "OddsWakurenInfo25Odds", "00349" },
                            { "OddsWakurenInfo25Ninki", "09" },
                            { "OddsWakurenInfo26Kumi", "" },
                            { "OddsWakurenInfo26Odds", "" },
                            { "OddsWakurenInfo26Ninki", "" },
                            { "OddsWakurenInfo27Kumi", "56" },
                            { "OddsWakurenInfo27Odds", "00168" },
                            { "OddsWakurenInfo27Ninki", "05" },
                            { "OddsWakurenInfo28Kumi", "57" },
                            { "OddsWakurenInfo28Odds", "00773" },
                            { "OddsWakurenInfo28Ninki", "14" },
                            { "OddsWakurenInfo29Kumi", "58" },
                            { "OddsWakurenInfo29Odds", "01715" },
                            { "OddsWakurenInfo29Ninki", "19" },
                            { "OddsWakurenInfo30Kumi", "66" },
                            { "OddsWakurenInfo30Odds", "00135" },
                            { "OddsWakurenInfo30Ninki", "04" },
                            { "OddsWakurenInfo31Kumi", "67" },
                            { "OddsWakurenInfo31Odds", "00244" },
                            { "OddsWakurenInfo31Ninki", "06" },
                            { "OddsWakurenInfo32Kumi", "68" },
                            { "OddsWakurenInfo32Odds", "00472" },
                            { "OddsWakurenInfo32Ninki", "11" },
                            { "OddsWakurenInfo33Kumi", "77" },
                            { "OddsWakurenInfo33Odds", "08684" },
                            { "OddsWakurenInfo33Ninki", "31" },
                            { "OddsWakurenInfo34Kumi", "78" },
                            { "OddsWakurenInfo34Odds", "00906" },
                            { "OddsWakurenInfo34Ninki", "16" },
                            { "OddsWakurenInfo35Kumi", "88" },
                            { "OddsWakurenInfo35Odds", "07330" },
                            { "OddsWakurenInfo35Ninki", "30" },
                            { "TotalHyosuTansyo", "00000394142" },
                            { "TotalHyosuFukusyo", "00001587058" },
                            { "TotalHyosuWakuren", "00000072836" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_O1_ODDS_TANFUKUWAKUConditionalDataBridge_BuildUpCreateTableCommand_can_build_executable_table_creation_command()
        {
            // Arrange
            var dataBridge = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
                                                                 JVOpenOptions.RealTime);

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
                    cmd.AssertTableCreation("RT_O1_ODDS_TANFUKUWAKU", JVDataStructColumns.O1Conditional);
                }
            }
        }

        [Test]
        public void JV_O1_ODDS_TANFUKUWAKUConditionalDataBridge_BuildUpCreateTableCommand_can_create_sql_runs_multiple_times_without_error()
        {
            // Arrange
            var dataBridge = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
                                                                 JVOpenOptions.RealTime);

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
        public void JV_O1_ODDS_TANFUKUWAKUConditionalDataBridge_BuildUpCreateTableCommand_should_drop_table_when_realtime_execution()
        {
            // Arrange
            var dataBridge = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
                                                                 JVOpenOptions.RealTime);

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
                    foreach (var bltCmd in dataBridge.BuildUpCreateTableCommand(cmdCache))
                    {
                        bltCmd.ExecuteNonQuery();
                    }

                    // Assert
                    cmd.AssertRecordsAreEmpty("RT_O1_ODDS_TANFUKUWAKU");
                }
            }
        }

        [Test]
        public void JV_O1_ODDS_TANFUKUWAKUConditionalDataBridge_BuildUpInsertCommand_can_build_executable_record_insertion_command()
        {
            // Arrange
            var dataBridge = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
                                                                 JVOpenOptions.RealTime);

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
                    cmd.AssertRecords("RT_O1_ODDS_TANFUKUWAKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "O1" },
                            { "headDataKubun", "5" },
                            { "headMakeDate", "20230828" },
                            { "idYear", "2023" },
                            { "idMonthDay", "0826" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "05" },
                            { "idRaceNum", "01" },
                            { "HappyoTime", "00000000" },
                            { "TorokuTosu", "11" },
                            { "SyussoTosu", "11" },
                            { "TansyoFlag", "7" },
                            { "FukusyoFlag", "7" },
                            { "WakurenFlag", "7" },
                            { "FukuChakuBaraiKey", "3" },
                            { "OddsTansyoInfo0Umaban", "01" },
                            { "OddsTansyoInfo0Odds", "0789" },
                            { "OddsTansyoInfo0Ninki", "07" },
                            { "OddsTansyoInfo1Umaban", "02" },
                            { "OddsTansyoInfo1Odds", "1079" },
                            { "OddsTansyoInfo1Ninki", "09" },
                            { "OddsTansyoInfo2Umaban", "03" },
                            { "OddsTansyoInfo2Odds", "0585" },
                            { "OddsTansyoInfo2Ninki", "06" },
                            { "OddsTansyoInfo3Umaban", "04" },
                            { "OddsTansyoInfo3Odds", "0012" },
                            { "OddsTansyoInfo3Ninki", "01" },
                            { "OddsTansyoInfo4Umaban", "05" },
                            { "OddsTansyoInfo4Odds", "0176" },
                            { "OddsTansyoInfo4Ninki", "04" },
                            { "OddsTansyoInfo5Umaban", "06" },
                            { "OddsTansyoInfo5Odds", "0056" },
                            { "OddsTansyoInfo5Ninki", "02" },
                            { "OddsTansyoInfo6Umaban", "07" },
                            { "OddsTansyoInfo6Odds", "0091" },
                            { "OddsTansyoInfo6Ninki", "03" },
                            { "OddsTansyoInfo7Umaban", "08" },
                            { "OddsTansyoInfo7Odds", "4608" },
                            { "OddsTansyoInfo7Ninki", "11" },
                            { "OddsTansyoInfo8Umaban", "09" },
                            { "OddsTansyoInfo8Odds", "0190" },
                            { "OddsTansyoInfo8Ninki", "05" },
                            { "OddsTansyoInfo9Umaban", "10" },
                            { "OddsTansyoInfo9Odds", "2835" },
                            { "OddsTansyoInfo9Ninki", "10" },
                            { "OddsTansyoInfo10Umaban", "11" },
                            { "OddsTansyoInfo10Odds", "0897" },
                            { "OddsTansyoInfo10Ninki", "08" },
                            { "OddsTansyoInfo11Umaban", "" },
                            { "OddsTansyoInfo11Odds", "" },
                            { "OddsTansyoInfo11Ninki", "" },
                            { "OddsTansyoInfo12Umaban", "" },
                            { "OddsTansyoInfo12Odds", "" },
                            { "OddsTansyoInfo12Ninki", "" },
                            { "OddsTansyoInfo13Umaban", "" },
                            { "OddsTansyoInfo13Odds", "" },
                            { "OddsTansyoInfo13Ninki", "" },
                            { "OddsTansyoInfo14Umaban", "" },
                            { "OddsTansyoInfo14Odds", "" },
                            { "OddsTansyoInfo14Ninki", "" },
                            { "OddsTansyoInfo15Umaban", "" },
                            { "OddsTansyoInfo15Odds", "" },
                            { "OddsTansyoInfo15Ninki", "" },
                            { "OddsTansyoInfo16Umaban", "" },
                            { "OddsTansyoInfo16Odds", "" },
                            { "OddsTansyoInfo16Ninki", "" },
                            { "OddsTansyoInfo17Umaban", "" },
                            { "OddsTansyoInfo17Odds", "" },
                            { "OddsTansyoInfo17Ninki", "" },
                            { "OddsTansyoInfo18Umaban", "" },
                            { "OddsTansyoInfo18Odds", "" },
                            { "OddsTansyoInfo18Ninki", "" },
                            { "OddsTansyoInfo19Umaban", "" },
                            { "OddsTansyoInfo19Odds", "" },
                            { "OddsTansyoInfo19Ninki", "" },
                            { "OddsTansyoInfo20Umaban", "" },
                            { "OddsTansyoInfo20Odds", "" },
                            { "OddsTansyoInfo20Ninki", "" },
                            { "OddsTansyoInfo21Umaban", "" },
                            { "OddsTansyoInfo21Odds", "" },
                            { "OddsTansyoInfo21Ninki", "" },
                            { "OddsTansyoInfo22Umaban", "" },
                            { "OddsTansyoInfo22Odds", "" },
                            { "OddsTansyoInfo22Ninki", "" },
                            { "OddsTansyoInfo23Umaban", "" },
                            { "OddsTansyoInfo23Odds", "" },
                            { "OddsTansyoInfo23Ninki", "" },
                            { "OddsTansyoInfo24Umaban", "" },
                            { "OddsTansyoInfo24Odds", "" },
                            { "OddsTansyoInfo24Ninki", "" },
                            { "OddsTansyoInfo25Umaban", "" },
                            { "OddsTansyoInfo25Odds", "" },
                            { "OddsTansyoInfo25Ninki", "" },
                            { "OddsTansyoInfo26Umaban", "" },
                            { "OddsTansyoInfo26Odds", "" },
                            { "OddsTansyoInfo26Ninki", "" },
                            { "OddsTansyoInfo27Umaban", "" },
                            { "OddsTansyoInfo27Odds", "" },
                            { "OddsTansyoInfo27Ninki", "" },
                            { "OddsFukusyoInfo0Umaban", "01" },
                            { "OddsFukusyoInfo0OddsLow", "0032" },
                            { "OddsFukusyoInfo0OddsHigh", "0233" },
                            { "OddsFukusyoInfo0Ninki", "06" },
                            { "OddsFukusyoInfo1Umaban", "02" },
                            { "OddsFukusyoInfo1OddsLow", "0050" },
                            { "OddsFukusyoInfo1OddsHigh", "0383" },
                            { "OddsFukusyoInfo1Ninki", "08" },
                            { "OddsFukusyoInfo2Umaban", "03" },
                            { "OddsFukusyoInfo2OddsLow", "0046" },
                            { "OddsFukusyoInfo2OddsHigh", "0347" },
                            { "OddsFukusyoInfo2Ninki", "07" },
                            { "OddsFukusyoInfo3Umaban", "04" },
                            { "OddsFukusyoInfo3OddsLow", "0010" },
                            { "OddsFukusyoInfo3OddsHigh", "0011" },
                            { "OddsFukusyoInfo3Ninki", "01" },
                            { "OddsFukusyoInfo4Umaban", "05" },
                            { "OddsFukusyoInfo4OddsLow", "0016" },
                            { "OddsFukusyoInfo4OddsHigh", "0096" },
                            { "OddsFukusyoInfo4Ninki", "04" },
                            { "OddsFukusyoInfo5Umaban", "06" },
                            { "OddsFukusyoInfo5OddsLow", "0011" },
                            { "OddsFukusyoInfo5OddsHigh", "0045" },
                            { "OddsFukusyoInfo5Ninki", "02" },
                            { "OddsFukusyoInfo6Umaban", "07" },
                            { "OddsFukusyoInfo6OddsLow", "0014" },
                            { "OddsFukusyoInfo6OddsHigh", "0082" },
                            { "OddsFukusyoInfo6Ninki", "03" },
                            { "OddsFukusyoInfo7Umaban", "08" },
                            { "OddsFukusyoInfo7OddsLow", "0294" },
                            { "OddsFukusyoInfo7OddsHigh", "2406" },
                            { "OddsFukusyoInfo7Ninki", "11" },
                            { "OddsFukusyoInfo8Umaban", "09" },
                            { "OddsFukusyoInfo8OddsLow", "0019" },
                            { "OddsFukusyoInfo8OddsHigh", "0123" },
                            { "OddsFukusyoInfo8Ninki", "05" },
                            { "OddsFukusyoInfo9Umaban", "10" },
                            { "OddsFukusyoInfo9OddsLow", "0184" },
                            { "OddsFukusyoInfo9OddsHigh", "1495" },
                            { "OddsFukusyoInfo9Ninki", "10" },
                            { "OddsFukusyoInfo10Umaban", "11" },
                            { "OddsFukusyoInfo10OddsLow", "0081" },
                            { "OddsFukusyoInfo10OddsHigh", "0641" },
                            { "OddsFukusyoInfo10Ninki", "09" },
                            { "OddsFukusyoInfo11Umaban", "" },
                            { "OddsFukusyoInfo11OddsLow", "" },
                            { "OddsFukusyoInfo11OddsHigh", "" },
                            { "OddsFukusyoInfo11Ninki", "" },
                            { "OddsFukusyoInfo12Umaban", "" },
                            { "OddsFukusyoInfo12OddsLow", "" },
                            { "OddsFukusyoInfo12OddsHigh", "" },
                            { "OddsFukusyoInfo12Ninki", "" },
                            { "OddsFukusyoInfo13Umaban", "" },
                            { "OddsFukusyoInfo13OddsLow", "" },
                            { "OddsFukusyoInfo13OddsHigh", "" },
                            { "OddsFukusyoInfo13Ninki", "" },
                            { "OddsFukusyoInfo14Umaban", "" },
                            { "OddsFukusyoInfo14OddsLow", "" },
                            { "OddsFukusyoInfo14OddsHigh", "" },
                            { "OddsFukusyoInfo14Ninki", "" },
                            { "OddsFukusyoInfo15Umaban", "" },
                            { "OddsFukusyoInfo15OddsLow", "" },
                            { "OddsFukusyoInfo15OddsHigh", "" },
                            { "OddsFukusyoInfo15Ninki", "" },
                            { "OddsFukusyoInfo16Umaban", "" },
                            { "OddsFukusyoInfo16OddsLow", "" },
                            { "OddsFukusyoInfo16OddsHigh", "" },
                            { "OddsFukusyoInfo16Ninki", "" },
                            { "OddsFukusyoInfo17Umaban", "" },
                            { "OddsFukusyoInfo17OddsLow", "" },
                            { "OddsFukusyoInfo17OddsHigh", "" },
                            { "OddsFukusyoInfo17Ninki", "" },
                            { "OddsFukusyoInfo18Umaban", "" },
                            { "OddsFukusyoInfo18OddsLow", "" },
                            { "OddsFukusyoInfo18OddsHigh", "" },
                            { "OddsFukusyoInfo18Ninki", "" },
                            { "OddsFukusyoInfo19Umaban", "" },
                            { "OddsFukusyoInfo19OddsLow", "" },
                            { "OddsFukusyoInfo19OddsHigh", "" },
                            { "OddsFukusyoInfo19Ninki", "" },
                            { "OddsFukusyoInfo20Umaban", "" },
                            { "OddsFukusyoInfo20OddsLow", "" },
                            { "OddsFukusyoInfo20OddsHigh", "" },
                            { "OddsFukusyoInfo20Ninki", "" },
                            { "OddsFukusyoInfo21Umaban", "" },
                            { "OddsFukusyoInfo21OddsLow", "" },
                            { "OddsFukusyoInfo21OddsHigh", "" },
                            { "OddsFukusyoInfo21Ninki", "" },
                            { "OddsFukusyoInfo22Umaban", "" },
                            { "OddsFukusyoInfo22OddsLow", "" },
                            { "OddsFukusyoInfo22OddsHigh", "" },
                            { "OddsFukusyoInfo22Ninki", "" },
                            { "OddsFukusyoInfo23Umaban", "" },
                            { "OddsFukusyoInfo23OddsLow", "" },
                            { "OddsFukusyoInfo23OddsHigh", "" },
                            { "OddsFukusyoInfo23Ninki", "" },
                            { "OddsFukusyoInfo24Umaban", "" },
                            { "OddsFukusyoInfo24OddsLow", "" },
                            { "OddsFukusyoInfo24OddsHigh", "" },
                            { "OddsFukusyoInfo24Ninki", "" },
                            { "OddsFukusyoInfo25Umaban", "" },
                            { "OddsFukusyoInfo25OddsLow", "" },
                            { "OddsFukusyoInfo25OddsHigh", "" },
                            { "OddsFukusyoInfo25Ninki", "" },
                            { "OddsFukusyoInfo26Umaban", "" },
                            { "OddsFukusyoInfo26OddsLow", "" },
                            { "OddsFukusyoInfo26OddsHigh", "" },
                            { "OddsFukusyoInfo26Ninki", "" },
                            { "OddsFukusyoInfo27Umaban", "" },
                            { "OddsFukusyoInfo27OddsLow", "" },
                            { "OddsFukusyoInfo27OddsHigh", "" },
                            { "OddsFukusyoInfo27Ninki", "" },
                            { "OddsWakurenInfo0Kumi", "" },
                            { "OddsWakurenInfo0Odds", "" },
                            { "OddsWakurenInfo0Ninki", "" },
                            { "OddsWakurenInfo1Kumi", "12" },
                            { "OddsWakurenInfo1Odds", "03595" },
                            { "OddsWakurenInfo1Ninki", "27" },
                            { "OddsWakurenInfo2Kumi", "13" },
                            { "OddsWakurenInfo2Odds", "04703" },
                            { "OddsWakurenInfo2Ninki", "29" },
                            { "OddsWakurenInfo3Kumi", "14" },
                            { "OddsWakurenInfo3Odds", "00350" },
                            { "OddsWakurenInfo3Ninki", "10" },
                            { "OddsWakurenInfo4Kumi", "15" },
                            { "OddsWakurenInfo4Odds", "04061" },
                            { "OddsWakurenInfo4Ninki", "28" },
                            { "OddsWakurenInfo5Kumi", "16" },
                            { "OddsWakurenInfo5Odds", "00853" },
                            { "OddsWakurenInfo5Ninki", "15" },
                            { "OddsWakurenInfo6Kumi", "17" },
                            { "OddsWakurenInfo6Odds", "03421" },
                            { "OddsWakurenInfo6Ninki", "25" },
                            { "OddsWakurenInfo7Kumi", "18" },
                            { "OddsWakurenInfo7Odds", "03002" },
                            { "OddsWakurenInfo7Ninki", "23" },
                            { "OddsWakurenInfo8Kumi", "" },
                            { "OddsWakurenInfo8Odds", "" },
                            { "OddsWakurenInfo8Ninki", "" },
                            { "OddsWakurenInfo9Kumi", "23" },
                            { "OddsWakurenInfo9Odds", "03118" },
                            { "OddsWakurenInfo9Ninki", "24" },
                            { "OddsWakurenInfo10Kumi", "24" },
                            { "OddsWakurenInfo10Odds", "00347" },
                            { "OddsWakurenInfo10Ninki", "08" },
                            { "OddsWakurenInfo11Kumi", "25" },
                            { "OddsWakurenInfo11Odds", "01913" },
                            { "OddsWakurenInfo11Ninki", "20" },
                            { "OddsWakurenInfo12Kumi", "26" },
                            { "OddsWakurenInfo12Odds", "00646" },
                            { "OddsWakurenInfo12Ninki", "12" },
                            { "OddsWakurenInfo13Kumi", "27" },
                            { "OddsWakurenInfo13Odds", "02454" },
                            { "OddsWakurenInfo13Ninki", "21" },
                            { "OddsWakurenInfo14Kumi", "28" },
                            { "OddsWakurenInfo14Odds", "03421" },
                            { "OddsWakurenInfo14Ninki", "25" },
                            { "OddsWakurenInfo15Kumi", "" },
                            { "OddsWakurenInfo15Odds", "" },
                            { "OddsWakurenInfo15Ninki", "" },
                            { "OddsWakurenInfo16Kumi", "34" },
                            { "OddsWakurenInfo16Odds", "00328" },
                            { "OddsWakurenInfo16Ninki", "07" },
                            { "OddsWakurenInfo17Kumi", "35" },
                            { "OddsWakurenInfo17Odds", "01695" },
                            { "OddsWakurenInfo17Ninki", "18" },
                            { "OddsWakurenInfo18Kumi", "36" },
                            { "OddsWakurenInfo18Odds", "00661" },
                            { "OddsWakurenInfo18Ninki", "13" },
                            { "OddsWakurenInfo19Kumi", "37" },
                            { "OddsWakurenInfo19Odds", "01485" },
                            { "OddsWakurenInfo19Ninki", "17" },
                            { "OddsWakurenInfo20Kumi", "38" },
                            { "OddsWakurenInfo20Odds", "02955" },
                            { "OddsWakurenInfo20Ninki", "22" },
                            { "OddsWakurenInfo21Kumi", "" },
                            { "OddsWakurenInfo21Odds", "" },
                            { "OddsWakurenInfo21Ninki", "" },
                            { "OddsWakurenInfo22Kumi", "45" },
                            { "OddsWakurenInfo22Odds", "00079" },
                            { "OddsWakurenInfo22Ninki", "02" },
                            { "OddsWakurenInfo23Kumi", "46" },
                            { "OddsWakurenInfo23Odds", "00015" },
                            { "OddsWakurenInfo23Ninki", "01" },
                            { "OddsWakurenInfo24Kumi", "47" },
                            { "OddsWakurenInfo24Odds", "00109" },
                            { "OddsWakurenInfo24Ninki", "03" },
                            { "OddsWakurenInfo25Kumi", "48" },
                            { "OddsWakurenInfo25Odds", "00349" },
                            { "OddsWakurenInfo25Ninki", "09" },
                            { "OddsWakurenInfo26Kumi", "" },
                            { "OddsWakurenInfo26Odds", "" },
                            { "OddsWakurenInfo26Ninki", "" },
                            { "OddsWakurenInfo27Kumi", "56" },
                            { "OddsWakurenInfo27Odds", "00168" },
                            { "OddsWakurenInfo27Ninki", "05" },
                            { "OddsWakurenInfo28Kumi", "57" },
                            { "OddsWakurenInfo28Odds", "00773" },
                            { "OddsWakurenInfo28Ninki", "14" },
                            { "OddsWakurenInfo29Kumi", "58" },
                            { "OddsWakurenInfo29Odds", "01715" },
                            { "OddsWakurenInfo29Ninki", "19" },
                            { "OddsWakurenInfo30Kumi", "66" },
                            { "OddsWakurenInfo30Odds", "00135" },
                            { "OddsWakurenInfo30Ninki", "04" },
                            { "OddsWakurenInfo31Kumi", "67" },
                            { "OddsWakurenInfo31Odds", "00244" },
                            { "OddsWakurenInfo31Ninki", "06" },
                            { "OddsWakurenInfo32Kumi", "68" },
                            { "OddsWakurenInfo32Odds", "00472" },
                            { "OddsWakurenInfo32Ninki", "11" },
                            { "OddsWakurenInfo33Kumi", "77" },
                            { "OddsWakurenInfo33Odds", "08684" },
                            { "OddsWakurenInfo33Ninki", "31" },
                            { "OddsWakurenInfo34Kumi", "78" },
                            { "OddsWakurenInfo34Odds", "00906" },
                            { "OddsWakurenInfo34Ninki", "16" },
                            { "OddsWakurenInfo35Kumi", "88" },
                            { "OddsWakurenInfo35Odds", "07330" },
                            { "OddsWakurenInfo35Ninki", "30" },
                            { "TotalHyosuTansyo", "00000394142" },
                            { "TotalHyosuFukusyo", "00001587058" },
                            { "TotalHyosuWakuren", "00000072836" }
                        },
                    });
                }
            }
        }

        [Test]
        public void JV_O1_ODDS_TANFUKUWAKUConditionalDataBridge_BuildUpInsertCommand_can_create_an_insert_command_that_overwrites_at_multiple_executions()
        {
            // Arrange
            var dataBridge1 = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077314580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
                                                                  JVOpenOptions.RealTime);
            var dataBridge2 = NewJV_O1_ODDS_TANFUKUWAKUDataBridge("O1520230828202308260102050100000000111177730107890702107909030585060400120105017604060056020700910308460811090190051028351011089708                                                                                                                                        010032023306020050038308030046034707040010001101050016009604060011004502070014008203080294240611090019012305100184149510110081064109                                                                                                                                                                                                                     120359527130470329140035010150406128160085315170342125180300223         230311824240034708250191320260064612270245421280342125         340032807350169518360066113370148517380295522         450007902460001501470010903480034909         560016805570077414580171519660013504670024406680047211770868431780090616880733030000003941420000158705800000072836\r\n",
                                                                  JVOpenOptions.RealTime);

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
                    cmd.AssertRecords("RT_O1_ODDS_TANFUKUWAKU", new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            { "headRecordSpec", "O1" },
                            { "headDataKubun", "5" },
                            { "headMakeDate", "20230828" },
                            { "idYear", "2023" },
                            { "idMonthDay", "0826" },
                            { "idJyoCD", "01" },
                            { "idKaiji", "02" },
                            { "idNichiji", "05" },
                            { "idRaceNum", "01" },
                            { "HappyoTime", "00000000" },
                            { "TorokuTosu", "11" },
                            { "SyussoTosu", "11" },
                            { "TansyoFlag", "7" },
                            { "FukusyoFlag", "7" },
                            { "WakurenFlag", "7" },
                            { "FukuChakuBaraiKey", "3" },
                            { "OddsTansyoInfo0Umaban", "01" },
                            { "OddsTansyoInfo0Odds", "0789" },
                            { "OddsTansyoInfo0Ninki", "07" },
                            { "OddsTansyoInfo1Umaban", "02" },
                            { "OddsTansyoInfo1Odds", "1079" },
                            { "OddsTansyoInfo1Ninki", "09" },
                            { "OddsTansyoInfo2Umaban", "03" },
                            { "OddsTansyoInfo2Odds", "0585" },
                            { "OddsTansyoInfo2Ninki", "06" },
                            { "OddsTansyoInfo3Umaban", "04" },
                            { "OddsTansyoInfo3Odds", "0012" },
                            { "OddsTansyoInfo3Ninki", "01" },
                            { "OddsTansyoInfo4Umaban", "05" },
                            { "OddsTansyoInfo4Odds", "0176" },
                            { "OddsTansyoInfo4Ninki", "04" },
                            { "OddsTansyoInfo5Umaban", "06" },
                            { "OddsTansyoInfo5Odds", "0056" },
                            { "OddsTansyoInfo5Ninki", "02" },
                            { "OddsTansyoInfo6Umaban", "07" },
                            { "OddsTansyoInfo6Odds", "0091" },
                            { "OddsTansyoInfo6Ninki", "03" },
                            { "OddsTansyoInfo7Umaban", "08" },
                            { "OddsTansyoInfo7Odds", "4608" },
                            { "OddsTansyoInfo7Ninki", "11" },
                            { "OddsTansyoInfo8Umaban", "09" },
                            { "OddsTansyoInfo8Odds", "0190" },
                            { "OddsTansyoInfo8Ninki", "05" },
                            { "OddsTansyoInfo9Umaban", "10" },
                            { "OddsTansyoInfo9Odds", "2835" },
                            { "OddsTansyoInfo9Ninki", "10" },
                            { "OddsTansyoInfo10Umaban", "11" },
                            { "OddsTansyoInfo10Odds", "0897" },
                            { "OddsTansyoInfo10Ninki", "08" },
                            { "OddsTansyoInfo11Umaban", "" },
                            { "OddsTansyoInfo11Odds", "" },
                            { "OddsTansyoInfo11Ninki", "" },
                            { "OddsTansyoInfo12Umaban", "" },
                            { "OddsTansyoInfo12Odds", "" },
                            { "OddsTansyoInfo12Ninki", "" },
                            { "OddsTansyoInfo13Umaban", "" },
                            { "OddsTansyoInfo13Odds", "" },
                            { "OddsTansyoInfo13Ninki", "" },
                            { "OddsTansyoInfo14Umaban", "" },
                            { "OddsTansyoInfo14Odds", "" },
                            { "OddsTansyoInfo14Ninki", "" },
                            { "OddsTansyoInfo15Umaban", "" },
                            { "OddsTansyoInfo15Odds", "" },
                            { "OddsTansyoInfo15Ninki", "" },
                            { "OddsTansyoInfo16Umaban", "" },
                            { "OddsTansyoInfo16Odds", "" },
                            { "OddsTansyoInfo16Ninki", "" },
                            { "OddsTansyoInfo17Umaban", "" },
                            { "OddsTansyoInfo17Odds", "" },
                            { "OddsTansyoInfo17Ninki", "" },
                            { "OddsTansyoInfo18Umaban", "" },
                            { "OddsTansyoInfo18Odds", "" },
                            { "OddsTansyoInfo18Ninki", "" },
                            { "OddsTansyoInfo19Umaban", "" },
                            { "OddsTansyoInfo19Odds", "" },
                            { "OddsTansyoInfo19Ninki", "" },
                            { "OddsTansyoInfo20Umaban", "" },
                            { "OddsTansyoInfo20Odds", "" },
                            { "OddsTansyoInfo20Ninki", "" },
                            { "OddsTansyoInfo21Umaban", "" },
                            { "OddsTansyoInfo21Odds", "" },
                            { "OddsTansyoInfo21Ninki", "" },
                            { "OddsTansyoInfo22Umaban", "" },
                            { "OddsTansyoInfo22Odds", "" },
                            { "OddsTansyoInfo22Ninki", "" },
                            { "OddsTansyoInfo23Umaban", "" },
                            { "OddsTansyoInfo23Odds", "" },
                            { "OddsTansyoInfo23Ninki", "" },
                            { "OddsTansyoInfo24Umaban", "" },
                            { "OddsTansyoInfo24Odds", "" },
                            { "OddsTansyoInfo24Ninki", "" },
                            { "OddsTansyoInfo25Umaban", "" },
                            { "OddsTansyoInfo25Odds", "" },
                            { "OddsTansyoInfo25Ninki", "" },
                            { "OddsTansyoInfo26Umaban", "" },
                            { "OddsTansyoInfo26Odds", "" },
                            { "OddsTansyoInfo26Ninki", "" },
                            { "OddsTansyoInfo27Umaban", "" },
                            { "OddsTansyoInfo27Odds", "" },
                            { "OddsTansyoInfo27Ninki", "" },
                            { "OddsFukusyoInfo0Umaban", "01" },
                            { "OddsFukusyoInfo0OddsLow", "0032" },
                            { "OddsFukusyoInfo0OddsHigh", "0233" },
                            { "OddsFukusyoInfo0Ninki", "06" },
                            { "OddsFukusyoInfo1Umaban", "02" },
                            { "OddsFukusyoInfo1OddsLow", "0050" },
                            { "OddsFukusyoInfo1OddsHigh", "0383" },
                            { "OddsFukusyoInfo1Ninki", "08" },
                            { "OddsFukusyoInfo2Umaban", "03" },
                            { "OddsFukusyoInfo2OddsLow", "0046" },
                            { "OddsFukusyoInfo2OddsHigh", "0347" },
                            { "OddsFukusyoInfo2Ninki", "07" },
                            { "OddsFukusyoInfo3Umaban", "04" },
                            { "OddsFukusyoInfo3OddsLow", "0010" },
                            { "OddsFukusyoInfo3OddsHigh", "0011" },
                            { "OddsFukusyoInfo3Ninki", "01" },
                            { "OddsFukusyoInfo4Umaban", "05" },
                            { "OddsFukusyoInfo4OddsLow", "0016" },
                            { "OddsFukusyoInfo4OddsHigh", "0096" },
                            { "OddsFukusyoInfo4Ninki", "04" },
                            { "OddsFukusyoInfo5Umaban", "06" },
                            { "OddsFukusyoInfo5OddsLow", "0011" },
                            { "OddsFukusyoInfo5OddsHigh", "0045" },
                            { "OddsFukusyoInfo5Ninki", "02" },
                            { "OddsFukusyoInfo6Umaban", "07" },
                            { "OddsFukusyoInfo6OddsLow", "0014" },
                            { "OddsFukusyoInfo6OddsHigh", "0082" },
                            { "OddsFukusyoInfo6Ninki", "03" },
                            { "OddsFukusyoInfo7Umaban", "08" },
                            { "OddsFukusyoInfo7OddsLow", "0294" },
                            { "OddsFukusyoInfo7OddsHigh", "2406" },
                            { "OddsFukusyoInfo7Ninki", "11" },
                            { "OddsFukusyoInfo8Umaban", "09" },
                            { "OddsFukusyoInfo8OddsLow", "0019" },
                            { "OddsFukusyoInfo8OddsHigh", "0123" },
                            { "OddsFukusyoInfo8Ninki", "05" },
                            { "OddsFukusyoInfo9Umaban", "10" },
                            { "OddsFukusyoInfo9OddsLow", "0184" },
                            { "OddsFukusyoInfo9OddsHigh", "1495" },
                            { "OddsFukusyoInfo9Ninki", "10" },
                            { "OddsFukusyoInfo10Umaban", "11" },
                            { "OddsFukusyoInfo10OddsLow", "0081" },
                            { "OddsFukusyoInfo10OddsHigh", "0641" },
                            { "OddsFukusyoInfo10Ninki", "09" },
                            { "OddsFukusyoInfo11Umaban", "" },
                            { "OddsFukusyoInfo11OddsLow", "" },
                            { "OddsFukusyoInfo11OddsHigh", "" },
                            { "OddsFukusyoInfo11Ninki", "" },
                            { "OddsFukusyoInfo12Umaban", "" },
                            { "OddsFukusyoInfo12OddsLow", "" },
                            { "OddsFukusyoInfo12OddsHigh", "" },
                            { "OddsFukusyoInfo12Ninki", "" },
                            { "OddsFukusyoInfo13Umaban", "" },
                            { "OddsFukusyoInfo13OddsLow", "" },
                            { "OddsFukusyoInfo13OddsHigh", "" },
                            { "OddsFukusyoInfo13Ninki", "" },
                            { "OddsFukusyoInfo14Umaban", "" },
                            { "OddsFukusyoInfo14OddsLow", "" },
                            { "OddsFukusyoInfo14OddsHigh", "" },
                            { "OddsFukusyoInfo14Ninki", "" },
                            { "OddsFukusyoInfo15Umaban", "" },
                            { "OddsFukusyoInfo15OddsLow", "" },
                            { "OddsFukusyoInfo15OddsHigh", "" },
                            { "OddsFukusyoInfo15Ninki", "" },
                            { "OddsFukusyoInfo16Umaban", "" },
                            { "OddsFukusyoInfo16OddsLow", "" },
                            { "OddsFukusyoInfo16OddsHigh", "" },
                            { "OddsFukusyoInfo16Ninki", "" },
                            { "OddsFukusyoInfo17Umaban", "" },
                            { "OddsFukusyoInfo17OddsLow", "" },
                            { "OddsFukusyoInfo17OddsHigh", "" },
                            { "OddsFukusyoInfo17Ninki", "" },
                            { "OddsFukusyoInfo18Umaban", "" },
                            { "OddsFukusyoInfo18OddsLow", "" },
                            { "OddsFukusyoInfo18OddsHigh", "" },
                            { "OddsFukusyoInfo18Ninki", "" },
                            { "OddsFukusyoInfo19Umaban", "" },
                            { "OddsFukusyoInfo19OddsLow", "" },
                            { "OddsFukusyoInfo19OddsHigh", "" },
                            { "OddsFukusyoInfo19Ninki", "" },
                            { "OddsFukusyoInfo20Umaban", "" },
                            { "OddsFukusyoInfo20OddsLow", "" },
                            { "OddsFukusyoInfo20OddsHigh", "" },
                            { "OddsFukusyoInfo20Ninki", "" },
                            { "OddsFukusyoInfo21Umaban", "" },
                            { "OddsFukusyoInfo21OddsLow", "" },
                            { "OddsFukusyoInfo21OddsHigh", "" },
                            { "OddsFukusyoInfo21Ninki", "" },
                            { "OddsFukusyoInfo22Umaban", "" },
                            { "OddsFukusyoInfo22OddsLow", "" },
                            { "OddsFukusyoInfo22OddsHigh", "" },
                            { "OddsFukusyoInfo22Ninki", "" },
                            { "OddsFukusyoInfo23Umaban", "" },
                            { "OddsFukusyoInfo23OddsLow", "" },
                            { "OddsFukusyoInfo23OddsHigh", "" },
                            { "OddsFukusyoInfo23Ninki", "" },
                            { "OddsFukusyoInfo24Umaban", "" },
                            { "OddsFukusyoInfo24OddsLow", "" },
                            { "OddsFukusyoInfo24OddsHigh", "" },
                            { "OddsFukusyoInfo24Ninki", "" },
                            { "OddsFukusyoInfo25Umaban", "" },
                            { "OddsFukusyoInfo25OddsLow", "" },
                            { "OddsFukusyoInfo25OddsHigh", "" },
                            { "OddsFukusyoInfo25Ninki", "" },
                            { "OddsFukusyoInfo26Umaban", "" },
                            { "OddsFukusyoInfo26OddsLow", "" },
                            { "OddsFukusyoInfo26OddsHigh", "" },
                            { "OddsFukusyoInfo26Ninki", "" },
                            { "OddsFukusyoInfo27Umaban", "" },
                            { "OddsFukusyoInfo27OddsLow", "" },
                            { "OddsFukusyoInfo27OddsHigh", "" },
                            { "OddsFukusyoInfo27Ninki", "" },
                            { "OddsWakurenInfo0Kumi", "" },
                            { "OddsWakurenInfo0Odds", "" },
                            { "OddsWakurenInfo0Ninki", "" },
                            { "OddsWakurenInfo1Kumi", "12" },
                            { "OddsWakurenInfo1Odds", "03595" },
                            { "OddsWakurenInfo1Ninki", "27" },
                            { "OddsWakurenInfo2Kumi", "13" },
                            { "OddsWakurenInfo2Odds", "04703" },
                            { "OddsWakurenInfo2Ninki", "29" },
                            { "OddsWakurenInfo3Kumi", "14" },
                            { "OddsWakurenInfo3Odds", "00350" },
                            { "OddsWakurenInfo3Ninki", "10" },
                            { "OddsWakurenInfo4Kumi", "15" },
                            { "OddsWakurenInfo4Odds", "04061" },
                            { "OddsWakurenInfo4Ninki", "28" },
                            { "OddsWakurenInfo5Kumi", "16" },
                            { "OddsWakurenInfo5Odds", "00853" },
                            { "OddsWakurenInfo5Ninki", "15" },
                            { "OddsWakurenInfo6Kumi", "17" },
                            { "OddsWakurenInfo6Odds", "03421" },
                            { "OddsWakurenInfo6Ninki", "25" },
                            { "OddsWakurenInfo7Kumi", "18" },
                            { "OddsWakurenInfo7Odds", "03002" },
                            { "OddsWakurenInfo7Ninki", "23" },
                            { "OddsWakurenInfo8Kumi", "" },
                            { "OddsWakurenInfo8Odds", "" },
                            { "OddsWakurenInfo8Ninki", "" },
                            { "OddsWakurenInfo9Kumi", "23" },
                            { "OddsWakurenInfo9Odds", "03118" },
                            { "OddsWakurenInfo9Ninki", "24" },
                            { "OddsWakurenInfo10Kumi", "24" },
                            { "OddsWakurenInfo10Odds", "00347" },
                            { "OddsWakurenInfo10Ninki", "08" },
                            { "OddsWakurenInfo11Kumi", "25" },
                            { "OddsWakurenInfo11Odds", "01913" },
                            { "OddsWakurenInfo11Ninki", "20" },
                            { "OddsWakurenInfo12Kumi", "26" },
                            { "OddsWakurenInfo12Odds", "00646" },
                            { "OddsWakurenInfo12Ninki", "12" },
                            { "OddsWakurenInfo13Kumi", "27" },
                            { "OddsWakurenInfo13Odds", "02454" },
                            { "OddsWakurenInfo13Ninki", "21" },
                            { "OddsWakurenInfo14Kumi", "28" },
                            { "OddsWakurenInfo14Odds", "03421" },
                            { "OddsWakurenInfo14Ninki", "25" },
                            { "OddsWakurenInfo15Kumi", "" },
                            { "OddsWakurenInfo15Odds", "" },
                            { "OddsWakurenInfo15Ninki", "" },
                            { "OddsWakurenInfo16Kumi", "34" },
                            { "OddsWakurenInfo16Odds", "00328" },
                            { "OddsWakurenInfo16Ninki", "07" },
                            { "OddsWakurenInfo17Kumi", "35" },
                            { "OddsWakurenInfo17Odds", "01695" },
                            { "OddsWakurenInfo17Ninki", "18" },
                            { "OddsWakurenInfo18Kumi", "36" },
                            { "OddsWakurenInfo18Odds", "00661" },
                            { "OddsWakurenInfo18Ninki", "13" },
                            { "OddsWakurenInfo19Kumi", "37" },
                            { "OddsWakurenInfo19Odds", "01485" },
                            { "OddsWakurenInfo19Ninki", "17" },
                            { "OddsWakurenInfo20Kumi", "38" },
                            { "OddsWakurenInfo20Odds", "02955" },
                            { "OddsWakurenInfo20Ninki", "22" },
                            { "OddsWakurenInfo21Kumi", "" },
                            { "OddsWakurenInfo21Odds", "" },
                            { "OddsWakurenInfo21Ninki", "" },
                            { "OddsWakurenInfo22Kumi", "45" },
                            { "OddsWakurenInfo22Odds", "00079" },
                            { "OddsWakurenInfo22Ninki", "02" },
                            { "OddsWakurenInfo23Kumi", "46" },
                            { "OddsWakurenInfo23Odds", "00015" },
                            { "OddsWakurenInfo23Ninki", "01" },
                            { "OddsWakurenInfo24Kumi", "47" },
                            { "OddsWakurenInfo24Odds", "00109" },
                            { "OddsWakurenInfo24Ninki", "03" },
                            { "OddsWakurenInfo25Kumi", "48" },
                            { "OddsWakurenInfo25Odds", "00349" },
                            { "OddsWakurenInfo25Ninki", "09" },
                            { "OddsWakurenInfo26Kumi", "" },
                            { "OddsWakurenInfo26Odds", "" },
                            { "OddsWakurenInfo26Ninki", "" },
                            { "OddsWakurenInfo27Kumi", "56" },
                            { "OddsWakurenInfo27Odds", "00168" },
                            { "OddsWakurenInfo27Ninki", "05" },
                            { "OddsWakurenInfo28Kumi", "57" },
                            { "OddsWakurenInfo28Odds", "00774" },
                            { "OddsWakurenInfo28Ninki", "14" },
                            { "OddsWakurenInfo29Kumi", "58" },
                            { "OddsWakurenInfo29Odds", "01715" },
                            { "OddsWakurenInfo29Ninki", "19" },
                            { "OddsWakurenInfo30Kumi", "66" },
                            { "OddsWakurenInfo30Odds", "00135" },
                            { "OddsWakurenInfo30Ninki", "04" },
                            { "OddsWakurenInfo31Kumi", "67" },
                            { "OddsWakurenInfo31Odds", "00244" },
                            { "OddsWakurenInfo31Ninki", "06" },
                            { "OddsWakurenInfo32Kumi", "68" },
                            { "OddsWakurenInfo32Odds", "00472" },
                            { "OddsWakurenInfo32Ninki", "11" },
                            { "OddsWakurenInfo33Kumi", "77" },
                            { "OddsWakurenInfo33Odds", "08684" },
                            { "OddsWakurenInfo33Ninki", "31" },
                            { "OddsWakurenInfo34Kumi", "78" },
                            { "OddsWakurenInfo34Odds", "00906" },
                            { "OddsWakurenInfo34Ninki", "16" },
                            { "OddsWakurenInfo35Kumi", "88" },
                            { "OddsWakurenInfo35Odds", "07330" },
                            { "OddsWakurenInfo35Ninki", "30" },
                            { "TotalHyosuTansyo", "00000394142" },
                            { "TotalHyosuFukusyo", "00001587058" },
                            { "TotalHyosuWakuren", "00000072836" }
                        },
                    });
                }
            }
        }

        private static JV_O1_ODDS_TANFUKUWAKUDataBridge NewJV_O1_ODDS_TANFUKUWAKUDataBridge(string buf, JVOpenOptions options)
        {
            var dataStruct = new JV_O1_ODDS_TANFUKUWAKU();
            dataStruct.SetDataB(ref buf);
            var dataBridge = new JV_O1_ODDS_TANFUKUWAKUDataBridge();
            dataBridge.SetProperties(dataStruct, options);
            return dataBridge;
        }
    }
}