using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Management
{
    public class JobStartupStateFilter : IJobStartupFilter
    {
        #region IJobStartupFilter Members

        public IEnumerable<JobMetaData> Filter(IEnumerable<JobMetaData> source)
        {
            return source.Where(j => (j.State & (JobState.Unstarted  | JobState.Suspended)) > 0)
                .OrderBy(j => j.State); 
        }

        #endregion
    }
}
