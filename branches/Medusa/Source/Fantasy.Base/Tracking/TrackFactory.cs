using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Tracking
{
    public class TrackFactory
    {

        private Uri _baseUri;
        private string _configurationName;

        private const string ProviderName = "ITrackProvider";

        private const string ListenerName = "ITrackListener";

        private const string ManagerName = "ITrackManager";

        public TrackFactory(string configurationName, Uri baseUri)
        {
            this._configurationName = configurationName;
            this._baseUri = baseUri; 
        }

        public TrackFactory()
        {
        }


        public ITrackProvider CreateProvider(Guid id, string name, string category, IDictionary<string, object> values)
        {
            Uri uri = this._baseUri != null ? new Uri(string.Format("{0}/{1}/", this._baseUri, ProviderName)) : null;
            TrackProvider rs = new TrackProvider(this, this._configurationName, uri, id, name, category, values);
            return rs;
        }

        public ITrackListener CreateTrackListener(Guid id)
        {
            Uri uri = this._baseUri != null ? new Uri(string.Format("{0}/{1}/", this._baseUri, ListenerName)) : null;
            TrackListener rs = new TrackListener(this, this._configurationName, uri, id);
            return rs;
        }

        public ITrackManager CreateManager()
        {
            Uri uri = this._baseUri != null ? new Uri(string.Format("{0}/{1}/", this._baseUri, ManagerName)) : null;
            TrackManager rs = new TrackManager(this, this._configurationName, uri);
            return rs;
        }
    }
}
