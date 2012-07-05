using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fantasy.ServiceModel;
using Fantasy.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("copy", Consts.XNamespaceURI, Description = "Copy files")]
    public class CopyTask : ObjectWithSite, ITask
    {
        public CopyTask()
        {
            this.TargetDirectory = string.Empty;
        }

        #region ITask Members

        public bool Execute()
        {
            ILogger logger = (ILogger)this.Site.GetService(typeof(ILogger));
            IJobEngine engine = (IJobEngine)this.Site.GetService(typeof(IJobEngine));
            IJob job = this.Site.GetRequiredService<IJob>();

            string targetDir = Fantasy.IO.LongPath.Combine(engine.JobDirectory, this.TargetDirectory);

            if (this.Source != null && this.Source.Length > 0)
            {
                logger.LogMessage("copy", MessageImportance.Low, "{0} files need to be copied.", this.Source.Length);

                IStatusBarService statusBar = this.Site.GetService<IStatusBarService>();
                IProgressMonitor progress = this.Site.GetService<IProgressMonitor>();
                SequenceProgressMonitor seqProgress = null;
                if (progress != null)
                {
                    seqProgress = new SequenceProgressMonitor(progress, this.Source.Length); 
                }

                List<TaskItem> items = new List<TaskItem>();
                if (this.Target != null)
                {
                    items.AddRange(this.Target); 
                }
                for(int i = 0; i < this.Source.Length; i ++ )
                {
                    TaskItem item = this.Source[i];
                    string source = item["FullName"];
                    string dest;
                    if (i < items.Count)
                    {
                        dest = items[i]["fullname"];
                    }
                    else
                    {
                        if (!LongPathDirectory.Exists(targetDir))
                        {
                            logger.LogMessage("copy", MessageImportance.Low, "Create directory {0}.", targetDir);
                            LongPathDirectory.Create(targetDir); 
                        }

                        dest = Fantasy.IO.LongPath.Combine(targetDir, Path.GetFileName(source));
                        string destName = Fantasy.IO.LongPath.GetRelativePath(engine.JobDirectory + "\\", dest);
                        TaskItem destItem = new TaskItem() { Name = destName };
                        items.Add(destItem); 
                    }

                    if (job.RuntimeStatus.Local.GetValue("copied.index", -1) < i)
                    {
                        if (logger != null)
                        {
                            logger.LogMessage("copy", MessageImportance.Low, "Copying file from {0} to {1}.", source, dest);
                        }
                        if (statusBar != null)
                        {
                            statusBar.SetStatus(string.Format("Copying file from {0} to {1}.", source, dest));
                        }

                        IProgressMonitor subProgress = seqProgress != null ? seqProgress[i] : null;

                        LongPathFile.Copy(source, dest, true, subProgress);
                        job.RuntimeStatus.Local["copied.index"] = i;
                    }
                    else
                    {
                        if (logger != null)
                        {
                            logger.LogMessage("copy", MessageImportance.Low, "Copying file from {0} to {1} is skipped.", source, dest);
                        }
                    }
                    

                    
                }

                this.Target = items.ToArray();
            }
            else
            {
                logger.LogMessage("copy", MessageImportance.Low, "No file need to be copy."); 
            }

            return true;
        }

        #endregion

        [TaskMember("source", Description="The files to copy")] 
        public TaskItem[] Source
        {
            get;set;
        }

        [TaskMember("target", Flags = TaskMemberFlags.Input | TaskMemberFlags.Output, Description="The list of files to copy the source files to.")]
        public TaskItem[] Target
        {
            get;set;
        }

        [TaskMember("targetDirectory", Flags= TaskMemberFlags.Input, Description="The target directory to copy the source files to.")]
        public string TargetDirectory { get; set; }
    }
}
