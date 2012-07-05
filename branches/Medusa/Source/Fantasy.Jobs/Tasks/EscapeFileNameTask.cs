using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Fantasy.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("escapeFileName", Consts.XNamespaceURI, Description="Replace invalid charactors of file name with underline(_)")] 
    public class EscapeFileNameTask : ObjectWithSite, ITask
    {
        public EscapeFileNameTask()
        {
            this.Replacement = "_";
        }

        Regex regex = new Regex(@"[\\\/\:\*\?\""\<\>\|]");

        #region ITask Members

        public bool Execute()
        {
            if (this.Source != null)
            {
                this.Result = LongPath.EscapeFileName(Source, this.Replacement).Trim(); 
            }
            return true;
        }

        #endregion

        [TaskMember("source", Flags= TaskMemberFlags.Input | TaskMemberFlags.Required, Description="File name to escape")] 
        public string Source { get; set; }

        [TaskMember("result", Flags=TaskMemberFlags.Output, Description="Escaped file name")]
        public string Result { get; set; }

        [TaskMember("replacement", Description="The string to replace all invalid characters in source string.")] 
        public string Replacement { get; set; }
    }
}
