using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.ServiceModel;
using System.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("move", Consts.XNamespaceURI, Description="Move files")]
    public class MoveTask : ObjectWithSite, ITask
    {
        public MoveTask()
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
                logger.LogMessage("move", MessageImportance.Low, "{0} files need to be moved.", this.Source.Length);


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
                for (int i = 0; i < this.Source.Length; i++)
                {
                    TaskItem item = this.Source[i];
                    string source = item["FullName"];
                    string dest;
                    if (i < items.Count)
                    {
                        dest = items[i]["fullname"];
                        string dir = items[i]["directory"];
                        if (!LongPathDirectory.Exists(dir))
                        {
                            logger.LogMessage("move", MessageImportance.Low, "Create directory {0}.", dir);
                            LongPathDirectory.Create(dir);
                        }
                    }
                    else
                    {
                        if (!LongPathDirectory.Exists(targetDir))
                        {
                            logger.LogMessage("move", MessageImportance.Low, "Create directory {0}.", targetDir);
                            LongPathDirectory.Create(targetDir);
                        }

                        dest = LongPath.Combine(targetDir, Path.GetFileName(source));
                        string destName = LongPath.GetRelativePath(engine.JobDirectory + "\\", dest);
                        TaskItem destItem = new TaskItem() { Name = destName };
                        items.Add(destItem);
                    }




                    if (job.RuntimeStatus.Local.GetValue("moved.index", -1) < i)
                    {
                        if (logger != null)
                        {
                            logger.LogMessage("move", MessageImportance.Low, "Moving file from {0} to {1}.", source, dest);
                        }
                        if (statusBar != null)
                        {
                            statusBar.SetStatus(string.Format("Moving file from {0} to {1}.", source, dest));
                        }

                        IProgressMonitor subProgress = seqProgress != null ? seqProgress[i] : null;
                        
                        LongPathFile.Move(source, dest, true, subProgress);
                        job.RuntimeStatus.Local["moved.index"] = i;
                    }
                    else
                    {
                        if (logger != null)
                        {
                            logger.LogMessage("move", MessageImportance.Low, "Moving file from {0} to {1} is skipped.", source, dest);
                        }
                    }



                }

                this.Target = items.ToArray();
            }
            else
            {
                logger.LogMessage("move", MessageImportance.Low, "No file need to be moved.");
            }

            return true;
        }

        #endregion

        [TaskMember("source", Description="The files to move." )]
        public TaskItem[] Source
        {
            get;
            set;
        }

        [TaskMember("target", Flags = TaskMemberFlags.Input | TaskMemberFlags.Output, Description= "The list of files to move the source files to.")]
        public TaskItem[] Target
        {
            get;
            set;
        }

        [TaskMember("targetDirectory", Flags = TaskMemberFlags.Input, Description="The target directory to move the source files to.")]
        public string TargetDirectory { get; set; }
    }
}
