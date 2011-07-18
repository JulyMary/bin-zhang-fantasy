using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Fantasy.ServiceModel
{
    [XmlRoot("address")]
    public class AddressSetting
    {
        public AddressSetting()
        {
            Port = -1;
        }

        [XmlAttribute("host")]
        public string Host { get; set; }

        [XmlAttribute("port")]
        public int Port { get; set; }

        [XmlArray("contracts"),
        XmlArrayItem("contract", Type = typeof(string))]
        public string[] Contracts { get; set; }
    }
}
