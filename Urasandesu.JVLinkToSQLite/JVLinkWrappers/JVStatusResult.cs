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
    public class JVStatusResult : JVLinkResult
    {
        public int Count { get; private set; }
        public bool DownloadCompleted { get; private set; }
        public int DownloadCount { get; private set; }

        protected override void SetReturnCodeCore(int returnCode)
        {
            if (returnCode >= 0)
            {
                Interpretation = JVResultInterpretation.SuccessTrue;
                DebugMessage = $"[{nameof(JVLink.JVStatus)}]正常（ダウンロード済みファイル数）。RC={returnCode}";
                DebugCauseAndTreatment = "-";
            }
            else if (returnCode == -201)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVStatus)}]JVInit が行なわれていない。RC={returnCode}";
                DebugCauseAndTreatment = "JVStatus に先立って JVInit が呼ばれていないと思われます。必ず JVInit を先に呼び出してください。";
            }
            else if (returnCode == -203)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVStatus)}]JVOpen が行なわれていない。RC={returnCode}";
                DebugCauseAndTreatment = "JVStatus に先立って JVOpen が呼ばれていないとわれます。必ず JVOpen を先に呼び出してください。";
            }
            else if (returnCode == -502)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVStatus)}]ダウンロード失敗（通信エラーやディスク エラーなど）。RC={returnCode}";
                DebugCauseAndTreatment = "ダウンロード処理に失敗しました。エラーの原因を除去しないかぎり解決しないと思われます。原因を除去できたら JVClose を呼び出し、JVOpen からの処理をやりなおしてください。サーバーが混雑している場合のタイムアウトでもこの戻り値が返されることがあります。";
            }
            else
            {
                SetUnknownReturnCode(nameof(JVLink.JVStatus), returnCode);
            }
            Count = returnCode;
            CoerceDownloadCompleted();
        }

        public void SetDownloadCount(int downloadCount)
        {
            DownloadCount = downloadCount;
            CoerceDownloadCompleted();
        }

        private void CoerceDownloadCompleted()
        {
            DownloadCompleted = Count == DownloadCount;
        }

        public static JVStatusResult New(int returnCode, int downloadCount = 0)
        {
            var statusRslt = new JVStatusResult();
            statusRslt.SetReturnCode(returnCode);
            statusRslt.SetDownloadCount(downloadCount);
            return statusRslt;
        }
    }
}
