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
    [XSerializable("monthlyDOWTrigger", NamespaceUri=Consts.ScheduleNamespaceURI)]
    public class MonthlyDOWTrigger : Trigger 
    {



        public override TriggerType Type
        {
            get { return TriggerType.MonthlyDayOfWeek; }
        }

        public MonthlyDOWTrigger()
        {
            this.MonthsOfYear = new int[0];
            this.WeeksOfMonth = new WeekOfMonth[0];
            this.DaysOfWeek = new DayOfWeek[0]; 
        }

        [DataMember]
        [XAttribute("daysOfWeek")]
        public DayOfWeek[] DaysOfWeek { get; set; }

        [DataMember]
        [XAttribute("monthsOfYear")]
        public int[] MonthsOfYear { get; set; }

        [DataMember]
        [XAttribute("runOnLastWeekOfMonth")]
        public bool RunOnLastWeekOfMonth { get; set; }

        [DataMember]
        [XAttribute("weeksOfMonth")]
        public WeekOfMonth[] WeeksOfMonth { get; set; }


        protected override string TriggerTimeDescription
        {
            get
            {
                DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
                DateTime time = new DateTime(this.StartTime.Ticks);
                return string.Format("At {0:t} on the {1} {2} each {3}, starting {4:g}.",time,
                    string.Join(",", WeeksOfMonth.Select(w=>w.ToString())), string.Join(",", DaysOfWeek.Select(d=>info.GetDayName(d))),
                    string.Join(",", MonthsOfYear.Select(m=>info.GetMonthName(m))), this.StartBoundary);
            }
        }
    }
}
