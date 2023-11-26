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
using JVDTLabLib;
using Mono.Cecil;
using ObfuscatedResources;
using System;
using System.Reflection;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Security.Cryptography;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers;
using Urasandesu.JVLinkToSQLite.JVLinkWrappers.DataBridges;
using Urasandesu.JVLinkToSQLite.OperatorAggregates;
using Urasandesu.JVLinkToSQLite.Operators;

namespace Urasandesu.JVLinkToSQLite
{
    public static class JVLinkToSQLiteDependency
    {
        private static class Dummy
        {
            // T4 自動生成のため、Mono.Cecil への強参照を保持しておく。
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            public static readonly Type _ = typeof(AssemblyDefinition);
        }

        public static Container NewDefaultContainer(IJVServiceOperationListener listener)
        {
            var container = new Container();
            container.Register<IEncryptor, RSAPublicKeyEncryptor>(reuse: Reuse.Singleton);

            container.RegisterInstance<IJVServiceOperationListener>(listener);
            container.RegisterInstance<JVLink>(new JVLink());
            container.RegisterInstance(new SIDBuilder(AuthorId.Value, SoftwareId.Value, Assembly.GetEntryAssembly()));
            container.Register<IJVLinkService, JVLinkService>(reuse: Reuse.Singleton);
            container.Register<IXmlSerializationService, XmlSerializationService>(reuse: Reuse.Singleton);

            container.Register<JVLinkToSQLiteBootstrap>();

            container.Register<JVNullOperatorAggregate>();
            container.Register<JVPastDataOperatorAggregate.Factory>();
            container.Register<JVImmediateDataOperatorAggregate.Factory>();
            container.Register<JVEventDataOperatorAggregate.Factory>();

            container.Register<JVNullServiceOperator>();
            container.Register<JVPastDataServiceOperator.Factory>();
            container.Register<JVRealTimeDataServiceOperator.Factory>();

            container.Register<JVStatusTimer.Factory>();
            container.Register<JVDataToSQLiteOperator.Factory>();
            container.Register<JVOpenResultReader.Factory>();
            container.Register<JVDataFileSkippabilityHandler.Factory>();
            container.Register<DataBridgeFactory.Factory>();

            return container;
        }
    }
}
