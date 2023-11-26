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

using JVDTLabLib;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    public class JVInitResult : JVLinkResult
    {
        protected override void SetReturnCodeCore(int returnCode)
        {
            if (returnCode == 0)
            {
                Interpretation = JVResultInterpretation.SuccessTrue;
                DebugMessage = $"[{nameof(JVLink.JVInit)}]正常。RC={returnCode}";
                DebugCauseAndTreatment = "-";
            }
            else if (returnCode == -101)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVInit)}]sid が設定されていない。RC={returnCode}";
                DebugCauseAndTreatment = "sid パラメータの渡し方に問題があると思われます。サンプル プログラム等を参照し、正しくパラメータが JV-Link に渡っているか確認してください。";
            }
            else if (returnCode == -102)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVInit)}]sid が 64byte を超えている。RC={returnCode}";
                DebugCauseAndTreatment = "sid パラメータの渡し方に問題があるか、渡した内容に問題があると思われます。64byte 以内の正しい sid を設定してください。";
            }
            else if (returnCode == -103)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVInit)}]sid が不正（sid の １ 桁目がスペース）。RC={returnCode}";
                DebugCauseAndTreatment = "sid パラメータの内容に問題があると思われます。sid の １ 桁目は必ずスペース以外である必要があります。";
            }
            else
            {
                SetUnknownReturnCode(nameof(JVLink.JVInit), returnCode);
            }
        }
    }
}
