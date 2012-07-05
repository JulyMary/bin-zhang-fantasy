using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Configuration;
using System.Xml.Serialization;

namespace Fantasy.Jobs.Resources
{
    public class TaskRuntimeScheduleSettings : RuntimeScheduleSetting
    {

        private List<TaskRuntimeScheduleSetting> _tasks = new List<TaskRuntimeScheduleSetting>();
         [XmlArray("tasks"),
        XmlArrayItem(ElementName = "task", Type = typeof(TaskRuntimeScheduleSetting))]
        public List<TaskRuntimeScheduleSetting> Tasks
        {
            get { return _tasks; }
        }
    }


    public class TaskRuntimeScheduleSetting : RuntimeScheduleSetting
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("namespace")]
        public string Namespace { get; set; }

       
    }
}
