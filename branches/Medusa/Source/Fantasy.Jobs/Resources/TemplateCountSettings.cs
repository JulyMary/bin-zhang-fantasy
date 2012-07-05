using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Fantasy.Configuration;

namespace Fantasy.Jobs.Resources
{
    public class TemplateCountSettings 
    {
        public TemplateCountSettings()
        {
            Count = Int32.MaxValue;
        }

        [XmlAttribute("count")]
        public int Count { get; set; }

        private List<TemplateCountSetting> _templates = new List<TemplateCountSetting>();

        [XmlArray("templates"),
        XmlArrayItem(Type = typeof(TemplateCountSetting), ElementName = "template")]
        public List<TemplateCountSetting> Templates
        {
            get { return _templates;  }
           
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
