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
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Collections.Concurrent;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.Settings;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite.Operators
{
    internal class JVDataToSQLiteOperator : IDisposable
    {
        public class Factory
        {
            private readonly IResolver _resolver;
            private readonly IJVServiceOperationListener _listener;

            public Factory(IResolver resolver, IJVServiceOperationListener listener)
            {
                _resolver = resolver;
                _listener = listener;
            }

            public virtual JVDataToSQLiteOperator New(SQLiteConnectionInfo connInfo, JVOpenResult openRslt, JVRecordSpec[] excludedRecordSpecs)
            {
                return new JVDataToSQLiteOperator(_resolver, _listener, connInfo, openRslt, excludedRecordSpecs);
            }
        }

        private readonly IResolver _resolver;
        private readonly IJVServiceOperationListener _listener;
        private readonly SQLiteConnection _conn;
        private readonly int _throttleSize;
        private readonly JVOpenResult _openRslt;
        private readonly JVRecordSpec[] _excludedRecordSpecs;

        public JVDataToSQLiteOperator(IResolver resolver,
                                      IJVServiceOperationListener listener,
                                      SQLiteConnectionInfo connInfo,
                                      JVOpenResult openRslt,
                                      JVRecordSpec[] excludedRecordSpecs)
        {
            _resolver = resolver;
            _listener = listener;

            var connStr = new SQLiteConnectionStringBuilder(@"
PRAGMA MMAP_SIZE = 2147483648;
PRAGMA JOURNAL_MODE = MEMORY;
PRAGMA SYNCHRONOUS = OFF;
PRAGMA LOCKING_MODE = EXCLUSIVE;
PRAGMA ENCODING = ""UTF-8"";
");
            connStr.DataSource = connInfo.DataSource;
            connStr.Version = 3;
            _conn = new SQLiteConnection(connStr.ToString());
            _throttleSize = connInfo.ThrottleSize;
            _openRslt = openRslt;
            _excludedRecordSpecs = excludedRecordSpecs;
        }

        public virtual JVLinkServiceOperationResult InsertOrUpdateAll()
        {
            _conn.Open();
            Command = _conn.CreateCommand();
            var commandCache = Command.NewPreparedCache(_conn.BeginTransaction());
            try
            {
                var handler = _resolver.Resolve<JVDataFileSkippabilityHandler.Factory>().New(Command, _excludedRecordSpecs);
                var reader = _resolver.Resolve<JVOpenResultReader.Factory>().New(_openRslt, handler);
                using (var bgSQLiteWkr = new BackgroundSQLiteWorker(this, commandCache))
                {
                    bgSQLiteWkr.Start();
                    foreach (var readRslt in reader)
                    {
                        bgSQLiteWkr.Enqueue(readRslt);
                    }
                    bgSQLiteWkr.Join();
                }
                return JVLinkServiceOperationResult.Success(nameof(InsertOrUpdateAll));
            }
            catch (JVLinkException ex)
            {
                commandCache.Rollback();
                var oprRslt = JVLinkServiceOperationResult.From(ex.JVLinkResult);
                Warning(_listener,
                        this,
                        args => $"エラーが発生しました。エラー (引数)：{args[0]} ({StringMixin.JoinIfAvailable(", ", args[1])})",
                        oprRslt.DebugMessage,
                        oprRslt.Arguments);
                return oprRslt;
            }
            catch
            {
                commandCache.Rollback();
                throw;
            }
        }

        protected virtual void CreateTable(SQLitePreparedCommandCache commandCache, DataBridge dataBridge)
        {
            var stopwatch = Stopwatch.StartNew();
            DebugStart(_listener, this, args => $"SQLite CreateTable．．．");
            foreach (var builtCommand in dataBridge.BuildUpCreateTableCommand(commandCache))
            {
                Verbose(_listener, this, args => $"テーブル作成：{((SQLitePreparedCommand)args[0]).GetLoggingQuery()}", builtCommand);
                builtCommand.ExecuteNonQuery();
            }
            DebugEnd(_listener, this, args => $"SQLite CreateTable．．． {args[0]}ms", stopwatch.ElapsedMilliseconds);
        }

        protected virtual void Insert(SQLitePreparedCommandCache commandCache, DataBridge dataBridge)
        {
            var stopwatch2 = Stopwatch.StartNew();
            DebugStart(_listener, this, args => $"SQLite Insert．．．");
            foreach (var builtCommand in dataBridge.BuildUpInsertCommand(commandCache))
            {
                Verbose(_listener, this, args => $"レコード作成：{((SQLitePreparedCommand)args[0]).GetLoggingQuery()}", builtCommand);
                builtCommand.ExecuteNonQuery();
            }
            DebugEnd(_listener, this, args => $"SQLite Insert．．． {args[0]}ms", stopwatch2.ElapsedMilliseconds);
        }

        public SQLiteCommand Command { get; private set; }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _conn.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private class BackgroundSQLiteWorker : IDisposable
        {
            private readonly JVDataToSQLiteOperator _this;
            private readonly SQLitePreparedCommandCache _commandCache;
            private readonly HashSet<string> _publishedDdlSet = new HashSet<string>();
            private readonly AsyncAccumulatingQueue<JVReadResult> _queue;
            private Thread _thread;
            private Exception _threadEx;

            public BackgroundSQLiteWorker(JVDataToSQLiteOperator @this, SQLitePreparedCommandCache commandCache)
            {
                _this = @this;
                _queue = new AsyncAccumulatingQueue<JVReadResult>(@this._throttleSize);
                _commandCache = commandCache;
            }

            public void Start()
            {
                _thread = new Thread(() =>
                {
                    try
                    {
                        var stopwatch = Stopwatch.StartNew();
                        var infoElapsed = stopwatch.Elapsed;
                        var recordCount = 0;
                        foreach (var readRslt in _queue)
                        {
                            if (readRslt.Status == JVReadStatus.RecordsExist)
                            {
                                var dataBridgeFactory = readRslt.GetDataBridgeFactory(_this._resolver);
                                var dataBridge = dataBridgeFactory.NewDataBridge();

                                if (!_publishedDdlSet.Contains(dataBridge.TableName))
                                {
                                    InfoStart(_this._listener, this, args => $"SQLite 更新．．．テーブル：{args[0]}", dataBridge.TableName);

                                    _this.CreateTable(_commandCache, dataBridge);

                                    _publishedDdlSet.Add(dataBridge.TableName);
                                }

                                recordCount++;
                                if (stopwatch.Elapsed - infoElapsed > TimeSpan.FromSeconds(1))
                                {
                                    Info(_this._listener, this, args => $"SQLite 更新中．．．{args[0]} レコード", recordCount);
                                    infoElapsed = stopwatch.Elapsed;
                                }
                                _this.Insert(_commandCache, dataBridge);
                            }
                            else if (readRslt.Status == JVReadStatus.FileChanged)
                            {
                                Info(_this._listener, this, args => $"SQLite 更新完了（ファイル '{args[0]}' 分）．．． {args[1]} レコード", readRslt.FileName, recordCount);
                                stopwatch = Stopwatch.StartNew();
                                infoElapsed = stopwatch.Elapsed;
                                recordCount = 0;
                                _commandCache.CommitAndNewTransaction();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _threadEx = ex;
                    }
                });
                _thread.Start();
            }

            public void Enqueue(JVReadResult readRslt)
            {
                _queue.Enqueue(readRslt, _ => _.Status == JVReadStatus.FileChanged);
            }

            public void Join()
            {
                _queue.Join();
                _thread.Join();
                if (_threadEx != null)
                {
                    ExceptionDispatchInfo.Capture(_threadEx).Throw();
                }
                _commandCache.Commit();
                InfoEnd(_this._listener, this, args => $"SQLite 更新．．．テーブル：{StringMixin.JoinIfAvailable(", ", args[0])}", _publishedDdlSet);
            }

            private bool disposedValue;
            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        _queue.Dispose();
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
}
