using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.Jobs.Solar
{

    [ServiceContract(Namespace = Consts.JobServiceNamespaceURI,
        CallbackContract = typeof(ISatellite),
        SessionMode = SessionMode.Required)]
    public interface  ISatelliteHandler
    {
        [OperationContract(IsInitiating=true)]
        void Connect(string name);

        [OperationContract]
        void Echo();

        [OperationContract(IsTerminating=true, IsOneWay=false)]
        void Disconnect();

      
    }



}
