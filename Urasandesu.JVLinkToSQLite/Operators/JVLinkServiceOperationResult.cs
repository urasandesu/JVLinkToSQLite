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
using System.Runtime.CompilerServices;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;

namespace Urasandesu.JVLinkToSQLite.Operators
{
    public class JVLinkServiceOperationResult
    {
        private JVLinkServiceOperationResult(IReadOnlyList<object> arguments,
                                            JVResultInterpretation interpretation,
                                            int returnCode,
                                            string debugMessage,
                                            string debugCauseAndTreatment,
                                            JVLinkResult rslt,
                                            Exception ex,
                                            string filePath,
                                            int lineNumber)
        {
            Arguments = arguments;
            Interpretation = interpretation;
            ReturnCode = returnCode;
            DebugMessage = debugMessage;
            DebugCauseAndTreatment = debugCauseAndTreatment;
            SourceResult = rslt;
            SourceException = ex;
            CallerFilePath = filePath;
            CallerLineNumber = lineNumber;
        }

        public IReadOnlyList<object> Arguments { get; }
        public JVResultInterpretation Interpretation { get; }
        public int ReturnCode { get; }
        public string DebugMessage { get; }
        public string DebugCauseAndTreatment { get; }
        public JVLinkResult SourceResult { get; }
        public Exception SourceException { get; }
        public string CallerFilePath { get; }
        public int CallerLineNumber { get; }

        public static JVLinkServiceOperationResult From(JVLinkResult rslt,
                                                        [CallerFilePath] string filePath = "",
                                                        [CallerLineNumber] int lineNumber = -1)
        {
            if (rslt == null)
            {
                throw new System.ArgumentNullException(nameof(rslt));
            }

            return new JVLinkServiceOperationResult(rslt.Arguments,
                                                    rslt.Interpretation,
                                                    rslt.ReturnCode,
                                                    rslt.DebugMessage,
                                                    rslt.DebugCauseAndTreatment,
                                                    rslt,
                                                    null, 
                                                    filePath,
                                                    lineNumber);
        }

        public static JVLinkServiceOperationResult Success(string methodName,
                                                           [CallerFilePath] string filePath = "",
                                                           [CallerLineNumber] int lineNumber = -1)
        {
            return new JVLinkServiceOperationResult(null,
                                                    JVResultInterpretation.SuccessTrue,
                                                    (int)ReturnCodeRanges.Success,
                                                    $"[{methodName}]正常。RC={(int)ReturnCodeRanges.Success}",
                                                    "-",
                                                    null,
                                                    null, 
                                                    filePath,
                                                    lineNumber);
        }

        public static JVLinkServiceOperationResult Exception(Exception ex,
                                                             [CallerFilePath] string filePath = "",
                                                             [CallerLineNumber] int lineNumber = -1)
        {
            if (ex == null)
            {
                throw new System.ArgumentNullException(nameof(ex));
            }

            return new JVLinkServiceOperationResult(null,
                                                    JVResultInterpretation.Error,
                                                    (int)ReturnCodeRanges.UnkownException,
                                                    ex.Message,
                                                    "-",
                                                    null,
                                                    ex,
                                                    filePath,
                                                    lineNumber);
        }
    }
}
