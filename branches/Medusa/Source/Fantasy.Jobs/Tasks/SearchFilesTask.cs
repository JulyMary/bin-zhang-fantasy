using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("searchFiles", Consts.XNamespaceURI, Description="Search files in directory")]
    public class SearchFilesTask : ObjectWithSite, ITask
    {
        public SearchFilesTask()
        {
           
            Recursive = false;
        }

        [TaskMember("path", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The directory from those to retrive the files.")] 
        public string Path { get; set; }

        [TaskMember("pattern", Description="The search string to match against the names of files in path." )]
        public string Pattern { get; set; }

        [TaskMember("result", Flags=TaskMemberFlags.Output, Description="The list contains items in specified directory that match the specified search pattern.")] 
        public TaskItem[] Result { get; set; }

        [TaskMember("recursive", Description="true if includes the current directory and all the subdirectories in a search operation; otherwise only top directory in a search.")]
        public bool Recursive { get; set; }

        #region ITask Members

        public bool Execute()
        {
            if (this.Path != null && LongPathDirectory.Exists(this.Path))
            {
                IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
                List<TaskItem> rs = new List<TaskItem>();
                IEnumerable<string> files = this.Recursive ? LongPathDirectory.EnumerateAllFiles(this.Path, Pattern) : LongPathDirectory.EnumerateFiles(this.Path, Pattern);
                foreach (string f in files)
                {
                    string name = LongPath.GetRelativePath(engine.JobDirectory + "\\", f);
                    TaskItem item = new TaskItem() { Name = name };
                    rs.Add(item);
                }
                this.Result = rs.ToArray();

            }
            return true;
        }

        #endregion
    }
}
