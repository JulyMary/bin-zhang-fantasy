using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.Tracking
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, Namespace=Consts.NamespaceUri)]
    public class TrackManagerService : ITrackManagerService
    {
        private ITrackManagerServiceHandler handler;
        public TrackManagerService()
        {
            handler = OperationContext.Current.GetCallbackChannel<ITrackManagerServiceHandler>();
            RemoteTrackManager.Manager.AddHandler(handler);
        }

        ~TrackManagerService()
        {
            RemoteTrackManager.Manager.RemoveHandler(handler);
        }
   
        public TrackMetaData[] GetActivedTrackMetaData()
        {
            return RemoteTrackManager.Manager.GetAllTrackMetaData();  
        }


        public void Echo()
        {
            
        }

        
    }
}
