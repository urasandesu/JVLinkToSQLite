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
using System.Linq;
using Urasandesu.JVLinkToSQLite.Basis.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil;

internal class JVDataStructColumnTextTemplate
{
    public string Id { get; private set; }
    private TypeReference _jvDataStructType;
    public TypeReference JVDataStructType
    {
        get { return _jvDataStructType; }
        internal set
        {
            _jvDataStructType = value;
            Id = JVDataStructTypes.ExtractJVDataStructTypeId(_jvDataStructType);
        }
    }

    private FieldDefinition _childInfo;
    public FieldDefinition ChildInfo
    {
        get { return _childInfo; }
        internal set
        {
            _childInfo = value;
            Id += $"_{_childInfo.Name}";
        }
    }

    private bool? _isHead;
    public bool IsHead
    {
        get
        {
            if (_isHead == null)
            {
                _isHead = Path.Any(_ => _.ExistsHeaderAttribute());
            }
            return _isHead.Value;
        }
    }

    public bool IsFixedId { get => PrimaryKey == JVDataPrimaryKeyTypes.Fixed; }
    public bool IsId { get => PrimaryKey == JVDataPrimaryKeyTypes.Fixed || PrimaryKey == JVDataPrimaryKeyTypes.Conditional; }

    private JVDataPrimaryKeyTypes? _primaryKey;
    public JVDataPrimaryKeyTypes PrimaryKey
    {
        get
        {
            if (_primaryKey == null)
            {
                _primaryKey = Path.Reverse().Select(_ => _.ToPrimaryKey()).Where(_ => _ != JVDataPrimaryKeyTypes.None).DefaultIfEmpty().First();
            }
            return _primaryKey.Value;
        }
    }

    private FieldIndex[] _path;
    public FieldIndex[] Path
    {
        get { return _path; }
        internal set
        {
            _path = value;
            _isHead = null;
            _primaryKey = null;
        }
    }
    public int? Index { get; internal set; }
    public int? RepeatCount { get; internal set; }
    public FieldDefinition LastField { get; internal set; }
    public FieldDefinition FirstField { get => Path[0].Field; }
    private string _compatibilityFixer;
    public string CompatibilityFixer
    {
        get
        {
            if (_compatibilityFixer == null)
            {
                var compatibilityFixer = Path.Reverse().Select(_ => _.ToCompatibilityFixer()).Where(_ => _ != null).DefaultIfEmpty().First();
                _compatibilityFixer = compatibilityFixer ?? "null";
            }
            return _compatibilityFixer;
        }
    }

    private string _columnComment;
    public string ColumnComment
    {
        get
        {
            if (_columnComment == null)
            {
                var columnComment = Path.Select(_ => _.ToColumnComment()).Aggregate((acc, _) => acc + " " + _);
                _columnComment = columnComment ?? string.Empty;
            }
            return _columnComment;
        }
    }

    private bool? _isolatesAsChildAtFirst;
    public bool IsolatesAsChildAtFirst
    {
        get
        {
            if (_isolatesAsChildAtFirst == null)
            {
                var isolatesAsChildAndIndexList = Path.Select(_ => _.ToIsolatesAsChildAndIndex()).ToArray();
                if (isolatesAsChildAndIndexList.Length == 0)
                {
                    _isolatesAsChildAtFirst = false;
                }
                else
                {
                    var (isolatesAsChild, index) = isolatesAsChildAndIndexList[0];
                    _isolatesAsChildAtFirst = isolatesAsChild && index == 0;
                }
            }
            return _isolatesAsChildAtFirst.Value;
        }
    }

    public TypeReference ColumnType { get => LastField.FieldType; }
    public TypeDefinition ColumnSetType { get => LastField.DeclaringType; }

    public string GetFullName(bool usesIndex = true)
    {
        return string.Join(string.Empty, Path.Select((_, depth) => _.GetFullName(depth, usesIndex)));
    }

    public string GetFullPropertyPath(bool usesIndex = true)
    {
        return string.Join(".", Path.Select((_, depth) => _.GetFullPropertyPath(depth, usesIndex)));
    }
    public bool IsCrLf(bool usesIndex = true)
    {
        return GetFullName(usesIndex) == "crlf";
    }

    public string GetJVDataStructColumnsValueElem(bool usesIdAsFixed = true, bool usesIndex = true)
    {
        return $"new BridgeColumn(\"{GetFullName(usesIndex)}\", typeof({PrimitiveTypeTextTemplate.GetAliasedTypeName(ColumnType)}), {IsHead.ToString().ToLower()}, {(usesIdAsFixed ? IsFixedId.ToString().ToLower() : IsId.ToString().ToLower())}, {CompatibilityFixer}, \"{ColumnComment}\")";
    }

    public string GetDataBridgeGetterDef(bool usesIndex = true)
    {
        return $"public {PrimitiveTypeTextTemplate.GetAliasedTypeName(ColumnType)} {GetFullName(usesIndex)} {{ get => _dataStruct.{GetFullPropertyPath(usesIndex)}; }}";
    }

    public string GetDataBridgeGettersElem(bool usesIndex = true)
    {
        return $"new Func<object>(()=>{GetFullName(usesIndex)})";
    }

    public string GetIdTypeExplanation()
    {
        var attr = default(JVDataStructTypeAttribute);
        if (ChildInfo == null)
        {
            attr = JVDataStructType.Resolve().GetAttribute<JVDataStructTypeAttribute>();
        }
        else
        {
            attr = ChildInfo.FieldType.Resolve().GetAttribute<JVDataStructTypeAttribute>();
        }

        if (attr == null)
        {
            return "null";
        }

        return $"\"{attr.TypeExplanation.Replace("\r\n", "\\r\\n").Replace("\"", "\\\"")}\"";
    }

    public override string ToString()
    {
        return $"{Id}\t{JVDataStructType.Name}\t{Index}\t{RepeatCount}\t{LastField.Name}\t{ColumnType.Name}\t{IsHead}\t{PrimaryKey}\t{GetFullPropertyPath()}";
    }
}
