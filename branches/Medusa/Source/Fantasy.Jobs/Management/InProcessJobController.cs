using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Threading;
using System.ComponentModel;
using System.Xml.Linq;
using Fantasy.XSerialization;
using System.Threading.Tasks;
using System.Reflection;

namespace Fantasy.Jobs.Management
{
    public class InProcessJobController : AbstractService, IJobController, IJobEngineEventHandler
    {
        public InProcessJobController()
        {
           _appDomainSetup = new AppDomainSetup();
                _appDomainSetup.ApplicationBase =
                    System.Environment.CurrentDirectory;
                _appDomainSetup.DisallowBindingRedirects = false;
                _appDomainSetup.DisallowCodeDownload = true;
                _appDomainSetup.ConfigurationFile =
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        }

        private Thread _checkProcessThread;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public override void InitializeService()
        {
            _checkProcessThread = ThreadFactory.CreateThread(this.CheckThreadExist).WithStart();
            base.InitializeService();
        }

        private void CheckThreadExist()
        {

            while (true)
            {
                Thread.Sleep(30 * 1000);
                lock (_syncRoot)
                {
                    foreach (JobThread jt in new List<JobThread>(this._threads))
                    {
                        bool exited = true;
                        exited = (jt.Thread.ThreadState & (ThreadState.Aborted | ThreadState.Stopped)) > 0; 
                        if (exited)
                        {
                            this.SetJobExited(jt.Job.Id, JobState.Failed);
                        }
                    }
                }
            }
        }

        private bool _uninitializing = false;
        public override void UninitializeService()
        {
            
            _uninitializing = true;
            this._checkProcessThread.Abort();
            base.UninitializeService();
            this.SuspendAll(true);

        }

        private List<JobThread> _threads = new List<JobThread>();

     



        public bool IsJobProccessExisted(Guid id)
        {
            bool rs = false;
            JobThread jp = GetJobThreadById(id);
            if (jp != null)
            {
                 rs = (jp.Thread.ThreadState & (ThreadState.Stopped | ThreadState.Aborted)) > 0;
            }

            return rs;
        }

        private object _syncRoot = new object();

        private void CommitChange(JobMetaData job)
        {
            IJobQueue queue = this.Site.GetRequiredService<IJobQueue>();
            queue.ApplyChange(job);
        }

        private void SetJobExited(Guid id, int exitState)
        {
            JobThread jp = this.GetJobThreadById(id);
            if (jp != null)
            {
                lock (_syncRoot)
                {
                    try
                    {
                        jp.Job.State = exitState;
                        if (jp.Job.IsTerminated)
                        {
                            jp.Job.EndTime = DateTime.Now;
                        }
                        this._threads.Remove(jp);
                        this.CommitChange(jp.Job);
                    }
                    finally
                    {
                        jp.ExitEvent.Set();
                    }
                }
            }
        }

        public virtual void Resume(JobMetaData job)
        {
            lock (_syncRoot)
            {

                Thread thread = CreateHostThread(job);

                JobThread jp = new JobThread(job, thread, true);
                this._threads.Add(jp);
                thread.Start();
                job.State = JobState.Running;
                this.CommitChange(job);
            }
        }


        private AppDomainSetup _appDomainSetup;

        private Thread CreateHostThread(JobMetaData job)
        {
            Thread rs = ThreadFactory.CreateThread(() =>
            {
                

                AppDomain dm = AppDomain.CreateDomain("AppDomain_" + job.Id.ToString(), null, _appDomainSetup);
                try
                {
                  

                    InProcessJobEngineHost jobHost = (InProcessJobEngineHost)dm.CreateInstanceAndUnwrap(typeof(InProcessJobEngineHost).Assembly.FullName, typeof(InProcessJobEngineHost).FullName);
                    jobHost.Run(JobManager.Default, job.Id);
                }
                finally
                {
                    AppDomain.Unload(dm);
                }
                
            });

            return rs;
        }


        public virtual void StartJob(JobMetaData job)
        {
            lock (_syncRoot)
            {
                Thread thread = CreateHostThread(job);

                JobThread jp = new JobThread(job, thread, false);
                this._threads.Add(jp);
                thread.Start();
                job.StartTime = DateTime.Now;
                job.State = JobState.Running;
                CommitChange(job);
            }

        }

        public virtual void Cancel(Guid id)
        {
            JobThread jp = this.GetJobThreadById(id);
            if (jp != null && jp.Engine != null)
            {
                jp.Engine.Terminate();
            }
        }

        public virtual void Suspend(Guid id)
        {
            JobThread jp = this.GetJobThreadById(id);
            if (jp != null && jp.Engine != null)
            {
                jp.Engine.Suspend();
            }

        }

        public virtual void UserPause(Guid id)
        {
            JobThread jp = this.GetJobThreadById(id);
            if (jp != null && jp.Engine != null)
            {
                jp.Engine.UserPause();
            }
        }

        private JobThread GetJobThreadById(Guid id)
        {
            lock (_syncRoot)
            {
                return (from p in this._threads where p.Job.Id == id select p).SingleOrDefault();
            }
        }

        public virtual void RegisterJobEngine(IJobEngine engine)
        {
            JobThread jp = this.GetJobThreadById(engine.JobId);
            if (jp != null)
            {
                jp.Engine = engine;
                engine.AddHandler(this);
                XElement doc = XElement.Parse(jp.Job.StartInfo);

                XSerializer ser = new XSerializer(typeof(JobStartInfo));
                JobStartInfo si = (JobStartInfo)ser.Deserialize(doc);
                if (!jp.IsResume)
                {
                    engine.Start(si);
                }
                else
                {
                    engine.Resume(si);
                }

            }
        }


        #region IJobEngineEventHandler Members

        void IJobEngineEventHandler.HandleStart(IJobEngine sender)
        {

        }

        void IJobEngineEventHandler.HandleResume(IJobEngine sender)
        {

        }

        void IJobEngineEventHandler.HandleExit(IJobEngine sender, JobExitEventArgs e)
        {
            this.SetJobExited(sender.JobId, e.ExitState);
        }

        void IJobEngineEventHandler.HandleLoad(IJobEngine sender)
        {

        }

        #endregion

        public virtual int GetAvailableProcessCount()
        {
            if (!_uninitializing)
            {
                lock (_syncRoot)
                {
                    return JobManagerSettings.Default.JobProcessCount - this._threads.Count;
                }
            }
            else
            {
                return 0;
            }
        }






        public JobMetaData[] GetRunningJobs()
        {
            lock (_syncRoot)
            {
                return this._threads.Select(p => p.Job).ToArray();
            }
        }



        #region IJobController Members


        public void SuspendAll(bool waitForExit)
        {
            JobThread[] process;
            lock (this._threads)
            {
                process = this._threads.ToArray();
            }
            Parallel.ForEach(this._threads.ToArray(), p =>
            {
                try
                {
                    if (p.Thread.IsAlive)
                    {
                        p.Engine.Suspend();
                        p.ExitEvent.WaitOne(10*1000);
                    }
                }
                catch
                {

                }
            });
        }

        #endregion
    }
}
