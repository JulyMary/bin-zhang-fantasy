using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    public class TemplateCountResourceProvider : ConcurrentCountResourceProviderBase
    {
      
        public override bool CanHandle(string name)
        {
            return string.Equals("RunJob", name, StringComparison.OrdinalIgnoreCase);
        }

        public override void Initialize()
        {
            JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsPropertyChanged);
        }

        protected override int GetMaxCount(string key)
        {
            var query = from t in JobManagerSettings.Default.ConcurrentTemplateCount.Templates
                        where key == t.Name
                        select t;
            TemplateCountSetting setting = query.SingleOrDefault();

            return setting != null ? setting.Count : JobManagerSettings.Default.ConcurrentTemplateCount.Count;

        }

        void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ConcurrentTemplateCount")
            {
                this.TryRevoke();
            }

            this.OnAvailable();
        }

        protected override string GetKey(ResourceParameter parameter)
        {
            return parameter.Values["template"];
        }

 
        
    }
}
