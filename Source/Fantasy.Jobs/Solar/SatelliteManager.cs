using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Fantasy.Jobs.Solar
{
    public class SatelliteManager : AbstractService
    {
        private IJobDispatcher _dispatcher;

        private object _syncRoot = new object();

        private List<ActionData> _actionQueue = new List<ActionData>();

        private Thread _refreshThread;
        private Thread _actionThread;
        

        public override void InitializeService()
        {
            this._dispatcher = this.Site.GetRequiredService<IJobDispatcher>();

            _refreshThread = ThreadFactory.CreateThread(this.Refresh).WithStart();
            
            _actionThread = ThreadFactory.CreateThread(this.RetryActions).WithStart();
           

            base.InitializeService();
        }


        public override void UninitializeService()
        {
            this._actionThread.Abort();
            this._refreshThread.Abort();
            _actionThread.Join();
            _refreshThread.Join();
            

            Parallel.ForEach(this._satellites, site => 
            {
                try
                {
                    site.Satellite.RequestSuspendAll();
                }
                catch
                {

                }
            }); 

            
            base.UninitializeService();
        }
       

        public bool SafeCallSatellite<T>(string name, Func<ISatellite, T> function, out T value)
        {
            bool rs = false;
            SatelliteSite site = null;
            value = default(T);
            try
            {

                site = this._satellites.Find(s => name == s.Name);
                if (site != null)
                {
                    value = function(site.Satellite);
                }

                rs = true;
                
            }
            catch
            {
                
                
            }
            return rs;
        }

        public void Enqueue(string name, object state, Action<ISatellite, object> action, Action<object> failAction)
        {

            ActionData data = new ActionData() { Action = action, State=state, Satellite = name, FailAction = failAction, RetryCount = 10 };

            if (!TryCallAction(data))
            {
                lock (this._actionQueue)
                {
                    this._actionQueue.Add(data);
                }
            }
            
        }

        private bool TryCallAction(ActionData data)
        {
            bool rs = false;
            try
            {
                SatelliteSite site = this._satellites.Find(s => s.Name == data.Satellite);
                if (site != null)
                {
                    data.Action(site.Satellite, data.State);
                    rs = true;
                }


            }
            catch
            {
            }
            return rs;

        }


        public bool IsValid(ISatellite satellite)
        {
            lock (_syncRoot)
            {
                return this._satellites.Find(s => satellite  == s.Satellite) != null;
            }
        }


        public void UpdateEchoTime(string name)
        {
            SatelliteSite site = this._satellites.Find(s => name == s.Name);
            if (site != null)
            {
                site.LastEchoTime = DateTime.Now;
            }
        }

        public void RegisterSatellite(string name, ISatellite satellite)
        {

            lock (_syncRoot)
            {
                SatelliteSite site = this._satellites.Find(s => name == s.Name);
                if (site != null)
                {
                    this._satellites.Remove(site);
                }
                site = new SatelliteSite() { Name = name, Satellite = satellite, LastEchoTime=DateTime.Now };
                this._satellites.Add(site);
            }

            this._dispatcher.TryDispatch();

            ILogger logger = this.Site.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogMessage("SatelliteManager", "Satellite {0} is connected", name);
            }


        }

        public void UnregisterSatellite(ISatellite satellite)
        {
            lock (_syncRoot)
            {
                SatelliteSite site = this._satellites.Find(s => satellite  == s.Satellite);
                if (site != null)
                {
                    this._satellites.Remove(site);
                }
                this._dispatcher.TryDispatch();
                ILogger logger = this.Site.GetService<ILogger>();
                if (logger != null)
                {
                    logger.LogMessage("SatelliteManager", "Satellite {0} is disconnected", site.Name);
                }
            }
        }


        private List<SatelliteSite> _satellites = new List<SatelliteSite>();

        public ICollection<SatelliteSite> Satellites
        {
            get
            {
                lock (_syncRoot)
                {
                    return new List<SatelliteSite>(_satellites);
                }
            }
        }


        private void Refresh()
        {
            TimeSpan timeout = new TimeSpan(0, 0, 15);
            ILogger logger = this.Site.GetService<ILogger>();
            while (true)
            {
                Thread.Sleep(15 * 1000);
                List<SatelliteSite> list;
                lock (_syncRoot)
                {
                    list = new List<SatelliteSite>(_satellites);
                }

                foreach (SatelliteSite site in list)
                {
                    try
                    {
                        if (DateTime.Now - site.LastEchoTime > timeout)
                        {
                            site.Satellite.Echo();
                            site.LastEchoTime = DateTime.Now;
                        }
                    }
                    catch(Exception error)
                    {
                        if (WCFExceptionHandler.CanCatch(error))
                        {
                            lock (_syncRoot)
                            {
                                _satellites.Remove(site);
                            }

                            if (logger != null)
                            {
                                logger.LogError("Solar", error, "WCF error");

                                logger.LogWarning("SatelliteManager", "Satellite {0} is stop working, remove it from satellite manager.", site.Name);
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                
            }
        }


        private void RetryActions()
        {
            while (true)
            {

                Thread.Sleep(60 * 1000);
                List<ActionData> list;
                lock (_actionQueue)
                {
                    list = new List<ActionData>(this._actionQueue);
                }

                foreach (ActionData data in list)
                {
                    bool success = this.TryCallAction(data);
                    if (!success)
                    {
                        data.RetryCount--;
                        if (data.RetryCount == 0 && data.FailAction != null)
                        {
                            try
                            {
                                data.FailAction(data.State);
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (success || data.RetryCount == 0)
                    {
                        lock (_actionQueue)
                        {
                            _actionQueue.Remove(data);
                        }
                    }

                   
                }

            }
        }

        private class ActionData
        {
            public string Satellite;

            public Action<ISatellite, object> Action;

            public Action<object> FailAction;

            public object State;

            public int RetryCount;
        }
    }

    public class SatelliteSite
    {
        public string Name { get; set; }

        public ISatellite Satellite { get; set; }

        public DateTime LastEchoTime { get; set; }
    }
}
