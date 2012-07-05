using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Management
{
    public class InProcessJobEngineHost : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }



        public void Run(JobManager manager, Guid jobId)
        {

            (new JobManagerAccessor()).SetJobManager(manager);

            JobEngine engine = new JobEngine(jobId);
            engine.Initialize();
            engine.WaitForExit();
        }
    }
}
