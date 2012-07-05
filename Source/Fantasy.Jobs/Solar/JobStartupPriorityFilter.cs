using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Solar
{
    public class JobStartupPriorityFilter : ObjectWithSite, IJobStartupFilter
    {

        #region IJobStartupFilter Members

        public IEnumerable<JobStartupData> Filter(IEnumerable<JobStartupData> source)
        {
            return source.OrderByDescending(j => j.JobMetaData.Priority); 
        }

        #endregion
    }
}
