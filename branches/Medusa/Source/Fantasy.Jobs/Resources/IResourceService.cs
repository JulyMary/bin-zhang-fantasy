using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public interface IResourceService
    {
        IResourceHandle Request(ResourceParameter[] parameter);

        IResourceHandle TryRequest(ResourceParameter[] parameters);

        void Release(IResourceHandle resource);
    }
}
