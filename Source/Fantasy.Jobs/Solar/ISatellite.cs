using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;
using Fantasy.Jobs.Resources;
using System.ServiceModel;

namespace Fantasy.Jobs.Solar
{
    [ServiceContract(Namespace=Consts.JobServiceNamespaceURI)]
    public interface ISatellite
    {
        [OperationContract]
        void Echo();

        

        #region Job queue events

        [OperationContract]
        void OnJobAdded(JobMetaData job);

        [OperationContract]
        void OnJobChanged(JobMetaData job);

        #endregion

        #region Resource service

        [OperationContract]
        bool IsResourceAvailable(ResourceParameter[] parameters);


        [OperationContract]
        double GetLoadFactor();

        #endregion

        #region Job Controller

        [OperationContract]
        void RequestStartJob(JobMetaData job);

        [OperationContract]
        void RequestResume(JobMetaData job);

        [OperationContract]
        void RequestCancel(Guid id);

        [OperationContract]
        void RequestSuspend(Guid id);

        [OperationContract]
        void RequestUserPause(Guid id);

        [OperationContract]
        void RequestSuspendAll();


        [OperationContract]
        bool IsRunning(Guid id);

        #endregion 

        event EventHandler<JobQueueEventArgs> JobChanged;

        event EventHandler<JobQueueEventArgs> JobAdded;

        
    }
}
