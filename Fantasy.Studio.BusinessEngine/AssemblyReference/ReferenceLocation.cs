using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    [XmlRoot("reference")]
    public class ReferenceLocation
    {
        [XmlAttribute("id")]
        public Guid ReferenceId { get; set; }

        [XmlAttribute("originPath")]
        public string OriginPath { get; set; }
    }
}
