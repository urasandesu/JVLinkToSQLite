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

using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using static Urasandesu.JVLinkToSQLite.JVOperationMessenger;

namespace Urasandesu.JVLinkToSQLite
{
    public class XmlSerializationService : IXmlSerializationService
    {
        private readonly IJVServiceOperationListener _listener;

        public XmlSerializationService(IJVServiceOperationListener listener)
        {
            _listener = listener;
        }

        public bool ExistsXmlFile(string xmlPath)
        {
            VerboseApiCalling(_listener, this, nameof(File), nameof(File.Exists), xmlPath);
            var ret = File.Exists(xmlPath);
            VerboseApiCalled(_listener, this, nameof(File), nameof(File.Exists), ret);
            return ret;
        }

        public void DeleteXmlFile(string xmlPath)
        {
            VerboseApiCalling(_listener, this, nameof(File), nameof(File.Delete), xmlPath);
            File.Delete(xmlPath);
            VerboseApiCalled(_listener, this, nameof(File), nameof(File.Exists), typeof(void));
        }

        public TextReader NewDeserializingTextReader(string xmlPath)
        {
            VerboseApiCalling(_listener, this, nameof(StreamReader), ".ctor", xmlPath);
            var sr = new StreamReader(xmlPath);
            VerboseApiCalled(_listener, this, nameof(StreamReader), ".ctor", RuntimeHelpers.GetHashCode(sr));
            return sr;
        }

        public T Deserialize<T>(TextReader tr)
        {
            VerboseApiCalling(_listener, this, nameof(XmlSerializer), nameof(XmlSerializer.Deserialize), RuntimeHelpers.GetHashCode(tr));
            var xs = new XmlSerializer(typeof(T));
            var ret = (T)xs.Deserialize(tr);
            VerboseApiCalled(_listener, this, nameof(XmlSerializer), nameof(XmlSerializer.Deserialize), RuntimeHelpers.GetHashCode(ret));
            return ret;
        }

        public TextWriter NewSerializingTextWriter(string xmlPath)
        {
            VerboseApiCalling(_listener, this, nameof(StreamWriter), ".ctor", xmlPath);
            var sw = new StreamWriter(xmlPath);
            VerboseApiCalled(_listener, this, nameof(StreamWriter), ".ctor", RuntimeHelpers.GetHashCode(sw));
            return sw;
        }

        public void Serialize<T>(TextWriter tw, T obj)
        {
            VerboseApiCalling(_listener, this, nameof(XmlSerializer), nameof(XmlSerializer.Serialize), RuntimeHelpers.GetHashCode(tw), RuntimeHelpers.GetHashCode(obj));
            var xs = new XmlSerializer(typeof(T));
            xs.Serialize(tw, obj);
            VerboseApiCalled(_listener, this, nameof(XmlSerializer), nameof(XmlSerializer.Serialize), typeof(void));
        }

        public string GetTempFileName()
        {
            VerboseApiCalling(_listener, this, nameof(Path), nameof(Path.GetTempFileName));
            var tempFileName = Path.GetTempFileName();
            VerboseApiCalled(_listener, this, nameof(Path), nameof(Path.GetTempFileName), tempFileName);
            return tempFileName;
        }

        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            VerboseApiCalling(_listener, this, nameof(File), nameof(File.Copy), sourceFileName, destFileName, overwrite);
            File.Copy(sourceFileName, destFileName, overwrite);
            VerboseApiCalled(_listener, this, nameof(File), nameof(File.Copy), typeof(void));
        }
    }
}
