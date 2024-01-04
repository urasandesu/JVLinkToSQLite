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
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Xml;

namespace Urasandesu.JVLinkToSQLite.Settings
{
    /// <summary>
    /// データ種別に関する動作設定を表します。
    /// </summary>
    public class JVDataSpecSetting : ICloneable
    {
        /// <summary>
        /// 空のデータ種別に関する動作設定を初期化します。
        /// </summary>
        public JVDataSpecSetting()
        {
        }

        /// <summary>
        /// データ種別 ID を指定して、データ種別に関する動作設定を初期化します。
        /// </summary>
        /// <param name="dataSpec">データ種別 ID</param>
        public JVDataSpecSetting(string dataSpec) :
            this(dataSpec, new JVKaisaiDateTimeKey(new DateTime(1986, 01, 01)))
        { }

        /// <summary>
        /// データ種別 ID とデータ種別検索キーを指定して、データ種別に関する動作設定を初期化します。
        /// </summary>
        /// <param name="dataSpec">データ種別 ID</param>
        /// <param name="dataSpecKey">データ種別検索キー</param>
        public JVDataSpecSetting(string dataSpec, JVDataSpecKey dataSpecKey) :
            this(dataSpec, dataSpecKey, TimeSpan.FromDays(121.67))
        { }

        /// <summary>
        /// データ種別 ID と検索時間単位を指定して、データ種別に関する動作設定を初期化します。
        /// </summary>
        /// <param name="dataSpec">データ種別 ID</param>
        /// <param name="timeIntervalUnit">検索時間単位</param>
        public JVDataSpecSetting(string dataSpec, TimeSpan timeIntervalUnit) :
            this(true, dataSpec, new JVKaisaiDateTimeKey(new DateTime(1986, 01, 01)), timeIntervalUnit)
        { }

        /// <summary>
        /// データ種別 ID、データ種別検索キー、検索時間単位を指定して、データ種別に関する動作設定を初期化します。
        /// </summary>
        /// <param name="dataSpec">データ種別 ID</param>
        /// <param name="dataSpecKey">データ種別検索キー</param>
        /// <param name="timeIntervalUnit">検索時間単位</param>
        public JVDataSpecSetting(string dataSpec, JVDataSpecKey dataSpecKey, TimeSpan timeIntervalUnit) :
            this(true, dataSpec, dataSpecKey, timeIntervalUnit)
        { }

        /// <summary>
        /// 有効かどうか、データ種別 ID、データ種別検索キー、検索時間単位を指定して、データ種別に関する動作設定を初期化します。
        /// </summary>
        /// <param name="isEnabled">有効かどうか</param>
        /// <param name="dataSpec">データ種別 ID</param>
        /// <param name="dataSpecKey">データ種別検索キー</param>
        /// <param name="timeIntervalUnit">検索時間単位</param>
        public JVDataSpecSetting(bool isEnabled, string dataSpec, JVDataSpecKey dataSpecKey, TimeSpan timeIntervalUnit)
        {
            IsEnabled = isEnabled;
            DataSpec = dataSpec;
            DataSpecKey = dataSpecKey;
            TimeIntervalUnit = timeIntervalUnit;
        }

        /// <summary>
        /// 有効かどうかを取得または設定します。
        /// </summary>
        public bool IsEnabled { get; set; }
        
        private string _dataSpec;
        /// <summary>
        /// データ種別 ID を取得または設定します。
        /// </summary>
        public string DataSpec
        {
            get { return _dataSpec; }
            set
            {
                _dataSpec = value;
                if (!string.IsNullOrEmpty(_dataSpec))
                {
                    _jvDataSpec = JVDataSpec.Parse(_dataSpec);
                }
            }
        }

        private JVDataSpec _jvDataSpec;
        /// <summary>
        /// データ種別を取得します。
        /// </summary>
        [XmlIgnore]
        public JVDataSpec JVDataSpec { get => _jvDataSpec; }

        private string[] _excludedRecordSpecs;
        /// <summary>
        /// 除外するレコード種別 ID の配列を取得または設定します。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] ExcludedRecordSpecs
        {
            get { return _excludedRecordSpecs; }
            set
            {
                _excludedRecordSpecs = value;
                if (_excludedRecordSpecs != null)
                {
                    _excludedJVRecordSpecs = new JVRecordSpec[_excludedRecordSpecs.Length];
                    for (var i = 0; i < _excludedRecordSpecs.Length; i++)
                    {
                        _excludedJVRecordSpecs[i] = JVRecordSpec.Parse(_excludedRecordSpecs[i]);
                    }
                }
            }
        }

        private JVRecordSpec[] _excludedJVRecordSpecs;
        /// <summary>
        /// 除外するレコード種別の配列を取得します。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public JVRecordSpec[] ExcludedJVRecordSpecs { get => _excludedJVRecordSpecs; }

        /// <summary>
        /// データ種別検索キーを取得または設定します。
        /// </summary>
        [XmlElement(typeof(JVRaceKey))]
        [XmlElement(typeof(JVKaisaiDateKey))]
        [XmlElement(typeof(JVKaisaiDateTimeKey))]
        [XmlElement(typeof(JVKaisaiDateTimeRangeKey))]
        [XmlElement(typeof(JVRawKey))]
        public JVDataSpecKey DataSpecKey { get; set; }

        /// <summary>
        /// 検索時間単位を取得または設定します。
        /// </summary>
        [XmlIgnore]
        public TimeSpan TimeIntervalUnit { get; set; }

        [Browsable(false)]
        [XmlElement(DataType = "duration", ElementName = nameof(TimeIntervalUnit))]
        public string TimeSinceLastEventString
        {
            get
            {
                return XmlConvert.ToString(TimeIntervalUnit);
            }
            set
            {
                TimeIntervalUnit = string.IsNullOrEmpty(value) ? TimeSpan.Zero : XmlConvert.ToTimeSpan(value);
            }
        }
        public JVDataSpecSetting Clone()
        {
            return new JVDataSpecSetting()
            {
                IsEnabled = IsEnabled,
                DataSpec = DataSpec,
                DataSpecKey = DataSpecKey.Clone(),
                ExcludedRecordSpecs = ExcludedRecordSpecs.ToArray(),
                TimeIntervalUnit = TimeIntervalUnit
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public static string GetXPath(XmlNode node)
        {
            var predicate = new Func<XmlNode, bool>(n => n.Name == nameof(JVDataSpecSetting));
            var trueFunc = new Func<XmlNode, Func<string>, string>((n, f) =>
            {
                var dsn = n[nameof(DataSpec)];
                return f() + $"[{dsn.Name}='{dsn.InnerText}']";
            });

            return node.GetXPath(predicate, trueFunc);
        }
    }
}
