﻿// JVLinkToSQLite は、JRA-VAN データラボが提供する競馬データを SQLite データベースに変換するツールです。
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
    public partial class JV_H6_HYOSU_SANRENTANDataBridge : DataBridge<JV_H6_HYOSU_SANRENTAN>
    {
        partial void InitializeIfNecessary()
        {
            ChildTableNameList = new[]
            {
                Prefix + nameof(JVDataStructCreateTableSources.H6_HyoSanrentan),
            };
            ChildRowCountList = new[]
            {
                _dataStruct.HyoSanrentan.Length,
            };
            ChildRowMasksList = new[]
            {
                _dataStruct.HyoSanrentan.Select(_ => !string.IsNullOrEmpty(_.Hyo.Trim())).ToArray(),
            };
            ChildPureColumnsList = new[]
            {
                JVDataStructColumns.H6_HyoSanrentanWithoutParent.Value,
            };
            ChildGetterList = new Func<Array[]>(() => new Array[]
            {
                _dataStruct.HyoSanrentan,
            });
            ChildCreateTableSourcesList = new[]
            {
                JVDataStructCreateTableSources.H6_HyoSanrentan,
            };
            ChildInsertSourcesList = new[]
            {
                JVDataStructInsertSources.H6_HyoSanrentan,
            };
        }
    }
}
