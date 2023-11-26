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
using System.Windows.Forms;
using Urasandesu.JVLinkToSQLite;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace JVLinkToSQLite
{
    internal partial class SetupForm : Form
    {
        private readonly IJVServiceOperationListener _listener;
        private readonly IJVLinkService _jvLinkSrv;

        public SetupForm(IJVServiceOperationListener listener, IJVLinkService jvLinkSrv)
        {
            InitializeComponent();
            _listener = listener;
            _jvLinkSrv = jvLinkSrv;
        }

        static string MessageForJVSetUIPropertiesError(params object[] args) =>
            $"JV-Link の設定変更でエラーが発生しました。エラー：{args[0]}";

        private void buttonSetUIProperties_Click(object sender, EventArgs e)
        {
            InfoStart(_listener, this, args => "設定ボタン Click");

            var setUIPropsRslt = _jvLinkSrv.JVSetUIProperties();
            if (setUIPropsRslt.Interpretation.Failed)
            {
                Warning(_listener,
                        this,
                        MessageForJVSetUIPropertiesError,
                        setUIPropsRslt.DebugMessage);
                MessageBox.Show(MessageForJVSetUIPropertiesError(setUIPropsRslt.DebugMessage),
                                "初期化モード",
                                MessageBoxButtons.OK);
            }

            InfoEnd(_listener, this, args => "設定ボタン Click");
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            Info(_listener, this, args => "初期化モード画面 Load");
        }

        private void SetupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Info(_listener, this, args => "初期化モード画面 Closed");
        }
    }
}
