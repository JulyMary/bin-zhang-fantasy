using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Management
{
    public class JobStartupProcessCountFilter : IJobStartupFilter, IObjectWithSite 
    {
        #region IJobStartupFilter Members

        public IEnumerable<JobMetaData> Filter(IEnumerable<JobMetaData> source)
        {
            IJobController ctrl = this.Site.GetRequiredService<IJobController>();
            if (ctrl.GetRunningJobs().Length < JobManagerSettings.Default.JobProcessCount)
            {
                return source;
            }
            else
            {
                return new JobMetaData[] { };
            }
        }

        #endregion

        #region IObjectWithSite Members

        public IServiceProvider Site
        {
            get;
            set;
        }

        #endregion
    }
}
