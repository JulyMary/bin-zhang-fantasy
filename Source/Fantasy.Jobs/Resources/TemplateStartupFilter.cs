using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    public class TemplateStartupFilter : ObjectWithSite, IJobStartupFilter
    {
        #region IJobStartupFilter Members

        public IEnumerable<JobMetaData> Filter(IEnumerable<JobMetaData> source)
        {
             IJobController controller = this.Site.GetRequiredService<IJobController>();

             JobMetaData[] running = controller.GetRunningJobs(); 
            
           
            foreach (JobMetaData job in source)
            {
                string template = job.Template;

                int max = this.GetMaxCount(template);
                int exists = 0;
                if (max < Int32.MaxValue)
                {
                    exists = running.Where(j => string.Equals(j.Template, template, StringComparison.OrdinalIgnoreCase)).Count();
                }
                if (exists < max)
                {
                    yield return job;
                }
            }
        }

        #endregion

       


        private int GetMaxCount(string name)
        {
            var query = from t in JobManagerSettings.Default.ConcurrentTemplateCount.Templates
                        where name == t.Name
                        select t;
            TemplateCountSetting setting = query.SingleOrDefault();

            return setting != null ? setting.Count : JobManagerSettings.Default.ConcurrentTemplateCount.Count;
        }
    }
}
