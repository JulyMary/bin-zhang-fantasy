using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy.Tracking
{
    public class TrackProviderService : ITrackProviderService
    {

        private RemoteTrack _remoteTrack;

        #region ITrackProviderService Members

        public void CreateTrackProvider(Guid id, string name, string category, TrackProperty[] properties, bool reconnect)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            if(properties != null)
            {
                foreach(TrackProperty prop in properties) 
                {
                    values.Add(prop.Name, TrackProperty.ToObject(prop)); 
                }
               
            }

            if (!RemoteTrackManager.Manager.CreateTrack(id, name, category, values, reconnect, out _remoteTrack))
            {
                throw new Exception();
            }
        }

        public void SetProperty(TrackProperty property)
        {
            if (_remoteTrack != null)
            {
                _remoteTrack[property.Name] = TrackProperty.ToObject(property);
            }
        }

    
        public void Echo()
        {
           
        }

        #endregion 
    }
}
