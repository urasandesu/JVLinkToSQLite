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

using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil
{
    public class FullNameEquatableTypeReference : IEquatable<FullNameEquatableTypeReference>
    {
        public FullNameEquatableTypeReference(TypeReference source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Source = source;
        }

        public TypeReference Source { get; }
        public string FullName { get => Source.FullName; }

        public override bool Equals(object obj)
        {
            return Equals(obj as FullNameEquatableTypeReference);
        }

        public bool Equals(FullNameEquatableTypeReference other)
        {
            return !(other is null) &&
                   FullName == other.FullName;
        }

        public override int GetHashCode()
        {
            return 733961487 + EqualityComparer<string>.Default.GetHashCode(FullName);
        }

        public static bool operator ==(FullNameEquatableTypeReference left, FullNameEquatableTypeReference right)
        {
            return EqualityComparer<FullNameEquatableTypeReference>.Default.Equals(left, right);
        }

        public static bool operator !=(FullNameEquatableTypeReference left, FullNameEquatableTypeReference right)
        {
            return !(left == right);
        }
    }
}
