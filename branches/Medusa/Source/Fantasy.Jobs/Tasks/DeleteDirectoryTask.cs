using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Tasks
{
    [Task("deleteDirectory", Consts.XNamespaceURI, Description="Delete directories")]
    public class DeleteDirectoryTask : ObjectWithSite, ITask
    {
        #region ITask Members

        [TaskMember("path", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of directory path to delete.")]
        public string[] Path { get; set; }

        [TaskMember("recursive", Flags = TaskMemberFlags.Input, Description="ture if delete all child directory in path; otherwise false")]
        public bool Recursive { get; set; }

        public DeleteDirectoryTask ()
	    {
            Recursive = false;
	    }

        public bool Execute()
        {
            if (Path != null && Path.Length > 0)
            {
                ILogger logger = this.Site.GetService<ILogger>();
                foreach (string path in this.Path)
                {
                    if (LongPathDirectory.Exists(path))
                    {
                        LongPathDirectory.Delete(path,this.Recursive);
                        if (logger != null)
                        {
                            logger.LogMessage("deleteDirectory", "Delete directory {0}.", path);
                        }
                    }
                    else
                    {
                        logger.LogWarning("deleteDirectory", MessageImportance.Low, "Directory {0} does not exist.", path);
                    }
                }

            }

            return true;
            
        }

        #endregion
    }
}
