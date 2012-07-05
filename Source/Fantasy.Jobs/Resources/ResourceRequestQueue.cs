using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Resources
{
    public class ResourceRequestQueue : AbstractService, IResourceRequestQueue
    {

        private Dictionary<Guid, ResourceParameter[]> _queue = new Dictionary<Guid, ResourceParameter[]>();

        private object _syncRoot = new object();
        

        public void RegisterResourceRequest(Guid jobId, ResourceParameter[] parameters)
        {
            lock (_syncRoot)
            {
                if (!_queue.ContainsKey(jobId))
                {
                    _queue.Add(jobId, parameters);
                }
            }
        }

        public void UnregisterResourceRequest(Guid jobId)
        {
            lock (_syncRoot)
            {
                this._queue.Remove(jobId);
            }
        }



        #region IResourceRequestQueue Members


        public ResourceParameter[] GetRequiredResources(Guid jobId)
        {
            ResourceParameter[] rs;
            lock (_syncRoot)
            {
                rs = this._queue.GetValueOrDefault(jobId, new ResourceParameter[] { });  
            }
            return rs;
        }

        #endregion
    }
}
