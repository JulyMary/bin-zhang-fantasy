using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fantasy.ServiceModel;
using Fantasy.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("delete", Consts.XNamespaceURI, Description="Delete files")]
    public class DeleteTask : ObjectWithSite, ITask
    {

        [TaskMember("source", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of file to delete." )]
        public TaskItem[] Source
        {
            get;
            set;
        }

        #region ITask Members

        public bool Execute()
        {
            if (Source != null)
            {
                ILogger logger = (ILogger)this.Site.GetService(typeof(ILogger));
                if (logger != null)
                {
                    logger.LogMessage("delete", MessageImportance.Low, "{0} files need to be deleted.", this.Source.Length);
                }

                IStatusBarService statusBar = this.Site.GetService<IStatusBarService>();
                IProgressMonitor progress = this.Site.GetService<IProgressMonitor>();
              
                if (progress != null)
                {
                    progress.Value = 0;
                    progress.Minimum = 0;
                    progress.Maximum = this.Source.Length; 

                }

                foreach (TaskItem item in Source)
                {
                    
                    
                    string file = item["fullname"];
                    if (LongPathFile.Exists(file))
                    {
                        logger.SafeLogMessage("Delete", "Delete file {0}", item.Name);
                        LongPathFile.Delete(file);
                    }
                    else
                    {
                        logger.SafeLogWarning("Delete", "File {0} does not exist", item.Name);
                    }

                    progress.Value++;
                }
            }

            return true;
        }

        #endregion
    }
}
