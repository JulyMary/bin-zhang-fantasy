using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Fantasy.Jobs.Management
{
    internal class JobProcess
    {
        public JobProcess(JobMetaData job, Process process, bool isResume)
        {
            this.Process = process;
            this.Job = job;
            this.IsResume = isResume;
            this.ExitEvent = new ManualResetEvent(false);

        }

        public Process Process { get; private set; }


        public JobMetaData Job { get; private set; }


        public ManualResetEvent ExitEvent { get; private set; }


        public bool IsResume { get; private set; }

        public IJobEngine Engine { get; set; }
    }
}
