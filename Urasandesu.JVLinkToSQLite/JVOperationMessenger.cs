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
using System.Linq;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;

namespace Urasandesu.JVLinkToSQLite
{
    public static class JVOperationMessenger
    {
        public static void Error(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            JVServiceOperationNotifier.NotifyError(listener, sender, new JVServiceMessageEventArgs(messaging, @params));
        }

        public static void Warning(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            JVServiceOperationNotifier.NotifyWarning(listener, sender, new JVServiceMessageEventArgs(messaging, @params));
        }

        public static void Info(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            JVServiceOperationNotifier.NotifyInfo(listener, sender, new JVServiceMessageEventArgs(messaging, @params));
        }

        public static void InfoStart(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            Info(listener, sender, args => $"[開始]{messaging(args)}", @params);
        }

        public static void InfoEnd(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            Info(listener, sender, args => $"[終了]{messaging(args)}", @params);
        }

        public static void Verbose(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            JVServiceOperationNotifier.NotifyVerbose(listener, sender, new JVServiceMessageEventArgs(messaging, @params));
        }

        private static string MessageForApiCalling(object[] args) =>
            $"[開始]{args[0]}.{args[1]} 呼び出し。引数： '{StringMixin.JoinSubString("', '", 50, args.Skip(2))}'";
        public static void VerboseApiCalling(IJVServiceOperationListener listener, object sender, string className, string memberName, params object[] args)
        {
            Verbose(listener, sender, MessageForApiCalling, new object[] { className, memberName }.Concat(args).ToArray());
        }

        private static string MessageForApiCalled(object[] args) =>
            $"[終了]{args[0]}.{args[1]} 呼び出し。戻り値： {args[2]}、その他出力： '{StringMixin.JoinSubString("', '", 50, args.Skip(3))}'";
        public static void VerboseApiCalled(IJVServiceOperationListener listener, object sender, string className, string memberName, object returnedValue, params object[] args)
        {
            Verbose(listener, sender, MessageForApiCalled, new object[] { className, memberName, returnedValue }.Concat(args).ToArray());
        }

        public static void Debug(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            JVServiceOperationNotifier.NotifyDebug(listener, sender, new JVServiceMessageEventArgs(messaging, @params));
        }

        public static void DebugStart(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            Debug(listener, sender, args => $"[開始]{messaging(args)}", @params);
        }

        public static void DebugEnd(IJVServiceOperationListener listener, object sender, Func<object[], string> messaging, params object[] @params)
        {
            Debug(listener, sender, args => $"[終了]{messaging(args)}", @params);
        }
    }
}
