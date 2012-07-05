using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    public class TaskRuntimeScheduleResourceProvider : RuntimeScheduleResourceProviderBase
    {
        protected override bool InternalCanHandle(string name)
        {
            return string.Equals("RunTask", name, StringComparison.OrdinalIgnoreCase); 
        }

        public override void Initialize()
        {
            base.Initialize();
            JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsPropertyChanged);
        }


        void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TaskRuntimeSchedule")
            {
                this.Reschedule();
            }
        }

        protected override RuntimeScheduleSetting GetSetting(string id)
        {

            RuntimeScheduleSetting rs = JobManagerSettings.Default.TaskRuntimeSchedule.Tasks.Find(setting => string.Equals(String.Format("{0}|{1}", setting.Namespace, setting.Name), id, StringComparison.OrdinalIgnoreCase));
            if (rs == null)
            {
                rs = JobManagerSettings.Default.TaskRuntimeSchedule;
            }
            return rs;
        }

        protected override string GetResourceId(ResourceParameter parameter)
        {
            return string.Format("{0}|{1}", parameter.Values["namespace"], parameter.Values["taskname"]);
        }
    }
}
