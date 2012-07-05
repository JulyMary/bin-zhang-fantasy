using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Tasks
{
    [Task("callTarget", Consts.XNamespaceURI, Description = "Invoke specified target immediately")] 
    public class CallTargetTask : ObjectWithSite, ITask
    {
        #region ITask Members

        public bool Execute()
        {
            IJob job = this.Site.GetRequiredService<IJob>();
            if(this.Targets != null)
            {
                foreach(string target in this.Targets)
                {
                    if(!string.IsNullOrEmpty(target))
                    {
                        job.ExecuteTarget(target);
                    }
                }
            }

            return true;
             
        }
        #endregion

        [TaskMember("target", Description = "The target or targets to execute")] 
        public string[] Targets { get; set; }
    }
}
