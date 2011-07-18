using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Fantasy.ServiceModel;

namespace Fantasy.Tracking
{
    class TrackManager : ITrackManager, IRefreshable, ITrackManagerServiceHandler
    {
        private ClientRef<ITrackManagerService> _wcfManager;
        private Uri _uri;
        private string _configurationName;
        private object _syncRoot = new object();

        public TrackManager(TrackFactory connection, string configurationName, Uri uri)
        {
            this.Connection = connection; 
            this._configurationName = configurationName;
            this._uri = uri;
            this.TryCreateWCF();
            RefreshManager.Register(this);  
        }

        private void TryCreateWCF()
        {
            lock (_syncRoot)
            {
                try
                {
                    //InstanceContext context = new InstanceContext(this);
                    //if (this._configurationName != null && this._uri != null)
                    //{
                    //    this._wcfManager = new TrackManagerClient(context, this._configurationName, this._uri.ToString());
                    //}
                    //else
                    //{
                    //    this._wcfManager = new TrackManagerClient(context);
                    //}
                    this._wcfManager = ClientFactory.CreateDuplex<ITrackManagerService>(this);

                    this._wcfManager.Client.Echo();

                }
                catch (Exception error)
                {
                    WCFExceptionHandler.CatchKnowns(error);
                    this._wcfManager = null;
                }
            }
        }

        #region ITrackManager Members

        public TrackMetaData[] GetActivedTrackMetaData()
        {
           
            try
            {
                if (this._wcfManager!= null)
                {
                    return _wcfManager.Client.GetActivedTrackMetaData();
                }
            }
            catch(Exception error)
            {
                WCFExceptionHandler.CatchKnowns(error);
                _wcfManager = null;
            }

            return new TrackMetaData[0];
        }

        public event EventHandler<TrackActivedEventArgs> TrackActived;

        #endregion

        #region IRefreshable Members

        public void Refresh()
        {
            if (this._wcfManager != null)
            {
                try
                {
                    this._wcfManager.Client.Echo();
                }
                catch(Exception error)
                {
                    WCFExceptionHandler.CatchKnowns(error);
                    this._wcfManager = null;
                }
            }

            if (this._wcfManager == null)
            {
                this.TryCreateWCF();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            RefreshManager.Unregister(this); 
        }

        #endregion

        #region ITrackManagerServiceHandler Members

        public void HandleTrackActived(TrackMetaData track)
        {
            if (this.TrackActived != null)
            {
                this.TrackActived(this, new TrackActivedEventArgs(track));
            }
        }

        #endregion

        public TrackFactory Connection { get; private set; }
    }
}
