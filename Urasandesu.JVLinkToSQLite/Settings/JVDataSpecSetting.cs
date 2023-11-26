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

namespace Urasandesu.JVLinkToSQLite.Settings
{
    public class JVDataSpecSetting : ICloneable
    {
        public JVDataSpecSetting()
        {
        }

        public JVDataSpecSetting(string dataSpec) :
            this(dataSpec, new JVKaisaiDateTimeKey(new DateTime(1986, 01, 01)))
        { }

        public JVDataSpecSetting(string dataSpec, JVDataSpecKey dataSpecKey) :
            this(dataSpec, dataSpecKey, TimeSpan.FromDays(121.67))
        { }

        public JVDataSpecSetting(string dataSpec, TimeSpan timeIntervalUnit) :
            this(true, dataSpec, new JVKaisaiDateTimeKey(new DateTime(1986, 01, 01)), timeIntervalUnit)
        { }

        public JVDataSpecSetting(string dataSpec, JVDataSpecKey dataSpecKey, TimeSpan timeIntervalUnit) :
            this(true, dataSpec, dataSpecKey, timeIntervalUnit)
        { }

        public JVDataSpecSetting(bool isEnabled, string dataSpec, JVDataSpecKey dataSpecKey, TimeSpan timeIntervalUnit)
        {
            IsEnabled = isEnabled;
            DataSpec = dataSpec;
            DataSpecKey = dataSpecKey;
            TimeIntervalUnit = timeIntervalUnit;
        }

        public bool IsEnabled { get; set; }
        private string _dataSpec;
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
        [XmlIgnore]
        public JVDataSpec JVDataSpec { get => _jvDataSpec; }

        private string[] _excludedRecordSpecs;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public JVRecordSpec[] ExcludedJVRecordSpecs { get => _excludedJVRecordSpecs; }

        [XmlElement(typeof(JVRaceKey))]
        [XmlElement(typeof(JVKaisaiDateKey))]
        [XmlElement(typeof(JVKaisaiDateTimeKey))]
        [XmlElement(typeof(JVKaisaiDateTimeRangeKey))]
        [XmlElement(typeof(JVRawKey))]
        public JVDataSpecKey DataSpecKey { get; set; }

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
    }
}
