using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Fantasy.Configuration;

namespace Fantasy.Jobs.Resources
{
   
    public class TaskCountSettings 
    {
        public TaskCountSettings()
        {
            Count = Int32.MaxValue;
        }

        [XmlAttribute("count")]
        public int Count { get; set; }


        private List<TaskCountSetting> _tasks = new List<TaskCountSetting>();

        [XmlArray("tasks"),
        XmlArrayItem(Type=typeof(TaskCountSetting), ElementName="task") ] 
        public List<TaskCountSetting> Tasks
        {
            get { return _tasks; }
        }

        public override bool Equals(object obj)
        {
            return ComparsionHelper.DeepEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
