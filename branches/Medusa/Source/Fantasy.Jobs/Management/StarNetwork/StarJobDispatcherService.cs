using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using ClickView.ServiceModel;

namespace ClickView.Jobs.Management.StarNetwork
{
    public class StarJobDispatcherService : AbstractService, IJobDispatcher
    {
        public override void InitializeService()
        {
            this._queue = (IJobQueue)this.Site.GetService(typeof(IJobQueue));

            this._filters = TypesBuilderConfigurationHandler.CreateObjects<IJobStartupFilter>("clickview/job.startupFilters");

            foreach (IJobStartupFilter filter in this._filters)
            {
                if (filter is IObjectWithSite)
                {
                    ((IObjectWithSite)filter).Site = this.Site;
                }
            }
            base.InitializeService();
        }


        private Dictionary<Guid, string> _processControllers = new Dictionary<Guid, string>();


        private IJobQueue _queue;

        private IJobStartupFilter[] _filters;

        private bool _serviceStarted = false;

        public void Start()
        {
            _serviceStarted = true;
            Run();

            _queue.Added += new EventHandler<JobQueueEventArgs>(QueueChanged);
            _queue.Changed += new EventHandler<JobQueueEventArgs>(QueueChanged);
            _queue.RequestCancel += new EventHandler<JobQueueEventArgs>(QueueRequestCancel);
            _queue.RequestSuspend += new EventHandler<JobQueueEventArgs>(QueueRequestSuspend);
            _queue.RequestUserPause += new EventHandler<JobQueueEventArgs>(QueueRequestUserPause);
        }

        private IJobController GetControllerById(Guid id)
        {
            string ctrlId = this._processControllers.GetValueOrDefault(id, null);
            if (ctrlId != null)
            {
                IJobController rs = this._controllers.GetValueOrDefault(ctrlId, null);
                return rs;
            }
            else
            {
                return null;
            }
        }

        private void DoJobOperation(Guid id, Action<IJobController> action)
        {
            Task.Factory.StartNew(() =>
            {
                IJobController controller = this.GetControllerById(id);
                if (controller != null)
                {
                    try
                    {
                        action(controller);
                           
                    }
                    catch(Exception error)
                    {
                        WCFExceptionHandler.CatchKnowns(error);
                    }
                }
            });
        }
        

        void QueueRequestUserPause(object sender, JobQueueEventArgs e)
        {

            this.DoJobOperation(e.Job.Id, controller => { controller.UserPause(e.Job.Id); });
            
        }

        void QueueRequestSuspend(object sender, JobQueueEventArgs e)
        {
            this.DoJobOperation(e.Job.Id, controller => { controller.Suspend(e.Job.Id); });
        }

        void QueueRequestCancel(object sender, JobQueueEventArgs e)
        {
            this.DoJobOperation(e.Job.Id, controller => { controller.Cancel(e.Job.Id); });
        }

        void QueueChanged(object sender, JobQueueEventArgs e)
        {
            if (_starting == false)
            {
                if (e.Job.State != JobState.Running)
                {
                    this._processControllers.Remove(e.Job.Id);
                }

                Run();
            }
        }

        public void Run()
        {
            if (!_serviceStarted) return;
            while (TryStartAJob()) ;
        }


        private bool _starting = false;
        object _syncRoot = new object();
        private bool TryStartAJob()
        {
            if(this._controllers.Count == 0)
            {
                return false;
            }
            
            if (_starting == false)
            {
                _starting = true;

                try
                {

                    IEnumerable<JobMetaData> jobs = this._queue.Unterminates;
                    foreach (IJobStartupFilter filter in this._filters)
                    {
                        jobs = filter.Filter(jobs);
                    }

                    JobMetaData job = jobs.FirstOrDefault();
                    if (job != null)
                    {

                        int[] counts = this.GetSatelliteAvailableCounts();

                        List<KeyValuePair<string, ISatelliteJobController>>  controllers;
                        lock (this._controllers)
                        {
                            controllers = new List<KeyValuePair<string, ISatelliteJobController>>(this._controllers);
                        }

                        var query = controllers.Select((item, index) => new { index = index, controller = item.Value, id = item.Key })
                            .Where(item => counts[item.index] > 0).
                            OrderByDescending(item => counts[item.index]);
                        foreach (var item in query)
                        {
                            IJobController controller = item.controller;
                            try
                            {
                
                                if (job.State == JobState.Unstarted)
                                {
                                    controller.StartJob(job);
                                }
                                else
                                {
                                    controller.Resume(job);
                                }

                                this._processControllers.Add(job.Id, item.id);
                              
                                return true;
                            }
                            catch(Exception error)
                            {
                                WCFExceptionHandler.CatchKnowns(error);
                                this.UnregisterSatellite(item.id); 
                            }
                        }

                    }
                    return false;
                }
                finally
                {
                    _starting = false;
                }
            }
            else
            {
                return false;
            }

           
        }

        public int GetAvailableProcessCount()
        {
            int[] counts = GetSatelliteAvailableCounts();

            int rs = 0;
            foreach (int c in counts)
            {
                rs += c;
            }

            return rs;
         }

        private int[] GetSatelliteAvailableCounts()
        {
            List<KeyValuePair<string, ISatelliteJobController>> controllers;
            lock (this._controllers)
            {
                controllers = new List<KeyValuePair<string, ISatelliteJobController>>(this._controllers);
            }
            int[] counts = new int[controllers.Count];
            int timeout = 10 * 1000; // 10 seconds
            CancellationTokenSource cts = new CancellationTokenSource();

            try
            {
                using (Timer t = new Timer(_ => cts.Cancel(), null, timeout, -1))
                {
                    Parallel.For(0, counts.Length, new ParallelOptions { CancellationToken = cts.Token }, i =>
                    {
                        KeyValuePair<string, ISatelliteJobController> item = controllers[i];
                        try
                        {
                            counts[i] = item.Value.GetAvailableProcessCount();
                        }
                        catch
                        {
                            this.UnregisterSatellite(item.Key); 
                        }
                    });
                }
            }
            catch
            {
            }
            return counts;
        }


        private SortedDictionary<string, ISatelliteJobController> _controllers = new SortedDictionary<string, ISatelliteJobController>();

        public void RegisterSatellite(string id, ISatelliteJobController controller)
        {
            lock (this._controllers)
            {
                this._controllers[id] = controller;
            }

            ILogger logger = this.Site.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogMessage(LogCategories.Manager, "Satellite {0} connected", id); 
            }

            Run();
        }

        public void UnregisterSatellite(string id)
        {
            lock (this._controllers)
            {
                this._controllers.Remove(id);
            }
            ILogger logger = this.Site.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogMessage(LogCategories.Manager, "Satellite {0} disconnected", id);
            }
        }
    }
}
