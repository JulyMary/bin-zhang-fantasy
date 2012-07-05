using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace ClickView.Jobs.Tasks
{
    public abstract class XPathTaskBase : XmlTaskBase
    {
       
        protected XmlNamespaceManager CreateNamespaceManager(XElement element)
        {
            NameTable table = new NameTable();
            XmlNamespaceManager rs = new XmlNamespaceManager(table);

            rs.AddNamespace(String.Empty, element.GetDefaultNamespace().NamespaceName); 

            var query = from attr in element.Attributes() where attr.IsNamespaceDeclaration select attr;
            foreach (XAttribute attr in query)
            {
                rs.AddNamespace(attr.Name.LocalName, attr.Value);
            }
            return rs;
        }
    }
}
