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

using DryIoc;
using JVDTLabLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;

namespace Urasandesu.JVLinkToSQLite.JVLinkWrappers
{
    [DebuggerDisplay(nameof(Interpretation) + "={" + nameof(Interpretation) + "." + nameof(JVResultInterpretation.Value) + "}, " +
                     nameof(ReturnCode) + "={" + nameof(ReturnCode) + "}, " +
                     nameof(DataSpec) + "={" + nameof(DataSpec) + "}, " +
                     nameof(Status) + "={" + nameof(Status) + "}")]
    public class JVReadResult : JVLinkResult, IEquatable<JVReadResult>
    {
        public JVDataSpec DataSpec { get; private set; }
        public JVReadStatus Status { get; private set; }
        public string Buffer { get; private set; }
        public int BufferSize { get; private set; }
        public string FileName { get; private set; }
        public JVOpenOptions OpenOption { get; private set; }

        public DataBridgeFactory GetDataBridgeFactory(IResolver resolver)
        {
            return resolver.Resolve<DataBridgeFactory.Factory>().New(this);
        }

        protected override void SetReturnCodeCore(int returnCode)
        {
            Status = JVReadStatus.ReadError;
            if (returnCode > 0)
            {
                Interpretation = JVResultInterpretation.SuccessTrue;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]正常（バッファにセットしたデータのサイズ）。RC={returnCode}";
                DebugCauseAndTreatment = "-";
                Status = JVReadStatus.RecordsExist;
            }
            else if (returnCode == 0)
            {
                Interpretation = JVResultInterpretation.SuccessTrue;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]全ファイル読み込み終了（EOF）。RC={returnCode}";
                DebugCauseAndTreatment = "JVOpen で取得した全てのデータの終わりを示しています。読み込み処理を終了してください。";
                Status = JVReadStatus.ReadExit;
            }
            else if (returnCode == -1)
            {
                Interpretation = JVResultInterpretation.SuccessFalse;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]ファイル切り替わり。RC={returnCode}";
                DebugCauseAndTreatment = "エラーではありません。物理ファイルの終わりを示しています。バッファーにはデータが返されませんのでそのまま読み込み処理を続行してください。";
                Status = JVReadStatus.FileChanged;
            }
            else if (returnCode == -3)
            {
                Interpretation = JVResultInterpretation.SuccessFalse;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]ファイルダウンロード中。RC={returnCode}";
                DebugCauseAndTreatment = "読み出そうとするファイルがダウンロードの最中です。少し待ってから読み込みを再開してください。";
            }
            else if (returnCode == -201)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]JVInit が行なわれていない。RC={returnCode}";
                DebugCauseAndTreatment = "JVRead/JVGets に先立って JVInit/JVOpen が呼ばれていないと思われます。必ず JVInit/JVOpen を先に呼び出してください。";
            }
            else if (returnCode == -202)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]前回の JVOpen/JVRTOpen/JVMVOpen に対して JVClose が呼ばれていない（オープン中）。RC={returnCode}";
                DebugCauseAndTreatment = "前回呼び出した JVOpen/JVRTOpen/JVMVOpen が JVClose によってクローズされていないと思われます。JVOpen/JVRTOpen/JVMVOpen を呼び出した後、別の Open を呼び出す場合、先に JVClose を呼び出してください。";
            }
            else if (returnCode == -203)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]JVOpen が行なわれていない。RC={returnCode}";
                DebugCauseAndTreatment = "JVRead/JVGets に先立って JVOpen が呼ばれていないと思われます。必ず JVOpen を先に呼び出してください。";
            }
            else if (returnCode == -402)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]ダウンロードしたファイルが異常（ファイルサイズ = 0）。RC={returnCode}";
                DebugCauseAndTreatment = "ダウンロード中に何らかの問題が発生しファイルが異常な状態になったと思われます。JVFiledelete で該当ファイル（JVRead/JVGets から戻されたファイル名）を削除し、再度 JVOpen からの処理をやりなおしてください。";
            }
            else if (returnCode == -403)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]ダウンロードしたファイルが異常（データ内容）。RC={returnCode}";
                DebugCauseAndTreatment = "ダウンロード中に何らかの問題が発生しファイルが異常な状態になったと思われます。JVFiledelete で該当ファイル（JVRead/JVGets から戻されたファイル名）を削除し、再度 JVOpen からの処理をやりなおしてください。";
            }
            else if (returnCode == -502)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]ダウンロード失敗（通信エラーやディスクエラーなど）。RC={returnCode}";
                DebugCauseAndTreatment = "ダウンロード処理に失敗しました。エラーの原因を除去しないかぎり解決しないと思われます。原因を除去できたら JVClose を呼び出し、JVOpen からの処理をやりなおしてください。サーバーが混雑している場合のタイムアウトでもこの戻り値が返されることがあります。";
            }
            else if (returnCode == -503)
            {
                Interpretation = JVResultInterpretation.Error;
                DebugMessage = $"[{nameof(JVLink.JVRead)}]ファイルが見つからない。RC={returnCode}";
                DebugCauseAndTreatment = "JVOpen から JVRead/JVGets までの間に読み出すべきファイルが削除された、または該当ファイルが使用中と思われます。JVOpen からやりなおせば解消しますが、削除された原因を除去する必要があります。";
            }
            else
            {
                SetUnknownReturnCode(nameof(JVLink.JVRead), returnCode);
            }
        }

        public void SetDataSpec(JVDataSpec dataSpec)
        {
            DataSpec = dataSpec;
        }

        public void SetOpenOption(JVOpenOptions openOption)
        {
            OpenOption = openOption;
        }

        public void SetBuffer(string buff)
        {
            Buffer = buff;
        }

        public void SetBufferSize(int size)
        {
            BufferSize = size;
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        public static JVReadResult New(int returnCode, string buff = null, string fileName = null)
        {
            var readRslt = new JVReadResult();
            readRslt.SetReturnCode(returnCode);
            readRslt.SetBuffer(buff);
            readRslt.SetFileName(fileName);
            return readRslt;
        }

        public static JVReadResult New(int returnCode, JVOpenResult openRslt, string buff = null, string fileName = null)
        {
            var readRslt = new JVReadResult();
            readRslt.SetReturnCode(returnCode);
            readRslt.SetBuffer(buff);
            readRslt.SetFileName(fileName);
            readRslt.SetDataSpec(openRslt?.DataSpec);
            readRslt.SetOpenOption(openRslt?.OpenOption ?? JVOpenOptions.None);
            return readRslt;
        }

        public JVRecordSpec GetIncompleteRecordSpec()
        {
            return JVRecordSpec.MakeIncomplete(Buffer.Substring(0, 2));
        }

        public JVRecordSpec GetRecordSpec()
        {
            var recordSpecTmp = GetIncompleteRecordSpec();
            if (!DataSpec.CandidateRecordSpecs.TryGetValue(recordSpecTmp, out var recordSpec))
            {
                throw new InvalidOperationException($"DataSpec '{DataSpec}' には、RecordSpec '{recordSpecTmp}' は含まれません。");
            }

            return recordSpec;
        }

        public JVDataFile GetDataFile()
        {
            return DataSpec.GetDataFile(FileName);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as JVReadResult);
        }

        public bool Equals(JVReadResult other)
        {
            return !(other is null) &&
                   base.Equals(other) &&
                   DataSpec == other.DataSpec &&
                   Status == other.Status &&
                   Buffer == other.Buffer &&
                   BufferSize == other.BufferSize &&
                   FileName == other.FileName &&
                   OpenOption == other.OpenOption;
        }

        public override int GetHashCode()
        {
            int hashCode = 1695071225;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<JVDataSpec>.Default.GetHashCode(DataSpec);
            hashCode = hashCode * -1521134295 + Status.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Buffer);
            hashCode = hashCode * -1521134295 + BufferSize.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FileName);
            hashCode = hashCode * -1521134295 + OpenOption.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(JVReadResult left, JVReadResult right)
        {
            return EqualityComparer<JVReadResult>.Default.Equals(left, right);
        }

        public static bool operator !=(JVReadResult left, JVReadResult right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(DataSpec)}={DataSpec}, {nameof(Status)}={Status}, {nameof(Buffer)}={Buffer}, {nameof(BufferSize)}={BufferSize}, {nameof(FileName)}={FileName}, {nameof(OpenOption)}={OpenOption}";
        }
    }
}
