using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract(Name="daily")]
    [XSerializable("dailyTrigger", NamespaceUri=Consts.ScheduleNamespaceURI)]
    public class DailyTrigger : Trigger 
    {
        public DailyTrigger()
        {
            DaysInterval = 1;
        }

        [DataMember]
        [XAttribute("daysInterval")]
        public int DaysInterval { get; set; }

        public override TriggerType Type
        {
            get { return TriggerType.Daily; }
        }

        protected override string TriggerTimeDescription
        {
            get 
            {
                DateTime time = new DateTime(this.StartTime.Ticks);
                return string.Format("At {0:t} {1}, starting {2:g}.", 
                    time, DaysInterval > 1 ? string.Format("every {0} days", DaysInterval) : "everyday"
                    , this.StartBoundary); 
            }
        }
    }
}
