using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    public class ResourceStartupFilter : ObjectWithSite, IJobStartupFilter
    {
        #region IJobStartupFilter Members

        public IEnumerable<JobMetaData> Filter(IEnumerable<JobMetaData> source)
        {
            IResourceManager mngr = this.Site.GetRequiredService<IResourceManager>();
            IResourceRequestQueue queue = this.Site.GetRequiredService<IResourceRequestQueue>();
            foreach (JobMetaData job in source)
            {
                ResourceParameter[] res = queue.GetRequiredResources(job.Id);
                if (res.Length == 0 || mngr.IsAvailable(res))
                {
                    yield return job;
                }
            }
        }

        #endregion
    }
}
