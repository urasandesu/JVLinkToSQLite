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
using System.Collections.Generic;
using System.Linq;
using Urasandesu.JVLinkToSQLite.Basis.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil;

internal class JVDataStructGetterTextTemplate
{
    private readonly TypeReference _type;
    private readonly FullNameEquatableTypeReference _eqType;

    public JVDataStructGetterTextTemplate(TypeReference type)
    {
        _type = type;
        _eqType = new FullNameEquatableTypeReference(_type);
    }

    public string Name { get => _type.Name; }

    private FieldDefinition[] _fields;
    public FieldDefinition[] Fields
    {
        get
        {
            if (_fields == null)
            {
                _fields = _type.Resolve().Fields.Where(_ => _.IsPublic && !_.IsStatic).Where(_ => !DoesIsolateAsChild(_)).ToArray();
            }
            return _fields;
        }
    }

    private bool DoesIsolateAsChild(FieldDefinition fld)
    {
        var attr = fld.GetAttribute<JVDataStructFieldAttribute>();
        if (attr == null)
        {
            return false;
        }

        return attr.IsolatesAsChild;
    }

    public IEnumerable<FieldDefinition> FieldsWithoutCrLf
    {
        get => Fields.Where(_ => _.Name != "crlf");
    }

    public IEnumerable<string> GetDataBridgeGetterDefs()
    {
        return Fields.Select(_ => $"public {PrimitiveTypeTextTemplate.GetAliasedTypeName(_.FieldType)} {_.Name} {{ get => _dataStruct.{_.Name}; }}");
    }
}
