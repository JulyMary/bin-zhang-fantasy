using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Scheduling;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    public class JobRuntimeScheduleResourceProvider : RuntimeScheduleResourceProviderBase
    {
       

        protected override bool InternalCanHandle(string name)
        {
            return string.Equals("RunJob", name, StringComparison.OrdinalIgnoreCase);
        }

        public override void Initialize()
        {
            base.Initialize();
            JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(JobManagerSettings_PropertyChanged);
        }

        void JobManagerSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "JobRuntimeSchedule")
            {
                this.Reschedule();
                
            }
        }

        protected override RuntimeScheduleSetting GetSetting(string id)
        {
            return JobManagerSettings.Default.JobRuntimeSchedule; 
        }

        protected override string GetResourceId(ResourceParameter parameter)
        {
            return "";
        }
    }
}
