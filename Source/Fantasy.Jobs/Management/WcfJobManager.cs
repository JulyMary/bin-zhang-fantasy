using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClickView.Jobs.Management
{
    public class WcfJobManager : IJobManager
    {
        private IJobManager _manager = ServicesManager.Services.GetService<JobManager>();  

        #region IJobManager Members

        public JobTemplate GetJobTemplateByName(string name)
        {
            return _manager.GetJobTemplateByName(name); 
        }

        public JobTemplate GetJobTemplateByPath(string path)
        {
            return _manager.GetJobTemplateByPath(path); 
        }

        #endregion
    }
}
