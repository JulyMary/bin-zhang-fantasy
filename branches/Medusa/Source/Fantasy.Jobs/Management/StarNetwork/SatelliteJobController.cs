using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Diagnostics;
using ClickView.ServiceModel;

namespace ClickView.Jobs.Management.StarNetwork
{
    [ServiceContract(Namespace = Consts.JobServiceNamespaceURI)]
    public interface ISatelliteJobController : IJobController
    {
        [OperationContract]
        void Echo();
    }

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class SatelliteJobController : JobControllerBase, ISatelliteJobController
    {
       
        private Thread _checkThread;

        private IStarService _dispatcher;

        private string _identity;

        private List<JobMetaData> _applyingQueue = new List<JobMetaData>();

        private void CheckStartService()
        {
            while (true)
            {
                try
                {
                    if (_dispatcher == null)
                    {
                        _dispatcher = new StarServiceClient(this);
                        _dispatcher.Connect(this._identity);
                    }
                    else
                    {
                        _dispatcher.Echo();
                    }


                    lock (this._applyingQueue)
                    {
                        foreach (JobMetaData item in new List<JobMetaData>(this._applyingQueue))
                        {
                            _dispatcher.CommitChange(item);
                            this._applyingQueue.Remove(item);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception error)
                {
                    WCFExceptionHandler.CatchKnowns(error);
                    _dispatcher = null;

                }
                Thread.Sleep(15000);
            }
        }

        public override void InitializeService()
        {
            _identity = Environment.MachineName + ":" + Process.GetCurrentProcess().Id.ToString();
            _checkThread = new Thread(this.CheckStartService);
            _checkThread.Priority = ThreadPriority.Lowest;
            _checkThread.IsBackground = true;
            _checkThread.Start();
            base.InitializeService();
        }

       
        public override void UninitializeService()
        {
            _checkThread.Abort();
            base.UninitializeService();
            if (this._dispatcher != null)
            {
                try
                {
                    this._dispatcher.Disconnect();
                }
                catch (Exception error)
                {
                    WCFExceptionHandler.CatchKnowns(error);
                }
            }
           
        }

        protected override void CommitChange(JobMetaData job)
        {

            try
            {
                if (this._dispatcher != null)
                {
                    _dispatcher.CommitChange(job);
                }
                else
                {
                    lock (this._applyingQueue)
                    {
                        if (this._applyingQueue.IndexOf(job) < 0)
                        {
                            this._applyingQueue.Add(job);
                        }
                    }
                }

            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception error)
            {
                WCFExceptionHandler.CatchKnowns(error);
                lock (this._applyingQueue)
                {
                    if (this._applyingQueue.IndexOf(job) < 0)
                    {
                        this._applyingQueue.Add(job);
                    }
                }
            }
        }

        public void Echo()
        {

        }

        
    }
}
