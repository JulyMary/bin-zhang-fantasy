using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Fantasy.ServiceModel;

namespace Fantasy.Tracking
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, Namespace=Consts.NamespaceUri)]
    public class TrackListenerService : ITrackListenerService, IRemoteTrackHandler, IRefreshable, ITrackManagerServiceHandler
    {
        private ITrackListenerServiceHandler _handler;
        private RemoteTrack _track;
        private Guid _id;
      
        public TrackListenerService()
        {
            _handler = OperationContext.Current.GetCallbackChannel<ITrackListenerServiceHandler>();
        }

        

        public TrackProperty[] GetProperties()
        {
            return this._track.GetTrackProperties(); 
        }

       

        #region ITrackListenerService Members


        public void Echo()
        {
            if (this._track == null || this._track.HandlerAdded(this) == false)
            {
                throw new FaultException<CallbackExpiredException>(new CallbackExpiredException(), new FaultReason("Handle has been removed."));
            }
        }

        #endregion

        #region ITrackListenerService Members


        public TrackMetaData GetMetaData(Guid id)
        {
            TrackMetaData rs = null;
            this._id = id;
            

            RefreshManager.Register(this);  

            this._track = RemoteTrackManager.Manager.FindTrack(id);
            if (this._track != null)
            {
                this._track.AddHandler(this); 
                rs = new TrackMetaData() { Id = id, Name = this._track.Name, Category = this._track.Category };
            }

            RemoteTrackManager.Manager.AddHandler(this);
            return rs;
            
        }

        #endregion

       

       
        #region IRefreshable Members

        public void Refresh()
        {
            if (_handler == null)
            {
                throw new ObjectDisposedException("TrackListenerService"); 
            }
            try
            {
                _handler.Echo(); 
            }
            catch
            {
                this._handler = null;
                this.Dispose();

            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            RefreshManager.Unregister(this);
            if (_track != null)
            {
                _track.RemoveHandler(this);
                RemoteTrackManager.Manager.RemoveHandler(this);
                _track = null;
            }
        }

        #endregion

        #region ITrackManagerServiceHandler Members

        public void HandleTrackActived(TrackMetaData meta)
        {
            if (meta.Id == this._id)
            {
                if (this._track != null)
                {
                    this._track.RemoveHandler(this);
                }

                this._track = RemoteTrackManager.Manager.FindTrack(this._id);
                this._track.AddHandler(this);
                try
                {
                    this._handler.HandleActived(meta, this._track.GetTrackProperties());
                }
                catch
                {
                    this.Dispose();
                }
            }

        }

        #endregion


        void IRemoteTrackHandler.HandleChanged(TrackChangedEventArgs e)
        {
            TrackProperty prop = TrackProperty.Create(e.Name, e.NewValue);
            try
            {
                this._handler.HandlePropertyChanged(prop);
            }
            catch
            {
                this.Dispose();
            }
        }

    }
}
