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
using Urasandesu.JVLinkToSQLite.Basis.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil;

internal class FieldIndex
{
    public FieldIndex(FieldDefinition field, int? index)
    {
        Field = field;
        Index = index;
    }

    public FieldDefinition Field { get; private set; }
    public int? Index { get; private set; }

    public bool UnusesIndex(int depth, bool usesIndex = true)
    {
        var (isolatesAsChild, _) = ToIsolatesAsChildAndIndex() ?? Tuple.Create(false, -1);
        return Index == null || !usesIndex || isolatesAsChild && depth == 0 && Index == 0;
    }

    public string GetFullName(int depth, bool usesIndex = true)
    {
        return UnusesIndex(depth, usesIndex) ? Field.Name : $"{Field.Name}{Index}";
    }

    public string GetFullPropertyPath(int depth, bool usesIndex = true)
    {
        return UnusesIndex(depth, usesIndex) ? Field.Name : $"{Field.Name}[{Index}]";
    }

    public bool ExistsHeaderAttribute()
    {
        var attr = Field.GetAttribute<JVDataStructFieldAttribute>();
        if (attr == null)
        {
            return false;
        }

        return attr.IsHeader;
    }

    public JVDataPrimaryKeyTypes ToPrimaryKey()
    {
        var attr = Field.GetAttribute<JVDataStructFieldAttribute>();
        if (attr == null)
        {
            return JVDataPrimaryKeyTypes.None;
        }

        return attr.PrimaryKey;
    }

    public string ToCompatibilityFixer()
    {
        var attr = Field.GetAttribute<JVDataStructFieldAttribute>();
        if (attr == null)
        {
            return null;
        }

        return attr.CompatibilityFixer;
    }

    public string ToColumnComment()
    {
        var attr = Field.GetAttribute<JVDataStructFieldAttribute>();
        if (attr == null)
        {
            return null;
        }

        return attr.FieldExplanation;
    }

    public Tuple<bool, int> ToIsolatesAsChildAndIndex()
    {
        var attr = Field.GetAttribute<JVDataStructFieldAttribute>();
        if (attr == null)
        {
            return null;
        }

        return Tuple.Create(attr.IsolatesAsChild, Index ?? -1);
    }
}