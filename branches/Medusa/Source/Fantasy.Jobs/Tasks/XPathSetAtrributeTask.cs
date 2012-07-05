using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Fantasy.Jobs.Tasks
{
    [Task("xpathSetAttribute", Consts.XNamespaceURI, Description = "Using XPath to set xml attribute value to xml file")]
    public class XPathSetAtrributeTask : XPathSetValueTaskBase 
    {
        protected override void SetValue(System.Xml.Linq.XElement element, XmlNamespaceManager nsMgr)
        {
            string[] names = this.Attribute.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
            XName name;
            if (names.Length == 2)
            {
                XNamespace ns = nsMgr.LookupNamespace(names[0]);
                name = ns + names[1];
            }
            else
            {
                name = names[0];
            }

            element.SetAttributeValue(name, this.Value);
 
        }


        [TaskMember("attribute", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The attribute name to set value.")] 
        public string Attribute { get; set; }
    }
}
