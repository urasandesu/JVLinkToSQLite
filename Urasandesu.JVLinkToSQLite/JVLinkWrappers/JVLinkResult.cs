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
    public abstract class JVLinkResult : MarshalByRefObject, IEquatable<JVLinkResult>
    {
        public IReadOnlyList<object> Arguments { get; protected set; }
        public JVResultInterpretation Interpretation { get; protected set; }
        public int ReturnCode { get; private set; }
        public string DebugMessage { get; protected set; }
        public string DebugCauseAndTreatment { get; protected set; }

        protected void SetUnknownReturnCode(string methodName, int returnCode)
        {
            Interpretation = JVResultInterpretation.Error;
            DebugMessage = $"[{methodName}]不明な戻り値。RC={returnCode}";
            DebugCauseAndTreatment = "JV-Link の仕様変更に JVLinkToSQLite が追従できていないかもしれません。バージョンアップの有無をご確認ください。";
        }

        public void SetArguments(params object[] args)
        {
            Arguments = args;
        }

        public void SetReturnCode(int returnCode)
        {
            ReturnCode = returnCode;
            SetReturnCodeCore(returnCode);
        }

        protected virtual void SetReturnCodeCore(int returnCode)
        {
            throw new NotImplementedException($"'{GetType()}' では未実装です。");
        }

        public override string ToString()
        {
            return $"{nameof(Interpretation)}={Interpretation.Value}, {nameof(ReturnCode)}={ReturnCode}, {nameof(DebugMessage)}={DebugMessage}, {nameof(DebugCauseAndTreatment)}={DebugCauseAndTreatment}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as JVLinkResult);
        }

        public bool Equals(JVLinkResult other)
        {
            return !(other is null) &&
                   EqualityComparer<IReadOnlyList<object>>.Default.Equals(Arguments, other.Arguments) &&
                   Interpretation.Equals(other.Interpretation) &&
                   ReturnCode == other.ReturnCode &&
                   DebugMessage == other.DebugMessage &&
                   DebugCauseAndTreatment == other.DebugCauseAndTreatment;
        }

        public override int GetHashCode()
        {
            int hashCode = -264337553;
            hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyList<object>>.Default.GetHashCode(Arguments);
            hashCode = hashCode * -1521134295 + Interpretation.GetHashCode();
            hashCode = hashCode * -1521134295 + ReturnCode.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DebugMessage);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DebugCauseAndTreatment);
            return hashCode;
        }

        public static bool operator ==(JVLinkResult left, JVLinkResult right)
        {
            return EqualityComparer<JVLinkResult>.Default.Equals(left, right);
        }

        public static bool operator !=(JVLinkResult left, JVLinkResult right)
        {
            return !(left == right);
        }
    }
}
