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

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;

namespace Urasandesu.JVLinkToSQLite.Settings
{
    /// <summary>
    /// 動作設定を表します。
    /// </summary>
    public class JVLinkToSQLiteSetting
    {
        /// <summary>
        /// デフォルトの動作設定を取得します。
        /// </summary>
        public static JVLinkToSQLiteSetting Default
        {
            get
            {
                var setting = new JVLinkToSQLiteSetting();
                var details = new List<JVLinkToSQLiteDetailSetting>();
                {
                    var detail = new JVNormalUpdateSetting();
                    detail.IsEnabled = true;
                    var dataSpecSettings = new[]
                    {
                        new JVDataSpecSetting("TOKU", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365)), TimeSpan.Zero),
                        new JVDataSpecSetting("RACE", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365))) { ExcludedRecordSpecs = new[]{ "H1", "H6", "O1", "O2", "O3", "O4", "O5", "O6", "WF" } },
                        new JVDataSpecSetting("DIFF", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365)), TimeSpan.Zero),
                        new JVDataSpecSetting("BLOD", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365))),
                        new JVDataSpecSetting("SNAP", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365))) { IsEnabled = false },
                        new JVDataSpecSetting("SLOP", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365))),
                        new JVDataSpecSetting("WOOD", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365))),
                        new JVDataSpecSetting("YSCH", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365))),
                        new JVDataSpecSetting("HOSE", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365)), TimeSpan.Zero),
                        new JVDataSpecSetting("HOYU", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365)), TimeSpan.Zero),
                        new JVDataSpecSetting("COMM", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365)), TimeSpan.Zero),
                        new JVDataSpecSetting("MING", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365))),
                        new JVDataSpecSetting("DIFN", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365)), TimeSpan.Zero),
                        new JVDataSpecSetting("BLDN", new JVKaisaiDateTimeRangeKey(DateTime.Today.AddDays(-365), DateTime.Today)),
                        new JVDataSpecSetting("SNPN", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365))) { IsEnabled = false },
                        new JVDataSpecSetting("HOSN", new JVKaisaiDateTimeKey(DateTime.Today.AddDays(-365)), TimeSpan.Zero),
                    };
                    detail.DataSpecSettings = dataSpecSettings;
                    details.Add(detail);
                }
                {
                    var detail = new JVSetupDataUpdateSetting();
                    detail.IsEnabled = false;
                    var dataSpecSettings = new[]
                    {
                        new JVDataSpecSetting("RACE") { ExcludedRecordSpecs = new[]{ "H1", "H6", "O1", "O2", "O3", "O4", "O5", "O6", "WF" } },
                        new JVDataSpecSetting("DIFF", TimeSpan.Zero),
                        new JVDataSpecSetting("BLOD"),
                        new JVDataSpecSetting("SNAP", new JVKaisaiDateTimeKey(new DateTime(2004, 01, 01))) { IsEnabled = false },
                        new JVDataSpecSetting("SLOP", new JVKaisaiDateTimeKey(new DateTime(2003, 01, 01))),
                        new JVDataSpecSetting("WOOD", new JVKaisaiDateTimeKey(new DateTime(2021, 07, 27))),
                        new JVDataSpecSetting("YSCH", new JVKaisaiDateTimeKey(new DateTime(2000, 01, 01))),
                        new JVDataSpecSetting("HOSE", new JVKaisaiDateTimeKey(new DateTime(1997, 01, 01)), TimeSpan.Zero),
                        new JVDataSpecSetting("HOYU", new JVKaisaiDateTimeKey(new DateTime(2000, 01, 01)), TimeSpan.Zero),
                        new JVDataSpecSetting("COMM", TimeSpan.Zero),
                        new JVDataSpecSetting("MING", new JVKaisaiDateTimeKey(new DateTime(2001, 09, 01))),
                        new JVDataSpecSetting("DIFN", new JVKaisaiDateTimeKey(new DateTime(2023, 08, 08)), TimeSpan.Zero),
                        new JVDataSpecSetting("BLDN", new JVKaisaiDateTimeKey(new DateTime(2023, 08, 08))),
                        new JVDataSpecSetting("SNPN", new JVKaisaiDateTimeKey(new DateTime(2023, 08, 08))) { IsEnabled = false },
                        new JVDataSpecSetting("HOSN", new JVKaisaiDateTimeKey(new DateTime(2023, 08, 08)), TimeSpan.Zero),
                    };
                    detail.DataSpecSettings = dataSpecSettings;
                    details.Add(detail);
                }
                {
                    var detail = new JVRealTimeDataUpdateSetting();
                    detail.IsEnabled = false;
                    var dataSpecSettings = new[]
                    {
                        new JVDataSpecSetting("0B12", new JVKaisaiDateKey(DateTime.Today)),
                        new JVDataSpecSetting("0B15", new JVKaisaiDateKey(DateTime.Today)),

                        new JVDataSpecSetting("0B30", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B31", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B32", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B33", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B34", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B35", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B36", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B20", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },

                        new JVDataSpecSetting("0B11", new JVKaisaiDateKey(DateTime.Today)),
                        new JVDataSpecSetting("0B14", new JVKaisaiDateKey(DateTime.Today)),

                        new JVDataSpecSetting("0B16", new JVRawKey("hoge")) { IsEnabled = false },

                        new JVDataSpecSetting("0B13", new JVKaisaiDateKey(DateTime.Today)),
                        new JVDataSpecSetting("0B17", new JVKaisaiDateKey(DateTime.Today)),

                        new JVDataSpecSetting("0B41", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B42", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                        new JVDataSpecSetting("0B51", new JVRaceKey(DateTime.Today, "01", "01", "01", "01")) { IsEnabled = false },
                    };
                    detail.DataSpecSettings = dataSpecSettings;
                    details.Add(detail);
                }
                setting.Details = details.ToArray();
                return setting;
            }
        }

        /// <summary>
        /// 動作設定詳細のリストを取得または設定します。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        [XmlArrayItem(typeof(JVNormalUpdateSetting))]
        [XmlArrayItem(typeof(JVSetupDataUpdateSetting))]
        [XmlArrayItem(typeof(JVRealTimeDataUpdateSetting))]
        public JVLinkToSQLiteDetailSetting[] Details { get; set; }

        /// <summary>
        /// SQLite データベースの接続情報を取得します。
        /// </summary>
        [XmlIgnore]
        public SQLiteConnectionInfo SQLiteConnectionInfo { get; private set; }

        internal void FillWithSQLiteConnectionInfo(SQLiteConnectionInfo connInfo)
        {
            SQLiteConnectionInfo = connInfo;

            if (Details == null)
            {
                return;
            }

            foreach (var detail in Details)
            {
                detail?.FillWithSQLiteConnectionInfo(connInfo);
            }
        }
    }
}
