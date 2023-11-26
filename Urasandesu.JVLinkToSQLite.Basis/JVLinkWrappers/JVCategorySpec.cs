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
    public class JVCategorySpec : IEquatable<JVCategorySpec>
    {
        public static readonly JVCategorySpec T = new JVCategorySpec() { Value = nameof(T) };
        public static readonly JVCategorySpec H = new JVCategorySpec() { Value = nameof(H) };
        public static readonly JVCategorySpec M = new JVCategorySpec() { Value = nameof(M) };
        public static readonly JVCategorySpec U = new JVCategorySpec() { Value = nameof(U) };
        public static readonly JVCategorySpec G = new JVCategorySpec() { Value = nameof(G) };
        public static readonly JVCategorySpec P = new JVCategorySpec() { Value = nameof(P) };
        public static readonly JVCategorySpec D = new JVCategorySpec() { Value = nameof(D) };
        public static readonly JVCategorySpec B = new JVCategorySpec() { Value = nameof(B) };
        public static readonly JVCategorySpec S = new JVCategorySpec() { Value = nameof(S) };
        public static readonly JVCategorySpec Q = new JVCategorySpec() { Value = nameof(Q) };
        public static readonly JVCategorySpec F = new JVCategorySpec() { Value = nameof(F) };
        public static readonly JVCategorySpec O = new JVCategorySpec() { Value = nameof(O) };
        public static readonly JVCategorySpec L = new JVCategorySpec() { Value = nameof(L) };
        public static readonly JVCategorySpec K = new JVCategorySpec() { Value = nameof(K) };
        public static readonly JVCategorySpec E = new JVCategorySpec() { Value = nameof(E) };
        public static readonly JVCategorySpec W = new JVCategorySpec() { Value = nameof(W) };
        public static readonly JVCategorySpec N = new JVCategorySpec() { Value = nameof(N) };
        public static readonly JVCategorySpec C = new JVCategorySpec() { Value = nameof(C) };
        public static readonly JVCategorySpec V = new JVCategorySpec() { Value = nameof(V) };
        public static readonly JVCategorySpec X = new JVCategorySpec() { Value = nameof(X) };
        public static readonly JVCategorySpec Y = new JVCategorySpec() { Value = nameof(Y) };

        private JVCategorySpec()
        {
        }

        public string Value { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JVCategorySpec);
        }

        public bool Equals(JVCategorySpec other)
        {
            return !(other is null) &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
        }

        public static bool operator ==(JVCategorySpec left, JVCategorySpec right)
        {
            return EqualityComparer<JVCategorySpec>.Default.Equals(left, right);
        }

        public static bool operator !=(JVCategorySpec left, JVCategorySpec right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
