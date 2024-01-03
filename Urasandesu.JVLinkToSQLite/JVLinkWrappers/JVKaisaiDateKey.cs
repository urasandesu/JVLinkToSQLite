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
using System.Collections.Generic;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    /// <summary>
    /// 開催日単位のデータ種別検索キーを表します。
    /// </summary>
    public class JVKaisaiDateKey : JVDataSpecKey
    {
        /// <summary>
        /// 空の開催日単位のデータ種別検索キーを初期化します。
        /// </summary>
        public JVKaisaiDateKey()
        { }

        /// <summary>
        /// 開催日を指定して、開催日単位のデータ種別検索キーを初期化します。
        /// </summary>
        /// <param name="kaisaiDate">開催日。時分秒以下の情報も保持できますが、検索キーとして使用する時には切り捨てられます。</param>
        public JVKaisaiDateKey(DateTime kaisaiDate)
        {
            KaisaiDate = kaisaiDate;
        }

        /// <summary>
        /// 開催日を取得もしくは設定します。
        /// </summary>
        public DateTime KaisaiDate { get; set; }

        public new JVKaisaiDateKey Clone()
        {
            return (JVKaisaiDateKey)base.Clone();
        }

        protected override JVDataSpecKey CloneCore()
        {
            return new JVKaisaiDateKey(KaisaiDate);
        }

        public override string GetKey()
        {
            return KaisaiDate.ToString("yyyyMMdd");
        }

        public override IEnumerable<JVDataSpecKey> GetSpecKeysInInterval(TimeSpan interval)
        {
            yield return this;
        }
    }
}
