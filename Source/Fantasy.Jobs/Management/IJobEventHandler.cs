using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.Jobs.Management
{

    [ServiceContract(Namespace = Consts.JobServiceNamespaceURI)]
    public interface IJobEventHandler
    {
        [OperationContract(IsOneWay = true)]
        void Added(JobMetaData job);

        [OperationContract(IsOneWay = true)]
        void Changed(JobMetaData job);

        [OperationContract]
        void Echo();
    }
}
