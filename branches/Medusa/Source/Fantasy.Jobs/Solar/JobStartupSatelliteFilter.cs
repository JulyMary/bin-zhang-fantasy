using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Solar
{
    public class JobStartupSatelliteFilter : ObjectWithSite, IJobStartupFilter
    {


        private double GetLoadFactor(SatelliteManager manager, string name)
        {
            double rs;
            if (manager.SafeCallSatellite(name, satellite => satellite.GetLoadFactor(), out rs))
            {
                return rs;
            }
            else
            {
                return 0;
            }
        }

        #region IJobStartupFilter Members

        public IEnumerable<JobStartupData> Filter(IEnumerable<JobStartupData> source)
        {
            SatelliteManager manager = this.Site.GetRequiredService<SatelliteManager>();


            var query = from site in manager.Satellites
                        select new
                        {
                            site = site,
                            load = this.GetLoadFactor(manager, site.Name)
                        };

            string[] names = (from o in query where o.load > 0 orderby o.load descending select o.site.Name).ToArray();


            foreach (JobStartupData data in source)
            {
                foreach (string name in names)
                {
                    yield return new JobStartupData() { JobMetaData = data.JobMetaData, Satellite = name };
                }
            }

        }

        #endregion
    }
}
