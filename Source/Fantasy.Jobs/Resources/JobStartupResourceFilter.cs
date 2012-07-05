using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClickView.Jobs.Management;

namespace ClickView.Jobs.Resources
{
    public class JobStartupResourceFilter : ObjectWithSite, IJobStartupFilter
    {
        #region IJobStartupFilter Members

        public IEnumerable<JobMetaData> Filter(IEnumerable<JobMetaData> source)
        {
            IResourceRequestQueue queue = this.Site.GetService<IResourceRequestQueue>();

            foreach (JobMetaData job in source)
            {
                if (queue == null || queue.IsAvailableFor(job.Id).IsAvilable)
                {
                    yield return job;

                }
            }
            
        }

        #endregion
    }
}
