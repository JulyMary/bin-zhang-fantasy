using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Fantasy.XSerialization;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Linq;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public class JobController : AbstractService, IJobController, IJobEngineEventHandler
    {

        public override object InitializeLifetimeService()
        {
            return null;
        }


        private Thread _checkProcessThread;
        public override void InitializeService()
        {

            base.InitializeService();
            _checkProcessThread = ThreadFactory.CreateThread(this.CheckProcessExist).WithStart();
           
        }



        private bool _uninitializing = false;
        public override void UninitializeService()
        {
            _uninitializing = true;
            base.UninitializeService();
            _checkProcessThread.Abort();
            this.SuspendAll(true);
            
        }

        private List<JobProcess> _process = new List<JobProcess>();

        private void CheckProcessExist()
        {
           
            while (true)
            {
                Thread.Sleep(30 * 1000);
                lock (_syncRoot)
                {
                    foreach (JobProcess process in new List<JobProcess>(_process))
                    {
                        bool exited = true;
                        try
                        {
                            exited = process.Process.HasExited;
                        }
                        catch (Win32Exception)
                        {

                        }
                        catch (InvalidOperationException)
                        {

                        }
                        if (exited)
                        {
                            this.SetJobExited(process.Job.Id, JobState.Failed);
                        }
                    }
                }
            }
        }

        public bool IsJobProccessExisted(Guid id)
        {
            bool rs = false;
            JobProcess jp = GetJobProcessById(id);
            if (jp != null)
            {
                try
                {
                    rs = ! jp.Process.HasExited;
                }
                catch (Win32Exception)
                {

                }
                catch (InvalidOperationException)
                {

                }
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
            JobProcess jp = this.GetJobProcessById(id);
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
                        this._process.Remove(jp);
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

                Process process = CreateHostProcess(job);

                JobProcess jp = new JobProcess(job, process, true);
                this._process.Add(jp);
                process.Start();
                job.State = JobState.Running;
                this.CommitChange(job);
            }
        }

        private  Process CreateHostProcess(JobMetaData job)
        {
            Process process = new Process();

            process.StartInfo.FileName = string.Format("{0}", JobManagerSettings.Default.JobHostFullPath);
            process.StartInfo.Arguments = string.Format("/id:{0}", job.Id);
            process.StartInfo.ErrorDialog = false;
#if !DEBUG
                process.StartInfo.CreateNoWindow = true;
#endif
            process.StartInfo.UseShellExecute = false;
            return process;
        }


        public virtual void StartJob(JobMetaData job)
        {
            lock (_syncRoot)
            {
                Process process = CreateHostProcess(job);

                JobProcess jp = new JobProcess(job, process, false);
                this._process.Add(jp);
                process.Start();
                job.StartTime = DateTime.Now;
                job.State = JobState.Running;
                CommitChange(job);
            }

        }

        public virtual void Cancel(Guid id)
        {
            JobProcess jp = this.GetJobProcessById(id);
            if (jp != null && jp.Engine != null)
            {
                jp.Engine.Terminate();
            }
        }

        public virtual void Suspend(Guid id)
        {
            JobProcess jp = this.GetJobProcessById(id);
            if (jp != null && jp.Engine != null)
            {
                jp.Engine.Suspend();
            }

        }

        public virtual void UserPause(Guid id)
        {
            JobProcess jp = this.GetJobProcessById(id);
            if (jp != null && jp.Engine != null)
            {
                jp.Engine.UserPause();
            }
        }

        private JobProcess GetJobProcessById(Guid id)
        {
            lock (_syncRoot)
            {
                return (from p in this._process where p.Job.Id == id select p).SingleOrDefault();
            }
        }

        public virtual void RegisterJobEngine(IJobEngine engine)
        {
            JobProcess jp = this.GetJobProcessById(engine.JobId);
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
                    return JobManagerSettings.Default.JobProcessCount - this._process.Count;
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
                return this._process.Select(p => p.Job).ToArray();
            }
        }



        #region IJobController Members


        public void SuspendAll(bool waitForExit)
        {
            JobProcess[] process;
            lock (this._process)
            {
                process = this._process.ToArray();
            }
            Parallel.ForEach(this._process.ToArray(), p =>
            {
                try
                {
                    if (!p.Process.HasExited)
                    {
                        p.Engine.Suspend();
                        p.ExitEvent.WaitOne();
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
