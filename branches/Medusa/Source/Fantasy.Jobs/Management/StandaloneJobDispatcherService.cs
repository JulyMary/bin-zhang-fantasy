using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Resources;
using System.Threading;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public class StandaloneJobDispatcherService : AbstractService, IJobDispatcher
    {
        public override void InitializeService()
        {
            this._queue = (IJobQueue)this.Site.GetService(typeof(IJobQueue));
            this._filters = AddIn.CreateObjects<IJobStartupFilter>("jobService/startupFilters/filter");
            this._waitHandle = new AutoResetEvent(false);

            _startJobThread = ThreadFactory.CreateThread(this.Run);

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
            _resourceManager = this.Site.GetService<IResourceManager>();
            if (_resourceManager != null)
            {
                _resourceManager.Available += new EventHandler(ResourceAvailable);
            }

            this._controller = this.Site.GetRequiredService<IJobController>();

            base.InitializeService();
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

        private IJobQueue _queue;
        private IResourceRequestQueue _resourceQueue;
        private IResourceManager _resourceManager;

        private IJobStartupFilter[] _filters;

        private IJobController _controller;

        public void Start()
        {
            _startJobThread.Start(); 
        }

        void QueueRequestUserPause(object sender, JobQueueEventArgs e)
        {
            IJobController controller = (IJobController)this.Site.GetService(typeof(IJobController));
            controller.UserPause(e.Job.Id);
        }

        void QueueRequestSuspend(object sender, JobQueueEventArgs e)
        {
            IJobController controller = (IJobController)this.Site.GetService(typeof(IJobController));
            controller.Suspend(e.Job.Id);
        }

        void QueueRequestCancel(object sender, JobQueueEventArgs e)
        {
            IJobController controller = (IJobController)this.Site.GetService(typeof(IJobController));
            controller.Cancel(e.Job.Id);
        }

        void QueueChanged(object sender, JobQueueEventArgs e)
        {
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

        private bool _starting = false;

        private bool TryStartAJob()
        {
            bool rs = false;
            if (!_starting)
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
                        if (this._resourceQueue != null)
                        {
                            this._resourceQueue.UnregisterResourceRequest(job.Id);
                        }
                        ILogger logger = this.Site.GetService<ILogger>();
                        if (job.State == JobState.Unstarted)
                        {
                            job.State = JobState.RequestStart;
                            if (logger != null)
                            {
                                logger.LogMessage("Dispatching", "Start Job {0} ({1}).", job.Name, job.Id);
                            }
                          

                            _controller.StartJob(job);
                        }
                        else
                        {
                            job.State = JobState.RequestStart;
                            if (logger != null)
                            {
                                logger.LogMessage("Dispatching", "Resume Job {0} ({1}).", job.Name, job.Id);
                            }
                            _controller.Resume(job);
                        }

                        rs = true;
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
