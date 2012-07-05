using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Solar
{
    public class JobStartupStateFilter : ObjectWithSite, IJobStartupFilter
    {
        #region IJobStartupFilter Members

        public IEnumerable<JobStartupData> Filter(IEnumerable<JobStartupData> source)
        {
            return source.Where(j => (j.JobMetaData.State & (JobState.Unstarted | JobState.Suspended)) > 0)
                .OrderBy(j => j.JobMetaData.State); 
        }

        #endregion
    }
}
