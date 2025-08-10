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
using Urasandesu.JVLinkToSQLite;

namespace JVLinkToSQLite
{
    internal class ConsoleListener : IJVServiceOperationListener
    {
        public ConsoleListener(ListeningLevels listeningLevel)
        {
            ListeningLevel = listeningLevel;
        }

        public ListeningLevels ListeningLevel { get; private set; }

        public void OnErrorNotified(object sender, JVServiceOperationEventArgs e)
        {
            if (e is JVServiceMessageEventArgs me)
            {
                MyConsole.WriteLine($"[ERROR]{me.Message}");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void OnWarningNotified(object sender, JVServiceOperationEventArgs e)
        {
            if (e is JVServiceMessageEventArgs me)
            {
                MyConsole.WriteLine($"[WARNING]{me.Message}");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void OnInfoNotified(object sender, JVServiceOperationEventArgs e)
        {
            if (e is JVServiceMessageEventArgs me)
            {
                MyConsole.WriteLine($"[INFO]{me.Message}");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void OnVerboseNotified(object sender, JVServiceOperationEventArgs e)
        {
            if (e is JVServiceMessageEventArgs me)
            {
                MyConsole.WriteLine($"[VERBOSE]{me.Message}");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void OnDebugNotified(object sender, JVServiceOperationEventArgs e)
        {
            if (e is JVServiceMessageEventArgs me)
            {
                MyConsole.WriteLine($"[DEBUG]{me.Message}");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void OnExternalNotificationRequired(object sender, JVServiceOperationEventArgs e)
        {
            if (e is JVServiceMessageEventArgs me)
            {
                MyConsole.WriteLine($"[INFO]{me.Message}");
                MyConsole.WriteLine($"[INFO]完了するには何かキーを押してください．．．");
                MyConsole.ReadKey(e.OperationCancelToken);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
