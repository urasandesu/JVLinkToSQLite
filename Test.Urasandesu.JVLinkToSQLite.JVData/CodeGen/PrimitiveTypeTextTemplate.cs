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
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;

internal static class PrimitiveTypeTextTemplate
{
    private static readonly AssemblyDefinition MSCorLibAsmDef = AssemblyDefinition.ReadAssembly(typeof(int).Assembly.Location);
    internal class TypeReferenceEqualityComparer : EqualityComparer<TypeReference>
    {
        public override bool Equals(TypeReference x, TypeReference y)
        {
            return x.FullName == y.FullName;
        }

        public override int GetHashCode(TypeReference obj)
        {
            int hashCode = 2118541809;
            hashCode = hashCode * -1521134295 + obj.FullName.GetHashCode();
            return hashCode;
        }
    }

    private static readonly IReadOnlyDictionary<TypeReference, string> Aliases = new Dictionary<TypeReference, string>(new TypeReferenceEqualityComparer())
    {
        { ToTypeReference(typeof(byte)), "byte" },
        { ToTypeReference(typeof(sbyte)), "sbyte" },
        { ToTypeReference(typeof(short)), "short" },
        { ToTypeReference(typeof(ushort)), "ushort" },
        { ToTypeReference(typeof(int)), "int" },
        { ToTypeReference(typeof(uint)), "uint" },
        { ToTypeReference(typeof(long)), "long" },
        { ToTypeReference(typeof(ulong)), "ulong" },
        { ToTypeReference(typeof(float)), "float" },
        { ToTypeReference(typeof(double)), "double" },
        { ToTypeReference(typeof(decimal)), "decimal" },
        { ToTypeReference(typeof(object)), "object" },
        { ToTypeReference(typeof(bool)), "bool" },
        { ToTypeReference(typeof(char)), "char" },
        { ToTypeReference(typeof(string)), "string" },
        { ToTypeReference(typeof(string[])), "IReadOnlyList<string>" },
        { ToTypeReference(typeof(void)), "void" }
    };

    private static TypeReference ToTypeReference(Type type)
    {
        if (type.IsArray)
        {
            return MSCorLibAsmDef.MainModule.GetType(type.GetElementType().FullName).MakeArrayType();
        }
        else
        {
            return MSCorLibAsmDef.MainModule.GetType(type.FullName);
        }
    }
    public static string GetAliasedTypeName(TypeReference typeRef)
    {
        if (!Aliases.TryGetValue(typeRef, out var typeName))
        {
            typeName = typeRef.Name;
        }
        if (typeRef.IsArray)
        {
            return $"IReadOnlyList<{typeRef.GetElementType().Name}>";
        }
        return typeName;
    }
}
