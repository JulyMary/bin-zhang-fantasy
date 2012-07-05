using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Scheduling;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Resources
{
    public class WaitTimeResourceProvider : ObjectWithSite, IResourceProvider 
    {

        private List<DateTime> _queue = new List<DateTime>();

        private object _syncRoot = new object();

        private IScheduleService _scheduleService;

        private long _scheduleCookie = -1;

        #region IResourceProvider Members

        public bool CanHandle(string name)
        {
            return string.Equals("WaitTime", name, StringComparison.OrdinalIgnoreCase);
        }

        public void Initialize()
        {
            _scheduleService = this.Site.GetRequiredService<IScheduleService>(); 
        }

        public bool IsAvailable(ResourceParameter parameter)
        {
            DateTime time = DateTime.Parse(parameter.Values["time"]);
            if (time <= DateTime.Now)
            {
                return true;
            }
            else
            {
                this.RegisterTime(time);
                return false;
            }
        }


        private void OnTime()
        {
            ILogger logger = this.Site.GetService<ILogger>();
            DateTime fireTime = DateTime.MinValue;
            lock (_syncRoot)
            {
                while (this._queue.Count > 0 && this._queue[0] < DateTime.Now)
                {
                    fireTime = this._queue[0]; 
                    this._queue.RemoveAt(0);
                }
                
                if (this._queue.Count > 0)
                {
                    this._scheduleCookie = _scheduleService.Register(_queue[0], OnTime);
                }
                else
                {
                    this._scheduleCookie = -1;
                }

            }
            logger.SafeLogMessage("WaitTime", "Requested WaitTime before {0} are available.", fireTime); 
            this.OnAvailable();
        }

        public bool Request(ResourceParameter parameter, out object resource)
        {
            
            bool rs;
            resource = null;
            DateTime time = DateTime.Parse(parameter.Values["time"]);
            if (time > DateTime.Now)
            {
               
                rs = false;
                RegisterTime(time);
            }
            else
            {
                rs = true;
            }
            return rs;
        }

        private void RegisterTime(DateTime time)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            lock (this._syncRoot)
            {
                int pos = this._queue.BinarySearch(time);
                if (pos < 0)
                {
                    pos = ~pos;
                    this._queue.Insert(pos, time);
                    if (pos == 0)
                    {
                        if (this._scheduleCookie != -1)
                        {
                            _scheduleService.Unregister(this._scheduleCookie);
                        }

                        this._scheduleCookie = _scheduleService.Register(time, OnTime);

                    }
                }

                logger.SafeLogMessage("WaitTime", "Register wait time {0}", time);
            }
        }

        public void Release(object resource)
        {
            
        }

        private void OnAvailable()
        {
            if (this.Available != null)
            {
                ILogger logger = this.Site.GetService<ILogger>();
                //logger.SafeLogMessage("WaitTime", "OnAvailable called.");
                this.Available(this, EventArgs.Empty);
            }
        }

        public event EventHandler Available;

        public event EventHandler<ProviderRevokeArgs> Revoke { add { } remove { } }

        #endregion
    }
}
