using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Fantasy.Jobs.Resources;
using System.Xml.Serialization;
using System.Configuration;

namespace Fantasy.Jobs.Management
{
    public sealed partial class JobManagerSettings 
    {

        [DefaultSettingValueAttribute("<TaskCountSettings />")]
        public TaskCountSettings ConcurrentTaskCount
        {
            get { return (TaskCountSettings)this.GetValue("ConcurrentTaskCount"); }
            set { this.SetValue("ConcurrentTaskCount", value); }
        }

        [DefaultSettingValueAttribute("<TemplateCountSettings />")]
        public TemplateCountSettings ConcurrentTemplateCount
        {
            get { return (TemplateCountSettings)this.GetValue("ConcurrentTemplateCount"); }
            set { this.SetValue("ConcurrentTemplateCount", value); }
        }

        [DefaultSettingValueAttribute("<RuntimeScheduleSetting />")]
        public RuntimeScheduleSetting JobRuntimeSchedule
        {
            get { return (RuntimeScheduleSetting)this.GetValue("JobRuntimeSchedule"); }
            set { this.SetValue("JobRuntimeSchedule", value); }
        }


        [DefaultSettingValueAttribute("<TemplateRuntimeScheduleSettings />")]
        public TemplateRuntimeScheduleSettings TemplateRuntimeSchedule
        {
            get { return (TemplateRuntimeScheduleSettings)this.GetValue("TemplateRuntimeSchedule"); ; }
            set { this.SetValue("TemplateRuntimeSchedule", value); }
        }

        [DefaultSettingValueAttribute("<TaskRuntimeScheduleSettings />")]
        public TaskRuntimeScheduleSettings TaskRuntimeSchedule
        {
            get { return (TaskRuntimeScheduleSettings)this.GetValue("TaskRuntimeSchedule"); ; }
            set { this.SetValue("TaskRuntimeSchedule", value); }
        }



    }
}
