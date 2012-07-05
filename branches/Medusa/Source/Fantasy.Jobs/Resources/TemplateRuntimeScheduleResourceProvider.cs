using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    public class TemplateRuntimeScheduleResourceProvider : RuntimeScheduleResourceProviderBase
    {
        public TemplateRuntimeScheduleResourceProvider()
        {

        }
        protected override bool InternalCanHandle(string name)
        {
            return string.Equals("RunJob", name, StringComparison.OrdinalIgnoreCase);
        }

        public override void Initialize()
        {
            base.Initialize();
            JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsPropertyChanged);
        }


        void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplateRuntimeSchedule")
            {
                this.Reschedule();
            }
        }

        protected override RuntimeScheduleSetting GetSetting(string id)
        {
            RuntimeScheduleSetting rs = JobManagerSettings.Default.TemplateRuntimeSchedule.Templates.Find(s => string.Equals(s.Name, id, StringComparison.OrdinalIgnoreCase));
            if (rs == null)
            {
                rs = JobManagerSettings.Default.TemplateRuntimeSchedule;
            }

            return rs;
        }

        protected override string GetResourceId(ResourceParameter parameter)
        {
            return parameter.Values["template"];
        }
    }
}
