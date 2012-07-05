using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Resources;

namespace Fantasy.Jobs.Solar
{

    public class JobStartupTemplateFilter : ObjectWithSite, IJobStartupFilter
    {
        #region IJobStartupFilter Members

        public IEnumerable<JobStartupData> Filter(IEnumerable<JobStartupData> source)
        {
            SatelliteManager manager = this.Site.GetRequiredService<SatelliteManager>();
            foreach (JobStartupData data in source)
            {
                ResourceParameter[] res = new ResourceParameter[] {new ResourceParameter("RunJob", new {template=data.JobMetaData.Template })};
                bool canRun = false;
                if (manager.SafeCallSatellite(data.Satellite, satellite => satellite.IsResourceAvailable(res), out canRun))
                {
                    if (canRun)
                    {
                        yield return data;
                    }
                }
            }
        }

        #endregion
    }
}
