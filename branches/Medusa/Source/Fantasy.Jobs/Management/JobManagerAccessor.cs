using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Management
{
    public class JobManagerAccessor : MarshalByRefObject
    {

        private static IJobManager _manager;

        public IJobManager GetJobManager()
        {
            return _manager ?? JobManager.Default; 
        }

        public void SetJobManager(JobManager manager)
        {
            _manager = manager;
        }
    }
}
