using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.Tracking
{

    [ServiceContract(Namespace = Consts.NamespaceUri, 
        SessionMode = SessionMode.Required, 
        CallbackContract = typeof(ITrackManagerServiceHandler))]
    public interface ITrackManagerService
    {
        [OperationContract] 
        TrackMetaData[] GetActivedTrackMetaData();

        [OperationContract]
        void Echo();

    }

    [ServiceContract(Namespace=Consts.NamespaceUri)] 
    public interface ITrackManagerServiceHandler
    {
        [OperationContract(IsOneWay = true)] 
        void HandleTrackActived(TrackMetaData track);

        
    }
}
