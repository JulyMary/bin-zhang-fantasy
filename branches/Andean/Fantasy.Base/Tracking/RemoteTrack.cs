using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy.Tracking
{


    class RemoteTrack : TrackBase
    {
        public RemoteTrack(Guid id, string name, string category, IDictionary<string, object> values)
        {
            this.Id = id;
            this.Name = name;
            this.Category = category;
            this.InitializeData(values);
        }

        private List<IRemoteTrackHandler> _handlers = new List<IRemoteTrackHandler>();

        public void AddHandler(IRemoteTrackHandler handler)
        {
            lock (_handlers)
            {
                if (this._handlers.IndexOf(handler) <= 0)
                {
                    this._handlers.Add(handler);
                }
            }
        }

        public void RemoveHandler(IRemoteTrackHandler handler)
        {
            lock (_handlers)
            {
                this._handlers.Remove(handler);
            }
        }


        public bool HandlerAdded(IRemoteTrackHandler handler)
        {
            lock (_handlers)
            {
                return this._handlers.IndexOf(handler) >= 0;
            }
        }



        protected override void OnChanged(TrackChangedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                List<IRemoteTrackHandler> handlers;
                lock (_handlers)
                {
                    handlers = new List<IRemoteTrackHandler>(this._handlers);
                }

                Parallel.ForEach(handlers, handler =>
                {

                    try
                    {
                        handler.HandleChanged(e);
                    }
                    catch
                    {
                        lock (_handlers)
                        {
                            this._handlers.Remove(handler);
                        }
                    }
                });

               
            });
            base.OnChanged(e);
        }

        public TrackProperty[] GetTrackProperties()
        {
            TrackProperty[] rs;
            List<KeyValuePair<string, object>> props;
            lock (this.Data)
            {
                props = new List<KeyValuePair<string, object>>(this.Data);
            }

            rs = new TrackProperty[props.Count];
            for (int i = 0; i < props.Count; i++)
            {

                TrackProperty prop = TrackProperty.Create(props[i].Key, props[i].Value);
                rs[i] = prop;
            }
           

            return rs;
        }




    }

    interface IRemoteTrackHandler
    {
        void HandleChanged(TrackChangedEventArgs e);
    }
}
