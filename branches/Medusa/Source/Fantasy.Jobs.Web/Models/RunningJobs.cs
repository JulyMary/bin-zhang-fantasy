using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.Jobs.Management;
using System.Threading;
using System.ServiceModel;
using Fantasy.Tracking;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Web.Models
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class RunningJobs : IJobEventHandler
    {
        private RunningJobs()
        {
            _refreshThread = ThreadFactory.CreateThread(this.Refresh).WithStart();
            
            
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
        public JobMetaData[] Jobs
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
                    _jobEvent.Client.Echo();
                    if (reload)
                    {
                        JobMetaData[] jobs;
                        using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
                        {
                            int total;
                            jobs = svc.Client.FindUnterminatedJob(out total, "job.State == JobState.Running");
                        }
                        ITrackListener[] tracks = new ITrackListener[jobs.Length];
                        for (int i = 0; i < jobs.Length; i++)
                        {
                            tracks[i] = _trackConnection.CreateTrackListener(jobs[i].Id);
                        }
                        lock (_syncRoot)
                        {

                            this.CleanUp();
                            this._jobs.AddRange(jobs);
                            this._tracks.AddRange(tracks);
                            this.NewVersion();
                        }
                    }
                    waitTime = new TimeSpan(0, 2, 0);
                }
                catch
                {
                    _jobEvent = null;
                    lock (_syncRoot)
                    {
                        CleanUp();
                        this.NewVersion();


                    }
                    waitTime = new TimeSpan(0, 0, 10);
                }
                Thread.Sleep(waitTime);
            }
        }

        private void CleanUp()
        {
            this._jobs.Clear();
            foreach (ITrackListener track in this._tracks)
            {
                track.Dispose();
            }
            this._tracks.Clear();
        }

        private static RunningJobs _default;
        public static RunningJobs Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new RunningJobs();
                }
                return _default;
            }
        }

        public void Run()
        {

        }

        #region IJobEventHandler Members

        void IJobEventHandler.Added(JobMetaData job)
        {

        }

        void IJobEventHandler.Changed(JobMetaData job)
        {
            if (job.State == JobState.Running)
            {
                lock (_syncRoot)
                {
                    if (this._jobs.Find(j => j.Id == job.Id) == null)
                    {
                        this._jobs.Add(job);
                        ITrackListener track = this._trackConnection.CreateTrackListener(job.Id);
                        this._tracks.Add(track);
                        this.NewVersion();
                    }
                }
            }
            else //if ((job.State & JobState.Terminated) == JobState.Terminated)
            {
                lock (_syncRoot)
                {
                    JobMetaData local = this._jobs.Find(j => j.Id == job.Id);
                    if (local != null)
                    {
                        this._jobs.Remove(local);
                        NewVersion();
                    }
                    ITrackListener track = this._tracks.Find(t => t.Id == job.Id);

                    if (track != null)
                    {
                        track.Dispose();
                        _tracks.Remove(track);
                    }
                }
            }
        }

        void IJobEventHandler.Echo()
        {

        }

        #endregion
    }
}