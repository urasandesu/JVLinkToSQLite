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

using NUnit.Framework;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data;

namespace Test.Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Data
{
    [TestFixture]
    public class SQLitePreparationIntegrationTest
    {
        [Test]
        public void Sql_should_be_prepared_at_once()
        {
            // Arrange
            using (var conn = new SQLiteConnection("Data Source=:memory:;Flags=LogAll"))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    SQLiteLog.Enabled = true;
                    var actual = new List<string>();
                    SQLiteLog.Log += (sender, e) => actual.Add(e.Message);

                    var tableName = "HOGE";
                    var createTableSql = $"create table {tableName} (Id integer primary key autoincrement, Name1 text, Name2 text, Name3 text)";
                    var insertSql = $"insert into {tableName} (Name1, Name2, Name3) values (@Name1, @Name2, @Name3)";


                    // Act
                    var cmdCache = cmd.NewPreparedCache(conn.BeginTransaction());
                    {
                        var builtCmd = cmdCache.Get(new SQLitePreparedCommandKey(1, tableName), () => createTableSql);
                        builtCmd.ExecuteNonQuery();
                    }
                    for (var i = 0; i < 3; i++)
                    {
                        var builtCmd = cmdCache.Get(new SQLitePreparedCommandKey(2, tableName), () => insertSql);
                        builtCmd.PrepareParameter("@Name1", $"{tableName + i}1");
                        builtCmd.PrepareParameterRange(new[]
                        {
                            new SQLiteParameter("@Name2", $"{tableName + i}2"),
                        });
                        builtCmd.PrepareParameter("@Name3", $"{tableName + i}3");
                        builtCmd.ExecuteNonQuery();
                    }


                    // Assert
                    Assert.That(actual.Where(_ => _.StartsWith($"Preparing {{{insertSql}}}")).Count(), Is.EqualTo(1), string.Join("\r\n", actual));
                }
            }
        }

    }
}
