using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Jobs.Scheduling;
using System.Collections;
using System.Text;
using System.IO;

namespace Fantasy.Jobs.Web.Models
{
    public class ScheduleModel
    {

        public ScheduleModel()
        {
            this.DaysInterval = 1;
            this.WeeksInterval = 1;

        }

        public bool IsInArray(string array, object value)
        {
            int v = (int)value;
            if (string.IsNullOrEmpty(array))
            {
                return false;
            }
            else
            {
                var query = from s in array.Split(' ')
                            let i = Int32.Parse(s)
                            where v == i
                            select i;
                return query.Any();
            }
        }

        public int[] ToArray(string array)
        {
            if (string.IsNullOrEmpty(array))
            {
                return new int[0];
            }
            else
            {
                var query = from s in array.Split(' ')
                            let i = Int32.Parse(s)
                            select i;
                return query.ToArray();
            }
        }

        public string ToString(Array array)
        {
            if (array == null || array.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder rs = new StringBuilder();
            foreach (int i in array)
            {
                if (rs.Length > 0)
                {
                    rs.Append(" ");
                }
                rs.Append(i);
            }
            return rs.ToString();
        }

        public void  LoadFromSchedule(ScheduleItem schedule)
        {
            this.Author = schedule.Author;
            this.Description = schedule.Description;
            this.Enabled = schedule.Enabled;
            this.Priority = schedule.Priority;
            this.StartWhenAvailable = schedule.StartWhenAvailable;
            this.CustomParams = schedule.CustomParams;
            this.MultipleInstance = schedule.MultipleInstance;
            if (schedule.Trigger != null)
            {
                this.StartTime = schedule.Trigger.StartBoundary;
                this.EndBoundary = schedule.Trigger.EndBoundary;
                this.ExecutionTimeLimit = schedule.Trigger.ExecutionTimeLimit;
                if (schedule.Trigger.Repetition != null)
                {
                    this.RepetitionInterval = (SplitTimeSpan)schedule.Trigger.Repetition.Interval;
                    this.RepetitionDuration = (SplitTimeSpan)schedule.Trigger.Repetition.Duration;
                }
                this.TriggerType = schedule.Trigger.Type;

                switch (this.TriggerType)
                {
                   
                    case TriggerType.Daily:
                        this.DaysInterval = ((DailyTrigger)schedule.Trigger).DaysInterval; 
                        break;
                    case TriggerType.Weekly:
                        {
                            WeeklyTrigger trigger = (WeeklyTrigger)schedule.Trigger;
                            this.DaysOfWeek = this.ToString(trigger.DaysOfWeek);
                            this.WeeksInterval = trigger.WeeksInterval;
                            break;
                        }
                    case TriggerType.Monthly:
                        {
                            MonthlyTrigger trigger = (MonthlyTrigger)schedule.Trigger;
                            this.MonthsOfYear = ToString(trigger.MonthsOfYear);
                            this.DaysOfMonth = ToString(trigger.DaysOfMonth);
                            this.RunOnLastDayOfMonth = trigger.RunOnLastDayOfMonth;
                            break;
                        }
                    case TriggerType.MonthlyDayOfWeek:
                        {
                            MonthlyDOWTrigger trigger = (MonthlyDOWTrigger)schedule.Trigger;
                            this.MonthsOfYear = ToString(trigger.MonthsOfYear);
                            this.DaysOfWeek = ToString(trigger.DaysOfWeek);
                            this.WeeksOfMonth = ToString(trigger.WeeksOfMonth);
                            this.RunOnLastWeekOfMonth = trigger.RunOnLastWeekOfMonth;
                            break;
                        }
                }
            }
        }


        public  ScheduleItem ToSchedule()
        {
            ScheduleItem rs = new ScheduleItem()
            {
                Author = this.Author,
                Description = this.Description,
                Enabled = this.Enabled,
                CustomParams = this.CustomParams,
                MultipleInstance = this.MultipleInstance,
                Priority = this.Priority,
                StartWhenAvailable = this.StartWhenAvailable
            };


            Trigger trigger = null;

            switch (this.TriggerType)
            {
                case TriggerType.Time:
                    {
                        trigger = new TimeTrigger()
                        {
                            Date = this.StartTime.Date
                        };
                       
                        break;
                    }
                case TriggerType.Daily:
                    {
                        trigger = new DailyTrigger()
                        {
                            DaysInterval = this.DaysInterval
                        };
                       
                        break;
                    }
                case TriggerType.Weekly:
                    {
                        trigger = new WeeklyTrigger()
                        {
                            DaysOfWeek = this.ToArray(this.DaysOfWeek).Select(i => (DayOfWeek)i).ToArray(),
                            WeeksInterval = this.WeeksInterval
                        };
                        
                        break;
                    }
                case TriggerType.Monthly:
                    {
                        trigger = new MonthlyTrigger()
                        {
                            DaysOfMonth = this.ToArray(this.DaysOfMonth),
                            MonthsOfYear = this.ToArray(this.MonthsOfYear),
                            RunOnLastDayOfMonth = this.RunOnLastDayOfMonth
                        };
                       
                        break;
                    }
                case TriggerType.MonthlyDayOfWeek:
                    {
                        trigger = new MonthlyDOWTrigger()
                        {
                            DaysOfWeek = this.ToArray(this.DaysOfWeek).Select(i => (DayOfWeek)i).ToArray(),
                            WeeksOfMonth = this.ToArray(this.WeeksOfMonth).Select(w => (WeekOfMonth)w).ToArray(),
                            MonthsOfYear = this.ToArray(this.MonthsOfYear),
                            RunOnLastWeekOfMonth = this.RunOnLastWeekOfMonth 
                        };
                        break;
                    }
                    
            }

            trigger.StartBoundary = this.StartTime;
            trigger.EndBoundary = this.EndBoundary;
            trigger.ExecutionTimeLimit = this.ExecutionTimeLimit;
            if (!this.RepetitionInterval.IsNull)
            {
                trigger.Repetition = new Repetition() { Interval = (TimeSpan)this.RepetitionInterval, Duration = (TimeSpan?)this.RepetitionDuration}; 
            }
            trigger.StartTime = this.StartTime.TimeOfDay;
            rs.Trigger = trigger;


            Scheduling.Action action = null;

            switch (this.Action)
            {
                case ActionType.Template:
                    {
                        action = new TemplateAction() { Template = this.Template };
                        break;
                    }

                case ActionType.Inline:
                    {
                        action = new InlineAction() { Xslt = this.InlineXslt };
                        break;
                    }

                case ActionType.Custom:
                    {
                        action = new CustomAction() { CustomType = this.CustomActionType }; 
                        break;
                    }
            }
            rs.Action = action;
            return rs;
        }


        public bool IsNew { get; set; }

        public string ParentPath { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public int Priority { get; set; }

     
        //public Restart RestartOnFailure { get; set; }

        public bool StartWhenAvailable { get; set; }

        public string CustomParams { get; set; }

        public InstancesPolicy MultipleInstance { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndBoundary { get; set; }

        public int? ExecutionTimeLimit { get; set; }

        public SplitTimeSpan RepetitionInterval { get; set; }

        public SplitTimeSpan RepetitionDuration { get; set; }

        public TriggerType TriggerType { get; set; }

        public int DaysInterval { get; set; }

        public string DaysOfWeek { get; set; }

        public int WeeksInterval { get; set; }

        public string DaysOfMonth { get; set; }

        public string MonthsOfYear { get; set; }

        public bool RunOnLastDayOfMonth { get; set; }

        public string WeeksOfMonth { get; set; }

        public bool RunOnLastWeekOfMonth { get; set; }

        public ActionType Action { get; set; }

        public string Template { get; set; }

        public string[] AvailableTemplates { get; set; }

        public string InlineXslt { get; set; }

        public string CustomActionType { get; set; }
  
    }
}
