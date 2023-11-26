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
using static JVData_Struct;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges
{
    public partial class JV_CK_CHAKUDataBridge : DataBridge<JV_CK_CHAKU>
    {
        partial void InitializeIfNecessary()
        {
            ChildTableNameList = new[]
            {
                Prefix + nameof(JVDataStructCreateTableSources.CK_KisyuChaku),
                Prefix + nameof(JVDataStructCreateTableSources.CK_ChokyoChaku),
                Prefix + nameof(JVDataStructCreateTableSources.CK_BanusiChaku),
                Prefix + nameof(JVDataStructCreateTableSources.CK_BreederChaku),
            };
            ChildRowCountList = new[]
            {
                _dataStruct.KisyuChaku.Length,
                _dataStruct.ChokyoChaku.Length,
                _dataStruct.BanusiChaku.Length,
                _dataStruct.BreederChaku.Length,
            };
            ChildPureColumnsList = new[]
            {
                JVDataStructColumns.CK_KisyuChakuWithoutParent.Value,
                JVDataStructColumns.CK_ChokyoChakuWithoutParent.Value,
                JVDataStructColumns.CK_BanusiChakuWithoutParent.Value,
                JVDataStructColumns.CK_BreederChakuWithoutParent.Value,
            };
            ChildGetterList = new Func<Array[]>(() => new Array[]
            {
                _dataStruct.KisyuChaku,
                _dataStruct.ChokyoChaku,
                _dataStruct.BanusiChaku,
                _dataStruct.BreederChaku,
            });
            ChildCreateTableSourcesList = new[]
            {
                JVDataStructCreateTableSources.CK_KisyuChaku,
                JVDataStructCreateTableSources.CK_ChokyoChaku,
                JVDataStructCreateTableSources.CK_BanusiChaku,
                JVDataStructCreateTableSources.CK_BreederChaku,
            };
            ChildInsertSourcesList = new[]
            {
                JVDataStructInsertSources.CK_KisyuChaku,
                JVDataStructInsertSources.CK_ChokyoChaku,
                JVDataStructInsertSources.CK_BanusiChaku,
                JVDataStructInsertSources.CK_BreederChaku,
            };
        }
    }

    public partial class JV_CK_CHAKU_V4802DataBridge : DataBridge<JV_CK_CHAKU_V4802>
    {
        partial void InitializeIfNecessary()
        {
            SetTableNameCore(Prefix, nameof(JV_CK_CHAKU));
            ChildTableNameList = new[]
            {
                Prefix + nameof(JVDataStructCreateTableSources.CK_KisyuChaku),
                Prefix + nameof(JVDataStructCreateTableSources.CK_ChokyoChaku),
                Prefix + nameof(JVDataStructCreateTableSources.CK_BanusiChaku),
                Prefix + nameof(JVDataStructCreateTableSources.CK_BreederChaku),
            };
            ChildRowCountList = new[]
            {
                _dataStruct.KisyuChaku.Length,
                _dataStruct.ChokyoChaku.Length,
                _dataStruct.BanusiChaku.Length,
                _dataStruct.BreederChaku.Length,
            };
            ChildPureColumnsList = new[]
            {
                JVDataStructColumns.CK_V4802_KisyuChakuWithoutParent.Value,
                JVDataStructColumns.CK_V4802_ChokyoChakuWithoutParent.Value,
                JVDataStructColumns.CK_V4802_BanusiChakuWithoutParent.Value,
                JVDataStructColumns.CK_V4802_BreederChakuWithoutParent.Value,
            };
            ChildGetterList = new Func<Array[]>(() => new Array[]
            {
                _dataStruct.KisyuChaku,
                _dataStruct.ChokyoChaku,
                _dataStruct.BanusiChaku,
                _dataStruct.BreederChaku,
            });
            ChildCreateTableSourcesList = new[]
            {
                JVDataStructCreateTableSources.CK_KisyuChaku,
                JVDataStructCreateTableSources.CK_ChokyoChaku,
                JVDataStructCreateTableSources.CK_BanusiChaku,
                JVDataStructCreateTableSources.CK_BreederChaku,
            };
            ChildInsertSourcesList = new[]
            {
                JVDataStructInsertSources.CK_KisyuChaku,
                JVDataStructInsertSources.CK_ChokyoChaku,
                JVDataStructInsertSources.CK_BanusiChaku,
                JVDataStructInsertSources.CK_BreederChaku,
            };
        }
    }
}
