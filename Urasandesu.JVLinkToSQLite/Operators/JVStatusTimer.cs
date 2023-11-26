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
using System.Threading;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite.Operators
{
    internal class JVStatusTimer : IDisposable
    {
        public class Factory
        {
            private readonly IJVServiceOperationListener _listener;
            private readonly IJVLinkService _jvLinkSrv;

            public Factory(IJVServiceOperationListener listener, IJVLinkService jvLinkSrv)
            {
                _listener = listener;
                _jvLinkSrv = jvLinkSrv;
            }

            public virtual JVStatusTimer New(JVOpenResult openRslt)
            {
                return new JVStatusTimer(_listener, _jvLinkSrv, openRslt);
            }
        }

        private const int TimerPeriod = 1000;

        private readonly IJVServiceOperationListener _listener;
        private readonly IJVLinkService _jvLinkSrv;
        private readonly ManualResetEventSlim _mre;
        private readonly Timer _timer;

        private JVStatusResult _lastStatusRslt;

        public JVStatusTimer(IJVServiceOperationListener listener, IJVLinkService jvLinkSrv, JVOpenResult openRslt)
        {
            _listener = listener;
            _jvLinkSrv = jvLinkSrv;
            _mre = new ManualResetEventSlim(false);
            var state = Tuple.Create(openRslt, _mre);
            _timer = new Timer(CheckStatusTimerCallback, state, Timeout.Infinite, TimerPeriod);
        }

        public virtual JVStatusResult StartAndWait()
        {
            _timer.Change(0, TimerPeriod);
            _mre.Wait();
            return _lastStatusRslt;
        }

        private static string MessageForCheckStatusTimerCallback(object[] args) => $"ダウンロード中．．．{args[0]} / {args[1]}";

        private void CheckStatusTimerCallback(object state)
        {
            var (openRslt, mre) = (Tuple<JVOpenResult, ManualResetEventSlim>)state;
            var statusRslt = _jvLinkSrv.JVStatus(openRslt);
            _lastStatusRslt = statusRslt;

            if (statusRslt.Interpretation.Failed)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                mre.Set();
            }
            else
            {
                Info(_listener, this, MessageForCheckStatusTimerCallback, statusRslt.Count, statusRslt.DownloadCount);
                if (statusRslt.DownloadCompleted)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    mre.Set();
                }
            }
        }

        private bool disposedValue;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_timer")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_mre")]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _mre?.Dispose();
                    _timer?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
