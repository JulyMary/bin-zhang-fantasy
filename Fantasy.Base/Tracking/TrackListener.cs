using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using Fantasy.ServiceModel;

namespace Fantasy.Tracking
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class TrackListener : TrackBase, ITrackListener, IRefreshable, ITrackListenerServiceHandler
    {
        private ClientRef<ITrackListenerService> _wcfListener;
        private Uri _uri;
        private string _configurationName;

        private bool _isActive = false;

        private object _syncRoot = new object();

        public TrackListener(TrackFactory connection, string configurationName, Uri uri, Guid id)
        {
            this.Connection = connection;
            this._configurationName = configurationName;
            this._uri = uri;
            this.Id = id;

            Task task = Task.Factory.StartNew(() =>
            {
                this.TryCreateWCF(true);
                RefreshManager.Register(this);
            });
            task.Wait(500);

        }

        private void TryCreateWCF(bool init)
        {
            lock (_syncRoot)
            {
                try
                {
                    //InstanceContext context = new InstanceContext(this);
                    //if (this._configurationName != null && this._uri != null)
                    //{
                    //    this._wcfListener = new TrackListenerClient(context, this._configurationName, this._uri.ToString());
                    //}
                    //else
                    //{
                    //    this._wcfListener = new TrackListenerClient(context);
                    //}
                    this._wcfListener = ClientFactory.CreateDuplex<ITrackListenerService>(this);


                    TrackMetaData data = this._wcfListener.Client.GetMetaData(this.Id);
                    if (data != null)
                    {
                        this.Name = data.Name;
                        this.Category = data.Category;

                        foreach (TrackProperty prop in this._wcfListener.Client.GetProperties())
                        {
                            object value = TrackProperty.ToObject(prop);
                            if (init)
                            {
                                this.Data[prop.Name] = value;
                            }
                            else
                            {
                                this[prop.Name] = value;
                            }
                        }
                    }
                    this.IsActived = true;

                }
                catch (Exception error)
                {

                    WCFExceptionHandler.CatchKnowns(error);
                    this._wcfListener = null;
                    this.IsActived = false;

                }

            }
        }


        #region IRefreshable Members

        public void Refresh()
        {
            if (this._wcfListener != null)
            {
                
                try
                {
                    this._wcfListener.Client.Echo();
                }
                catch
                {
                    this._wcfListener = null;
                }

            }
            if (this._wcfListener == null)
            {
                this.TryCreateWCF(false);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            RefreshManager.Unregister(this);
            if (this._wcfListener != null)
            {
                try
                {

                    this._wcfListener.Dispose();
                }
                catch
                {
                }
            }
        }

        #endregion

        #region ITrackListenerServiceHandler Members

        public void HandlePropertyChanged(TrackProperty property)
        {
            object value = TrackProperty.ToObject(property);
            //System.Diagnostics.Debug.WriteLine("{0}: {1}", property.Name, value); 
            this[property.Name] = value;
        }

        public override object this[string name]
        {
            get
            {
                return base[name];
            }
            set
            {
                base[name] = value;
            }
        }



        #endregion



        public TrackFactory Connection { get; private set; }

        #region ITrackListenerServiceHandler Members

        public void Connect()
        {
        }

        public void Echo()
        {

        }

        #endregion

        #region ITrackListenerServiceHandler Members


        public void HandleActived(TrackMetaData metaData, TrackProperty[] properties)
        {
            this.Name = metaData.Name;
            this.Category = metaData.Category;

            foreach (TrackProperty prop in properties)
            {
                object value = TrackProperty.ToObject(prop);
                this[prop.Name] = value;
            }
            this.IsActived = true;

        }

        #endregion

        #region ITrackListener Members


        public bool IsActived
        {
            get
            {
                return this._isActive;
            }
            set
            {
                if (this._isActive != value)
                {
                    this._isActive = value;
                    if (this.ActiveStateChanged != null)
                    {
                        this.ActiveStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler ActiveStateChanged;

        #endregion
    }
}
