using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;
using System.ServiceModel;
using Fantasy.Jobs.Resources;

namespace Fantasy.Jobs.Solar
{
    [ServiceContract(Namespace=Consts.JobServiceNamespaceURI)]
    public interface ISolar
    {
        #region operators for IJobQueue
        [OperationContract]
        JobMetaData[] Unterminates(int skip, int take);

        [OperationContract]
        JobMetaData[] Terminates(int skip, int take);

        [OperationContract]
        JobMetaData FindJobMetaDataById(Guid id);

        [OperationContract]
        bool IsTermianted(Guid id);

        [OperationContract]
        JobMetaData[] FindTerminated(out int totalCount, string filter, string[] args, string order, int skip, int take);

        [OperationContract]
        JobMetaData[] FindUnterminated(out int totalCount, string filter, string[] args, string order, int skip, int take);

        [OperationContract]
        void ApplyChange(JobMetaData job);

        [OperationContract]
        void Resume(Guid id);

        [OperationContract]
        void Cancel(Guid id);

        [OperationContract]
        void Suspend(Guid id);

        [OperationContract]
        void UserPause(Guid id);

        string[] GetAllApplications();

        string[] GetAllUsers();
        #endregion

        #region operatiors for resource management

        [OperationContract]
        void RegisterResourceRequest(Guid jobId, ResourceParameter[] parameters);

        [OperationContract]
        void UnregisterResourceRequest(Guid jobId);

        [OperationContract]
        void ResourceAvaiable();

        [OperationContract]
        ResourceParameter[] GetRequiredResources(Guid jobId);

        #endregion


    }
}
