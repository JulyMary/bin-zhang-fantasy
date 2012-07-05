using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ClickView.Jobs.Management.StarNetwork
{
    [ServiceContract(Namespace = Consts.JobServiceNamespaceURI, CallbackContract = typeof(IJobController), SessionMode = SessionMode.Required)]
    public interface IStarServieBase
    {
    }
    
    [ServiceContract(Namespace = Consts.JobServiceNamespaceURI, CallbackContract = typeof(ISatelliteJobController), SessionMode=SessionMode.Required)]
    public interface IStarService : IStarServieBase
    {
        [OperationContract] 
        void CommitChange(JobMetaData job);

        [OperationContract(IsInitiating=true, IsOneWay=true)]
        void Connect(string id);

        [OperationContract(IsTerminating=true, IsOneWay= true)] 
        void Disconnect();

        [OperationContract] 
        void Echo();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class StarService : IStarService
    {

        private string _id;
        public StarService()
        {
           
        }

        private ISatelliteJobController _controller;

        public void CommitChange(JobMetaData job)
        {
            IJobQueue queue = JobManager.Default.GetRequiredService<IJobQueue>();
            queue.ApplyChange(job);
        }

        public void Disconnect()
        {
            StarJobDispatcherService disp = JobManager.Default.GetRequiredService<StarJobDispatcherService>();
            disp.UnregisterSatellite(_id);
        }

        public void Echo()
        {
            _controller.Echo();
        }

        public void Connect(string id)
        {
            _id = id;
            _controller = OperationContext.Current.GetCallbackChannel<ISatelliteJobController>();
            StarJobDispatcherService disp = JobManager.Default.GetRequiredService<StarJobDispatcherService>();
            disp.RegisterSatellite(id, _controller);
        }

    }


    public class StarServiceClient : DuplexClientBase<IStarService>, IStarService
    {

        public StarServiceClient(ISatelliteJobController controller) : 
                base(new InstanceContext(controller)) {
        }

      
        public void CommitChange(JobMetaData job)
        {
            this.Channel.CommitChange(job);
        }


        public void Disconnect()
        {
            this.Channel.Disconnect();
        }

        public void Echo()
        {
            this.Channel.Echo();
        }


        public void Connect(string id)
        {
            this.Channel.Connect(id);
        }

      
    }


}
