using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public interface IResourceManager
    {
        Guid Request(Guid jobId, ResourceParameter[] parameters);

        void Release(Guid id);

        bool IsAvailable(ResourceParameter[] parameters);

        void RegisterHandler(IResourceManagerHandler handler);

        void UnregisterHandler(Guid id);

        event EventHandler Available;


    }
}
