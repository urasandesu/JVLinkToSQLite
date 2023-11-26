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
using System.Linq;
using Urasandesu.JVLinkToSQLite.Basis.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;

internal class JVDataStructColumnTextTemplateFactory
{
    public IReadOnlyList<JVDataStructColumnTextTemplate> NewAll(string jvdataAssemblyPath)
    {
        var columntts = new List<JVDataStructColumnTextTemplate>();
        foreach (var jvDataStructType in JVDataStructTypes.GetList(jvdataAssemblyPath))
        {
            var path = new Stack<FieldIndex>();
            AddJVDataStructColumnsInfo(columntts, jvDataStructType, jvDataStructType, path);
        }
        return columntts;
    }

    private static void AddJVDataStructColumnsInfo(List<JVDataStructColumnTextTemplate> textTemplates,
                                                   TypeReference jvDataStructType,
                                                   TypeReference declType,
                                                   Stack<FieldIndex> path,
                                                   int? index = null,
                                                   int? repeatCount = null,
                                                   FieldDefinition childInfo = null)
    {
        var flds = declType.Resolve().Fields.Where(_ => _.IsPublic && !_.IsStatic);
        foreach (var fld in flds)
        {
            path.Push(new FieldIndex(fld, null));
            if (fld.FieldType.IsPrimitiveType() || JVDataStructTypes.HasCombinableFields(fld.FieldType))
            {
                var textTemplate = new JVDataStructColumnTextTemplate()
                {
                    JVDataStructType = jvDataStructType,
                    LastField = fld,
                    Path = path.Reverse().ToArray(),
                    Index = index,
                    RepeatCount = repeatCount
                };
                if (childInfo != null)
                {
                    textTemplate.ChildInfo = childInfo;
                }
                textTemplates.Add(textTemplate);
            }
            else if (fld.FieldType.IsArray)
            {
                var attr = fld.GetAttribute<JVDataStructFieldAttribute>();
                if (attr == null)
                {
                    throw new InvalidOperationException($"'{fld.DeclaringType.FullName}.{fld.Name}' に {nameof(JVDataStructFieldAttribute)} が正しく付与されていない。");
                }

                if (attr.RepeatCount < 1 && !attr.IsolatesAsChild)
                {
                    throw new InvalidOperationException($"'{fld.DeclaringType.FullName}.{fld.Name}' に {nameof(JVDataStructFieldAttribute)} が正しく付与されていない。");
                }


                if (attr.IsolatesAsChild)
                {
                    if (attr.RepeatCount < 1)
                    {
                        throw new InvalidOperationException($"'{fld.DeclaringType.FullName}.{fld.Name}' に {nameof(JVDataStructFieldAttribute)} が正しく付与されていない。");
                    }

                    for (int i = 0; i < attr.RepeatCount; i++)
                    {
                        var childPath = new Stack<FieldIndex>();
                        childPath.Push(new FieldIndex(fld, i));
                        AddJVDataStructColumnsInfo(textTemplates,
                                                   jvDataStructType,
                                                   fld.FieldType.GetElementType(),
                                                   childPath,
                                                   index: i,
                                                   repeatCount: attr.RepeatCount,
                                                   childInfo: fld);
                    }
                }
                else if (1 <= attr.RepeatCount)
                {
                    if (fld.FieldType.GetElementType().IsPrimitiveType())
                    {
                        for (int i = 0; i < attr.RepeatCount; i++)
                        {
                            path.Pop();
                            path.Push(new FieldIndex(fld, i));
                            var textTemplate = new JVDataStructColumnTextTemplate()
                            {
                                JVDataStructType = jvDataStructType,
                                LastField = new FieldDefinition(fld.Name, fld.Attributes, fld.FieldType.GetElementType()) { DeclaringType = fld.DeclaringType },
                                Path = path.Reverse().ToArray(),
                                Index = i,
                                RepeatCount = repeatCount
                            };
                            if (childInfo != null)
                            {
                                textTemplate.ChildInfo = childInfo;
                            }
                            textTemplates.Add(textTemplate);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < attr.RepeatCount; i++)
                        {
                            path.Pop();
                            path.Push(new FieldIndex(fld, i));
                            AddJVDataStructColumnsInfo(textTemplates,
                                                       jvDataStructType,
                                                       fld.FieldType.GetElementType(),
                                                       path,
                                                       index: i,
                                                       repeatCount: attr.RepeatCount,
                                                       childInfo: childInfo);
                        }
                    }
                }
            }
            else
            {
                AddJVDataStructColumnsInfo(textTemplates,
                                           jvDataStructType,
                                           fld.FieldType,
                                           path,
                                           index: index,
                                           repeatCount: repeatCount,
                                           childInfo: childInfo);
            }
            path.Pop();
        }
    }
}
