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
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class JVSaveSpec : IEquatable<JVSaveSpec>
    {
        public static readonly JVSaveSpec W = new JVSaveSpec() { Value = nameof(W) };
        public static readonly JVSaveSpec M = new JVSaveSpec() { Value = nameof(M) };

        private JVSaveSpec()
        {
        }
        public string Value { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JVSaveSpec);
        }

        public bool Equals(JVSaveSpec other)
        {
            return !(other is null) &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
        }

        public static bool operator ==(JVSaveSpec left, JVSaveSpec right)
        {
            return EqualityComparer<JVSaveSpec>.Default.Equals(left, right);
        }

        public static bool operator !=(JVSaveSpec left, JVSaveSpec right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
