using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public interface IResourceHandle : IDisposable 
    {
        event EventHandler<RevokeArgs> Revoke;
    }
}
