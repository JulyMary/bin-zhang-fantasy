using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fantasy.Jobs.Resources
{
    internal class ResourceHandle : IResourceHandle
    {
        public IResourceService ResourceService {get;set;}

        public bool SuspendEngine { get; set; }

        public ResourceParameter[] Parameters { get; set; }

        public Guid Id { get; set; }

        private bool _disposed = false;

        public void Dispose()
        {
            if(!_disposed)
            {
                _disposed = true;
                this.ResourceService.Release(this); 
            }
        }

        public event EventHandler<RevokeArgs> Revoke;

        public virtual void OnRevoke(RevokeArgs e)
        {
            if (this.Revoke != null)
            {
                this.Revoke(this, e);
            }
        }
        
    }
}
