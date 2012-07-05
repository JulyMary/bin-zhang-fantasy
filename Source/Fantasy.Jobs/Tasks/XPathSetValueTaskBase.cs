using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using Fantasy.IO;

namespace Fantasy.Jobs.Tasks
{
   
    public abstract class XPathSetValueTaskBase : XmlTaskBase
    {

        protected abstract void SetValue(XElement element, XmlNamespaceManager nsMgr);

        public override bool Execute()
        {
            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            if (Include != null && Include.Length > 0)
            {
                foreach (TaskItem item in Include)
                {
                    
                   
                    string file = item["fullname"];
                    XDocument doc = LongPathXNode.LoadXDocument(file);
                    XmlNamespaceManager mngr = this.Site.GetRequiredService<XmlNamespaceManager>();

                    IEnumerable<XElement> targets = (IEnumerable<XElement>)doc.XPathSelectElements(XPath, mngr);

                    foreach (XElement target in targets)
                    {
                        SetValue(target, mngr);
                    }

                    FileStream fs = LongPathFile.Open(file, FileMode.Create, FileAccess.ReadWrite);
                    XmlWriter writer = XmlWriter.Create(fs, this.XmlWriterSettings);
                    try
                    {
                        doc.WriteTo(writer);
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
            }

            return true;
        }

        [TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items to set values")]
        public TaskItem[] Include { get; set; }

        [TaskMember("xpath", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The XPath expression indicating the target elements in incude XML files.")]
        public string XPath { get; set; }

        [TaskMember("value", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The value to set to.")] 
        public string Value { get; set; }
    }
}
