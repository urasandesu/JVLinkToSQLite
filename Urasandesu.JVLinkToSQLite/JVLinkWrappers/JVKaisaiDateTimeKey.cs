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
    /// 開催日時単位のデータ種別検索キーを表します。
    /// </summary>
    public sealed class JVKaisaiDateTimeKey : JVDataSpecKey, IIntervalDivisibleDataSpecKey
    {
        /// <summary>
        /// 空の開催日時単位のデータ種別検索キーを初期化します。
        /// </summary>
        public JVKaisaiDateTimeKey()
        { }

        /// <summary>
        /// 開催日時を指定して、開催日時単位のデータ種別検索キーを初期化します。
        /// </summary>
        /// <param name="kaisaiDateTime">開催日時</param>
        public JVKaisaiDateTimeKey(DateTime kaisaiDateTime)
        {
            KaisaiDateTime = kaisaiDateTime;
        }

        private DateTime _kaisaiDateTime;
        /// <summary>
        /// 開催日時を取得または設定します。
        /// </summary>
        public DateTime KaisaiDateTime
        {
            get { return _kaisaiDateTime; }
            set
            {
                _kaisaiDateTime = value;
                var now = DateTime.Now;
                _dateTimeTo = _kaisaiDateTime < now ? now : _kaisaiDateTime;
            }
        }

        DateTime IIntervalDivisibleDataSpecKey.DateTimeFrom => KaisaiDateTime;

        private DateTime _dateTimeTo;
        DateTime IIntervalDivisibleDataSpecKey.DateTimeTo { get => _dateTimeTo; }

        public new JVKaisaiDateTimeKey Clone()
        {
            return (JVKaisaiDateTimeKey)base.Clone();
        }

        protected override JVDataSpecKey CloneCore()
        {
            return new JVKaisaiDateTimeKey(KaisaiDateTime);
        }

        public override string GetKey()
        {
            return KaisaiDateTime.ToString("yyyyMMddHHmmss");
        }

        public override IEnumerable<JVDataSpecKey> GetSpecKeysInInterval(TimeSpan interval)
        {
            return this.DivideSpecKeysInInterval(interval);
        }
    }
}
