using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Fantasy.Jobs.Management;
using System.Threading;
using Fantasy.ServiceModel;
using Fantasy.Tracking;

namespace Fantasy.Jobs.Client
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class JobListener : IJobEventHandler, IDisposable
    {

        public JobListener()
        {
            _refreshThread = ThreadFactory.CreateThread(this.Refresh);
            

            _listenedJobs = new Collection<Guid>();
            _listenedJobs.Inserted += new EventHandler<CollectionEventArgs<Guid>>(ListenedJobsInserted);
            _listenedJobs.Removed += new EventHandler<CollectionEventArgs<Guid>>(ListenedJobsRemoved);
            _listenedJobs.Cleared += new EventHandler(ListenedJobsCleared);
            
        }

        void ListenedJobsCleared(object sender, EventArgs e)
        {
            
        }

        void ListenedJobsRemoved(object sender, CollectionEventArgs<Guid> e)
        {
            
        }

        void ListenedJobsInserted(object sender, CollectionEventArgs<Guid> e)
        {
            
        }

        public void Start()
        {
            _refreshThread.Start();
        }


        private bool _connected;

        public bool Connected
        {
            get { return _connected; }
            set 
            {
                if (_connected != value)
                {
                    _connected = value;

                    if (_connected)
                    {
                        this.OnConnect(EventArgs.Empty);
                    }
                    else
                    {
                        this.OnDisconnect(EventArgs.Empty); 
                    }
                }
            }
        }

        public event EventHandler Connect;

        protected virtual void OnConnect(EventArgs e)
        {
            if (this.Connect != null)
            {
                this.Connect(this, e);
            }
        }


        public event EventHandler Disconnect;

        protected virtual void OnDisconnect(EventArgs e)
        {
            if (this.Disconnect != null)
            {
                this.Disconnect(this, e);
            }
        }


        public event EventHandler<JobListenerJobEventArgs> JobAdded;

        protected virtual void OnJobAdded(JobListenerJobEventArgs e)
        {
            if (this.JobAdded != null)
            {
                this.JobAdded(this, e);
            }
        }


        public event EventHandler<JobListenerJobEventArgs > JobStateChanged;

        protected virtual void OnJobStateChanged(JobListenerJobEventArgs  e)
        {
            if (this.JobStateChanged != null)
            {
                this.JobStateChanged(this, e);
            }
        }


        public event EventHandler<JobListenerTrackEventArgs> TrackCreated;

        protected virtual void OnTrackCreated(JobListenerTrackEventArgs e)
        {
            if (this.TrackCreated != null)
            {
                this.TrackCreated(this, e);
            }
        }

        public event EventHandler<JobListenerTrackEventArgs> TrackChanged;

        protected virtual void OnTrackChanged(JobListenerTrackEventArgs e)
        {
            if (this.TrackChanged != null)
            {
                this.TrackChanged(this, e);
            }
        }


        private Collection<Guid> _listenedJobs;

        public IList<Guid> ListenedJobs
        {
            get { return _listenedJobs; }
        }

       

        private object _syncRoot = new object();

        private Thread _refreshThread;
        private ClientRef<IJobEventService> _jobEvent;

        private TrackFactory _trackConnection;

        public long Version { get; private set; }

        private void NewVersion()
        {
            lock (this._syncRoot)
            {
                Version = DateTime.Now.Ticks;
            }
        }

        private List<JobMetaData> _jobs = new List<JobMetaData>();
        public JobMetaData[] JobMetaData
        {
            get
            {
                lock (this._syncRoot)
                {
                    return _jobs.ToArray();
                }
            }
        }

        private List<ITrackListener> _tracks = new List<ITrackListener>();
        public ITrackListener[] Tracks
        {
            get
            {
                lock (this._syncRoot)
                {
                    return _tracks.ToArray();
                }
            }
        }


        private void Refresh()
        {
            TimeSpan waitTime;
            _trackConnection = new TrackFactory();
            while (true)
            {

                try
                {
                    bool reload = false;

                    if (_jobEvent == null)
                    {
                        reload = true;
                        _jobEvent = ClientFactory.CreateDuplex<IJobEventService>(this);
                        _jobEvent.Client.Connect();
                    }
                    else
                    {
                        _jobEvent.Client.Echo();
                    }
                    if (reload)
                    {

                        using (ClientRef<IJobService> service = ClientFactory.Create<IJobService>())
                        {
                            foreach (Guid id in this._listenedJobs)
                            {

                                JobMetaData job = service.Client.FindJobById(id);
                                if (job != null)
                                {

                                    JobMetaData old = this._jobs.Find(j => j.Id == id);
                                    if (old == null)
                                    {
                                        this._jobs.Add(job);
                                        this.OnJobAdded(new JobListenerJobEventArgs(job));
                                    }
                                    else if (old.State != job.State)
                                    {
                                        this.OnJobStateChanged(new JobListenerJobEventArgs(job));
                                    }

                                    CreateOrDisposeTrack(job);
                                }
                            }
                        }

                        lock (_syncRoot)
                        {
                            this.NewVersion();
                        }
                    }
                    waitTime = new TimeSpan(0, 2, 0);
                    this.Connected = true;
                }
                catch (ThreadAbortException)
                {
                    waitTime = TimeSpan.MinValue; 
                    if (_jobEvent != null && _jobEvent.State == CommunicationState.Opened)
                    {
                        _jobEvent.Client.Disconnect();
                    }
                }
                catch
                {
                    _jobEvent = null;
                    lock (_syncRoot)
                    {
                        this.NewVersion();
                    }
                    waitTime = new TimeSpan(0, 0, 10);
                    this.Connected = false;
                }
                Thread.Sleep(waitTime);
            }
        }

        private void CreateOrDisposeTrack(JobMetaData job)
        {
            Guid id = job.Id;
            ITrackListener oldTrack = this._tracks.Find(t => t.Id == id);
            if (job.State == JobState.Running && oldTrack == null)
            {
                ITrackListener track = this._trackConnection.CreateTrackListener(id);
                track.Changed += new EventHandler<TrackChangedEventArgs>(TrackListenerChanged);
                this._tracks.Add(track);
                this.OnTrackCreated(new JobListenerTrackEventArgs(track, null));
            }
            else if (job.State != JobState.Running && oldTrack != null)
            {
                this._tracks.Remove(oldTrack);
                oldTrack.Changed -= new EventHandler<TrackChangedEventArgs>(TrackListenerChanged);
                oldTrack.Dispose();
            }
        }

        void TrackListenerChanged(object sender, TrackChangedEventArgs e)
        {
            this.OnTrackChanged(new JobListenerTrackEventArgs((ITrackListener)sender, e)); 
        }

      
        #region IJobEventHandler Members

        void IJobEventHandler.Added(JobMetaData job)
        {
            lock (_syncRoot)
            {
                if (this._listenedJobs.IndexOf(job.Id) >= 0 && this._jobs.Find(j => j.Id == job.Id) == null)
                {
                    this._jobs.Add(job);
                    this.OnJobAdded(new JobListenerJobEventArgs(job));
                }
            }
        }

        void IJobEventHandler.Changed(JobMetaData job)
        {
            lock (_syncRoot)
            {
                if (this._listenedJobs.IndexOf(job.Id) >= 0)
                {
                    int idx = this._jobs.FindIndex(j => j.Id == job.Id);
                    if (idx < 0)
                    {
                        this._jobs.Add(job);
                        this.OnJobAdded(new JobListenerJobEventArgs(job));
                    }
                    else
                    {
                        this._jobs[idx] = job;
                        this.OnJobStateChanged(new JobListenerJobEventArgs(job)); 
                    }

                    CreateOrDisposeTrack(job);
                }
                this.NewVersion();
            }
        }

        void IJobEventHandler.Echo()
        {

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this._refreshThread.Abort();
        }

        #endregion
    }


    public class JobListenerJobEventArgs : EventArgs
    {
    
        internal JobListenerJobEventArgs(JobMetaData job)
        {
            this.Job = job;
        }

        public JobMetaData Job { get; private set; }
    }

    public class JobListenerTrackEventArgs : EventArgs
    {

        internal JobListenerTrackEventArgs(ITrackListener track, TrackChangedEventArgs e)
        {
            this.Track = track;
            if (e != null)
            {
                Name = e.Name;
                OldValue = e.OldValue;
                NewValue = e.NewValue;
            }
        }

        public ITrackListener Track { get; private set; }

        public Guid Id
        {
            get
            {
                return Track.Id;
            }
        }

        public string Name { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }
    }
}
