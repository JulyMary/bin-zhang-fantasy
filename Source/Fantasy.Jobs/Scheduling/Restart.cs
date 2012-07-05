using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    [XSerializable("restart", NamespaceUri = Consts.ScheduleNamespaceURI)]
    public class Restart
    {
        [DataMember]
        [XAttribute("count")]
        public int Count { get; set; }
        
        [DataMember]
        [XAttribute("duration")]
        public TimeSpan Duration { get; set; }

    }
}
