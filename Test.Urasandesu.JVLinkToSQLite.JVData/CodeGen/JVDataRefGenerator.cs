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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil;

namespace Test.Urasandesu.JVLinkToSQLite.CodeGen
{
    [TestFixture]
    public class JVDataRefGenerator
    {
        [Test]
        public void GenerateAll()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine(Directory.GetParent(Environment.CurrentDirectory).Name);
            var columntts = new JVDataStructColumnTextTemplateFactory().NewAll("Urasandesu.JVLinkToSQLite.JVData.dll");

            Console.WriteLine("★★JVDataStructColumns の定義生成");
            {
                var columnttDict = new Dictionary<string, IEnumerable<JVDataStructColumnTextTemplate>>();
                foreach (var gj in columntts.GroupBy(_ => _.Id))
                {
                    Console.WriteLine($"★★★{gj.Key}");
                    foreach (var columntt in gj)
                    {
                        Console.WriteLine(columntt);
                    }
                    if (JVDataStructTypes.IsJVDataStructTypeId(gj.Key))
                    {
                        Console.WriteLine($"public static readonly JVDataStructColumns {gj.Key} = new JVDataStructColumns() {{ Value = new[] {{ {string.Join(",", gj.Where(_ => !_.IsCrLf()).Select(_ => _.GetJVDataStructColumnsValueElem()))} }} }};");
                        columnttDict[gj.Key] = gj;
                        if (JVDataStructTypes.HasConditionalColumnId(gj.Key))
                        {
                            Console.WriteLine($"public static readonly JVDataStructColumns {gj.Key}Conditional = new JVDataStructColumns() {{ Value = new[] {{ {string.Join(",", gj.Where(_ => !_.IsCrLf()).Select(_ => _.GetJVDataStructColumnsValueElem(usesIdAsFixed: false)))} }} }};");
                        }
                    }
                    else
                    {
                        var pgj = columnttDict[JVDataStructTypes.ExtractJVDataStructTypeId(gj.Key)];
                        Console.WriteLine($"public static readonly JVDataStructColumns {gj.Key} = new JVDataStructColumns() {{ Value = new[] {{ {string.Join(",", pgj.Where(_ => _.IsFixedId).Select(_ => _.GetJVDataStructColumnsValueElem()))}, new BridgeColumn(\"{JVDataStructTypes.ExtractJVDataStructTypeChildId(gj.Key)}Idx\", typeof(string), false, true, null, \"インデックス\"), {string.Join(",", gj.TakeWhile(_ => _.IsolatesAsChildAtFirst).Select(_ => _.GetJVDataStructColumnsValueElem()))} }} }};");
                        Console.WriteLine($"public static readonly JVDataStructColumns {gj.Key}WithoutParent = new JVDataStructColumns() {{ Value = new[] {{ new BridgeColumn(\"{JVDataStructTypes.ExtractJVDataStructTypeChildId(gj.Key)}Idx\", typeof(string), false, true, null, \"インデックス\"), {string.Join(",", gj.TakeWhile(_ => _.IsolatesAsChildAtFirst).Select(_ => _.GetJVDataStructColumnsValueElem()))} }} }};");
                    }
                }
            }

            Console.WriteLine("★★JVDataStructCreateTableSources の定義生成");
            {
                var columnttDict = new Dictionary<string, IEnumerable<JVDataStructColumnTextTemplate>>();
                foreach (var gj in columntts.GroupBy(_ => _.Id))
                {
                    if (JVDataStructTypes.IsJVDataStructTypeId(gj.Key))
                    {
                        Console.WriteLine($"public static readonly JVDataStructCreateTableSources {gj.Key} = new JVDataStructCreateTableSources() {{ Name = nameof({gj.Key}), Value = ToTextTemplate(JVDataStructColumns.{gj.Key}, {gj.First().GetIdTypeExplanation()}) }};");
                        columnttDict[gj.Key] = gj;
                        if (JVDataStructTypes.HasConditionalColumnId(gj.Key))
                        {
                            Console.WriteLine($"public static readonly JVDataStructCreateTableSources {gj.Key}Conditional = new JVDataStructCreateTableSources() {{ Name = nameof({gj.Key}), Value = ToTextTemplate(JVDataStructColumns.{gj.Key}Conditional, {gj.First().GetIdTypeExplanation()}) }};");
                        }
                    }
                    else
                    {
                        var pgj = columnttDict[JVDataStructTypes.ExtractJVDataStructTypeId(gj.Key)];
                        Console.WriteLine($"public static readonly JVDataStructCreateTableSources {gj.Key} = new JVDataStructCreateTableSources() {{ Name = nameof({gj.Key}), Value = ToTextTemplate(JVDataStructColumns.{gj.Key}, {gj.First().GetIdTypeExplanation()}) }};");
                    }
                }
            }

            Console.WriteLine("★★JVDataStructInsertSources の定義生成");
            {
                var columnttDict = new Dictionary<string, IEnumerable<JVDataStructColumnTextTemplate>>();
                foreach (var gj in columntts.GroupBy(_ => _.Id))
                {
                    if (JVDataStructTypes.IsJVDataStructTypeId(gj.Key))
                    {
                        Console.WriteLine($"public static readonly JVDataStructInsertSources {gj.Key} = new JVDataStructInsertSources() {{ Name = nameof({gj.Key}), Value = ToTextTemplate(JVDataStructColumns.{gj.Key}) }};");
                        columnttDict[gj.Key] = gj;
                        if (JVDataStructTypes.HasConditionalColumnId(gj.Key))
                        {
                            Console.WriteLine($"public static readonly JVDataStructInsertSources {gj.Key}Conditional = new JVDataStructInsertSources() {{ Name = nameof({gj.Key}), Value = ToTextTemplate(JVDataStructColumns.{gj.Key}Conditional) }};");
                        }
                    }
                    else
                    {
                        var pgj = columnttDict[JVDataStructTypes.ExtractJVDataStructTypeId(gj.Key)];
                        Console.WriteLine($"public static readonly JVDataStructInsertSources {gj.Key} = new JVDataStructInsertSources() {{ Name = nameof({gj.Key}), Values = ToRepeatedTextTemplate(JVDataStructColumns.{gj.Key}, {gj.First().RepeatCount}) }};");
                    }
                }
            }

            Console.WriteLine("★★JVDataStructGetters の定義生成");
            {
                foreach (var declType in columntts.Select(_ => _.ColumnSetType).Concat(columntts.Select(_ => _.ColumnType).Where(_ => !_.IsPrimitiveType())).Distinct())
                {
                    var gettertt = new JVDataStructGetterTextTemplate(declType);
                    Console.WriteLine($"public static readonly JVDataStructGetters {gettertt.Name} = new JVDataStructGetters() {{ Value = new Func<object, object[]>(obj => {{ var @this = ({gettertt.Name})obj; return new object[] {{ {string.Join(", ", gettertt.FieldsWithoutCrLf.Select(_ => $"@this.{_.Name}"))} }}; }}) }};");
                }
                Console.WriteLine("★★★DictionaryHolder 構成");
                foreach (var declType in columntts.Select(_ => _.ColumnSetType).Distinct())
                {
                    var gettertt = new JVDataStructGetterTextTemplate(declType);
                    Console.WriteLine($"{{ typeof({gettertt.Name}), {gettertt.Name} }},");
                }
                Console.WriteLine("★★★CombinableFieldsTypeDictionaryHolder 構成");
                foreach (var declType in columntts.Select(_ => _.ColumnType).Where(_ => !_.IsPrimitiveType()).Distinct())
                {
                    var gettertt = new JVDataStructGetterTextTemplate(declType);
                    Console.WriteLine($"{{ typeof({gettertt.Name}), {gettertt.Name} }},");
                }
            }

            Console.WriteLine("★★JVDataStructBridges お試し");
            {
                var columnttDict = new Dictionary<string, IEnumerable<JVDataStructColumnTextTemplate>>();
                foreach (var gj in columntts.GroupBy(_ => _.Id))
                {
                    if (JVDataStructTypes.IsJVDataStructTypeId(gj.Key))
                    {
                        Console.WriteLine($"★★★{gj.Key}");
                        Console.WriteLine($"public class {gj.First().JVDataStructType.Name}DataBridge : DataBridge<{gj.First().JVDataStructType.Name}>");
                        Console.WriteLine($"    Columns = JVDataStructColumns.{gj.Key}.Value;");
                        var gettertt = new JVDataStructGetterTextTemplate(gj.First().JVDataStructType);
                        Console.WriteLine($"    BaseGetter = new Func<object[]>(() => new object[] {{ {string.Join(", ", gettertt.FieldsWithoutCrLf.Select(_ => _.Name))} }});");
                        var dataBridgeGetterDefs = gettertt.GetDataBridgeGetterDefs();
                        foreach (var dataBridgeGetterDef in dataBridgeGetterDefs)
                        {
                            Console.WriteLine($"    {dataBridgeGetterDef}");
                        }
                        foreach (var columntt in gj)
                        {
                            Console.WriteLine(columntt);
                        }
                    }
                }
            }

            Console.WriteLine("★★DataBridgeFactory お試し");
            {
                var columnttDict = new Dictionary<string, IEnumerable<JVDataStructColumnTextTemplate>>();
                foreach (var gj in columntts.GroupBy(_ => _.Id))
                {
                    if (JVDataStructTypes.IsJVDataStructTypeId(gj.Key))
                    {
                        Console.WriteLine($"★★★{gj.Key}");
                        columnttDict[gj.Key] = gj;
                    }
                }

                var dataBridgeFactoryttList = new List<DataBridgeFactoryTextTemplate>();
                foreach (var tuple in JVDataSpecFields.GetAll())
                {
                    var (fld, dataSpec) = tuple;
                    var value = dataSpec.Value;
                    var candidateRecordSpecs = dataSpec.CandidateRecordSpecs;
                    foreach (var candidateRecordSpec in candidateRecordSpecs)
                    {
                        var dataBridgeFactorytt = new DataBridgeFactoryTextTemplate(candidateRecordSpec, fld, dataSpec);
                        dataBridgeFactoryttList.Add(dataBridgeFactorytt);
                    }
                }

                foreach (var gj in dataBridgeFactoryttList.GroupBy(_ => _.Id))
                {
                    Console.WriteLine($"★★★★{gj.Key}");
                    if (!columnttDict.TryGetValue(gj.Key, out var pgj)
                        && !columnttDict.TryGetValue(JVDataStructTypes.ExtractJVDataStructTypeId(gj.Key, withoutVersion: true), out pgj))
                    {
                        throw new InvalidOperationException($"Urasandesu.JVLinkToSQLite.Basis.dll と Urasandesu.JVLinkToSQLite.JVData.dll で不一致: {gj.Key}");
                    }
                    Console.WriteLine($"else if (({string.Join(" || ", gj.Select(_ => _.GetDataSpecConditionElem()))}) && {gj.First().GetRecordSpecConditionElem()})");
                    Console.WriteLine($"    return NewDataBridge((ref {pgj.First().JVDataStructType.Name} dataStruct, ref string buf) => dataStruct.SetDataB(ref buf), new {pgj.First().JVDataStructType.Name}DataBridge());");
                }
            }
        }
    }
}
