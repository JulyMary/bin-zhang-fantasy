using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using Fantasy.XSerialization;
namespace Fantasy.Jobs
{
    [Serializable]
    [XSerializable("jobStart", NamespaceUri=Consts.XNamespaceURI)] 
    public class JobStartInfo
    {
        public JobStartInfo()
        {
            this.ID = Guid.NewGuid();
        }

        [XAttribute("id")]
        public Guid ID { get; set; }

        [XAttribute("target")]
        public string Target { get; set; }

        [XAttribute("template")]
        public string Template { get; set; }


        [XAttribute("name")]
        public string Name { get; set; }

        private List<JobPropertyGroup> _propertyGroups = new List<JobPropertyGroup>();

        [XArray]
        [XArrayItem(Name="properties", Type=typeof(JobPropertyGroup))]  
        public IList<JobPropertyGroup> PropertyGroups
        {
            get
            {
                return _propertyGroups;
            }
        }

        private List<TaskItemGroup> _itemGroups = new List<TaskItemGroup>();
        [XArray]
        [XArrayItem(Name="items", Type = typeof(TaskItemGroup))]   
        public IList<TaskItemGroup> ItemGroups
        {
            get
            {
                return _itemGroups;
            }
        }

    
    }


}
