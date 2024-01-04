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
using System.Xml;
using Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Xml;

namespace Test.Urasandesu.JVLinkToSQLite.Basis.Mixins.System.Xml
{
    [TestFixture]
    public class XmlNodeMixinTest
    {
        [Test]
        public void GetXPath_should_return_empty_string_if_node_has_no_parent()
        {
            // Arrange
            var node = new XmlDocument().CreateElement("root");

            // Act
            var xpath = node.GetXPath();

            // Assert
            Assert.That(xpath, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GetXPath_should_return_xpath_if_node_has_parent()
        {
            // Arrange
            var doc = new XmlDocument();
            var root = doc.CreateElement("root");
            var node = doc.CreateElement("node");
            doc.AppendChild(root);
            root.AppendChild(node);

            // Act
            var xpath = node.GetXPath();

            // Assert
            Assert.That(xpath, Is.EqualTo("/root/node"));
        }

        [Test]
        public void GetXPath_should_return_xpath_if_node_has_parent_and_predicate_is_true()
        {
            // Arrange
            var doc = new XmlDocument();
            var root = doc.CreateElement("root");
            var node = doc.CreateElement("node");
            doc.AppendChild(root);
            root.AppendChild(node);

            // Act
            var xpath = node.GetXPath(predicate: n => n.Name == "node");

            // Assert
            Assert.That(xpath, Is.EqualTo("/root/node"));
        }

        [Test]
        public void GetXPath_should_return_xpath_if_node_has_parent_and_predicate_is_false()
        {
            // Arrange
            var doc = new XmlDocument();
            var root = doc.CreateElement("root");
            var node = doc.CreateElement("node");
            doc.AppendChild(root);
            root.AppendChild(node);

            // Act
            var xpath = node.GetXPath(predicate: n => n.Name == "root");

            // Assert
            Assert.That(xpath, Is.EqualTo("/root/node"));
        }

        [Test]
        public void GetXPath_should_return_xpath_if_node_has_parent_and_trueFunc_is_specified()
        {
            // Arrange
            var doc = new XmlDocument();
            var root = doc.CreateElement("root");
            var node1 = doc.CreateElement("node1");
            var node2 = doc.CreateElement("node2");
            doc.AppendChild(root);
            root.AppendChild(node1);
            node1.AppendChild(node2);

            // Act
            var xpath = node2.GetXPath(predicate: n => n.Name == "node1", trueFunc: (n, f) => f() + "[.='value1']");

            // Assert
            Assert.That(xpath, Is.EqualTo("/root/node1[.='value1']/node2"));
        }

        [Test]
        public void GetXPath_should_return_xpath_if_node_has_parent_and_falseFunc_is_specified()
        {
            // Arrange
            var doc = new XmlDocument();
            var root = doc.CreateElement("root");
            var node = doc.CreateElement("node");
            doc.AppendChild(root);
            root.AppendChild(node);

            // Act
            var xpath = node.GetXPath(predicate: n => n.Name == "root", falseFunc: (n, f) => f() + "[.='value2']");

            // Assert
            Assert.That(xpath, Is.EqualTo("/root/node[.='value2']"));
        }
    }
}
