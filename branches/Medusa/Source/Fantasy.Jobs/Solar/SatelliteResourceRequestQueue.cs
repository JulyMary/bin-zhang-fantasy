using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Resources;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Solar
{
    public class SatelliteResourceRequestQueue : AbstractService, IResourceRequestQueue
    {
        private ISolarActionQueue _actionQueue;

        public override void InitializeService()
        {
            this._actionQueue = this.Site.GetRequiredService<ISolarActionQueue>();
            base.InitializeService();
        }

        #region IResourceRequestQueue Members

        public void RegisterResourceRequest(Guid jobId, ResourceParameter[] parameters)
        {
            this._actionQueue.Enqueue(solar => solar.RegisterResourceRequest(jobId, parameters));
        }

        public void UnregisterResourceRequest(Guid jobId)
        {
            this._actionQueue.Enqueue(solar => solar.UnregisterResourceRequest(jobId));
        }

        public ResourceParameter[] GetRequiredResources(Guid jobId)
        {
            using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
            {
                return client.Client.GetRequiredResources(jobId);
            }
        }

        #endregion
    }
}
