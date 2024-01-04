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
using System.Diagnostics;
using System.Reflection;
using Urasandesu.JVLinkToSQLite;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace JVLinkToSQLite
{
    internal abstract class OptionsHandler<TOptions> where TOptions : Options
    {
        protected static string MessageForArgument(object[] args) => $"Argument {args[0]}: {args[1]}";
        protected static string MessageForException(object[] args) => $"例外が発生しました。例外：{args[0]}";

        public int Main(TOptions options)
        {
            if (TryProcessByUninvocableOption(options, out var returnCode))
            {
                return returnCode;
            }

            ShowShortNotice();

            var listener = new ConsoleListener((ListeningLevels)options.LogLevel);
            InfoStart(listener, typeof(Program), args => "JVLinkToSQLite");
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var ret = ProcessByInvocableOption(listener, options);
                InfoEnd(listener, typeof(Program), args => $"JVLinkToSQLite {args[0]}s", stopwatch.Elapsed.TotalSeconds);
                return ret;
            }
            catch (Exception ex)
            {
                Error(listener, typeof(Program), MessageForException, ex);
                return (int)ReturnCodes.Error;
            }
        }

        protected virtual bool TryProcessByUninvocableOption(TOptions options, out int returnCode)
        {
            returnCode = (int)ReturnCodes.Success;
            return false;
        }

        protected abstract int ProcessByInvocableOption(IJVServiceOperationListener listener, TOptions options);

        private static void ShowShortNotice()
        {
            Console.WriteLine($@"
JVLinkToSQLite バージョン {Assembly.GetEntryAssembly().GetName().Version}, Copyright (C) 2023 Akira Sugiura
JVLinkToSQLite は **全くの無保証** で提供されます。また、これはフリー ソフトウェアであり、ある条件の下で再頒布することが奨励されています。詳しくは「JVLinkToSQLite -m about」とタイプして下さい。
");
        }
    }
}
