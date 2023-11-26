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
using System;
using System.Globalization;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    public class JVOpenResult : JVLinkResult, IDisposable
    {
        public bool IsEmpty { get; private set; }
        public int ReadCount { get; private set; }
        public bool NeedsDownload { get; private set; }
        public int DownloadCount { get; private set; }
        public DateTime? LastFileTimestamp { get; private set; }
        public JVDataSpec DataSpec { get; private set; }
        public JVOpenOptions OpenOption { get; private set; }

        protected override void SetReturnCodeCore(int returnCode)
        {
            if (returnCode == 0)
            {
                Interpretation = JVResultInterpretation.SuccessTrue;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]正常 。RC={returnCode}";
                DebugCauseAndTreatment = "-";
            }
            else if (returnCode == -1)
            {
                Interpretation = JVResultInterpretation.SuccessFalse;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]該当データ無し。RC={returnCode}";
                DebugCauseAndTreatment = "指定されたパラメータに合致する新しいデータがサーバーに存在しない。又は、最新バージョンが公開され、ユーザーが最新バージョンのダウンロードを選択しました。JVClose を呼び出して取り込み処理を終了してください。";
            }
            else if (returnCode == -2)
            {
                Interpretation = JVResultInterpretation.SuccessFalse;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]セットアップ ダイアログでキャンセルが押された。RC={returnCode}";
                DebugCauseAndTreatment = "セットアップ用データの取り込み時にユーザーがダイアログでキャンセルを押しました。JVClose を呼び出して取り込み処理を終了してください。";
            }
            else if (returnCode == -111)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]dataspec パラメータが不正。RC={returnCode}";
                DebugCauseAndTreatment = "パラメータの渡し方かパラメータの内容に問題があると思われます。サンプル プログラム等を参照し、正しくパラメータが JV-Link に渡っているか確認してください。";
            }
            else if (returnCode == -112)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]fromtime パラメータが不正（読み出し開始ポイント時刻不正）。RC={returnCode}";
                DebugCauseAndTreatment = "パラメータ（読み出し開始ポイント時刻）の渡し方かパラメータ（読み出し開始ポイント時刻）の内容に問題があると思われます。サンプル プログラム等を参照し、正しくパラメータが JV-Link に渡っているか確認してください。";
            }
            else if (returnCode == -113)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]fromtime パラメータが不正（読み出し終了ポイント時刻不正）。RC={returnCode}";
                DebugCauseAndTreatment = "パラメータ（読み出し終了ポイント時刻）の渡し方かパラメータ（読み出し終了ポイント時刻）の内容に問題があると思われます。サンプル プログラム等を参照し、正しくパラメータが JV-Link に渡っているか確認してください。";
            }
            else if (returnCode == -114)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]key パラメータが不正。RC={returnCode}";
                DebugCauseAndTreatment = "パラメータの渡し方かパラメータの内容に問題があると思われます。サンプル プログラム等を参照し、正しくパラメータが JV-Link に渡っているか確認してください。";
            }
            else if (returnCode == -115)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]option パラメータが不正。RC={returnCode}";
                DebugCauseAndTreatment = "パラメータの渡し方かパラメータの内容に問題があると思われます。サンプル プログラム等を参照し、正しくパラメータが JV-Link に渡っているか確認してください。";
            }
            else if (returnCode == -116)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]dataspec と option の組み合わせが不正。RC={returnCode}";
                DebugCauseAndTreatment = "パラメータの渡し方かパラメータの内容に問題があると思われます。サンプル プログラム等を参照し、正しくパラメータが JV-Link に渡っているか確認してください。";
            }
            else if (returnCode == -201)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]JVInit が行なわれていない。RC={returnCode}";
                DebugCauseAndTreatment = "JVOpen/JVRTOpen に先立って JVInit が呼ばれていないと思われます。必ず JVInit を先に呼び出してください。";
            }
            else if (returnCode == -202)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]前回の JVOpen/JVRTOpen/JVMVOpen に対して JVClose が呼ばれていない（オープン中）。RC={returnCode}";
                DebugCauseAndTreatment = "前回呼び出した JVOpen/JVRTOpen/JVMVOpen が JVClose によってクローズされていないと思われます。JVOpen/JVRTOpen/JVMVOpen を呼び出した後は次に呼び出すまでの間に JVClose を必ず呼び出してください。";
            }
            else if (returnCode == -211)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]レジストリ内容が不正（レジストリ内容が不正に変更された）。RC={returnCode}";
                DebugCauseAndTreatment = "JV-Link はレジストリに値をセットする際に値のチェックを行います（例えば利用キーの桁数など）が、レジストリから値を読み出して使用する際に問題が発生するとこのエラーが発生します。レジストリが直接書き換えられたなどの状況が考えられない場合には JRA-VAN へご連絡ください。";
            }
            else if (returnCode == -301)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]認証エラー。RC={returnCode}";
                DebugCauseAndTreatment = "利用キーが正しくない。あるいは複数のマシンで同一利用キーを使用した場合に発生します。複数のマシンで同じ利用キーをしようした場合には、このエラーが発生したマシンの JV-Link アンインストールし、再インストール後、利用キーの再発行が必要となります。";
            }
            else if (returnCode == -302)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]利用キーの有効期限切れ。RC={returnCode}";
                DebugCauseAndTreatment = "Data Lab.サービスの有効期限が切れています。サービス権の自動延長が停止していると思われます。解消するにはサービス権の再購入が必要です。現在ソフト作者様に配布している利用キーではこのエラーは発生しません。";
            }
            else if (returnCode == -303)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]利用キーが設定されていない（利用キーが空値）。RC={returnCode}";
                DebugCauseAndTreatment = "利用キーを設定していないと思われます。JV-Link インストール直後は利用キーが空なので必ず設定する必要があります。";
            }
            else if (returnCode == -305)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]利用規約に同意していない。RC={returnCode}";
                DebugCauseAndTreatment = "利用規約に同意していないため、JVOpen 処理で蓄積系データを取得することができません。利用規約同意画面にて利用規約を一読し同意してください。";
            }
            else if (returnCode == -401)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]JV-Link 内部エラー。RC={returnCode}";
                DebugCauseAndTreatment = "JV-Link 内部でエラーが発生したと思われます。JRA-VAN へご連絡ください。";
            }
            else if (returnCode == -411)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]サーバー エラー（HTTP ステータス 404 Not Found）。RC={returnCode}";
                DebugCauseAndTreatment = "レジストリが直接変更されたか、DataLab. 用サーバーに問題が発生したと思われます。JRA-VAN のメンテナンス中でない場合で、このエラーが続く場合は JRA-VAN へご連絡ください。";
            }
            else if (returnCode == -412)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]サーバー エラー（HTTP ステータス 403 Forbidden）。RC={returnCode}";
                DebugCauseAndTreatment = "DataLab. 用サーバーに問題が発生したと思われます。このエラーが続く場合は JRA-VAN へご連絡ください。";
            }
            else if (returnCode == -413)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]サーバー エラー（HTTP ステータス 200, 403, 404 以外）。RC={returnCode}";
                DebugCauseAndTreatment = "DataLab. 用サーバーに問題が発生したと思われます。このエラーが続く場合は JRA-VAN へご連絡ください。";
            }
            else if (returnCode == -421)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]サーバー エラー（サーバーの応答が不正）。RC={returnCode}";
                DebugCauseAndTreatment = "DataLab. 用サーバーに問題が発生したと思われます。このエラーが続く場合は JRA-VAN へご連絡ください。";
            }
            else if (returnCode == -431)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]サーバー エラー（サーバー アプリケーション内部エラー）。RC={returnCode}";
                DebugCauseAndTreatment = "DataLab. 用サーバーに問題が発生したと思われます。このエラーが続く場合は JRA-VAN へご連絡ください。";
            }
            else if (returnCode == -501)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]セットアップ処理においてスタート キット（CD/DVD-ROM）が無効。RC={returnCode}";
                DebugCauseAndTreatment = "JRA-VAN が提供した正しいスタート キット（CD/DVD-ROM）をセットしていないと思われます。正しいスタートキット（CD/DVD-ROM）をセットしてください。スタートキット（CD/DVD-ROM）の提供は 2022 年 3 月をもちまして終了いたしました。";
            }
            else if (returnCode == -504)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVOpen)}]サーバー メンテナンス中。RC={returnCode}";
                DebugCauseAndTreatment = "サーバーがメンテナンス中です。";
            }
            else
            {
                SetUnknownReturnCode(nameof(JVLink.JVOpen), returnCode);
            }
            CoerceIsEmpty();
        }

        public void SetJVLink(IJVLink jVLink)
        {
            _jVLink = jVLink;
        }

        public void SetDataSpec(JVDataSpec dataSpec)
        {
            DataSpec = dataSpec;
        }

        public void SetOpenOption(JVOpenOptions openOption)
        {
            OpenOption = openOption;
        }

        public void SetReadCount(int readCount)
        {
            ReadCount = readCount;
            CoerceIsEmpty();
        }

        public void SetDownloadCount(int downloadCount)
        {
            DownloadCount = downloadCount;
            CoerceNeedsDownload();
        }

        public void SetLastFileTimestamp(string lastFileTimestamp)
        {
            LastFileTimestamp = string.IsNullOrEmpty(lastFileTimestamp) ?
                                    null :
                                    (DateTime?)DateTime.ParseExact(lastFileTimestamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        }

        private void CoerceIsEmpty()
        {
            IsEmpty = ReturnCode == -1 || ReadCount == 0;
        }

        private void CoerceNeedsDownload()
        {
            NeedsDownload = 0 < DownloadCount;
        }

        private IJVLink _jVLink;
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _jVLink?.JVClose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public static JVOpenResult New(int returnCode, JVDataSpec dataSpec, JVOpenOptions openOption, int readCount = 0, int downloadCount = 0)
        {
            var openRslt = new JVOpenResult();
            openRslt.SetReturnCode(returnCode);
            openRslt.SetDataSpec(dataSpec);
            openRslt.SetOpenOption(openOption);
            openRslt.SetReadCount(readCount);
            openRslt.SetDownloadCount(downloadCount);
            return openRslt;
        }
    }
}
