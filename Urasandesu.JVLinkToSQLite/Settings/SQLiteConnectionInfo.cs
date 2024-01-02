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

namespace Urasandesu.JVLinkToSQLite.Settings
{
    /// <summary>
    /// SQLite データベース接続情報を表します。
    /// </summary>
    public class SQLiteConnectionInfo
    {
        /// <summary>
        /// SQLite データベースのファイル パスを指定して、クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dataSource">SQLite データベースのファイル パス</param>
        public SQLiteConnectionInfo(string dataSource) :
            this(dataSource, 0)
        { }

        /// <summary>
        /// SQLite データベースのファイル パスと、スロットルサイズを指定して、クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dataSource">SQLite データベースのファイル パス</param>
        /// <param name="throttleSize">スロットルサイズ</param>
        public SQLiteConnectionInfo(string dataSource, int throttleSize)
        {
            DataSource = dataSource;
            ThrottleSize = throttleSize;
        }

        /// <summary>
        /// SQLite データベースのファイル パスを取得します。
        /// </summary>
        public string DataSource { get; }

        /// <summary>
        /// スロットルサイズを取得します。
        /// </summary>
        public int ThrottleSize { get; }
    }
}
