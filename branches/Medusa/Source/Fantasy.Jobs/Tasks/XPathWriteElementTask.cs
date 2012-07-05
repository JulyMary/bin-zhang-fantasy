using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using Fantasy.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("xpathWriteElement", Consts.XNamespaceURI, Description="Using XPath to write xml element to xml file")] 
    public class XPathWriteElementTask : XmlTaskBase 
    {
        public XPathWriteElementTask()
        {
            this.Mode = XPathWriteElementMode.Add;
        }

        #region ITask Members

        public override bool Execute()
        {
            if (Include != null && Include.Length > 0 )
            {

                XmlNamespaceManager mngr = this.Site.GetRequiredService<XmlNamespaceManager>();
                foreach (TaskItem item in Include)
                {
                    string file = item["fullname"];
                    XDocument doc = LongPathXNode.LoadXDocument(file);
                    

                    XElement[] targets = doc.XPathSelectElements(XPath, mngr).ToArray();

                    foreach (XElement target in targets)
                    {
                        XElement[] newElements = (from e in this.Content.Elements() select new XElement(e)).ToArray();

                        switch (this.Mode)
                        {
                            case XPathWriteElementMode.AddAfterSelf:
                                target.AddAfterSelf(newElements);
                                break;
                            case XPathWriteElementMode.AddBeforeSelf:
                                target.AddBeforeSelf(newElements);
                                break;
                            case XPathWriteElementMode.AddFirst:
                                target.AddFirst(newElements);
                                break;
                            case XPathWriteElementMode.Add:
                                target.Add(newElements);
                                break;
                            case XPathWriteElementMode.ReplaceWith:
                                target.ReplaceWith(newElements);
                                break;
                            case XPathWriteElementMode.ReplaceAll:
                                target.ReplaceAll(newElements);
                                break;
                        }
                    }

                    XmlWriter writer = XmlWriter.Create(file, this.XmlWriterSettings);
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

        #endregion


        [TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items to write.")]
        public TaskItem[] Include { get; set; }

        [TaskMember("xpath", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="A XPath expression indicating the target elements to write.")]
        public string XPath { get; set; }

        [TaskMember("content", Flags = TaskMemberFlags.Input | TaskMemberFlags.Inline | TaskMemberFlags.Required, ParseInline=true,
            Description="The XML content to write.")]
        public XElement Content { get; set; }

        [TaskMember("mode", Description="A value indicating how the conent write to target elements. Default is Add for adding new a child element in the end of target elements.")]
        XPathWriteElementMode Mode { get; set; }
    }

    public enum XPathWriteElementMode {Add, AddFirst, AddAfterSelf, AddBeforeSelf, ReplaceWith, ReplaceAll }
}
