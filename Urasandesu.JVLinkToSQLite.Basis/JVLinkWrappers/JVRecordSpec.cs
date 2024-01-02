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
using System.Diagnostics;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    /// <summary>
    /// レコード種別を表します。
    /// </summary>
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class JVRecordSpec : IEquatable<JVRecordSpec>
    {
        public static readonly JVRecordSpec TK = new JVRecordSpec() { Value = nameof(TK), LengthInByte = 21657 + 1 };
        public static readonly JVRecordSpec RA = new JVRecordSpec() { Value = nameof(RA), LengthInByte = 1272 + 1 };
        public static readonly JVRecordSpec SE = new JVRecordSpec() { Value = nameof(SE), LengthInByte = 555 + 1 };
        public static readonly JVRecordSpec HR = new JVRecordSpec() { Value = nameof(HR), LengthInByte = 719 + 1 };
        public static readonly JVRecordSpec H1 = new JVRecordSpec() { Value = nameof(H1), LengthInByte = 28955 + 1 };
        public static readonly JVRecordSpec H6 = new JVRecordSpec() { Value = nameof(H6), LengthInByte = 102890 + 1 };
        public static readonly JVRecordSpec O1 = new JVRecordSpec() { Value = nameof(O1), LengthInByte = 962 + 1 };
        public static readonly JVRecordSpec O2 = new JVRecordSpec() { Value = nameof(O2), LengthInByte = 2042 + 1 };
        public static readonly JVRecordSpec O3 = new JVRecordSpec() { Value = nameof(O3), LengthInByte = 2654 + 1 };
        public static readonly JVRecordSpec O4 = new JVRecordSpec() { Value = nameof(O4), LengthInByte = 4031 + 1 };
        public static readonly JVRecordSpec O5 = new JVRecordSpec() { Value = nameof(O5), LengthInByte = 12293 + 1 };
        public static readonly JVRecordSpec O6 = new JVRecordSpec() { Value = nameof(O6), LengthInByte = 83285 + 1 };
        public static readonly JVRecordSpec UM = new JVRecordSpec() { Value = nameof(UM), LengthInByte = 1609 + 1 };
        public static readonly JVRecordSpec KS = new JVRecordSpec() { Value = nameof(KS), LengthInByte = 4173 + 1 };
        public static readonly JVRecordSpec CH = new JVRecordSpec() { Value = nameof(CH), LengthInByte = 3862 + 1 };
        public static readonly JVRecordSpec BR = new JVRecordSpec() { Value = nameof(BR), LengthInByte = 545 + 1 };
        public static readonly JVRecordSpec BN = new JVRecordSpec() { Value = nameof(BN), LengthInByte = 477 + 1 };
        public static readonly JVRecordSpec HN = new JVRecordSpec() { Value = nameof(HN), LengthInByte = 251 + 1 };
        public static readonly JVRecordSpec SK = new JVRecordSpec() { Value = nameof(SK), LengthInByte = 208 + 1 };
        public static readonly JVRecordSpec CK = new JVRecordSpec() { Value = nameof(CK), LengthInByte = 6870 + 1 };
        public static readonly JVRecordSpec RC = new JVRecordSpec() { Value = nameof(RC), LengthInByte = 501 + 1 };
        public static readonly JVRecordSpec HC = new JVRecordSpec() { Value = nameof(HC), LengthInByte = 60 + 1 };
        public static readonly JVRecordSpec HS = new JVRecordSpec() { Value = nameof(HS), LengthInByte = 200 + 1 };
        public static readonly JVRecordSpec HY = new JVRecordSpec() { Value = nameof(HY), LengthInByte = 123 + 1 };
        public static readonly JVRecordSpec YS = new JVRecordSpec() { Value = nameof(YS), LengthInByte = 382 + 1 };
        public static readonly JVRecordSpec BT = new JVRecordSpec() { Value = nameof(BT), LengthInByte = 6889 + 1 };
        public static readonly JVRecordSpec CS = new JVRecordSpec() { Value = nameof(CS), LengthInByte = 6829 + 1 };
        public static readonly JVRecordSpec DM = new JVRecordSpec() { Value = nameof(DM), LengthInByte = 303 + 1 };
        public static readonly JVRecordSpec TM = new JVRecordSpec() { Value = nameof(TM), LengthInByte = 141 + 1 };
        public static readonly JVRecordSpec WF = new JVRecordSpec() { Value = nameof(WF), LengthInByte = 7215 + 1 };
        public static readonly JVRecordSpec JG = new JVRecordSpec() { Value = nameof(JG), LengthInByte = 80 + 1 };
        public static readonly JVRecordSpec WC = new JVRecordSpec() { Value = nameof(WC), LengthInByte = 105 + 1 };
        public static readonly JVRecordSpec WH = new JVRecordSpec() { Value = nameof(WH), LengthInByte = 847 + 1 };
        public static readonly JVRecordSpec WE = new JVRecordSpec() { Value = nameof(WE), LengthInByte = 42 + 1 };
        public static readonly JVRecordSpec AV = new JVRecordSpec() { Value = nameof(AV), LengthInByte = 78 + 1 };
        public static readonly JVRecordSpec JC = new JVRecordSpec() { Value = nameof(JC), LengthInByte = 161 + 1 };
        public static readonly JVRecordSpec TC = new JVRecordSpec() { Value = nameof(TC), LengthInByte = 45 + 1 };
        public static readonly JVRecordSpec CC = new JVRecordSpec() { Value = nameof(CC), LengthInByte = 50 + 1 };

        private static Dictionary<string, JVRecordSpec> _recordSpecDictionary;
        private static Dictionary<string, JVRecordSpec> RecordSpecDictionary
        {
            get
            {
                if (_recordSpecDictionary == null)
                {
                    _recordSpecDictionary = new Dictionary<string, JVRecordSpec>
                    {
                        { TK.Value, TK },
                        { RA.Value, RA },
                        { SE.Value, SE },
                        { HR.Value, HR },
                        { H1.Value, H1 },
                        { H6.Value, H6 },
                        { O1.Value, O1 },
                        { O2.Value, O2 },
                        { O3.Value, O3 },
                        { O4.Value, O4 },
                        { O5.Value, O5 },
                        { O6.Value, O6 },
                        { UM.Value, UM },
                        { KS.Value, KS },
                        { CH.Value, CH },
                        { BR.Value, BR },
                        { BN.Value, BN },
                        { HN.Value, HN },
                        { SK.Value, SK },
                        { CK.Value, CK },
                        { RC.Value, RC },
                        { HC.Value, HC },
                        { HS.Value, HS },
                        { HY.Value, HY },
                        { YS.Value, YS },
                        { BT.Value, BT },
                        { CS.Value, CS },
                        { DM.Value, DM },
                        { TM.Value, TM },
                        { WF.Value, WF },
                        { JG.Value, JG },
                        { WC.Value, WC },
                        { WH.Value, WH },
                        { WE.Value, WE },
                        { AV.Value, AV },
                        { JC.Value, JC },
                        { TC.Value, TC },
                        { CC.Value, CC }
                    };
                }
                return _recordSpecDictionary;
            }
        }
        public static JVRecordSpec Parse(string s)
        {
            return RecordSpecDictionary[s];
        }

        public static JVRecordSpec MakeIncomplete(string s)
        {
            return new JVRecordSpec() { Value = s };
        }

        private JVRecordSpec() { }

        /// <summary>
        /// レコード種別の ID を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// レコード種別のデータ長をバイト単位で取得します。
        /// </summary>
        public int LengthInByte { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JVRecordSpec);
        }

        public bool Equals(JVRecordSpec other)
        {
            return !(other is null) &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
        }

        public static bool operator ==(JVRecordSpec left, JVRecordSpec right)
        {
            return EqualityComparer<JVRecordSpec>.Default.Equals(left, right);
        }

        public static bool operator !=(JVRecordSpec left, JVRecordSpec right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
