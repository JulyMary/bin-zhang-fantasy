using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Xml;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //AddInParser parser = new AddInParser();
            //XmlReader reader = XmlReader.Create("test.addin.xaml");
            //AddIn addIn = parser.Parse(reader);
            //reader.Close();

            DefaultAddInTree.Initialize(new string[] { "test.addin.xaml" });
            object[] rs = AddInTree.Tree.GetTreeNode("fantasy/services").BuildChildItems<object>(null).ToArray();

        }
    }
}
