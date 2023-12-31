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

using CommandLine;
using Urasandesu.JVLinkToSQLite;

namespace JVLinkToSQLite
{
    internal class Options
    {
        [Option('m', "mode", HelpText =
            "モードの設定。以下のパターンを指定可能です：\n" +
            "* " + nameof(Modes.Exec) + "\n" +
            "* " + nameof(Modes.Event) + "\n" +
            "* " + nameof(Modes.Init) + "\n" +
            "* " + nameof(Modes.About) + "\n" +
            "* " + nameof(Modes.DefaultSetting))]
        public Modes Mode { get; set; }

        [Option('d', "datasource", Default = @"race.db", HelpText =
            "データ ソース。SQLite ファイルのパスを指定します。")]
        public string DataSource { get; set; }

        [Option('t', "throttlesize", Default = 100, HelpText =
            "スロットルサイズ。JVLinkToSQLiteは、JV-Dataのレコード読み取りと " + 
            "SQLiteへの書き込みを非同期で行いますが、このパラメータはSQLite " + 
            "へ書き込むまでに、JV-Dataを何レコード分遅らせるかを指定します。 " +
            "書き込みを遅らせないほうがスループットもメモリ効率も良いのですが、 " + 
            "開発中に何度か、サーバーへの単位時間当たりのアクセス頻度オーバーが " + 
            "原因と考えられる、JV-Link側の不規則なエラーが発生したため、処理を " + 
            "遅らせるようにしました。もし動作設定を変えていないのに不規則な " + 
            "エラーが発生するようでしたら、この値を増やしてみてください。")]
        public int ThrottleSize { get; set; }

        [Option('s', "setting", Default = @"setting.xml", HelpText =
            "動作設定。JVLinkToSQLiteSetting クラスのインスタンスの情報を記載した XML ファイルのパスを指定します。")]
        public string Setting { get; set; }

        [Option('l', "loglevel", Default = LogLevels.Info, HelpText =
            "ログ レベルの設定。以下のパターンを指定可能です：\n" +
            "* " + nameof(LogLevels.Error) + "\n" +
            "* " + nameof(LogLevels.Warning) + "\n" +
            "* " + nameof(LogLevels.Info) + "\n" +
            "* " + nameof(LogLevels.Verbose) + "\n" +
            "* " + nameof(LogLevels.Debug))]
        public LogLevels LogLevel { get; set; }

        [Option('u', "skipslastmodifiedupdate", Default = false, HelpText =
            "最新読み出し開始ポイント日時の更新をスキップするかどうかを指定します。")]
        public bool SkipsLastModifiedUpdate { get; set; }

        public JVLinkToSQLiteBootstrap.LoadSettingParameter ToLoadSettingParameter()
        {
            return new JVLinkToSQLiteBootstrap.LoadSettingParameter
            {
                SettingXmlPath = Setting,
                SQLiteDataSource = DataSource,
                SQLiteThrottleSize = ThrottleSize
            };
        }
    }
}
