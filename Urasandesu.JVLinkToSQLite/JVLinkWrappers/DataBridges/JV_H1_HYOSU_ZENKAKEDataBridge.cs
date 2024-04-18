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
using System.Linq;
using static JVData_Struct;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    public partial class JV_H1_HYOSU_ZENKAKEDataBridge : DataBridge<JV_H1_HYOSU_ZENKAKE>
    {
        partial void InitializeIfNecessary()
        {
            ChildTableNameList = new[]
            {
                Prefix + nameof(JVDataStructCreateTableSources.H1_HyoTansyo),
                Prefix + nameof(JVDataStructCreateTableSources.H1_HyoFukusyo),
                Prefix + nameof(JVDataStructCreateTableSources.H1_HyoWakuren),
                Prefix + nameof(JVDataStructCreateTableSources.H1_HyoUmaren),
                Prefix + nameof(JVDataStructCreateTableSources.H1_HyoWide),
                Prefix + nameof(JVDataStructCreateTableSources.H1_HyoUmatan),
                Prefix + nameof(JVDataStructCreateTableSources.H1_HyoSanrenpuku),
            };
            ChildRowCountList = new[]
            {
                _dataStruct.HyoTansyo.Length,
                _dataStruct.HyoFukusyo.Length,
                _dataStruct.HyoWakuren.Length,
                _dataStruct.HyoUmaren.Length,
                _dataStruct.HyoWide.Length,
                _dataStruct.HyoUmatan.Length,
                _dataStruct.HyoSanrenpuku.Length,
            };
            ChildRowMasksList = new[]
            {
                _dataStruct.HyoTansyo.Select(_ => !string.IsNullOrEmpty(_.Hyo.Trim())).ToArray(),
                _dataStruct.HyoFukusyo.Select(_ => !string.IsNullOrEmpty(_.Hyo.Trim())).ToArray(),
                _dataStruct.HyoWakuren.Select(_ => !string.IsNullOrEmpty(_.Hyo.Trim())).ToArray(),
                _dataStruct.HyoUmaren.Select(_ => !string.IsNullOrEmpty(_.Hyo.Trim())).ToArray(),
                _dataStruct.HyoWide.Select(_ => !string.IsNullOrEmpty(_.Hyo.Trim())).ToArray(),
                _dataStruct.HyoUmatan.Select(_ => !string.IsNullOrEmpty(_.Hyo.Trim())).ToArray(),
                _dataStruct.HyoSanrenpuku.Select(_ => !string.IsNullOrEmpty(_.Hyo.Trim())).ToArray(),
            };
            ChildPureColumnsList = new[]
            {
                JVDataStructColumns.H1_HyoTansyoWithoutParent.Value,
                JVDataStructColumns.H1_HyoFukusyoWithoutParent.Value,
                JVDataStructColumns.H1_HyoWakurenWithoutParent.Value,
                JVDataStructColumns.H1_HyoUmarenWithoutParent.Value,
                JVDataStructColumns.H1_HyoWideWithoutParent.Value,
                JVDataStructColumns.H1_HyoUmatanWithoutParent.Value,
                JVDataStructColumns.H1_HyoSanrenpukuWithoutParent.Value,
            };
            ChildGetterList = new Func<Array[]>(() => new Array[]
            {
                _dataStruct.HyoTansyo,
                _dataStruct.HyoFukusyo,
                _dataStruct.HyoWakuren,
                _dataStruct.HyoUmaren,
                _dataStruct.HyoWide,
                _dataStruct.HyoUmatan,
                _dataStruct.HyoSanrenpuku,
            });
            ChildCreateTableSourcesList = new[]
            {
                JVDataStructCreateTableSources.H1_HyoTansyo,
                JVDataStructCreateTableSources.H1_HyoFukusyo,
                JVDataStructCreateTableSources.H1_HyoWakuren,
                JVDataStructCreateTableSources.H1_HyoUmaren,
                JVDataStructCreateTableSources.H1_HyoWide,
                JVDataStructCreateTableSources.H1_HyoUmatan,
                JVDataStructCreateTableSources.H1_HyoSanrenpuku,
            };
            ChildInsertSourcesList = new[]
            {
                JVDataStructInsertSources.H1_HyoTansyo,
                JVDataStructInsertSources.H1_HyoFukusyo,
                JVDataStructInsertSources.H1_HyoWakuren,
                JVDataStructInsertSources.H1_HyoUmaren,
                JVDataStructInsertSources.H1_HyoWide,
                JVDataStructInsertSources.H1_HyoUmatan,
                JVDataStructInsertSources.H1_HyoSanrenpuku,
            };
        }
    }
}
