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
using System.Diagnostics;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    [DebuggerDisplay("{" + nameof(DataFileSpec) + "." + nameof(JVDataFileSpec.Value) + "}, " + nameof(FileName) + "={" + nameof(FileName) + "}, " + nameof(DatetimeKey) + "={" + nameof(DatetimeKey) + "}, " + nameof(PublishedDateTime) + "={" + nameof(PublishedDateTime) + "}")]
    public class JVDataFile : IEquatable<JVDataFile>
    {
        public JVDataFile(JVDataFileSpec dataFileSpec, string fileName, string datetimeKey, DateTime publishedDateTime)
        {
            DataFileSpec = dataFileSpec;
            FileName = fileName;
            DatetimeKey = datetimeKey;
            PublishedDateTime = publishedDateTime;
        }

        public JVDataFileSpec DataFileSpec { get; private set; }
        public string FileName { get; private set; }
        public string DatetimeKey { get; private set; }
        public DateTime PublishedDateTime { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JVDataFile);
        }

        public bool Equals(JVDataFile other)
        {
            return !(other is null) &&
                   EqualityComparer<JVDataFileSpec>.Default.Equals(DataFileSpec, other.DataFileSpec) &&
                   FileName == other.FileName &&
                   DatetimeKey == other.DatetimeKey &&
                   PublishedDateTime == other.PublishedDateTime;
        }

        public override int GetHashCode()
        {
            int hashCode = 831134279;
            hashCode = hashCode * -1521134295 + EqualityComparer<JVDataFileSpec>.Default.GetHashCode(DataFileSpec);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DatetimeKey);
            hashCode = hashCode * -1521134295 + PublishedDateTime.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(JVDataFile left, JVDataFile right)
        {
            return EqualityComparer<JVDataFile>.Default.Equals(left, right);
        }

        public static bool operator !=(JVDataFile left, JVDataFile right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{DataFileSpec.Value}, {nameof(FileName)}={FileName}, {nameof(DatetimeKey)}={DatetimeKey}, {nameof(PublishedDateTime)}={PublishedDateTime}";
        }
    }
}
