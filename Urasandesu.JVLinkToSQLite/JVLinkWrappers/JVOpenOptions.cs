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

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    /// <summary>
    /// 取得方法種別を表します。
    /// </summary>
    /// <remarks>
    /// JVOpen の option 引数に指定する値をベースに、現在のデータの元となった JV-Link の取得方法種別を表します。
    /// </remarks>
    public enum JVOpenOptions
    {
        /// <summary>
        /// 未指定。
        /// </summary>
        None = 0,
        /// <summary>
        /// 通常データ。JVOpen を option=0 で呼び出して取得したデータです。
        /// </summary>
        Normal = 1,
        /// <summary>
        /// セットアップデータ。JVOpen を option=3 で呼び出して取得したデータです。
        /// </summary>
        SetupData = 3,
        /// <summary>
        /// ダイアログ無しセットアップデータ。JVOpen を option=4 で呼び出して取得したデータです。
        /// </summary>
        SetupDataNoDialog = 4,
        /// <summary>
        /// 速報系データ。JVRTOpen を呼び出して取得したデータです。
        /// </summary>
        RealTime = 9
    }
}
