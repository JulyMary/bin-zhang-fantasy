using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    [XSerializable("event", NamespaceUri=Consts.ScheduleNamespaceURI)]
    public class ScheduleEvent
    {
        [DataMember]
        [XAttribute("exectionTime")] 
        public DateTime ExecutionTime { get; set; }

        [DataMember]
        [XAttribute("scheduleTime")]
        public DateTime ScheduleTime { get; set; }

        [DataMember]
        [XArray]
        [XArrayItem(Name="job", Type = typeof(Guid))] 
        public Guid[] CreatedJobs { get; set; }
    }
}
