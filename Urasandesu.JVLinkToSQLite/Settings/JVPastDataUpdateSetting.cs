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

using DryIoc;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.OperatorAggregates;

namespace Urasandesu.JVLinkToSQLite.Settings
{
    /// <summary>
    /// 動作設定詳細の内、過去データ更新を表す基底クラスです。
    /// </summary>
    public abstract class JVPastDataUpdateSetting : JVLinkToSQLiteDetailSetting
    {
        /// <summary>
        /// 取得方法種別を取得または設定します。
        /// </summary>
        public JVOpenOptions OpenOption { get; set; }

        public override JVOperatorAggregate NewOperatorAggregate(IResolver resolver, bool isImmediate)
        {
            if (IsEnabled && isImmediate)
            {
                return resolver.Resolve<JVPastDataOperatorAggregate.Factory>().New(SQLiteConnectionInfo, DataSpecSettings, OpenOption);
            }
            else
            {
                return resolver.Resolve<JVNullOperatorAggregate>();
            }
        }
    }
}
