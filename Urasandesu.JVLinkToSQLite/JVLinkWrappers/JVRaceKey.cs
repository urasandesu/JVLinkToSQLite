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
    public class JVRaceKey : JVDataSpecKey
    {
        public JVRaceKey()
        { }

        public JVRaceKey(DateTime kaisaiDate, string jyoCD, string kaiji, string nichiji, string raceNum)
        {
            KaisaiDate = kaisaiDate;
            JyoCD = jyoCD;
            Kaiji = kaiji;
            Nichiji = nichiji;
            RaceNum = raceNum;
        }

        public DateTime KaisaiDate { get; set; }
        public string JyoCD { get; set; }
        public string Kaiji { get; set; }
        public string Nichiji { get; set; }
        public string RaceNum { get; set; }

        public new JVRaceKey Clone()
        {
            return (JVRaceKey)base.Clone();
        }

        protected override JVDataSpecKey CloneCore()
        {
            return new JVRaceKey(KaisaiDate, JyoCD, Kaiji, Nichiji, RaceNum);
        }

        public override string GetKey()
        {
            return KaisaiDate.ToString("yyyyMMdd") + JyoCD + Kaiji + Nichiji + RaceNum;
        }

        public override IEnumerable<JVDataSpecKey> GetSpecKeysInInterval(TimeSpan interval)
        {
            yield return this;
        }
    }
}
