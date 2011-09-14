using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace Fantasy.AddInFormatter
{
    class Program
    {
        static void Main(string[] args)
        {
            XNamespace ns = "urn:schema-fantasy:xaml";

            foreach (string file in args)
            {
                XElement root = XElement.Load(file);
                XElement[] extensions = root.Elements(ns + "Extension").OrderBy(ex => (string)ex.Attribute("Path")).ToArray();
                foreach (XElement extension in extensions)
                {
                    extension.Remove();
                }

                foreach (XElement extension in extensions)
                {
                    root.Add(extension);
                }

                XmlWriter writer = XmlWriter.Create(file, new XmlWriterSettings() { CloseOutput = true, Encoding = Encoding.UTF8, Indent = true, IndentChars = "    ", NamespaceHandling = NamespaceHandling.OmitDuplicates, OmitXmlDeclaration=true });

                root.Save(writer);

                writer.Close();


            }

        }
    }
}
