using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Windows.Forms;
using Fantasy.ServiceModel;

namespace Fantasy.Tracking
{
    class TrackProvider : TrackBase, ITrackProvider, IRefreshable 
    {
        public TrackFactory Connection { get; private set; }

        private ClientRef<ITrackProviderService> _wcfProvider;
#pragma warning disable 0414
        private bool _created = false;
#pragma warning restore 0414

        private Uri _uri;
        private string _configurationName;

        private object _syncRoot = new object();

        public TrackProvider(TrackFactory connection, string configurationName, Uri uri, Guid id, string name, string category, IDictionary<string, object> values)
        {
            this.Connection = connection; 
            this._configurationName = configurationName;
            this._uri = uri;
            this.Id = id;
            this.Name = name;
            this.Category = category;
            this.InitializeData(values);

            Task task = Task.Factory.StartNew(() =>
            {
                this.TryCreateWCF();

                RefreshManager.Register(this);
            });
            task.Wait(100);
        }

        private void TryCreateWCF()
        {
            lock (_syncRoot)
            {
                try
                {
                    

                    this._wcfProvider = ClientFactory.Create<ITrackProviderService>();
                    List<KeyValuePair<string, object>> values;
                    lock (this.Data)
                    {
                        values = new List<KeyValuePair<string, object>>(this.Data);
                    }
                    TrackProperty[] props = new TrackProperty[values.Count];
                    for (int i = 0; i < values.Count; i++)
                    {
                        props[i] = TrackProperty.Create(values[i].Key, values[i].Value);
                    }

                    this._wcfProvider.Client.CreateTrackProvider(this.Id, this.Name, this.Category, props, true);
                    this._created = true;

                }
                catch (Exception error)
                {
                    WCFExceptionHandler.CatchKnowns(error);
                    this._wcfProvider = null;
                }
            }
        }

        protected override void OnChanged(TrackChangedEventArgs e)
        {
            if (this._wcfProvider != null)
            {
                MethodInvoker invoker = new MethodInvoker(() =>
                {
                    try
                    {
                        TrackProperty prop = TrackProperty.Create(e.Name, e.NewValue);
                        this._wcfProvider.Client.SetProperty(prop);
                    }
                    catch (Exception err)
                    {
                        WCFExceptionHandler.CatchKnowns(err);
                        _wcfProvider = null;
                    }
                });

                invoker.BeginInvoke(null, null); 
                
            }

            base.OnChanged(e);
        }

        #region IRefreshable Members

        public void Refresh()
        {
            if (this._wcfProvider != null)
            {
                
                    try
                    {
                        this._wcfProvider.Client.Echo();
                    }
                    catch (Exception error)
                    {
                        WCFExceptionHandler.CatchKnowns(error);
                        this._wcfProvider = null;
                    }
               

               
            }
            if (this._wcfProvider == null)
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
    }
}
