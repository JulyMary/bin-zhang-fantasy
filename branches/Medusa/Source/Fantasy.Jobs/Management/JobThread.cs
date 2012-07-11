using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fantasy.Jobs.Management
{
    internal class JobThread
    {
        public JobThread(JobMetaData job, Thread thread, bool isResume)
        {
            this.Thread = thread;
            this.Job = job;
            this.IsResume = isResume;
            this.ExitEvent = new ManualResetEvent(false);
        }

        public Thread Thread { get; private set; }


        public JobMetaData Job { get; private set; }


        public ManualResetEvent ExitEvent { get; private set; }


        public bool IsResume { get; private set; }

        public IJobEngine Engine { get; set; }
    }
}
