using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, Namespace=Consts.JobServiceNamespaceURI)]
    public class JobEventService : IJobEventService
    {
        private IJobEventHandler _handler;
        private IJobQueue _queue;
        private bool _expired = false;
        public JobEventService()
        {
            

        }

        void JobChanged(object sender, JobQueueEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    _handler.Changed(e.Job);
                }
                catch
                {
                    _expired = true;
                    _queue.Added -= new EventHandler<JobQueueEventArgs>(JobAdded);
                    _queue.Changed -= new EventHandler<JobQueueEventArgs>(JobChanged);
                }
            });
        }

        void JobAdded(object sender, JobQueueEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    _handler.Added(e.Job);
                }
                catch
                {
                    _expired = true;
                    _queue.Added -= new EventHandler<JobQueueEventArgs>(JobAdded);
                    _queue.Changed -= new EventHandler<JobQueueEventArgs>(JobChanged);
                }
            });
        }

        #region IJobEventService Members

        public void Echo()
        {
            if (_expired)
            {
                throw new FaultException<CallbackExpiredException>(new CallbackExpiredException(), "Callback is expired.");
            }
            
        }

        #endregion

        #region IJobEventService Members

        public void Connect()
        {
            _handler = OperationContext.Current.GetCallbackChannel<IJobEventHandler>();
            _queue = JobManager.Default.GetRequiredService<IJobQueue>();
            _queue.Added += new EventHandler<JobQueueEventArgs>(JobAdded);
            _queue.Changed += new EventHandler<JobQueueEventArgs>(JobChanged);
        }

        public void Disconnect()
        {
            _expired = true;
            _queue.Added -= new EventHandler<JobQueueEventArgs>(JobAdded);
            _queue.Changed -= new EventHandler<JobQueueEventArgs>(JobChanged);
        }

        #endregion
    }
}
