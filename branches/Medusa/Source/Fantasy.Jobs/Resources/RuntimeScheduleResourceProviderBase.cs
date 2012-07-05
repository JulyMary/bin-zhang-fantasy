using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Scheduling;
using Fantasy.Jobs.Management;
using System.Threading;
using Microsoft.Win32;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Resources
{
    public abstract class RuntimeScheduleResourceProviderBase : ObjectWithSite, IResourceProvider
    {

        private object _syncRoot = new object();

         

        #region IResourceProvider Members

        public  bool CanHandle(string name)
        {
            return InternalCanHandle(name);
        }


        protected abstract bool InternalCanHandle(string name);
      

        protected IScheduleService _scheduleService;

        public virtual void Initialize()
        {
            _scheduleService = this.Site.GetRequiredService<IScheduleService>();
            SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);
        }

        void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogMessage("ScheduleResourceProvider", "System time changed. Rescheduling.");
            }

            this.Reschedule();
        }
       

        protected abstract RuntimeScheduleSetting GetSetting(string id);
        protected abstract string GetResourceId(ResourceParameter parameter);

        public bool IsAvailable(ResourceParameter parameter)
        {
            string id = this.GetResourceId(parameter);
            RuntimeScheduleSetting setting = this.GetSetting(id);
            if (setting.IsDisabledOrInPeriod(DateTime.Now) )
            {
                return true;
            }
            else
            {
                return false;
            }

        }

       

        public bool Request(ResourceParameter parameter, out object resource)
        {
            
            bool rs = false;
            lock (_syncRoot)
            {
                resource = null;
                string id = this.GetResourceId(parameter);
                RuntimeScheduleSetting setting = this.GetSetting(id);
                Resource res = this.FindOrCreateResource(id);
                if (setting.IsDisabledOrInPeriod(DateTime.Now))
                {
                    rs = true;
                    resource = res;
                }
            }
            return rs;
        }

        private Resource FindOrCreateResource(string id)
        {
            lock (_syncRoot)
            {
                Resource rs = this._resources.Find(r => r.Id == id);
                if (rs == null)
                {
                    rs = new Resource() { Id = id };

                    this._resources.Add(rs);

                    CreateSchedule(id);
                }


                return rs;
            }
        }

        private ScheduledResource CreateSchedule(string id)
        {
            RuntimeScheduleSetting setting = this.GetSetting(id);
            ScheduledResource rs = new ScheduledResource() { Id = id, ScheduleSetting = setting };
            this._scheduledResources.Add(rs);
            if (setting.Enabled)
            {
                DateTime now = DateTime.Now;
                Period period = setting.GetPeriod(DateTime.Now);
                if (period != null)
                {
                    if (now < period.Start)
                    {
                        this.RegisterPeriodStart(rs, period);
                    }
                    else
                    {
                        this.RegisterPeriodEnd(rs, period.End);
                    }
                }
            }
            return rs;
        }

        private void RegisterPeriodStart(ScheduledResource schedule, Period period)
        {

            long cookie = this._scheduleService.Register(period.Start, () => 
            {
                this.RegisterPeriodEnd(schedule, period.End);

                Thread.Sleep(1);
                this.OnAvailable();
            });
            schedule.Cookie = cookie;
            
        }

        private void RegisterPeriodEnd(ScheduledResource schedule, DateTime endTime)
        {

           
            RuntimeScheduleSetting setting = schedule.ScheduleSetting; 
            long cookie = this._scheduleService.Register(endTime, () =>
            {
                Period period = setting.GetPeriod(endTime.AddTicks(1));
                this.RegisterPeriodStart(schedule, period);

                Resource res = this.FindOrCreateResource(schedule.Id);
                this.OnRevoke(res);
                
            });
            schedule.Cookie = cookie;
        }


        protected void Reschedule()
        {
           
            bool available = false;
            List<Resource> expiredResources = new List<Resource>();
            lock (_syncRoot)
            {
                foreach (ScheduledResource schedule in this._scheduledResources)
                {
                    if (schedule.Cookie > 0)
                    {
                        _scheduleService.Unregister(schedule.Cookie);
                        schedule.Cookie = 0;
                    }
                }

                this._scheduledResources.Clear();


                DateTime now = DateTime.Now;

                foreach (Resource resource in this._resources)
                {
                    ScheduledResource schedule = this.CreateSchedule(resource.Id);
                    if (schedule.ScheduleSetting.IsDisabledOrInPeriod(now))
                    {
                        available = true;
                    }
                    else
                    {
                        expiredResources.Add(resource);
                    }
                }
            }

            foreach (Resource res in expiredResources)
            {
                this.OnRevoke(res);
            }

            if (available)
            {
                this.OnAvailable();
            }


        }


        public virtual void Release(object resource)
        {
            
        }

        protected virtual void OnAvailable()
        {
            if (this.Available != null)
            {
                this.Available(this, EventArgs.Empty);
            }
        }

        public event EventHandler Available;

        protected virtual void OnRevoke(Resource res)
        {
            if (this.Revoke != null)
            {
                this.Revoke(this, new ProviderRevokeArgs() { Resource = res });
            }
        }


       

        public event EventHandler<ProviderRevokeArgs> Revoke;

        #endregion

        private List<Resource> _resources = new List<Resource>();

        private List<ScheduledResource> _scheduledResources = new List<ScheduledResource>();


        protected class Resource
        {
            public string Id {get;set;}
        }

        protected class ScheduledResource
        {
            public long Cookie { get; set; }

            public RuntimeScheduleSetting ScheduleSetting { get; set; }

            public string Id { get; set; }
        }
    }
}
