using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.Jobs.Resources
{
    [ServiceContract(Namespace=Consts.JobServiceNamespaceURI)] 
    public interface IGlobalMutexService
    {
        [OperationContract]
        bool IsAvaiable(string key);

        [OperationContract]
        bool Request(string key, TimeSpan timeout);

        [OperationContract]
        void Release(string key);
     }
}
