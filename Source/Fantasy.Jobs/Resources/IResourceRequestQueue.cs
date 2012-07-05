using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public interface IResourceRequestQueue
    {
        void RegisterResourceRequest(Guid jobId, ResourceParameter[] parameters);
        void UnregisterResourceRequest(Guid jobId);

        ResourceParameter[] GetRequiredResources(Guid jobId);
    }
}
