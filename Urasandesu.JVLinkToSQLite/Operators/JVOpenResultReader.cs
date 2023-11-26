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
using System.Collections.Generic;
using System.Diagnostics;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite.Operators
{
    internal class JVOpenResultReader : IEnumerable<JVReadResult>
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

            public virtual JVOpenResultReader New(JVOpenResult openRslt, JVDataFileSkippabilityHandler jvdfSkippabilityHandler)
            {
                return new JVOpenResultReader(_listener, _jvLinkSrv, openRslt, jvdfSkippabilityHandler);
            }
        }

        private readonly IJVServiceOperationListener _listener;
        private readonly IJVLinkService _jvLinkSrv;
        private readonly JVOpenResult _openRslt;
        private readonly JVDataFileSkippabilityHandler _jvdfSkippabilityHandler;
        private static readonly int[] s_initialized = new int[1];

        public JVOpenResultReader(IJVServiceOperationListener listener,
                                  IJVLinkService jvLinkSrv,
                                  JVOpenResult openRslt,
                                  JVDataFileSkippabilityHandler jvdfSkippabilityHandler)
        {
            _listener = listener;
            _jvLinkSrv = jvLinkSrv;
            _openRslt = openRslt;
            _jvdfSkippabilityHandler = jvdfSkippabilityHandler;
        }

        private class Enumerator : IEnumerator<JVReadResult>
        {
            private readonly JVOpenResultReader _this;
            private readonly HashSet<string> _processedJVFileNameSet;
            private Stopwatch _stopwatch;
            private int _recordCount;
            private TimeSpan _infoElapsed;

            public Enumerator(JVOpenResultReader @this)
            {
                _this = @this;
                _processedJVFileNameSet = new HashSet<string>();
            }

            public JVReadResult Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                // nop
            }

            public bool MoveNext()
            {
                Current = _this._jvLinkSrv.JVGets(_this._openRslt);
                if (Current.Status == JVReadStatus.RecordsExist)
                {
                    if (!_processedJVFileNameSet.Contains(Current.FileName))
                    {
                        _stopwatch = Stopwatch.StartNew();
                        _infoElapsed = _stopwatch.Elapsed;
                        _recordCount = 0;
                        InfoStart(_this._listener,
                                  this,
                                  args => $"JV-Data ファイル '{args[0]}' の読み込み．．．{(int)args[1] + 1} / {args[2]}",
                                  Current.FileName,
                                  _processedJVFileNameSet.Count,
                                  _this._openRslt.ReadCount);
                        _processedJVFileNameSet.Add(Current.FileName);

                        if (_this._jvdfSkippabilityHandler.CanSkip(Current))
                        {
                            _this._jvLinkSrv.JVSkip(_this._openRslt);
                            Current.SetReturnCode(-1);

                            InfoEnd(_this._listener,
                                    this,
                                    args => $"JV-Data ファイル '{args[0]}' の読み込み．．．スキップ",
                                    Current.FileName);
                            return true;
                        }
                    }

                    _recordCount++;
                    if (_stopwatch.Elapsed - _infoElapsed > TimeSpan.FromSeconds(1))
                    {
                        Info(_this._listener,
                             this,
                             args => $"JV-Data ファイル '{args[0]}' の読み込み中．．．{args[1]} レコード",
                             Current.FileName,
                             _recordCount);
                        _infoElapsed = _stopwatch.Elapsed;
                    }

                    return true;
                }
                else if (Current.Status == JVReadStatus.FileChanged)
                {
                    _this._jvdfSkippabilityHandler.RegisterOrUpdate(DateTime.Now, Current);

                    InfoEnd(_this._listener,
                            this,
                            args => $"JV-Data ファイル '{args[0]}' の読み込み。{args[1]} レコード、{args[2]}ms",
                            Current.FileName,
                            _recordCount,
                            _stopwatch.ElapsedMilliseconds);
                    return true;
                }
                else if (Current.Status == JVReadStatus.ReadExit)
                {
                    return false;
                }
                else
                {
                    throw new JVLinkException(Current);
                }
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        public virtual IEnumerator<JVReadResult> GetEnumerator()
        {
            _jvdfSkippabilityHandler.Initialize(DateTime.Now, s_initialized);
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
