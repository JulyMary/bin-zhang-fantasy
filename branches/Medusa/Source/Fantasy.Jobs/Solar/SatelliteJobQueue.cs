using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Solar
{
    public class SatelliteJobQueue : AbstractService, IJobQueue
    {

        public override void InitializeService()
        {
            this._actionQueue = this.Site.GetRequiredService<ISolarActionQueue>();
            this._satellite = this.Site.GetRequiredService<ISatellite>();
            this._satellite.JobAdded += new EventHandler<JobQueueEventArgs>(SatelliteJobAdded);
            this._satellite.JobChanged += new EventHandler<JobQueueEventArgs>(SatelliteJobChanged);
            base.InitializeService();
        }

        void SatelliteJobChanged(object sender, JobQueueEventArgs e)
        {
            this.OnChanged(e);
        }

        void SatelliteJobAdded(object sender, JobQueueEventArgs e)
        {
            this.OnAdded(e);
        }


        private ISolarActionQueue _actionQueue;
        private ISatellite _satellite;

        #region IJobQueue Members

        public IEnumerable<JobMetaData> Unterminates
        {
            get 
            {
                using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
                {
                    int count = Int32.MaxValue;
                    int i = 0;
                    while (count >= 100)
                    {
                        JobMetaData[] rs = client.Client.Unterminates(i * 100, 100);
                        i += rs.Length;
                        count = rs.Length;

                        foreach (JobMetaData job in rs)
                        {
                            yield return job;
                        }
                    }
                }
                
            }
        }

        public IEnumerable<JobMetaData> Terminates
        {
            get
            {
                using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
                {
                    int count = Int32.MaxValue;
                    int i = 0;
                    while (count >= 100)
                    {
                        JobMetaData[] rs = client.Client.Terminates(i * 100, 100);
                        i += rs.Length;
                        count = rs.Length;

                        foreach (JobMetaData job in rs)
                        {
                            yield return job;
                        }
                    }
                }
            }
        }

        public JobMetaData FindJobMetaDataById(Guid id)
        {
            using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
            {
                return client.Client.FindJobMetaDataById(id);
            }
        }

        public IEnumerable<JobMetaData> FindTerminated(out int totalCount, string filter, string[] args, string order, int skip, int take)
        {
            using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
            {
                return client.Client.FindTerminated(out totalCount, filter, args, order, skip, take);
            }
        }

        public IEnumerable<JobMetaData> FindUnterminated(out int totalCount, string filter, string[] args, string order, int skip, int take)
        {
            using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
            {
                return client.Client.FindUnterminated(out totalCount, filter, args, order, skip, take);
            }
        }

        public IEnumerable<JobMetaData> FindAll()
        {
            return this.Unterminates.Union(this.Terminates);
        }

        public JobMetaData CreateJobMetaData()
        {
            return new JobMetaData();
        }

        public void ApplyChange(JobMetaData job)
        {
            _actionQueue.Enqueue(solar => solar.ApplyChange(job));
        }

        public void Resume(Guid id)
        {
            _actionQueue.Enqueue(solar => solar.Resume(id));
        }

        public void Cancel(Guid id)
        {
            _actionQueue.Enqueue(solar => solar.Cancel(id));
        }

        public void Suspend(Guid id)
        {
            _actionQueue.Enqueue(solar => solar.Suspend(id));
        }

        public void UserPause(Guid id)
        {
            _actionQueue.Enqueue(solar => solar.UserPause(id));
        }

        

        public event EventHandler<JobQueueEventArgs> Changed;

        protected virtual void OnChanged(JobQueueEventArgs e)
        {
            if (this.Changed != null)
            {
                this.Changed(this, e);
            }
        }


        

        public event EventHandler<JobQueueEventArgs> Added;

        protected virtual void OnAdded(JobQueueEventArgs e)
        {
            if (this.Added != null)
            {
                this.Added(this, e);
            }
        }


        event EventHandler<JobQueueEventArgs> IJobQueue.RequestCancel
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler<JobQueueEventArgs> IJobQueue.RequestSuspend
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event EventHandler<JobQueueEventArgs> IJobQueue.RequestUserPause
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        public string[] GetAllApplications()
        {
            using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
            {
                return client.Client.GetAllApplications();
            }
        }

        public string[] GetAllUsers()
        {
            using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
            {
                return client.Client.GetAllUsers();
            }
        }

        public bool IsTerminated(Guid id)
        {
            using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
            {
                return client.Client.IsTermianted(id);
            }
        }

        #endregion
    }
}
