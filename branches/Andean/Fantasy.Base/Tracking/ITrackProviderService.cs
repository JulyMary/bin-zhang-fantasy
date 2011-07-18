using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Fantasy.Tracking
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = Consts.NamespaceUri)]
    public interface ITrackProviderService
    {
        [OperationContract(IsInitiating=true)]
        void CreateTrackProvider(Guid id, string name, string category, TrackProperty[] properties, bool reconnect);

        [OperationContract(IsOneWay=true)]
        void SetProperty(TrackProperty property);

        [OperationContract] 
        void Echo();
    }




    
}
