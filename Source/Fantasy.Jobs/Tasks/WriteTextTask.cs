using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("writeText", Consts.XNamespaceURI, Description = "Write text to file")]
    public class WriteTextTask : ObjectWithSite, ITask
    {
        public WriteTextTask()
        {
            this.Append = true;
           
        }

        #region ITask Members

        public bool Execute()
        {
            if (!String.IsNullOrWhiteSpace(this.Path) && this.Value != null)
            {
                System.Text.Encoding encoding = System.Text.Encoding.Default;
                if (!string.IsNullOrEmpty(this.Encoding))
                {
                    encoding = System.Text.Encoding.GetEncoding(this.Encoding);
                }
                if (this.Append)
                {
                    LongPathFile.AppendAllText(this.Path, this.Value, encoding);
                }
                else
                {
                    LongPathFile.WriteAllText(this.Path, this.Value, encoding); 
                }
            }

            return true;
        }

        #endregion

        [TaskMember("path", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The file to write to.")] 
        public string Path { get; set; }


        [TaskMember("append", Description="true if append text to the end of file if the source file exists; otherwise the write opration overwrite existing file." )] 
        public bool Append { get; set; }


        [TaskMember("text", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The content to write to.")]
        public string Value { get; set; }

        [TaskMember("encoding", Flags=TaskMemberFlags.Input, Description="The encoding to apply to the string. ")] 
        public string Encoding { get; set; }
    }
}
