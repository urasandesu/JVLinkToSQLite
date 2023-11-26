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
using System.Diagnostics;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    [DebuggerDisplay("{" + nameof(Value) + "}, " + nameof(Succeeded) + "={" + nameof(Succeeded) + "}, " + nameof(Failed) + "={" + nameof(Failed) + "}")]
    public struct JVResultInterpretation : IEquatable<JVResultInterpretation>
    {
        public static readonly JVResultInterpretation None = new JVResultInterpretation(Interpretations.None);
        public static readonly JVResultInterpretation SuccessTrue = new JVResultInterpretation(Interpretations.SuccessTrue);
        public static readonly JVResultInterpretation SuccessFalse = new JVResultInterpretation(Interpretations.SuccessFalse);
        public static readonly JVResultInterpretation Error = new JVResultInterpretation(Interpretations.Error);

        public JVResultInterpretation(Interpretations value)
        {
            Value = value;
        }

        public Interpretations Value { get; private set; }

        public bool Succeeded { get => Value == Interpretations.SuccessTrue || Value == Interpretations.SuccessFalse; }
        public bool Failed { get => Value == Interpretations.Error; }
        public override bool Equals(object obj)
        {
            return obj is JVResultInterpretation interpretation && Equals(interpretation);
        }

        public bool Equals(JVResultInterpretation other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }

        public static bool operator ==(JVResultInterpretation left, JVResultInterpretation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(JVResultInterpretation left, JVResultInterpretation right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{nameof(Value)}={Value}, {nameof(Succeeded)}={Succeeded}, {nameof(Failed)}={Failed}";
        }
    }
}
