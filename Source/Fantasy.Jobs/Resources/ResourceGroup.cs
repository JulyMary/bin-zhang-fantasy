using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Resources
{
    internal class ResourceGroup : ObjectWithSite
    {

        

        public ResourceGroup(Guid jobId, ResourceParameter[] parameters )
        {
            this.JobId = jobId;
            this.Parameters = (ResourceParameter[])parameters.Clone();
            this.Id = Guid.NewGuid();
            this.CreationTime = DateTime.Now;
        }

        public DateTime CreationTime { get; private set; }

        public ResourceParameter[] Parameters { get; private set; }

        public Guid Id { get; private set; }

        public Guid JobId { get; private set; }

        private Dictionary<IResourceProvider, object> _resources = new Dictionary<IResourceProvider, object>();

        public bool ContainsResource(object resource)
        {
            return _resources.ContainsValue(resource);  
        }

        public void AddResource(IResourceProvider provider, object resource)
        {
            this._resources[provider] = resource;
        }

        public void Release()
        {
            _released = true;
            ILogger logger = this.Site.GetService<ILogger>();
            foreach(KeyValuePair<IResourceProvider, object> pair in this._resources)
            {
                IResourceProvider provider = pair.Key;
                object res = pair.Value;
                try
                {
                    provider.Release(res);
                    
                }
                catch(ThreadAbortException)
                {
                }
                catch(Exception error)
                {
                    if (logger != null)
                    {
                        logger.LogWarning("Resource", error, MessageImportance.Normal, "A error occurs when release resource {0}", provider.GetType());   
                    }
                }
            }

            this._resources.Clear();


        }

        private bool _released = false;

        public bool Released
        {
            get { return _released; }
        }

    }
}
