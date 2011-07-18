using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xaml;

namespace Fantasy.AddIns
{
    public class AddInParser
    {
        public AddIn Parse(XmlReader reader)
        {
            XamlXmlReader xxr = new XamlXmlReader(reader);
            XamlObjectWriter xow = new XamlObjectWriter(xxr.SchemaContext);

            xow.
            while (xxr.Read())
            {
                xow.WriteNode(xxr);
            }

            return (AddIn)xow.Result;

        }
    }
}
