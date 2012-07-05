using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Resources;

namespace Fantasy.Jobs.Solar
{
    public class JobStartupResourceFilter : ObjectWithSite, IJobStartupFilter
    {

        #region IJobStartupFilter Members

        public IEnumerable<JobStartupData> Filter(IEnumerable<JobStartupData> source)
        {
            SatelliteManager manager = this.Site.GetRequiredService<SatelliteManager>();
            IResourceRequestQueue resQueue = this.Site.GetRequiredService<IResourceRequestQueue>();
            foreach (JobStartupData data in source)
            {
                ResourceParameter[] res = resQueue.GetRequiredResources(data.JobMetaData.Id);
                if (res.Length > 0)
                {
                    bool hasRes = false;
                    if (manager.SafeCallSatellite(data.Satellite, satellite => satellite.IsResourceAvailable(res), out hasRes))
                    {
                        if (hasRes)
                        {
                            yield return data;
                        }
                    }
                }
                else
                {
                    yield return data;
                }
            }
        }

        #endregion
    }
}
