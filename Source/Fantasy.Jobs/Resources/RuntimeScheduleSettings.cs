using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Fantasy.Configuration;
using System.Text.RegularExpressions;

namespace Fantasy.Jobs.Resources
{
    public  class RuntimeScheduleSetting
    {
        public override bool Equals(object obj)
        {
            return ComparsionHelper.DeepEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal DayRuntimeSetting[] DaySettings
        {
            get
            {
                return new DayRuntimeSetting[] { this.Sunday, this.Monday, this.Tuesday, this.Wednesday, this.Thursday, this.Friday, this.Saturday };
            }
        }


      


        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }

        
        [XmlElement("Monday")]
        public DayRuntimeSetting Monday { get; set; }

        [XmlElement("Tuesday")]
        public DayRuntimeSetting Tuesday { get; set; }

        [XmlElement("Wednesday")]
        public DayRuntimeSetting Wednesday { get; set; }

        [XmlElement("Thursday")]
        public DayRuntimeSetting Thursday { get; set; }

        [XmlElement("Friday")]
        public DayRuntimeSetting Friday { get; set; }

        [XmlElement("Saturday")]
        public DayRuntimeSetting Saturday { get; set; }

        [XmlElement("Sunday")]
        public DayRuntimeSetting Sunday { get; set; }



      
        public bool IsDisabledOrInPeriod(DateTime datetime)
        {
            if (this.Enabled)
            {
                DayRuntimeSetting settings = this.DaySettings[(int)datetime.DayOfWeek];
                if (settings != null)
                {
                    TimeSpan time = datetime.TimeOfDay;
                    foreach (TimeOfDayPeriod period in settings.Periods)
                    {
                        if (time >= period.Start && time <= period.End)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public Period GetPeriod(DateTime baseTime)
        {

            Period rs = null;
            TimeSpan aWeek = new TimeSpan(7, 0, 0, 0);
            foreach(Period period in GetPeriods(baseTime))
            {
                if (rs == null)
                {
                    rs = new Period() { Start = period.Start, End = period.End };
                }
                else
                {
                    if(period.Start == rs.End)
                    {
                        rs.End = period.End;

                        if (rs.End - rs.Start >= aWeek)
                        {
                            rs.End = DateTime.MaxValue;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }
            return rs;
        }

        private IEnumerable<Period> GetPeriods(DateTime baseTime)
        {
            DateTime baseDay = baseTime.Date; 

            for (int i = 0;; i++)
            {
                DayRuntimeSetting settings = this.DaySettings[((int)baseTime.DayOfWeek + i) % 7];
                if (settings != null)
                {
                    DateTime day = baseDay.AddDays(i);
                    foreach (TimeOfDayPeriod tp in settings.Periods)
                    {
                        DateTime end = day + tp.End;
                        if (end > baseTime)
                        {
                            yield return new Period() { Start = day + tp.Start, End = end };
                        }
                    }
                }
            }
        }
    }

    public class DayRuntimeSetting : IXmlSerializable
    {
        private List<TimeOfDayPeriod> _periods = new List<TimeOfDayPeriod>();
        public List<TimeOfDayPeriod> Periods
        {
            get
            {
                return _periods;
            }
        }

        public override bool Equals(object obj)
        {
            DayRuntimeSetting other = obj as DayRuntimeSetting;
            if (other != null && other.Periods.Count == this.Periods.Count)
            {
                for (int i = 0; i < this.Periods.Count; i++)
                {
                    if (this.Periods[i].Start != other.Periods[i].Start || this.Periods[i].End != other.Periods[i].End)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            this.Periods.Clear();
            reader.MoveToContent();
            string value = reader.ReadElementString();
            

            if (!string.IsNullOrWhiteSpace(value))
            {
                foreach (string periodText in value.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] times = periodText.Split('-');
                    if (times.Length == 2)
                    {
                        TimeOfDayPeriod period = new TimeOfDayPeriod();
                        period.Start = PaseTimeSpan(times[0]);
                        period.End = PaseTimeSpan(times[1]);
                        this.Periods.Add(period);
                    }
                    else
                    {
                        throw new FormatException("Invalid period format."); 
                    }

                }
            }
        }


        private static readonly Regex _timeSpanRegex = new Regex(@"^\s*(?<h>\d{1,2}):(?<m>\d{1,2}):(?<s>\d{1,2})\s*$");
        private static readonly TimeSpan _oneDay = new TimeSpan(1, 0, 0, 0);
        private TimeSpan PaseTimeSpan(string text)
        {
            Match match = _timeSpanRegex.Match(text);
            if (match.Success)
            {
                int h = Int32.Parse(match.Groups["h"].Value);
                int m = Int32.Parse(match.Groups["m"].Value);
                int s = Int32.Parse(match.Groups["s"].Value);

                TimeSpan rs = new TimeSpan(h, m, s);

                if (rs > _oneDay)
                {
                    throw new OverflowException("Period time must between 00:00:00 to 24:00:00");
                }

                return rs;

            }
            else
            {
                throw new FormatException("Invalid period time format.");
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            StringBuilder text = new StringBuilder();
            foreach (TimeOfDayPeriod period in this.Periods)
            {
                if (text.Length > 0)
                {
                    text.Append(';');
                }
                text.AppendFormat("{0:d2}:{1:d2}:{2:d2}-{3:d2}:{4:d2}:{5:d2}", period.Start.Days * 24 + period.Start.Hours, period.Start.Minutes, period.Start.Seconds,
                    period.End.Days * 24 + period.End.Hours, period.End.Minutes, period.End.Seconds);
            }
            writer.WriteString(text.ToString());
        }

        #endregion
    }

    public struct TimeOfDayPeriod
    {
        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }
    }

    public class Period
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Start, End);
        }
    }
}
