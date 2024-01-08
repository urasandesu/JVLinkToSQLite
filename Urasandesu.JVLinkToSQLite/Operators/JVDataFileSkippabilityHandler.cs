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
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data.Common;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite.Operators
{
    internal class JVDataFileSkippabilityHandler
    {
        public class Factory
        {
            private readonly IJVServiceOperationListener _listener;

            public Factory(IJVServiceOperationListener listener)
            {
                _listener = listener;
            }

            public virtual JVDataFileSkippabilityHandler New(SQLiteCommand command, JVRecordSpec[] excludedRecordSpecs)
            {
                return new JVDataFileSkippabilityHandler(_listener, command, excludedRecordSpecs);
            }
        }

        private const int True = 1;
        private const int False = 0;
        public static readonly string TableName = "SY_PROC_FILES";
        public static readonly string CreateTableSql = $"create table if not exists {TableName}( \r\n"
                                                       + "  -- 処理済みファイル情報 \r\n"
                                                       + "  --   1.   . ファイル名:処理したデータ ファイル名（*.jvd ファイル）。 \r\n"
                                                       + "  --   2.   . データ種別 ID: 処理したデータ ファイルに含まれるであろうデータ種別 ID。 \r\n"
                                                       + "  --   3.   . レコード種別 ID: データ ファイルのレコード種別 ID。 \r\n"
                                                       + "  --   4.   . 区分 ID: データ ファイルの区分 ID。 \r\n"
                                                       + "  --   5.   . 保存 ID: データ ファイルの保存 ID。 \r\n"
                                                       + "  --   6.   . 日時キー: データ ファイルの日時キー。 \r\n"
                                                       + "  --   7.   . 公開日時: データ ファイルの公開日時。 \r\n"
                                                       + "  --   8.   . 処理日時: データ ファイルを処理した日時。 \r\n"
                                                       + "  --   9.   . 有効期限: 処理済みファイル情報の有効期限日時。 \r\n"
                                                       + "  FileName text primary key, \r\n"
                                                       + "  DataSpec text, \r\n"
                                                       + "  RecordSpec text, \r\n"
                                                       + "  CategorySpec text, \r\n"
                                                       + "  SaveSpec text, \r\n"
                                                       + "  DatetimeKey text, \r\n"
                                                       + "  PublishedDateTime datetime, \r\n"
                                                       + "  ProcessedAt datetime, \r\n"
                                                       + "  ExpirationDate datetime \r\n"
                                                       + ") ";
        public static readonly string InsertRecordSql = "insert "
                                                        + $"into {TableName}( "
                                                        + "  FileName, "
                                                        + "  DataSpec, "
                                                        + "  RecordSpec, "
                                                        + "  CategorySpec, "
                                                        + "  SaveSpec, "
                                                        + "  DatetimeKey, "
                                                        + "  PublishedDateTime, "
                                                        + "  ProcessedAt, "
                                                        + "  ExpirationDate "
                                                        + ") "
                                                        + "values ( "
                                                        + "  @FileName, "
                                                        + "  @DataSpec, "
                                                        + "  @RecordSpec, "
                                                        + "  @CategorySpec, "
                                                        + "  @SaveSpec, "
                                                        + "  @DatetimeKey, "
                                                        + "  @PublishedDateTime, "
                                                        + "  @ProcessedAt, "
                                                        + "  @ExpirationDate "
                                                        + ") "
                                                        + "on conflict ( "
                                                        + "  FileName"
                                                        + ") "
                                                        + "do update set "
                                                        + "  DataSpec=@DataSpec, "
                                                        + "  RecordSpec=@RecordSpec, "
                                                        + "  CategorySpec=@CategorySpec, "
                                                        + "  SaveSpec=@SaveSpec, "
                                                        + "  DatetimeKey=@DatetimeKey, "
                                                        + "  PublishedDateTime=@PublishedDateTime, "
                                                        + "  ProcessedAt=@ProcessedAt, "
                                                        + "  ExpirationDate=@ExpirationDate ";
        public static readonly string SelectCountSql = $"select count(*) from {TableName} where FileName = @FileName";
        public static readonly string DeleteExpiredSql = $"delete from {TableName} where ExpirationDate < @now";
        private readonly IJVServiceOperationListener _listener;
        private readonly SQLiteCommand _command;
        private readonly HashSet<JVRecordSpec> _excludedRecordSpecSet;

        public JVDataFileSkippabilityHandler(IJVServiceOperationListener listener, SQLiteCommand command, JVRecordSpec[] excludedRecordSpecs)
        {
            _listener = listener;
            _command = (SQLiteCommand)command?.Clone();
            _excludedRecordSpecSet = excludedRecordSpecs == null ? null : new HashSet<JVRecordSpec>(excludedRecordSpecs);
        }

        public virtual void Initialize(DateTime now, int[] initialized)
        {
            if (_command == null)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref initialized[0], True, False) != False)
            {
                return;
            }

            _command.CommandText = CreateTableSql;
            Verbose(_listener, this, args => $"テーブル作成：{((DbCommand)args[0]).GetLoggingQuery()}", _command);
            _command.ExecuteNonQuery();

            _command.CommandText = DeleteExpiredSql;
            _command.Parameters.Clear();
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@now", now));
            Verbose(_listener, this, args => $"レコード削除：{((DbCommand)args[0]).GetLoggingQuery()}", _command);
            _command.ExecuteNonQuery();
        }

        public virtual bool CanSkip(JVReadResult readRslt)
        {
            if (_command == null)
            {
                return false;
            }

            if (_excludedRecordSpecSet?.Contains(readRslt.GetRecordSpec()) == true)
            {
                return true;
            }

            _command.CommandText = SelectCountSql;
            _command.Parameters.Clear();
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@FileName", readRslt.FileName));
            Verbose(_listener, this, args => $"レコード選択：{((DbCommand)args[0]).GetLoggingQuery()}", _command);
            var count = (long)_command.ExecuteScalar();
            Verbose(_listener, this, args => $"レコード選択結果：{args[0]}", count);
            return count > 0;
        }

        public virtual void RegisterOrUpdate(DateTime now, JVReadResult readRslt)
        {
            if (_command == null)
            {
                return;
            }

            var dataFile = readRslt.GetDataFile();
            if (dataFile == null)
            {
                return;
            }

            _command.CommandText = InsertRecordSql;
            _command.Parameters.Clear();
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@FileName", readRslt.FileName));
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@DataSpec", readRslt.DataSpec.Value));
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@RecordSpec", dataFile.DataFileSpec.RecordSpec.Value));
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@CategorySpec", dataFile.DataFileSpec.CategorySpec.Value));
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@SaveSpec", dataFile.DataFileSpec.SaveSpec.Value));
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@DatetimeKey", dataFile.DatetimeKey));
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@PublishedDateTime", dataFile.PublishedDateTime));
            _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@ProcessedAt", now));
            if (dataFile.DataFileSpec.ProvidedDuration != TimeSpan.MaxValue)
            {
                var expirationDate = now.Add(TimeSpan.FromTicks(dataFile.DataFileSpec.ProvidedDuration.Ticks * 2));
                _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@ExpirationDate", expirationDate));
            }
            else
            {
                _command.Parameters.Insert(_command.Parameters.Count, new SQLiteParameter("@ExpirationDate", null));
            }

            Verbose(_listener, this, args => $"レコード作成：{((DbCommand)args[0]).GetLoggingQuery()}", _command);
            _command.ExecuteNonQuery();
        }
    }
}
