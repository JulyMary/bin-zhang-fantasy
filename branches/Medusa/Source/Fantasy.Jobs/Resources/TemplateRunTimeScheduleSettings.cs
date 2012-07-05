using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Fantasy.Configuration;

namespace Fantasy.Jobs.Resources
{
    public class TemplateRuntimeScheduleSettings : RuntimeScheduleSetting
    {
        private List<TemplateRuntimeScheduleSetting> _templates = new List<TemplateRuntimeScheduleSetting>();

        [XmlArray("templates"),
        XmlArrayItem(ElementName="template", Type = typeof(TemplateRuntimeScheduleSetting))]
        public List<TemplateRuntimeScheduleSetting>  Templates
        {
            get { return _templates; }
        }



      
    }


    public class TemplateRuntimeScheduleSetting : RuntimeScheduleSetting
    {

        [XmlAttribute("name")]
        public string Name { get; set; }



       
    }
}
