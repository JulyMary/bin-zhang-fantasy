using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;
using Fantasy.Jobs.Resources;

using System.ServiceModel;
using Fantasy.ServiceModel;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fantasy.Jobs.Solar
{

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class SatelliteService : AbstractService, ISatellite
    {

        private ClientRef<ISatelliteHandler> _satelliteHandler;

        private IJobController _controller;

        private string _name;

        private Thread _refreshThread;

        private IComputerLoadFactorEvaluator _loadFactorEvaluator;
        private IResourceManager _resourceManager;

        public override void InitializeService()
        {
            this._controller = this.Site.GetRequiredService<IJobController>();
            _name = Environment.MachineName + ":" + Process.GetCurrentProcess().Id;

            this._loadFactorEvaluator = AddIn.CreateObjects<IComputerLoadFactorEvaluator>("jobService/computerLoadFactorEvaluator").SingleOrDefault();

            if (this._loadFactorEvaluator != null && this._loadFactorEvaluator is IObjectWithSite)
            {
                ((IObjectWithSite)_loadFactorEvaluator).Site = this.Site; 
            }

            this._resourceManager = this.Site.GetService<IResourceManager>();
            if (_resourceManager != null)
            {
                this._resourceManager.Available += new EventHandler(ResourceManager_Available);
            }

            _refreshThread = ThreadFactory.CreateThread(this.Refresh).WithStart();
          

            base.InitializeService();
        }

        void ResourceManager_Available(object sender, EventArgs e)
        {
            ISolarActionQueue actionQueue = this.Site.GetRequiredService<ISolarActionQueue>();
            actionQueue.Enqueue(solar => solar.ResourceAvaiable());
        }

        private void Refresh()
        {
            while (true)
            {
                TryCreateHandler();
                Thread.Sleep(15 * 1000);
            }
              
        }
      
        private void TryCreateHandler()
        {

            ILogger logger = this.Site.GetService<ILogger>();
           
            if (this._satelliteHandler != null)
            {
                try
                {
                    this._satelliteHandler.Client.Echo();
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception error)
                {
                    _satelliteHandler.Dispose();
                    _satelliteHandler = null;
                    if (!WCFExceptionHandler.CanCatch(error))
                    {
                        throw;
                    }
                    else
                    {
                        logger.SafeLogError("Solar", error, "WCF error");
                    }

                }
            }

            if (_satelliteHandler == null)
            {
               
                try
                {
                    _satelliteHandler = ClientFactory.CreateDuplex<ISatelliteHandler>(this);
                    _satelliteHandler.Client.Connect(this._name);
                   
                    logger.SafeLogMessage("Satellite", "Success connect to solar service.");
                   
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception error)
                {

                    logger.SafeLogError("Solar", error, "WCF error");
                    logger.SafeLogWarning("Satellite", error, MessageImportance.Normal, "Failed to connect to solor service.");
                    _satelliteHandler.Dispose();
                    _satelliteHandler = null;
                    
                    if (!WCFExceptionHandler.CanCatch(error))
                    {
                        throw;
                    }
                   
                }
            }
        }


        

        public override void UninitializeService()
        {
            this._refreshThread.Abort();
            this._refreshThread.Join();
            if (this._satelliteHandler != null)
            {
                
                try
                {
                    this._satelliteHandler.Client.Disconnect();
                }
                catch
                {

                }
                
                this._satelliteHandler.Dispose();
            }

            base.UninitializeService();
        }

        #region ISatellite Members

        public void Echo()
        {
            
        }


        public event EventHandler<JobQueueEventArgs> JobChanged;

        public event EventHandler<JobQueueEventArgs> JobAdded;

        public void OnJobAdded(JobMetaData job)
        {
            if (this.JobAdded != null)
            {
                this.JobAdded(this, new JobQueueEventArgs(job));
            }
        }

        public void OnJobChanged(JobMetaData job)
        {
            if (this.JobChanged != null)
            {
                this.JobChanged(this, new JobQueueEventArgs(job));
            }
        }

        public bool IsResourceAvailable(ResourceParameter[] parameters)
        {
           
            if (_resourceManager != null)
            {
                return _resourceManager.IsAvailable(parameters);
            }
            else
            {
                return true;
            }
        }

        public double GetLoadFactor()
        {
            return this._loadFactorEvaluator != null ? this._loadFactorEvaluator.Evaluate() : this._controller.GetAvailableProcessCount();
        }

        public void RequestStartJob(JobMetaData job)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            logger.SafeLogMessage("Satellite", "Start job {0} ({1}).", job.Name, job.Id); 
            this._controller.StartJob(job);
        }

        public void RequestResume(JobMetaData job)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            logger.SafeLogMessage("Satellite", "Resume job {0} ({1}).", job.Name, job.Id); 
            this._controller.Resume(job);
        }

        public void RequestCancel(Guid id)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            logger.SafeLogMessage("Satellite", "Cancel job {0}.", id); 
            this._controller.Cancel(id);
        }

        public void RequestSuspend(Guid id)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            logger.SafeLogMessage("Satellite", "Suspend job {0}.", id); 
            this._controller.Suspend(id);
        }

        public void RequestUserPause(Guid id)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            logger.SafeLogMessage("Satellite", "Pause job {0}.", id); 
            this._controller.UserPause(id);
        }

       

        public void RequestSuspendAll()
        {
            ILogger logger = this.Site.GetService<ILogger>();
            logger.SafeLogMessage("Satellite", "Suspend all running jobs."); 
            _controller.SuspendAll(true);
        }

 
        public bool IsRunning(Guid id)
        {
            return _controller.GetRunningJobs().Any(job => job.Id == id);
        }

        #endregion
    }
}
