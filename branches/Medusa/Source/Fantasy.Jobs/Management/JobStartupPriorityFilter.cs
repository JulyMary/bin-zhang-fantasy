using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Management
{
    public class JobStartupPriorityFilter : IJobStartupFilter 
    {

        #region IJobStartupFilter Members

        public IEnumerable<JobMetaData> Filter(IEnumerable<JobMetaData> source)
        {
            return source.OrderByDescending(j => j.Priority); 
        }

        #endregion
    }
}
