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

using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using SR = System.Reflection;

namespace Urasandesu.JVLinkToSQLite.Basis.Mixins.Mono.Cecil
{
    public static class ICustomAttributeProviderMixin
    {
        private class PropertyInfoHolder<TAttribute>
        {
            public static readonly Dictionary<string, SR::PropertyInfo> Properties = typeof(TAttribute).GetProperties().ToDictionary(_ => _.Name);
        }

        public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider self) where TAttribute : Attribute, new()
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            var ca = self.CustomAttributes.FirstOrDefault(_ => _.AttributeType.Name == typeof(TAttribute).Name);
            if (ca == null)
            {
                return null;
            }

            var attr = new TAttribute();
            foreach (var cana in ca.Properties)
            {
                var propInfo = PropertyInfoHolder<TAttribute>.Properties[cana.Name];
                propInfo.SetValue(attr, cana.Argument.Value);
            }
            return attr;
        }
    }
}
