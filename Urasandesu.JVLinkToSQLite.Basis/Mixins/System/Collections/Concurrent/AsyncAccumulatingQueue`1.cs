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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Collections.Concurrent
{
    public class AsyncAccumulatingQueue<T> : IEnumerable<T>, IEnumerable, IDisposable
    {
        private readonly ManualResetEventSlim _mre = new ManualResetEventSlim(false);
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _enqueueCount;

        public int ThrottleSize { get; private set; }

        public AsyncAccumulatingQueue() :
            this(1000)
        { }

        public AsyncAccumulatingQueue(int throttleSize)
        {
            ThrottleSize = throttleSize;
        }

        public void Enqueue(T item, Predicate<T> orNotifyCondition = null)
        {
            _queue.Enqueue(item);
            if (_enqueueCount++ >= ThrottleSize || orNotifyCondition?.Invoke(item) == true)
            {
                _enqueueCount = 0;
                _mre.Set();
            }
        }

        public void Join()
        {
            _mre.Set();
            _cts.Cancel();
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (true)
            {
                _mre.Wait(500);
                _mre.Reset();

            PROCESS_REST:
                while (_queue.TryDequeue(out var result))
                {
                    yield return result;
                }

                if (_cts.IsCancellationRequested)
                {
                    if (!_queue.IsEmpty)
                    {
                        goto PROCESS_REST;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _mre.Dispose();
                    _cts.Dispose();
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
