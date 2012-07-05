using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Resources;
using System.Threading;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;
using System.Threading.Tasks;

namespace Fantasy.Jobs.Solar
{
    public class SolarJobDispatcherService : AbstractService, IJobDispatcher
    {

        private List<JobStartupData> _runningJobs = new List<JobStartupData>();
        private object _syncRoot = new object();
        private IResourceManager _resourceManager;
        

        public override void InitializeService()
        {
            this._satelliteManager = this.Site.GetRequiredService<SatelliteManager>();
            this._queue = this.Site.GetRequiredService<IJobQueue>();
            this._filters = AddIn.CreateObjects<IJobStartupFilter>("jobService/startupFilters/filter");
            this._waitHandle = new AutoResetEvent(false);

            _startJobThread = ThreadFactory.CreateThread(this.Run);

            _refreshThread = ThreadFactory.CreateThread(this.Refresh);

            foreach (IJobStartupFilter filter in this._filters)
            {
                if (filter is IObjectWithSite)
                {
                    ((IObjectWithSite)filter).Site = this.Site;
                }
            }

            _queue.Added += new EventHandler<JobQueueEventArgs>(QueueChanged);
            _queue.Changed += new EventHandler<JobQueueEventArgs>(QueueChanged);
            _queue.RequestCancel += new EventHandler<JobQueueEventArgs>(QueueRequestCancel);
            _queue.RequestSuspend += new EventHandler<JobQueueEventArgs>(QueueRequestSuspend);
            _queue.RequestUserPause += new EventHandler<JobQueueEventArgs>(QueueRequestUserPause);

            _resourceQueue = this.Site.GetService<IResourceRequestQueue>();
            _resourceManager = this.Site.GetRequiredService<IResourceManager>();
            _resourceManager.Available += new EventHandler(ResourceManager_Available);
          
            base.InitializeService();
        }

        void ResourceManager_Available(object sender, EventArgs e)
        {
            this.TryDispatch();
        }

        void ResourceAvailable(object sender, EventArgs e)
        {
            this._waitHandle.Set();
        }

        public override void UninitializeService()
        {
            this._startJobThread.Abort(); 
            base.UninitializeService();
        }

        private AutoResetEvent _waitHandle;

        private Thread _startJobThread;
        private Thread _refreshThread;

        private IJobQueue _queue;
        private IResourceRequestQueue _resourceQueue;

        private SatelliteManager _satelliteManager;
      
        private IJobStartupFilter[] _filters;

        public void Start()
        {
            _startJobThread.Start();
            _refreshThread.Start();
        }


        private JobStartupData GetStartupData(Guid id)
        {
            lock (_syncRoot)
            {
                return this._runningJobs.Find(data => data.JobMetaData.Id == id);
            }
        }

        void QueueRequestUserPause(object sender, JobQueueEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                JobStartupData data = this.GetStartupData(e.Job.Id);
                if (data != null)
                {
                    this._satelliteManager.Enqueue(data.Satellite, null,
                        (satellite, state) =>
                        {
                            satellite.RequestUserPause(e.Job.Id);
                        },
                        state =>
                        {
                            lock (_syncRoot)
                            {
                                this._runningJobs.Remove(data);
                            }
                            e.Job.State = JobState.UserPaused;
                            this._queue.ApplyChange(e.Job);
                        });
                }
            });
        }

        void QueueRequestSuspend(object sender, JobQueueEventArgs e)
        {
            Task.Factory.StartNew(() =>
               {
                   JobStartupData data = this.GetStartupData(e.Job.Id);
                   if (data != null)
                   {
                       this._satelliteManager.Enqueue(data.Satellite,null,
                           (satellite, state) =>
                           {
                               satellite.RequestSuspend(e.Job.Id);
                           },
                           (state) =>
                           {
                               lock (_syncRoot)
                               {
                                   this._runningJobs.Remove(data);
                               }
                               e.Job.State = JobState.Suspended;
                               this._queue.ApplyChange(e.Job);
                           });
                   }
               });
        }

        void QueueRequestCancel(object sender, JobQueueEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                JobStartupData data = this.GetStartupData(e.Job.Id);
                if (data != null)
                {
                    this._satelliteManager.Enqueue(data.Satellite,null,
                        (satellite, state) =>
                        {
                            satellite.RequestCancel(e.Job.Id);
                        },
                        (state) =>
                        {
                            lock (_syncRoot)
                            {
                                this._runningJobs.Remove(data);
                            }
                            e.Job.State = JobState.Cancelled;
                            e.Job.EndTime = DateTime.Now;
                            this._queue.ApplyChange(e.Job);
                        });
                }
            });
        }

        void QueueChanged(object sender, JobQueueEventArgs e)
        {
            if (e.Job.State != JobState.Running)
            {
                lock (_syncRoot)
                {
                    JobStartupData data = this.GetStartupData(e.Job.Id);
                    if (data != null)
                    {
                        this._runningJobs.Remove(data);
                    }
                }
            }

            this._waitHandle.Set();
        }

        public void TryDispatch()
        {
            _waitHandle.Set();
        }

        public void Run()
        {
            ILogger logger = this.Site.GetService<ILogger>(); 
            while (true)
            {
               
                try
                {
                    
                    while (TryStartAJob()) ;
                }
                catch (ThreadAbortException)
                {
                }
                catch(Exception error)
                {
                    if (logger != null)
                    {
                        logger.LogError(LogCategories.Manager, error, "An error occurs when try start a new job.");  
                    }
                }
                _waitHandle.WaitOne();
            }
        }


        private void Refresh()
        {
            while (true)
            {

                List<JobStartupData> list;
                lock (this._syncRoot)
                {
                    list = new List<JobStartupData>(this._runningJobs);
                }
                foreach (JobStartupData data in list)
                {

                    this._satelliteManager.Enqueue(data.Satellite, data, (satellite, state) =>
                    {
                        bool running = false;
                        JobStartupData d = (JobStartupData)state;
                        lock (this._syncRoot)
                        {
                            if (this._runningJobs.Contains(d))
                            {
                                running = satellite.IsRunning(d.JobMetaData.Id);
                                if (!running)
                                {
                                    this._runningJobs.Remove(d);
                                }
                            }

                        }
                        if (!running)
                        {
                            d.JobMetaData.State = JobState.Suspended;
                            this._queue.ApplyChange(d.JobMetaData);
                        }
                    },
                    state =>
                    {
                        JobStartupData d = (JobStartupData)state;
                        bool contains = false;
                        lock (this._syncRoot)
                        {

                            contains = this._runningJobs.Contains(d);
                            if (contains)
                            {
                                this._runningJobs.Remove(d);
                            }
                        }
                        if (contains)
                        {
                            d.JobMetaData.State = JobState.Suspended;
                            this._queue.ApplyChange(d.JobMetaData);
                        }
                    });
                    
                }
                Thread.Sleep(15 * 1000);
            }
        }

        private bool _starting = false;

        private IEnumerable<JobStartupData> GetUnterminatesData()
        {
            foreach (JobMetaData job in this._queue.Unterminates)
            {
                yield return new JobStartupData() { JobMetaData = job };
            }
        }

        private bool TryStartAJob()
        {
            ILogger logger = this.Site.GetService<ILogger>();
            bool rs = false;
            if (!_starting)
            {
                _starting = true;
                try
                {
                    
                    IEnumerable<JobStartupData> jobs = GetUnterminatesData();
                    foreach (IJobStartupFilter filter in this._filters)
                    {
                        jobs = filter.Filter(jobs);
                    }

                    foreach(JobStartupData data in jobs)
                    {
                    
                        JobMetaData job = data.JobMetaData;
                        
                        SatelliteSite site = _satelliteManager.Satellites.FirstOrDefault(s => s.Name == data.Satellite);

                        if (site != null)
                        {
                            try
                            {
                                if (job.State == JobState.Unstarted)
                                {
                                    if (logger != null)
                                    {
                                        logger.LogMessage("Dispatch", "Start job {0} ({1}) on satellite {2}", job.Name, job.Id, data.Satellite); 
                                    }
                                    job.State = JobState.RequestStart;
                                    site.Satellite.RequestStartJob(data.JobMetaData);
                                }
                                else
                                {
                                    if (logger != null)
                                    {
                                        logger.LogMessage("Dispatch", "Resume job {0} ({1}) on satellite {2}", job.Name, job.Id, data.Satellite);
                                    }
                                    job.State = JobState.RequestStart;
                                    site.Satellite.RequestResume(job);
                                }

                                this._runningJobs.Add(data);

                                rs = true;
                            }
                            catch (ThreadAbortException)
                            {
                            }
                            catch (Exception error)
                            {
                                if (!WCFExceptionHandler.CanCatch(error))
                                {
                                    
                                    if (logger != null)
                                    {
                                        logger.LogError("Dispatch", error, "An error occurs while try start/resume job {0} ({1}) on satellite service {2}.", job.Name, job.Id, data.Satellite); 
                                    }
                                }
                            }

                            if (rs)
                            {
                                if (this._resourceQueue != null)
                                {
                                    this._resourceQueue.UnregisterResourceRequest(job.Id);
                                }

                                break;
                            }

                        }
                       
                    }
                }
                finally
                {
                    _starting = false;
                }
               
            }

            return rs;

        }



    }
}
