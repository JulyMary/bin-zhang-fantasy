using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.Tracking
{


    [ServiceContract(Namespace = Consts.NamespaceUri,
        CallbackContract = typeof(ITrackListenerServiceHandler),
        SessionMode = SessionMode.Required) ]
    public interface ITrackListenerService
    {

       

        [OperationContract()]
        TrackProperty[] GetProperties();

        [OperationContract] 
        void Echo();

        [OperationContract(IsInitiating=true)]
        TrackMetaData GetMetaData(Guid id);
 
    }

    [ServiceContract(Namespace=Consts.NamespaceUri)]  
    public interface ITrackListenerServiceHandler
    {


       

        [OperationContract()] 
        void HandlePropertyChanged(TrackProperty property);

        [OperationContract]
        void Echo();

        [OperationContract()]
        void HandleActived(TrackMetaData metaData, TrackProperty[] properties); 

    }
}
