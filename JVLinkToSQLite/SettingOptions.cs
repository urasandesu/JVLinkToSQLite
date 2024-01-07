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

namespace JVLinkToSQLite
{
    [Verb("setting", HelpText = "動作設定処理")]
    internal class SettingOptions : Options
    {
        [Option('s', "setting", Default = @"setting.xml", HelpText =
            "動作設定。設定を変更する XML ファイルのパスを指定します。")]
        public string Setting { get; set; }

        [Option('x', "xpath", Required = true, HelpText =
            "変更先を指定する XPath。対象のノードが複数存在する場合は、全て同じ値に書き換えられます。")]
        public string XPath { get; set; }

        [Option('v', "value", Required = true, HelpText =
            "変更後の値。")]
        public string Value { get; set; }

        [Option('f', "force", Default = false, HelpText =
            "変更時の確認メッセージを表示せず、強制的に実行するかどうか。")]
        public bool Force { get; set; }
    }
}
