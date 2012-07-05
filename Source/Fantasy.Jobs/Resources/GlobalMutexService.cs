using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Fantasy.Jobs.Scheduling;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Resources
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Multiple, Namespace=Consts.JobServiceNamespaceURI)] 
    public class GlobalMutexService : WCFSingletonService, IGlobalMutexService
    {

        private List<Mutex> _allocated = new List<Mutex>();
        private object _syncRoot = new object();
        private IScheduleService _scheduleService;
        private IJobDispatcher _jobDispatcher;

        public override void InitializeService()
        {
            _scheduleService = this.Site.GetRequiredService<IScheduleService>();
            _jobDispatcher = this.Site.GetService<IJobDispatcher>();
            base.InitializeService();
        }

        private class Mutex
        {
            public string Name { get; set; }

            public long Cookie { get; set; }
        }


        #region IGlobalMutexService Members

        public bool IsAvaiable(string key)
        {
            lock (_syncRoot)
            {
                return ! this._allocated.Any(m => String.Equals(key, m.Name, StringComparison.OrdinalIgnoreCase)); 
            }
        }

        public bool Request(string key, TimeSpan timeout)
        {
            lock (_syncRoot)
            {
                Mutex mutex =  this._allocated.Find(m => String.Equals(key, m.Name, StringComparison.OrdinalIgnoreCase));
                if (mutex == null)
                {
                    mutex = new Mutex() { Name = key };
                    mutex.Cookie = _scheduleService.Register(DateTime.Now + timeout, () => { this.Release(mutex); });
                    this._allocated.Add(mutex);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void Release(Mutex mutex)
        {
            lock (_syncRoot)
            {
                _allocated.Remove(mutex);
            }
            if (_jobDispatcher != null)
            {
                _jobDispatcher.TryDispatch();
            }

        }

        public void Release(string key)
        {
            lock (_syncRoot)
            {
                Mutex mutex = this._allocated.Find(m => String.Equals(key, m.Name, StringComparison.OrdinalIgnoreCase));
                if (mutex != null)
                {
                    this.Release(mutex);
                    _scheduleService.Unregister(mutex.Cookie);
                }
            }
        }

        #endregion
    }
}
