using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Fantasy.Tracking
{
    class RemoteTrackManager : IRefreshable
    {

        private RemoteTrackManager()
        {
            RefreshManager.Register(this); 
        }


        private static RemoteTrackManager _manager = new RemoteTrackManager();

        public static RemoteTrackManager Manager
        {
            get { return _manager; }
        }


        #region IRefreshable Members

        private object _syncroot = new object();

        public void Refresh()
        {
            lock (_syncroot)
            {
                foreach (KeyValuePair<Guid, WeakReference> pair in new List<KeyValuePair<Guid, WeakReference>>(this._remoteTracks))
                {
                    if (!pair.Value.IsAlive)
                    {
                        this._remoteTracks.Remove(pair.Key); 
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            RefreshManager.Unregister(this);
        }

        #endregion

        private Dictionary<Guid, WeakReference> _remoteTracks = new Dictionary<Guid, WeakReference>();

        public RemoteTrack FindTrack(Guid id)
        {
            RemoteTrack rs = null;
            lock (_syncroot)
            {
                WeakReference wr;
                if (_remoteTracks.TryGetValue(id, out wr))
                {
                    if (wr.IsAlive)
                    {
                        rs = (RemoteTrack)wr.Target; 
                    }
                }
            }

            return rs;
        }

        public bool CreateTrack(Guid id, string name, string category, IDictionary<string, object> values, bool reconnect, out RemoteTrack track)
        {

            lock (_syncroot)
            {
                RemoteTrack t = FindTrack(id); 
                if (!reconnect && t != null)
                {
                    track = null;
                    return false;
                }

                if (t != null)
                {
                    if (values != null)
                    {
                        foreach (KeyValuePair<string, object> pair in values)
                        {
                            t[pair.Key] = pair.Value;
                        }
                    }
                }
                else
                {
                    t = new RemoteTrack(id, name, category, values);
                    _remoteTracks[id] = new WeakReference(t, true);
                    Task.Factory.StartNew(() =>
                    {
                        OnAdded(t);
                    });
                }

                track = t;

            }

           
            return true;
        }



        private List<WeakReference> _handlers = new List<WeakReference>();

        public void AddHandler(ITrackManagerServiceHandler handler)
        {
            lock (_handlers)
            {
                this._handlers.Add(new WeakReference(handler, true)); 
            }
        }

        public void RemoveHandler(ITrackManagerServiceHandler handler)
        {
            lock (_handlers)
            {
                WeakReference wr = this._handlers.Find(w => (ITrackManagerServiceHandler)w.Target == handler);
                if (wr != null)
                {
                    this._handlers.Remove(wr);
                }
                
            }
        }

        public TrackMetaData[] GetAllTrackMetaData()
        {
            List<TrackMetaData> rs = new List<TrackMetaData>();
            List<WeakReference> tracks;
            lock (this._remoteTracks)
            {
                tracks = new List<WeakReference>(this._remoteTracks.Values);
            }
           

            foreach(WeakReference wr in tracks)
            {
                if (wr.IsAlive)
                {
                    RemoteTrack t = (RemoteTrack)wr.Target;
                    rs.Add(new TrackMetaData() { Id = t.Id, Name = t.Name, Category = t.Category });
                }
            }
           

            return rs.ToArray();
        }

        private void OnAdded(RemoteTrack track)
        {
            TrackMetaData data = new TrackMetaData() { Id = track.Id, Name = track.Name, Category = track.Category };
            ConcurrentQueue<WeakReference> expires = new ConcurrentQueue<WeakReference>();
            List<WeakReference> handlers;
            lock (_handlers)
            {
                handlers = new List<WeakReference>(this._handlers);
            }
            Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(handlers, wr =>
                {
                    try
                    {
                        if (wr.IsAlive)
                        {
                            ITrackManagerServiceHandler handler = (ITrackManagerServiceHandler)wr.Target;
                            handler.HandleTrackActived(data);
                        }
                        else
                        {
                            expires.Enqueue(wr);
                        }
                    }
                    catch
                    {
                        expires.Enqueue(wr);
                    }
                }
                );
                lock (_handlers)
                {
                    foreach (WeakReference wr in expires)
                    {
                        this._handlers.Remove(wr);
                    }
                }
            });
        }

        public void RemoveTrack(RemoteTrack remoteTrack)
        {
            lock (_remoteTracks)
            {
                this._remoteTracks.Remove(remoteTrack.Id);
            }
        }
    }
}
