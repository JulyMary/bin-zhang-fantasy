using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace Fantasy.Jobs.Tasks
{
    [Task("xpathSetElement", Consts.XNamespaceURI, Description="Using XPath to set xml element value to xml file" )] 
    public class XPathSetElementTask : XPathSetValueTaskBase 
    {
        protected override void SetValue(XElement element, XmlNamespaceManager nsMgr)
        {
            element.Value = this.Value;
        }
    }
}
