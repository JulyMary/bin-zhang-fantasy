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
    [XSerializable("monthlyTrigger", NamespaceUri=Consts.ScheduleNamespaceURI)]
    public class MonthlyTrigger : Trigger 
    {
        [DataMember]
        [XAttribute("daysOfMonth")]
        public int[] DaysOfMonth { get; set; }

        [DataMember]
        [XAttribute("monthsOfYear")]
        public int[] MonthsOfYear { get; set; }

        [DataMember]
        [XAttribute("runOnLastDayOfMonth")]
        public bool RunOnLastDayOfMonth { get; set; }


        public MonthlyTrigger()
        {
            this.DaysOfMonth = new int[0];
            this.MonthsOfYear = new int[0];
        }


        public override TriggerType Type
        {
            get { return TriggerType.Monthly; }
        }


        private IEnumerable<string> DayNames()
        {
            foreach (int d in DaysOfMonth)
            {
                yield return d.ToString();
            }

            if (this.RunOnLastDayOfMonth)
            {
                yield return "last";
            }
        }

        private IEnumerable<string> MonthNames()
        {
            DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
            foreach (int m in MonthsOfYear)
            {
                yield return info.GetMonthName(m);
            }
        }

        protected override string TriggerTimeDescription
        {
            get
            {
                DateTime time = new DateTime(this.StartTime.Ticks);
                return string.Format("At {0:t} on day {1} of {2}, starting {3:g}.",time,
                    string.Join(",", DayNames()),string.Join(",", MonthNames()), this.StartBoundary );
            }
        }
    }
}
