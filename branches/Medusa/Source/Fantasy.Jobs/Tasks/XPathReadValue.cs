using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Collections;
using System.IO;
using Fantasy.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("xpathReadValue", Consts.XNamespaceURI, Description="Using XPath to read value from xml file")] 
    public class XPathReadValue : ObjectWithSite, ITask
    {
        public bool Execute()
        {
            List<string> values = new List<string>();
            if (Include != null && Include.Length > 0)
            {
                
                foreach (TaskItem item in Include)
                {
                    string file = item["fullname"];
                    XDocument doc = LongPathXNode.LoadXDocument(file);
                   



                    XmlNamespaceManager mngr = this.Site.GetRequiredService<XmlNamespaceManager>();
                    IEnumerable targets = (IEnumerable)doc.XPathEvaluate(XPath, mngr);

                    foreach (object target in targets)
                    {
                        if (target is XElement)
                        {
                            values.Add((string)(XElement)target);
                        }
                        else if (target is XAttribute)
                        {
                            values.Add((string)(XAttribute)target);
                        }
                    }
                   
                }
            }
            this.Value = values.ToArray();

            return true;
        }

        [TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items from those to retrive values")]
        public TaskItem[] Include { get; set; }

        [TaskMember("xpath", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The XPath expression to read value.")]
        public string XPath { get; set; }

        [TaskMember("value", Flags = TaskMemberFlags.Output, Description="A list of string contains the value read form include items using specified XPath." )] 
        public string[] Value { get; set; }
    }
}
