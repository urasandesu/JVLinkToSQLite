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
using System.Globalization;
using System.Linq;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Collections.Generic;
using static Urasandesu.JVLinkToSQLite.JVLinkWrappers.JVDataFileSpec;
using static Urasandesu.JVLinkToSQLite.JVLinkWrappers.JVRecordSpec;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    /// <summary>
    /// データ種別を表します。
    /// </summary>
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class JVDataSpec : IEquatable<JVDataSpec>
    {
        public static readonly JVDataSpec TOKU = new JVDataSpec() { Value = "TOKU", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { TK.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { TK }), DataFileSpecs = new[] { TOKU_TK_T_W, TOKU_TK_H_W, TOKU_TK_M_W } };
        public static readonly JVDataSpec RACE = new JVDataSpec() { Value = "RACE", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { RA.LengthInByte, SE.LengthInByte, HR.LengthInByte, H1.LengthInByte, H6.LengthInByte, O1.LengthInByte, O2.LengthInByte, O3.LengthInByte, O4.LengthInByte, O5.LengthInByte, O6.LengthInByte, WF.LengthInByte, JG.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { RA, SE, HR, H1, H6, O1, O2, O3, O4, O5, O6, WF, JG }), DataFileSpecs = new[] { RACE_RA_G_W, RACE_RA_P_W, RACE_RA_D_W, RACE_RA_B_W, RACE_RA_S_W, RACE_RA_M_W, RACE_SE_G_W, RACE_SE_P_W, RACE_SE_D_W, RACE_SE_B_W, RACE_SE_S_W, RACE_SE_M_W, RACE_HR_S_W, RACE_HR_M_W, RACE_H1_S_W, RACE_H1_M_W, RACE_H6_S_W, RACE_H6_M_W, RACE_O1_S_W, RACE_O1_M_W, RACE_O2_S_W, RACE_O2_M_W, RACE_O3_S_W, RACE_O3_M_W, RACE_O4_S_W, RACE_O4_M_W, RACE_O5_S_W, RACE_O5_M_W, RACE_O6_S_W, RACE_O6_M_W, RACE_WF_S_W, RACE_WF_M_W, RACE_JG_D_W, RACE_JG_M_W, RACE_RA_V_M, RACE_RA_M_M, RACE_SE_V_M, RACE_SE_M_M, RACE_HR_V_M, RACE_HR_M_M, RACE_H1_V_M, RACE_H1_M_M, RACE_H6_V_M, RACE_H6_M_M, RACE_O1_V_M, RACE_O1_M_M, RACE_O2_V_M, RACE_O2_M_M, RACE_O3_V_M, RACE_O3_M_M, RACE_O4_V_M, RACE_O4_M_M, RACE_O5_V_M, RACE_O5_M_M, RACE_O6_V_M, RACE_O6_M_M, RACE_WF_V_M, RACE_WF_M_M, RACE_JG_V_M, RACE_JG_M_M } };
        public static readonly JVDataSpec DIFF = new JVDataSpec() { Value = "DIFF", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { UM.LengthInByte, KS.LengthInByte, CH.LengthInByte, BR.LengthInByte, BN.LengthInByte, RC.LengthInByte, RA.LengthInByte, SE.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { UM, KS, CH, BR, BN, RC, RA, SE }), LatestVersion = new EmbeddableVersion(4, 8, 0, 2), DataFileSpecs = new[] { DIFF_UM_F_W, DIFF_UM_O_W, DIFF_KS_F_W, DIFF_KS_O_W, DIFF_CH_F_W, DIFF_CH_O_W, DIFF_BR_F_W, DIFF_BR_O_W, DIFF_BN_F_W, DIFF_BN_O_W, DIFF_RC_F_W, DIFF_RC_O_W, DIFF_RA_L_W, DIFF_RA_K_W, DIFF_RA_O_W, DIFF_SE_L_W, DIFF_SE_K_W, DIFF_SE_O_W, DIFF_UM_X_M, DIFF_KS_X_M, DIFF_CH_X_M, DIFF_BR_X_M, DIFF_BN_X_M, DIFF_RC_X_M } };
        public static readonly JVDataSpec DIFN = new JVDataSpec() { Value = "DIFN", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { UM.LengthInByte, KS.LengthInByte, CH.LengthInByte, BR.LengthInByte, BN.LengthInByte, RC.LengthInByte, RA.LengthInByte, SE.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { UM, KS, CH, BR, BN, RC, RA, SE }), DataFileSpecs = new[] { DIFN_UM_F_W, DIFN_UM_O_W, DIFN_KS_F_W, DIFN_KS_O_W, DIFN_CH_F_W, DIFN_CH_O_W, DIFN_BR_F_W, DIFN_BR_O_W, DIFN_BN_F_W, DIFN_BN_O_W, DIFN_RC_F_W, DIFN_RC_O_W, DIFN_RA_L_W, DIFN_RA_K_W, DIFN_RA_O_W, DIFN_SE_L_W, DIFN_SE_K_W, DIFN_SE_O_W, DIFN_UM_X_M, DIFN_KS_X_M, DIFN_CH_X_M, DIFN_BR_X_M, DIFN_BN_X_M, DIFN_RC_X_M } };
        public static readonly JVDataSpec BLOD = new JVDataSpec() { Value = "BLOD", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { HN.LengthInByte, SK.LengthInByte, BT.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { HN, SK, BT }), LatestVersion = new EmbeddableVersion(4, 8, 0, 2), DataFileSpecs = new[] { BLOD_HN_F_W, BLOD_HN_M_W, BLOD_SK_F_W, BLOD_SK_M_W, BLOD_BT_F_W, BLOD_HN_V_M, BLOD_HN_M_M, BLOD_SK_V_M, BLOD_SK_M_M, BLOD_BT_X_M } };
        public static readonly JVDataSpec BLDN = new JVDataSpec() { Value = "BLDN", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { HN.LengthInByte, SK.LengthInByte, BT.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { HN, SK, BT }), DataFileSpecs = new[] { BLDN_HN_F_W, BLDN_HN_M_W, BLDN_SK_F_W, BLDN_SK_M_W, BLDN_BT_F_W, BLDN_HN_V_M, BLDN_HN_M_M, BLDN_SK_V_M, BLDN_SK_M_M, BLDN_BT_X_M } };
        public static readonly JVDataSpec MING = new JVDataSpec() { Value = "MING", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { DM.LengthInByte, TM.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { DM, TM }), DataFileSpecs = new[] { MING_DM_S_W, MING_TM_S_W, MING_DM_V_M, MING_TM_V_M } };
        public static readonly JVDataSpec SNAP = new JVDataSpec() { Value = "SNAP", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { CK.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { CK }), LatestVersion = new EmbeddableVersion(4, 8, 0, 2), DataFileSpecs = new[] { SNAP_CK_F_W, SNAP_CK_C_W, SNAP_CK_M_W, SNAP_CK_V_M, SNAP_CK_M_M } };
        public static readonly JVDataSpec SNPN = new JVDataSpec() { Value = "SNPN", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { CK.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { CK }), DataFileSpecs = new[] { SNPN_CK_F_W, SNPN_CK_C_W, SNPN_CK_M_W, SNPN_CK_V_M, SNPN_CK_M_M } };
        public static readonly JVDataSpec SLOP = new JVDataSpec() { Value = "SLOP", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { HC.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { HC }), DataFileSpecs = new[] { SLOP_HC_E_W, SLOP_HC_W_W, SLOP_HC_M_W, SLOP_HC_V_M, SLOP_HC_M_M } };
        public static readonly JVDataSpec YSCH = new JVDataSpec() { Value = "YSCH", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { YS.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { YS }), DataFileSpecs = new[] { YSCH_YS_N_W, YSCH_YS_M_W, YSCH_YS_Y_M, YSCH_YS_M_M } };
        public static readonly JVDataSpec HOSE = new JVDataSpec() { Value = "HOSE", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { HS.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { HS }), LatestVersion = new EmbeddableVersion(4, 8, 0, 2), DataFileSpecs = new[] { HOSE_HS_F_W, HOSE_HS_X_M } };
        public static readonly JVDataSpec HOSN = new JVDataSpec() { Value = "HOSN", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { HS.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { HS }), DataFileSpecs = new[] { HOSN_HS_F_W, HOSN_HS_X_M } };
        public static readonly JVDataSpec HOYU = new JVDataSpec() { Value = "HOYU", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { HY.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { HY }), DataFileSpecs = new[] { HOYU_HY_F_W, HOYU_HY_O_W, HOYU_HY_X_M } };
        public static readonly JVDataSpec COMM = new JVDataSpec() { Value = "COMM", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { CS.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { CS }), DataFileSpecs = new[] { COMM_CS_F_W, COMM_CS_X_M } };
        public static readonly JVDataSpec WOOD = new JVDataSpec() { Value = "WOOD", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { WC.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { WC }), DataFileSpecs = new[] { WOOD_WC_E_W, WOOD_WC_W_W, WOOD_WC_M_W, WOOD_WC_V_M, WOOD_WC_M_M } };
        public static readonly JVDataSpec TCOV = new JVDataSpec() { Value = "TCOV", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { UM.LengthInByte, CH.LengthInByte, BR.LengthInByte, BN.LengthInByte, RC.LengthInByte, RA.LengthInByte, SE.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { UM, CH, BR, BN, RC, RA, SE }), LatestVersion = new EmbeddableVersion(4, 8, 0, 2), DataFileSpecs = new[] { TCOV_UM_T_W, TCOV_CH_T_W, TCOV_BR_T_W, TCOV_BN_T_W, TCOV_RC_T_W, TCOV_RA_U_W, TCOV_SE_U_W } };
        public static readonly JVDataSpec TCVN = new JVDataSpec() { Value = "TCVN", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { UM.LengthInByte, CH.LengthInByte, BR.LengthInByte, BN.LengthInByte, RC.LengthInByte, RA.LengthInByte, SE.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { UM, CH, BR, BN, RC, RA, SE }), DataFileSpecs = new[] { TCVN_UM_T_W, TCVN_CH_T_W, TCVN_BR_T_W, TCVN_BN_T_W, TCVN_RC_T_W, TCVN_RA_U_W, TCVN_SE_U_W } };
        public static readonly JVDataSpec RCOV = new JVDataSpec() { Value = "RCOV", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { UM.LengthInByte, KS.LengthInByte, CH.LengthInByte, BR.LengthInByte, BN.LengthInByte, RC.LengthInByte, RA.LengthInByte, SE.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { UM, KS, CH, BR, BN, RC, RA, SE }), LatestVersion = new EmbeddableVersion(4, 8, 0, 2), DataFileSpecs = new[] { RCOV_UM_P_W, RCOV_KS_P_W, RCOV_CH_P_W, RCOV_BR_P_W, RCOV_BN_P_W, RCOV_RC_P_W, RCOV_RA_Q_W, RCOV_SE_Q_W } };
        public static readonly JVDataSpec RCVN = new JVDataSpec() { Value = "RCVN", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { UM.LengthInByte, KS.LengthInByte, CH.LengthInByte, BR.LengthInByte, BN.LengthInByte, RC.LengthInByte, RA.LengthInByte, SE.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { UM, KS, CH, BR, BN, RC, RA, SE }), DataFileSpecs = new[] { RCVN_UM_P_W, RCVN_KS_P_W, RCVN_CH_P_W, RCVN_BR_P_W, RCVN_BN_P_W, RCVN_RC_P_W, RCVN_RA_Q_W, RCVN_SE_Q_W } };
        public static readonly JVDataSpec _0B12 = new JVDataSpec() { Value = "0B12", IsImmediatelyExecutable = true, CanWatchEvent = true, MaxLengthInByte = Enumerable.Max(new[] { RA.LengthInByte, SE.LengthInByte, HR.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { RA, SE, HR }) };
        public static readonly JVDataSpec _0B15 = new JVDataSpec() { Value = "0B15", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { RA.LengthInByte, SE.LengthInByte, HR.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { RA, SE, HR }) };
        public static readonly JVDataSpec _0B30 = new JVDataSpec() { Value = "0B30", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O1.LengthInByte, O2.LengthInByte, O3.LengthInByte, O4.LengthInByte, O5.LengthInByte, O6.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O1, O2, O3, O4, O5, O6 }) };
        public static readonly JVDataSpec _0B31 = new JVDataSpec() { Value = "0B31", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O1.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O1 }) };
        public static readonly JVDataSpec _0B32 = new JVDataSpec() { Value = "0B32", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O2.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O2 }) };
        public static readonly JVDataSpec _0B33 = new JVDataSpec() { Value = "0B33", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O3.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O3 }) };
        public static readonly JVDataSpec _0B34 = new JVDataSpec() { Value = "0B34", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O4.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O4 }) };
        public static readonly JVDataSpec _0B35 = new JVDataSpec() { Value = "0B35", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O5.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O5 }) };
        public static readonly JVDataSpec _0B36 = new JVDataSpec() { Value = "0B36", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O6.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O6 }) };
        public static readonly JVDataSpec _0B20 = new JVDataSpec() { Value = "0B20", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { H1.LengthInByte, H6.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { H1, H6 }) };
        public static readonly JVDataSpec _0B11 = new JVDataSpec() { Value = "0B11", IsImmediatelyExecutable = true, CanWatchEvent = true, MaxLengthInByte = Enumerable.Max(new[] { WH.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { WH }) };
        public static readonly JVDataSpec _0B14 = new JVDataSpec() { Value = "0B14", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { WE.LengthInByte, AV.LengthInByte, JC.LengthInByte, TC.LengthInByte, CC.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { WE, AV, JC, TC, CC }) };
        public static readonly JVDataSpec _0B16 = new JVDataSpec() { Value = "0B16", IsImmediatelyExecutable = false, CanWatchEvent = true, MaxLengthInByte = Enumerable.Max(new[] { WE.LengthInByte, AV.LengthInByte, JC.LengthInByte, TC.LengthInByte, CC.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { WE, AV, JC, TC, CC }) };
        public static readonly JVDataSpec _0B13 = new JVDataSpec() { Value = "0B13", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { DM.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { DM }) };
        public static readonly JVDataSpec _0B17 = new JVDataSpec() { Value = "0B17", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { TM.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { TM }) };
        public static readonly JVDataSpec _0B41 = new JVDataSpec() { Value = "0B41", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O1.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O1 }) };
        public static readonly JVDataSpec _0B42 = new JVDataSpec() { Value = "0B42", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { O2.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { O2 }) };
        public static readonly JVDataSpec _0B51 = new JVDataSpec() { Value = "0B51", IsImmediatelyExecutable = true, CanWatchEvent = false, MaxLengthInByte = Enumerable.Max(new[] { WF.LengthInByte }), CandidateRecordSpecs = new ReadOnlyHashSet<JVRecordSpec>(new[] { WF }) };

        private class DataSpecDictionary
        {
            public static readonly Dictionary<string, JVDataSpec> Instance = NewInstance();

            private static Dictionary<string, JVDataSpec> NewInstance()
            {
                return new Dictionary<string, JVDataSpec>()
                {
                    { TOKU.Value, TOKU },
                    { RACE.Value, RACE },
                    { DIFF.Value, DIFF },
                    { DIFN.Value, DIFN },
                    { BLOD.Value, BLOD },
                    { BLDN.Value, BLDN },
                    { MING.Value, MING },
                    { SNAP.Value, SNAP },
                    { SNPN.Value, SNPN },
                    { SLOP.Value, SLOP },
                    { YSCH.Value, YSCH },
                    { HOSE.Value, HOSE },
                    { HOSN.Value, HOSN },
                    { HOYU.Value, HOYU },
                    { COMM.Value, COMM },
                    { WOOD.Value, WOOD },
                    { TCOV.Value, TCOV },
                    { TCVN.Value, TCVN },
                    { RCOV.Value, RCOV },
                    { RCVN.Value, RCVN },
                    { _0B12.Value, _0B12 },
                    { _0B15.Value, _0B15 },
                    { _0B30.Value, _0B30 },
                    { _0B31.Value, _0B31 },
                    { _0B32.Value, _0B32 },
                    { _0B33.Value, _0B33 },
                    { _0B34.Value, _0B34 },
                    { _0B35.Value, _0B35 },
                    { _0B36.Value, _0B36 },
                    { _0B20.Value, _0B20 },
                    { _0B11.Value, _0B11 },
                    { _0B14.Value, _0B14 },
                    { _0B16.Value, _0B16 },
                    { _0B13.Value, _0B13 },
                    { _0B17.Value, _0B17 },
                    { _0B41.Value, _0B41 },
                    { _0B42.Value, _0B42 },
                    { _0B51.Value, _0B51 },
                };
            }
        }

        public static JVDataSpec Parse(string s)
        {
            return DataSpecDictionary.Instance[s];
        }

        public static JVDataSpec MakeIncomplete(string s)
        {
            return new JVDataSpec() { Value = s };
        }

        private JVDataSpec() { }

        /// <summary>
        /// データ種別の ID を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 即時実行可能かどうかを取得します。
        /// </summary>
        public bool IsImmediatelyExecutable { get; private set; }

        /// <summary>
        /// イベントを監視できるかどうかを取得します。
        /// </summary>
        public bool CanWatchEvent { get; private set; }

        /// <summary>
        /// 最大データ長をバイト単位で取得します。
        /// </summary>
        public int MaxLengthInByte { get; private set; }

        /// <summary>
        /// このデータ種別に含めることが可能なレコード種別のリストを取得します。
        /// </summary>
        public ReadOnlyHashSet<JVRecordSpec> CandidateRecordSpecs { get; private set; }

        /// <summary>
        /// このデータ種別の最新バージョンを取得します。
        /// </summary>
        /// <remarks>
        /// このプロパティは、同じデータ種別に、過去のある時点のバージョンにおいて、現在とは別のデータ種別 ID が割り振られていた場合にのみ設定されます。
        /// それ以外の場合、null になります。\n
        /// \n
        /// ・「蓄積系ソフト用　蓄積情報」は、JV-Data V4.8.0.2 の時はデータ種別 ID=DIFF だったが、V4.9.0 以降はデータ種別 ID=DIFN に変更された\n
        /// ⇒ この場合、このプロパティは、DIFF を表すオブジェクトでは EmbeddableVersion(4, 8, 0, 2) に、DIFN を表すオブジェクトでは null になる
        /// </remarks>
        public EmbeddableVersion LatestVersion { get; private set; }

        /// <summary>
        /// このデータ種別に含まれるデータ ファイル仕様のリストを取得します。
        /// </summary>
        public IReadOnlyList<JVDataFileSpec> DataFileSpecs { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as JVDataSpec);
        }

        public bool Equals(JVDataSpec other)
        {
            return !(other is null) &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = 730050499;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }

        public static bool operator ==(JVDataSpec left, JVDataSpec right)
        {
            return EqualityComparer<JVDataSpec>.Default.Equals(left, right);
        }

        public static bool operator !=(JVDataSpec left, JVDataSpec right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Value;
        }

        public JVDataFile GetDataFile(string fileName)
        {
            if (DataFileSpecs == null || fileName == null)
            {
                return null;
            }

            foreach (var dataFileSpec in DataFileSpecs)
            {
                var m = dataFileSpec.FileNameRegex.Match(fileName);
                if (m.Success)
                {
                    var datetimeKey = m.Groups["datekey"].Value;
                    var publishedDateTime = DateTime.ParseExact(m.Groups["datetime"].Value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    return new JVDataFile(dataFileSpec, fileName, datetimeKey, publishedDateTime);
                }
            }

            throw new NotSupportedException($"サポートしていないファイル名 '{fileName}' が指定されました。" +
                                            "JV-Link の仕様変更に JVLinkToSQLite が追従できていないかもしれません。バージョンアップの有無をご確認ください。");
        }
    }
}
