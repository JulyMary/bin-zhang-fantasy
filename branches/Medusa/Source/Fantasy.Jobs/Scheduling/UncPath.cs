using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    [XSerializable("uncPath", NamespaceUri = Consts.ScheduleNamespaceURI)]
    public class UncPath
    {
        [DataMember]
        [XAttribute("path")]
        public string Path { get; set; }

        [DataMember]
        [XAttribute("username")]
        public string UserName { get; set; }

        [DataMember]
        [XAttribute("password")]
        public string Password { get; set; }
    }
}
