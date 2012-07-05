using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Fantasy.Jobs.Management
{
    [ServiceContract(Namespace = Consts.JobServiceNamespaceURI, CallbackContract = typeof(IJobEventHandler), SessionMode = SessionMode.Required)]
    public interface IJobEventService
    {

        [OperationContract(IsInitiating = true)]
        void Connect();

        [OperationContract]
        void Echo();

        [OperationContract(IsTerminating = true)]
        void Disconnect();
    }


   
}
