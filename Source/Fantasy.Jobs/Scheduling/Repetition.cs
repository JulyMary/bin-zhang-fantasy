using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    [XSerializable("repetition", NamespaceUri=Consts.ScheduleNamespaceURI)]
    public class Repetition
    {
        [DataMember]
        [XAttribute("duration")]
        public TimeSpan? Duration { get; set; }

        [DataMember]
        [XAttribute("interval")]
        public TimeSpan Interval { get; set; }
    }
}
