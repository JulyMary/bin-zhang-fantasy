using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    public class TaskCountResouseProvider : ConcurrentCountResourceProviderBase 
    {
        public override bool CanHandle(string name)
        {
            return string.Equals("RunTask", name, StringComparison.OrdinalIgnoreCase); 
        }

        protected override string GetKey(ResourceParameter parameter)
        {
            return parameter.Values["namespace"] + "|" + parameter.Values["taskname"];
        }

        public override void Initialize()
        {
            JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsPropertyChanged);
        }

        void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ConcurrentTaskCount")
            {
                this.TryRevoke();
            }

            this.OnAvailable();
        }


        protected override int GetMaxCount(string key)
        {
            string[] strs = key.Split(new char[] {'|'} , 2);

            string ns = strs[0];
            string name = strs[1];
            var query = from s in JobManagerSettings.Default.ConcurrentTaskCount.Tasks
                        where ns == s.Namespace && name == s.Name
                        select s;
            TaskCountSetting setting = query.SingleOrDefault();

            return setting != null ? setting.Count : JobManagerSettings.Default.ConcurrentTaskCount.Count; 
        }

    }
}
