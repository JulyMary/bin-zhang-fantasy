using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace Fantasy.Jobs.Tasks
{
    [Task("writeXml", Consts.XNamespaceURI, Description="Write xml to files")]
    public class WriteXmlTask : XmlTaskBase
    {
       

        [TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items to write to.")]
        public TaskItem[] Include { get; set; }

        [TaskMember("content", Flags = TaskMemberFlags.Input | TaskMemberFlags.Inline | TaskMemberFlags.Required, 
            Description="The XML content to write to.")]
        public XElement Content { get; set; }

        #region ITask Members

        public override bool Execute()
        {
            if (this.Include != null && this.Include.Length > 0)
            {
                

                XElement root = this.Content.Elements().First();
               

                foreach (TaskItem item in this.Include)
                {

                    string path = item["fullname"];

                    XmlWriter writer = XmlWriter.Create(path, this.XmlWriterSettings);
                    try
                    {
                        root.WriteTo(writer);
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
    }
}
