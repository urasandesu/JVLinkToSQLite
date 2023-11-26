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
using System.Runtime.Serialization;

namespace Test.Urasandesu.JVLinkToSQLite
{
    public static class AppDomainMixin
    {
        public static void RunAtIsolatedDomain(this AppDomain source, Action action, out AppDomain domain)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!action.CanCrossDomain())
            {
                throw new ArgumentException("パラメータは AppDomain を超えられる必要があります。以下のポイントを確認してみてください：" +
                                            "ラムダ式は外部の変数をキャプチャしていないですか？" +
                                            "もしくは、静的メソッドですか？" +
                                            "もしくは、シリアライズ可能/マーシャリング可能なオブジェクトのインスタンス メソッドですか？", nameof(action));
            }

            var securityInfo = source.Evidence;
            var info = source.SetupInformation;
            try
            {
                domain = AppDomain.CreateDomain("Domain " + action.Method, securityInfo, info);
                var type = typeof(MarshalByRefRunner);
                var runner = (MarshalByRefRunner)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                runner.Action = action;
                runner.Run();
            }
            catch (SerializationException e)
            {
                throw new ArgumentException("パラメータは AppDomain を超えられる必要があります。対象の型が MarshalByRefObject を継承しているか、" +
                                            "もしくは SerializableAttribute が適用されているかを確認してください。", e);
            }
        }
    }
}
