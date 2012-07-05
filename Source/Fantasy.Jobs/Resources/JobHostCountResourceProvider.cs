using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    public class JobHostCountResourceProvider : ConcurrentCountResourceProviderBase
    {
       

        public override bool CanHandle(string name)
        {
            return string.Equals("RunJob", name, StringComparison.OrdinalIgnoreCase);
        }

        public override void Initialize()
        {
            
            JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsChanged);
        }

        void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "JobProcessCount")
            {
                this.TryRevoke();
            }
            this.OnAvailable();
        }


        protected override int  GetMaxCount(string key)
        {
 	        return JobManagerSettings.Default.JobProcessCount; 
        }

        protected override string GetKey(ResourceParameter parameter)
        {
            return string.Empty;
        }
       

       
    }
}
