using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Scheduling
{
    [XSerializable("schedule.data", NamespaceUri = Consts.ScheduleNamespaceURI)]
    internal class ScheduleData : ObjectWithSite
    {
        public ScheduleData()
        {
            this.History = new List<ScheduleEvent>();
            this.ExecutedCount = 0;
            this.NextRunTime = null;
        }

        [XAttribute("executedCount")]
        public int ExecutedCount;


        [XAttribute("name")]
        public string Name = null;

        [XArray(Name = "history")]
        [XArrayItem(Type = typeof(ScheduleEvent), Name = "event")]
        public List<ScheduleEvent> History { get; private set; }

        [XAttribute("nextRunTime")]
        public DateTime? NextRunTime = null;


        private DateTime? _lastRunTime;

        [XAttribute("expired")]
        public bool Expired = false;

        public ScheduleItem ScheduleItem { get; set; }

        public long ScheduleCookie { get; set; }

        #region Trig time evaluation
        public bool OnLoad()
        {
            bool rs = false;
            if (!this.Expired)
            {
                if (this.NextRunTime == null)
                {
                    EvalNext();
                    rs = true;
                }
                else if (this.ScheduleItem.StartWhenAvailable == false && this.NextRunTime < DateTime.Now)
                {
                    EvalNext();
                    rs = true;
                }
            }
            return rs;
        }

        public void EvalNext()
        {

            _lastRunTime = this.NextRunTime;
            DateTime? rs = null;

            if (this.ScheduleItem.Trigger != null)
            {
                if (ScheduleItem.Trigger.ExecutionTimeLimit != null && ExecutedCount >= ScheduleItem.Trigger.ExecutionTimeLimit)
                {
                    rs = null;
                }
                else
                {

                    DateTime? baseTime;

                    if (ScheduleItem.StartWhenAvailable)
                    {
                        baseTime = _lastRunTime != null ? _lastRunTime.Value.AddTicks(1) : ScheduleItem.Trigger.StartBoundary;
                    }
                    else
                    {
                        if (_lastRunTime == null || DateTime.Now > _lastRunTime)
                        {
                            baseTime = DateTime.Now;
                        }
                        else
                        {
                            baseTime = _lastRunTime.Value.AddTicks(1);
                        }
                    }

                    bool success = false;
                    do
                    {
                        baseTime = EvalNextDate((DateTime)baseTime);
                        if (baseTime != null)
                        {
                            DateTime b = (DateTime)baseTime;
                            success = EvalNextTime(ref b);
                            baseTime = b;
                        }
                    } while (baseTime != null && !success);

                    if (baseTime == null)
                    {
                        rs = null;
                    }
                    else
                    {
                        DateTime? endBoundary = this.ScheduleItem.Trigger.EndBoundary;
                        rs = endBoundary == null || endBoundary >= baseTime ? baseTime : null;
                    }
                }

                this.ExecutedCount++;
                this.NextRunTime = rs;
                this.ScheduleItem.Trigger.NextRunTime = rs;
                this.ScheduleItem.Trigger.LastRunTime = _lastRunTime;
            }
            if (rs == null)
            {
                this.ScheduleItem.Expired = this.Expired = true;
            }
        }

        private static TimeSpan MaxTimeOfDay = new TimeSpan(24, 0, 0);

        private bool EvalNextTime(ref DateTime baseTime)
        {
            bool rs = false;
            Trigger trigger = ScheduleItem.Trigger;
            Repetition repetition = trigger.Repetition;
            TimeSpan startTime = ScheduleItem.Trigger.StartTime;
            if (baseTime.TimeOfDay <= startTime)
            {
                baseTime = baseTime.Date + startTime;
                rs = true;
            }
            else if (repetition != null && repetition.Interval > TimeSpan.Zero)
            {
                TimeSpan boundary;
                if (repetition.Duration != null)
                {
                    boundary = startTime + (TimeSpan)repetition.Duration;
                    if (boundary > MaxTimeOfDay)
                    {
                        boundary = MaxTimeOfDay;
                    }
                }
                else
                {
                    boundary = MaxTimeOfDay;
                }

                double currentTicks = baseTime.TimeOfDay.Ticks;
                int count = (int)Math.Floor((currentTicks - (double)startTime.Ticks) / (double)repetition.Interval.Ticks) + 1;
                TimeSpan nextTime = new TimeSpan(repetition.Interval.Ticks * count + startTime.Ticks);
                if (nextTime < boundary)
                {
                    baseTime = baseTime.Date + nextTime;
                    rs = true;
                }
            }
            if (!rs)
            {
                baseTime = baseTime.Date.AddDays(1);
            }
            return rs;
        }

        private DateTime? EvalNextDate(DateTime baseTime)
        {
            switch (ScheduleItem.Trigger.Type)
            {
                case TriggerType.Time:
                    return EvalOneTime(baseTime);

                case TriggerType.Daily:
                    return EvalDaily(baseTime);

                case TriggerType.Weekly:
                    return EvalWeekly(baseTime);

                case TriggerType.Monthly:
                    return EvalMonthly(baseTime);

                case TriggerType.MonthlyDayOfWeek:
                    return EvalMonthlyDOW(baseTime);
                default:
                    throw new Exception("Absurd!");

            }
        }

        private DateTime? EvalMonthlyDOW(DateTime baseTime)
        {
            MonthlyDOWTrigger trigger = (MonthlyDOWTrigger)this.ScheduleItem.Trigger;

            if (trigger.MonthsOfYear.Length == 0 || (trigger.WeeksOfMonth.Length == 0 && trigger.RunOnLastWeekOfMonth == false))
            {
                return null;
            }
            DateTime? rs = null;
            do
            {
                DateTime? b = EvalMonthOfYear(baseTime, trigger.MonthsOfYear);
                if (b == null)
                {
                    return null;
                }
                else
                {
                    baseTime = (DateTime)b;
                }

                rs = EvalDayOfWeekOfMonth(baseTime, trigger);
                if (rs == null)
                {
                    baseTime = new DateTime(baseTime.Year, baseTime.Month, 1).AddMonths(1);
                }
            } while (rs == null);



            return rs;


        }

        private DateTime? EvalDayOfWeekOfMonth(DateTime baseTime, MonthlyDOWTrigger trigger)
        {
            DateTime? rs = null;
            int lastDOM = GetLastDayOfMonth(baseTime.Year, baseTime.Month);
            int day = -1;
            for (int i = baseTime.Day; i <= lastDOM; i++)
            {
                bool inWeeks = false;
                WeekOfMonth wom = (WeekOfMonth)(i / 7);
                inWeeks = Array.IndexOf(trigger.WeeksOfMonth, wom) >= 0;
                if (!inWeeks && trigger.RunOnLastWeekOfMonth && i > lastDOM - 7)
                {
                    inWeeks = true;
                }
                if (inWeeks)
                {
                    DateTime dt = new DateTime(baseTime.Year, baseTime.Month, i);
                    if (Array.IndexOf(trigger.DaysOfWeek, dt.DayOfWeek) >= 0)
                    {
                        day = i;
                        break;
                    }
                }

            }
            if (day != -1)
            {
                if (baseTime.Day < day)
                {
                    rs = baseTime.Date.AddDays(day - baseTime.Day);
                }
                else
                {
                    rs = baseTime;
                }
            }
            return rs;
        }

        private DateTime? EvalMonthly(DateTime baseTime)
        {
            MonthlyTrigger trigger = (MonthlyTrigger)this.ScheduleItem.Trigger;
            if (trigger.MonthsOfYear.Length == 0 || (trigger.DaysOfMonth.Length == 0 && trigger.RunOnLastDayOfMonth == false))
            {
                return null;
            }


            DateTime? rs;
            do
            {
                DateTime? b = EvalMonthOfYear(baseTime, trigger.MonthsOfYear);
                if (b == null)
                {
                    return null;
                }
                else
                {
                    baseTime = (DateTime)b;
                }
                rs = EvalDayOfMonth(baseTime, trigger);
                if (rs == null)
                {
                    baseTime = new DateTime(baseTime.Year, baseTime.Month, 1).AddMonths(1);
                }
            } while (rs == null);


            return rs;

        }

        private DateTime? EvalDayOfMonth(DateTime baseTime, MonthlyTrigger trigger)
        {
            DateTime? rs = null;


            int day = -1;
            int lastDOM = this.GetLastDayOfMonth(baseTime.Year, baseTime.Month);
            for (int i = baseTime.Day; i <= lastDOM; i++)
            {
                if (Array.IndexOf(trigger.DaysOfMonth, i) >= 0)
                {
                    day = i;
                    break;
                }
            }

            if (day == -1 && trigger.RunOnLastDayOfMonth)
            {
                day = lastDOM;
            }
            if (day != -1)
            {
                if (day != baseTime.Day)
                {
                    rs = baseTime.Date.AddDays(day - baseTime.Day);
                }
                else
                {
                    rs = baseTime;
                }
            }
            return rs;
        }

        private int GetLastDayOfMonth(int year, int month)
        {
            DateTime dt = new DateTime(year, month, 1);
            return dt.AddMonths(1).AddDays(-1).Day;

        }

        private DateTime? EvalMonthOfYear(DateTime baseTime, int[] monthsOfYear)
        {

            DateTime? rs = null;

            for (int i = 0; i < 12; i++)
            {
                int m = (baseTime.Month + i) % 12;
                if (m == 0)
                {
                    m = 12;
                }
                if (Array.IndexOf(monthsOfYear, m) >= 0)
                {
                    if (i > 0)
                    {
                        rs = new DateTime(baseTime.Year, baseTime.Month, 1).AddMonths(i);

                    }
                    else
                    {
                        rs = baseTime;
                    }
                    break;
                }
            }
            return rs;
        }

        private DateTime? EvalWeekly(DateTime baseTime)
        {
            WeeklyTrigger trigger = (WeeklyTrigger)this.ScheduleItem.Trigger;
            if (trigger.DaysOfWeek.Length == 0)
            {
                return null;
            }

            DateTime l = this._lastRunTime ?? trigger.StartBoundary;

            DateTime lastSunday = GetSunday(l);
            DateTime baseSunday = GetSunday(baseTime);

            int mod = ((int)(baseSunday.Date - lastSunday.Date).TotalDays / 7) % trigger.WeeksInterval;

            if (mod > 0)
            {
                baseTime = baseSunday.AddDays((trigger.WeeksInterval - mod) * 7);
            }

            DateTime? rs;
            do
            {
                rs = EvalDayOfWeek(baseTime, trigger.DaysOfWeek);
                if (rs == null)
                {
                    baseSunday = baseSunday.AddDays(7 * trigger.WeeksInterval);
                    baseTime = baseSunday;
                }
            } while (rs == null);
            return rs;

        }

        private DateTime? EvalDayOfWeek(DateTime baseTime, DayOfWeek[] daysOfWeek)
        {
            for (DayOfWeek day = baseTime.DayOfWeek; day <= DayOfWeek.Saturday; day++)
            {
                if (Array.IndexOf(daysOfWeek, day) >= 0)
                {
                    if (baseTime.DayOfWeek != day)
                    {
                        return baseTime.AddDays(day - baseTime.DayOfWeek).Date;
                    }
                    else
                    {
                        return baseTime;
                    }
                }
            }

            return null;
        }


        private DateTime GetSunday(DateTime baseTime)
        {
            return baseTime.Date.AddDays(-(int)baseTime.DayOfWeek);
        }

        private DateTime? EvalDaily(DateTime baseTime)
        {
            DailyTrigger trigger = (DailyTrigger)this.ScheduleItem.Trigger;
            DateTime rs;
            DateTime l = this._lastRunTime ?? trigger.StartBoundary;
            l = l.Date; 
            double lapsedDays = (baseTime.Date - l).TotalDays;
            double mod = lapsedDays % trigger.DaysInterval;
            if (mod > 0)
            {
                rs = baseTime.Date.AddDays(trigger.DaysInterval - mod);
            }
            else
            {
                rs = baseTime;
            }
            return rs;
        }

        private DateTime? EvalOneTime(DateTime baseTime)
        {
            TimeTrigger trigger = (TimeTrigger)this.ScheduleItem.Trigger;
            if (baseTime.Date < trigger.Date)
            {
                return trigger.Date;
            }
            else if (baseTime.Date == trigger.Date)
            {
                return baseTime;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Action Evaluation

        public void RunAction()
        {
            IJobQueue queue = this.Site.GetRequiredService<IJobQueue>();
            ILogger logger = this.Site.GetService<ILogger>();
            string waitAll = string.Empty; 
            if (this.ScheduleItem.MultipleInstance != InstancesPolicy.Parallel && this.History.Count > 0)
            {
                var query = from id in this.History.Last().CreatedJobs select new { Id = id, IsTerminated = queue.IsTerminated(id) };
                switch (this.ScheduleItem.MultipleInstance)
                {

                    case InstancesPolicy.Queue:
                        waitAll = string.Join(";", query.Where(j => !j.IsTerminated).Select(j => j.Id));
                        break;
                    case InstancesPolicy.IgnoreNew:
                        if (query.Any(j => !j.IsTerminated))
                        {
                            return;
                        }
                        break;
                    case InstancesPolicy.StopExisting:
                        foreach (var job in query.Where(j => !j.IsTerminated))
                        {
                            queue.Cancel(job.Id);
                            if (logger != null)
                            {
                                logger.LogMessage("Schedule", "Cancel job {0} because a new scheduled task will start", job.Id);
                            }
                        }
                        break;
                }
            }

            ScheduleEvent evt = new ScheduleEvent() { ScheduleTime = (DateTime)this.NextRunTime, ExecutionTime = DateTime.Now };
            List<Guid> ids = new List<Guid>();
            foreach (string xml in this.CreateStartInfo(waitAll))
            {

                JobMetaData meta = queue.CreateJobMetaData();
                meta.LoadXml(xml);
                queue.ApplyChange(meta);
                ids.Add(meta.Id);

            }
            evt.CreatedJobs = ids.ToArray();

            this.History.Add(evt);
        }


        private IEnumerable<string> CreateStartInfo(string waitAll)
        {

            if (this.ScheduleItem.Action != null)
            {
                XNamespace ns = Consts.ScheduleNamespaceURI;
                string name = this.ScheduleItem.Name.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last(); 
                XElement @params = new XElement(ns + "params",
                    new XElement(ns + "scheduleTime", this.NextRunTime.ToString()),
                    new XElement(ns + "author", this.ScheduleItem.Author),
                    new XElement(ns + "runTime", DateTime.Now.ToString()),
                    new XElement(ns + "name", name),
                    new XElement(ns + "fullname", this.ScheduleItem.Name),
                    new XElement(ns + "priority", this.ScheduleItem.Priority),
                    new XElement(ns + "waitAll", waitAll));
                if (this.ScheduleItem.CustomParams != null)
                {
                    XElement customParams = XElement.Parse(this.ScheduleItem.CustomParams);
                    @params.Add(customParams);
                }

                
                switch (this.ScheduleItem.Action.Type)
                {
                    case ActionType.Template:
                        return this.TemplateCreateStartInfo(@params);
                    case ActionType.Inline:
                        return this.InlineCreateStartInfo(@params);
                    case ActionType.Custom:
                        return this.CustomCreateStartInfo(@params);
                    default:
                        throw new Exception("Absurd!");
                }
            }
            else
            {
                return new string[0];
            }
        }

        private IEnumerable<string> CustomCreateStartInfo(XElement @params)
        {
            CustomAction action = (CustomAction)this.ScheduleItem.Action;
            Type t = Type.GetType(action.CustomType, true);
            ICustomJobBuilder builder = (ICustomJobBuilder)Activator.CreateInstance(t);
            if (builder is IObjectWithSite)
            {
                ((IObjectWithSite)builder).Site = this.Site;
            }
            return builder.Execute(@params);
        }


        private IEnumerable<string> Trasform(string xsltString, XElement @params)
        {
            XsltSettings xsltSettings = new XsltSettings()
            {
                EnableDocumentFunction = true,
                EnableScript = true
            };
            XslCompiledTransform xslt = new XslCompiledTransform();
            XElement root = XElement.Parse(xsltString);
            xslt.Load(root.CreateReader(), xsltSettings, null);
            Stream stream = new MemoryStream();
            xslt.Transform(@params.CreateReader(), null, stream);
            stream.Seek(0, SeekOrigin.Begin);
            XElement rs = XElement.Load(stream);
            XNamespace ns = Consts.XNamespaceURI;
            if (rs.Name == ns + "jobStartList")
            {
                foreach (XElement si in rs.Elements(ns + "jobStart"))
                {
                    yield return si.ToString(SaveOptions.OmitDuplicateNamespaces);
                }
            }
            else if (rs.Name == ns + "jobStart")
            {
                yield return rs.ToString(SaveOptions.OmitDuplicateNamespaces);
            }

        }

        private IEnumerable<string> InlineCreateStartInfo(XElement @params)
        {
            InlineAction action = (InlineAction)this.ScheduleItem.Action;
            return this.Trasform(action.Xslt, @params);

        }

        private IEnumerable<string> TemplateCreateStartInfo(XElement @params)
        {
            IScheduleLibraryService svc = this.Site.GetRequiredService<IScheduleLibraryService>();
            TemplateAction action = (TemplateAction)this.ScheduleItem.Action;
            string xslt = svc.GetTemplate(action.Template);
            return this.Trasform(xslt, @params);
        }
        #endregion

    }
}
