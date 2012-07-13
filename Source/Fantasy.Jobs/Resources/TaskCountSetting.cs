﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Fantasy.Configuration;

namespace Fantasy.Jobs.Resources
{
    [XmlRoot("task")]
    public class TaskCountSetting
    {
        [XmlAttribute("namespace")]
        public string Namespace { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("count")]
        public int Count { get; set; }


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