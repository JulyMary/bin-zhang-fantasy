using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public interface IResourceProvider
    {
        bool CanHandle(string name);

        void Initialize();

        bool IsAvailable(ResourceParameter parameter);

        bool Request(ResourceParameter parameter, out object resource);

        void Release(object resource);

        event EventHandler Available;

        event EventHandler<ProviderRevokeArgs> Revoke;
    }
}
