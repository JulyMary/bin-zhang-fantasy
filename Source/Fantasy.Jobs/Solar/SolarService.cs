using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;
using System.ServiceModel;
using Fantasy.Jobs.Resources;
using System.Threading;

namespace Fantasy.Jobs.Solar
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Multiple, Namespace=Consts.JobServiceNamespaceURI)]
    public class SolarService : WCFSingletonService, ISolar
    {
        private IJobQueue _queue;

        public override void InitializeService()
        {
            this._queue = this.Site.GetRequiredService<IJobQueue>();
           
            base.InitializeService();
        }

        public override void UninitializeService()
        {

            base.UninitializeService();
           
        }

       

        public JobMetaData[] Unterminates(int skip, int take)
        {
            return this._queue.Unterminates.Skip(skip).Take(take).ToArray();
        }

        public JobMetaData[] Terminates(int skip, int take)
        {
            return this._queue.Terminates.Skip(skip).Take(take).ToArray();
        }

        public JobMetaData FindJobMetaDataById(Guid id)
        {
            return this._queue.FindJobMetaDataById(id);
        }

        public JobMetaData[] FindTerminated(out int totalCount, string filter, string[] args, string order, int skip, int take)
        {
            return this._queue.FindTerminated(out totalCount, filter, args, order, skip, take).ToArray();
        }

        public JobMetaData[] FindUnterminated(out int totalCount, string filter, string[] args, string order, int skip, int take)
        {
            return this._queue.FindUnterminated(out totalCount, filter, args, order, skip, take).ToArray();
        }

        public void ApplyChange(JobMetaData job)
        {
            this._queue.ApplyChange(job);
        }

        public void Resume(Guid id)
        {
            this._queue.Resume(id);
        }

        public void Cancel(Guid id)
        {
            this._queue.Cancel(id);
        }

        public void Suspend(Guid id)
        {
            this._queue.Suspend(id);
        }

        public void UserPause(Guid id)
        {
            this._queue.UserPause(id);
        }

        public string[] GetAllApplications()
        {
            return this._queue.GetAllApplications();
        }

        public string[] GetAllUsers()
        {
            return this._queue.GetAllUsers();
        }

 
        public void RegisterResourceRequest(Guid jobId, Resources.ResourceParameter[] parameters)
        {
            IResourceRequestQueue resQueue = this.Site.GetService<IResourceRequestQueue>();
            if (resQueue != null)
            {
                resQueue.RegisterResourceRequest(jobId, parameters);
            }
        }

       


        public void UnregisterResourceRequest(Guid jobId)
        {
            IResourceRequestQueue resQueue = this.Site.GetService<IResourceRequestQueue>();
            if (resQueue != null)
            {
                resQueue.UnregisterResourceRequest(jobId);
            }
        }

        public ResourceParameter[] GetRequiredResources(Guid jobId)
        {
            IResourceRequestQueue resQueue = this.Site.GetService<IResourceRequestQueue>();
            if (resQueue != null)
            {
                return resQueue.GetRequiredResources(jobId);
            }
            else
            {
                return new ResourceParameter[] { };
            }
        }

      

        public void ResourceAvaiable()
        {
            IJobDispatcher dispatcher = this.Site.GetRequiredService<IJobDispatcher>();
            dispatcher.TryDispatch();
        }


        public bool IsTermianted(Guid id)
        {
            return _queue.IsTerminated(id);
        }

       
    }
}
