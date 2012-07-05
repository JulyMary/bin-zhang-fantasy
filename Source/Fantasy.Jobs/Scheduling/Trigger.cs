using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract]
    //KnownType(typeof(DailyTrigger)) ]
    [XSerializable("trigger", NamespaceUri = Consts.ScheduleNamespaceURI)]
    public  class Trigger
    {
        public Trigger()
        {
            this.StartBoundary = DateTime.Now;
            this.StartTime = DateTime.Now.AddHours(1).TimeOfDay;  
            
        }

        [DataMember]
        [XAttribute("startBoundary")]
        public DateTime StartBoundary { get; set; }

        [DataMember]
        [XAttribute("endBoundary")]
        public DateTime? EndBoundary { get; set; }

        [DataMember]
        [XAttribute("executionTimeLimit")]
        public int? ExecutionTimeLimit { get; set; }

        [DataMember]
        [XElement("repetition")]
        public Repetition Repetition { get; set; }

        public virtual TriggerType Type { get; private set; }

        [DataMember]
        [XAttribute("startTime")]
        public TimeSpan StartTime { get; set; }

        [DataMember(Name="NextRunTime")]
        private DateTime? _nextRunTime;

        public DateTime? NextRunTime
        {
            get { return _nextRunTime; }
            internal set
            {
                _nextRunTime = value;
            }
        }

       

        [DataMember(Name="LastRunTime")]
        [XAttribute("lastRunTime")]
        private DateTime? _lastRunTime;

        public DateTime? LastRunTime
        {
            get { return _lastRunTime; }
            internal set
            {
                _lastRunTime = value;
            }
        }



        protected virtual string TriggerTimeDescription { get; private set; }


        public virtual string Description
        {
            get
            {
                StringBuilder rs = new StringBuilder();
                foreach (string s in new string[] { TriggerTimeDescription, RepetitionDescription, ExpriedDescription  })
                {

                    if (s != string.Empty)
                    {
                        if (rs.Length > 0)
                        {
                            rs.Append(" ");
                        }
                        rs.Append(s);
                    }
                }

                return rs.ToString();
            }
        }

        protected virtual string RepetitionDescription
        {
            get
            {
                StringBuilder rs = new StringBuilder();
                if (this.Repetition != null)
                {
                    rs.AppendFormat("After triggered repeat every {0}", ToDurationDescription(Repetition.Interval));
                    if (Repetition.Duration != null)
                    {
                        rs.AppendFormat(" for a duration of  {0}", ToDurationDescription((TimeSpan)Repetition.Duration));
                    }
                    rs.Append(".");
                }

                return rs.ToString();
            }
        }

        protected virtual string ExpriedDescription
        {
            get
            {
                return this.EndBoundary != null ? String.Format("Trigger expires at {0}.", this.EndBoundary) : String.Empty;
            }
        }


        protected string ToDurationDescription(TimeSpan timespan)
        {

            string day;
            switch (timespan.Days)
            {
                case 1:
                    day = " 1 day";
                    break;
                case 0:
                    day = "";
                    break;
                default:
                    day = timespan.Days.ToString() + " days";
                    break;
            }

            string hour;
            switch (timespan.Hours)
            {
                case 1:
                    hour = "1 hour";
                    break;
                case 0:
                    hour = "";
                    break;

                default:
                    hour = timespan.Hours.ToString() + " hours";
                    break;
            }


            string minutes;
            switch (timespan.Minutes)
            {
                case 1:
                    minutes = "1 minute";
                    break;
                case 0:
                    minutes = "";
                    break;
                default:
                    minutes = timespan.Minutes.ToString() + " minutes";
                    break;
            }


            string seconds;
            switch (timespan.Seconds)
            {
                case 1:
                    seconds = "1 second";
                    break;
                case 0:
                    seconds = "";
                    break;
                default:
                    seconds = timespan.Seconds.ToString() + " seconds";
                    break;
            }

            StringBuilder rs = new StringBuilder();
            foreach (string s in new string[] {day, hour, minutes, seconds })
            {

                if (s != string.Empty)
                {
                    if (rs.Length > 0)
                    {
                        rs.Append(" ");
                    }
                    rs.Append(s);
                }
            }

            return rs.ToString();
        }


    }
}
