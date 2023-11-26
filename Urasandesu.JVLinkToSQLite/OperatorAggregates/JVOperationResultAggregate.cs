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

using System.Collections.Generic;
using System.Linq;
using Urasandesu.JVLinkToSQLite.Operators;

namespace Urasandesu.JVLinkToSQLite.OperatorAggregates
{
    public class JVOperationResultAggregate
    {
        public IReadOnlyList<JVLinkServiceOperationResult> OperationResults { get; }
        public bool AreAllSucceeded { get; }
        public string FirstDebugMessage { get; }
        public int FirstReturnCode { get; }
        public IReadOnlyList<object> FirstArguments { get; }
        public string FirstCallerFilePath { get; }
        public int FirstCallerLineNumber { get; }

        public JVOperationResultAggregate(JVLinkServiceOperationResult[] oprRslts)
        {
            OperationResults = oprRslts;
            if (oprRslts != null)
            {
                var firstErrorResult = oprRslts.FirstOrDefault(_ => _.Interpretation.Failed);

                AreAllSucceeded = firstErrorResult == null;
                if (firstErrorResult != null)
                {
                    FirstReturnCode = firstErrorResult.ReturnCode;
                    FirstDebugMessage = firstErrorResult.DebugMessage;
                    FirstArguments = firstErrorResult.Arguments;
                    FirstCallerFilePath = firstErrorResult.CallerFilePath;
                    FirstCallerLineNumber = firstErrorResult.CallerLineNumber;
                }
            }
        }
    }
}
