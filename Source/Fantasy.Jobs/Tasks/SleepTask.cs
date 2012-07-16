using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Resources;
using Fantasy.ServiceModel;
using System.Threading;

namespace Fantasy.Jobs.Tasks
{
    [Task("sleep", Consts.XNamespaceURI, Description="Suspend current job until specified time or duration")] 
    public class SleepTask : ObjectWithSite, ITask
    {
        [TaskMember("time", Description="Absolute time to sleep to.")] 
        public DateTime Time { get; set; }

        [TaskMember("duration", Description= "Duration to sleep.")]
        public TimeSpan Duration { get; set; }

        #region ITask Members

        public bool Execute()
        {
            IResourceService ressvc = this.Site.GetService<IResourceService>();
            ILogger logger = this.Site.GetService<ILogger>();
            if (ressvc != null)
            {
                IJob job = this.Site.GetRequiredService<IJob>();
                DateTime timeToWait = DateTime.MinValue; 
                string s = (string)job.RuntimeStatus.Local["sleep.waittime"];
                if (!String.IsNullOrWhiteSpace(s))
                {
                    timeToWait = DateTime.Parse(s);
                }
                else
                {
                    if (this.Time != default(DateTime))
                    {
                        timeToWait = this.Time;
                    }
                    else
                    {
                        timeToWait = DateTime.Now + this.Duration;
                    }
                    job.RuntimeStatus.Local["sleep.waittime"] = timeToWait.ToString();
                }
                if (timeToWait != DateTime.MinValue)
                {
                    ResourceParameter res = new ResourceParameter("WaitTime", new Dictionary<string, string>(){ { "time", timeToWait.ToString()} });
                    if (logger != null)
                    {
                        logger.LogMessage("Sleep", "Sleep current job till {0}.", timeToWait);
                    }

                    IResourceHandle handler = ressvc.Request(new ResourceParameter[] { res });
                   
                    // if request succeed, release resource immediately, otherwise, job engine will block executing.
                    if (handler != null)
                    {
                        handler.Dispose();
                    }
                   
                    
                }
            }
            else
            {
                if (logger != null)
                {
                    logger.LogMessage("Sleep", "IResourceService is not available on this system, this task is skipped.");
                }
            }

            return true;
        }

        #endregion
    }
}
