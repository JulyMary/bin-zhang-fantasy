using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Jobs.Web.Models
{
    public struct SplitTimeSpan
    {
       
        public int? Days { get; set; }

        public int? Hours { get; set; }

        public int? Minutes { get; set; }

        public int? Seconds { get; set; }

        public int? Milliseconds { get; set; }

        public bool IsNull
        {
            get
            {
                foreach (int? i in new int?[] { Days, Hours, Minutes, Seconds, Milliseconds })
                {
                    if (i != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static explicit  operator TimeSpan(SplitTimeSpan value)
        {
            return new TimeSpan(value.Days ?? 0, value.Hours ?? 0, value.Minutes ?? 0, value.Seconds ?? 0, value.Milliseconds ?? 0);
        }

        public static explicit operator TimeSpan?(SplitTimeSpan value)
        {
            if (!value.IsNull)
            {
                return (TimeSpan)value;
            }
            else
            {
                return null;
            }
        }

        public static explicit operator SplitTimeSpan(TimeSpan value)
        {
            return new SplitTimeSpan() { Days = value.Days, Hours = value.Hours, Minutes = value.Minutes, Seconds = value.Seconds, Milliseconds = value.Milliseconds };
        }

        public static explicit operator SplitTimeSpan(TimeSpan? value)
        {
            return value != null ? (SplitTimeSpan)(TimeSpan)value : new SplitTimeSpan();
        }

       

       
    }
}