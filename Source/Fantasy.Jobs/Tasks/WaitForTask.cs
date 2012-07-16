using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Resources;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Tasks
{

    [Task("waitFor", Consts.XNamespaceURI, Description="Suspend current job until specified job terminated")]
    public class WaitForTask : ObjectWithSite, ITask
    {
        public WaitForTask()
        {
            this.Mode = WaitForMode.All; 
        }
       
        public bool Execute()
        {
 
            IResourceService ressvc = this.Site.GetService<IResourceService>();
            ILogger logger = this.Site.GetService<ILogger>();
            if (this.Jobs != null)
            {
                if (ressvc != null)
                {
                    string jobs = string.Join(";", this.Jobs);
                    ResourceParameter res = new ResourceParameter("WaitFor", new Dictionary<string, string>() { {"mode" , this.Mode.ToString()}, {"jobs", jobs}});
                    if (logger != null)
                    {
                        if (this.Mode == WaitForMode.All)
                        {
                            logger.LogMessage("WaitFor", "Wait for all of following jobs terminated {0}", jobs);
                        }
                        else
                        {
                            logger.LogMessage("WaitFor", "Wait for any of following jobs terminated {0}", jobs);
                        }
                    }

                    IResourceHandle handler = ressvc.Request(new ResourceParameter[] { res });
                    // if request succeed, release resource immediately, otherwise, job engine will block executing.
                    handler.Dispose();
                    
                }
                else
                {
                    if (logger != null)
                    {
                        logger.LogMessage("WaitFor", "IResourceService is not available on this system, this task is skipped.");
                    }
                }
            }

            return true;

        }

        [TaskMember("jobs", Flags=TaskMemberFlags.Input | TaskMemberFlags.Output | TaskMemberFlags.Required, Description="The list of Job Id to wait.")]
        public Guid[] Jobs { get; set; }

        [TaskMember("mode", Description="Mode of WaitFor task. All for wait until all job to termiated in jobs; Any for wait until any of job termianted in jobs." )]
        public WaitForMode Mode { get; set;}
    }
}
