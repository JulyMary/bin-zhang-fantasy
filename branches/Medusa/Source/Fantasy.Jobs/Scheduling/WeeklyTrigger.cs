using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;
using System.Globalization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    [XSerializable("weeklyTrigger", NamespaceUri = Consts.ScheduleNamespaceURI)]
    public class WeeklyTrigger : Trigger 
    {
        [DataMember]
        [XAttribute("daysOfWeek")]
        public DayOfWeek[] DaysOfWeek { get; set; }

        [DataMember]
        [XAttribute("weeksInterval")]
        public int WeeksInterval { get; set; }

        public WeeklyTrigger()
        {
            this.DaysOfWeek = new DayOfWeek[0]; 
            WeeksInterval = 1;
        }
       
        public override TriggerType Type
        {
            get { return TriggerType.Weekly; }
        }

        protected override string TriggerTimeDescription
        {
            get
            {
                 DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
                 DateTime time = new DateTime(this.StartTime.Ticks);
                return string.Format("At {0:t} every {1} of every {2} {3}, starting {4:g}.",time,
                    string.Join(",", DaysOfWeek.Select(d=>info.GetDayName(d))), WeeksInterval, 
                    WeeksInterval > 1 ? "weeks" : "week", this.StartBoundary);   
            }
        }
    }
}
