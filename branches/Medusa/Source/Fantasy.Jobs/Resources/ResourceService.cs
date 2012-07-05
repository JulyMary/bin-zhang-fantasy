using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Resources
{
    public class ResourceService : AbstractService, IResourceService, IResourceManagerHandler 
    {

        public override object InitializeLifetimeService()
        {
            return null;
        }

        private Dictionary<Guid, ResourceHandle> _resources = new Dictionary<Guid, ResourceHandle>();

        public IResourceHandle Request(ResourceParameter[] parameters)
        {

            ResourceHandle rs = InnerRequest(parameters);
            if (rs != null)
            {
                rs.SuspendEngine = true;
                return rs;
            }
            else
            {
                IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
                IResourceRequestQueue queue = this.Site.GetRequiredService<IResourceRequestQueue>();
                queue.RegisterResourceRequest(engine.JobId, parameters);
                ILogger logger = this.Site.GetService<ILogger>();
                logger.LogMessage("Resource", "Suspend job engine because it failed to request resource:" + Environment.NewLine + string.Join(Environment.NewLine, parameters.Select(p=>p.ToString())));
                Suspend();
                return null;
            }
        }

        private void Suspend()
        {
            IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
            engine.Suspend();
            //ManualResetEvent _waitHandler = new ManualResetEvent(false);
            //Thread thread = ThreadFactory.CreateThread(() => {
            //    ILogger logger = this.Site.GetService<ILogger>();
            //    logger.SafeLogMessage("Resource", "Suspending thread started.");

            //    engine.Suspend();

            //    _waitHandler.Set();
            //});
            //thread.Start();
            
          
            //_waitHandler.WaitOne();
            //Thread.CurrentThread.Abort();
           
        }

        private ResourceHandle InnerRequest(ResourceParameter[] parameters)
        {
            IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
            IResourceManager manager = this.Site.GetRequiredService<IResourceManager>();
            Guid resId = manager.Request(engine.JobId, parameters);
            if (resId != Guid.Empty)
            {
                ResourceHandle rs = new ResourceHandle() { Id = resId, ResourceService = this, Parameters = parameters };
                lock (this._resources)
                {
                    this._resources.Add(resId, rs);
                }
                return rs;
            }
            else
            {
                return null;
            }
        }

        public IResourceHandle TryRequest(ResourceParameter[] parameters)
        {
            return InnerRequest(parameters);
        }

        public void Release(IResourceHandle resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource"); 
            }
            IResourceManager manager = this.Site.GetRequiredService<IResourceManager>();
            manager.Release(((ResourceHandle)resource).Id); 
        }


        #region IResourceManagerHandler Members

        public override void InitializeService()
        {
            IResourceManager manager = this.Site.GetRequiredService<IResourceManager>();
            manager.RegisterHandler(this); 
            base.InitializeService();
        }

        public override void UninitializeService()
        {
            base.UninitializeService();
            IResourceManager manager = this.Site.GetRequiredService<IResourceManager>();
            IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
            manager.UnregisterHandler(engine.JobId); 
        }

        void IResourceManagerHandler.Revoke(Guid id)
        {
            ResourceHandle resource;
            lock (_resources)
            {
                resource = this._resources.GetValueOrDefault(id); 
            }
            if (resource != null)
            {
                try
                {
                    RevokeArgs args = new RevokeArgs();
                    resource.OnRevoke(args);

                    if (resource.SuspendEngine && args.SuspendEngine)
                    {
                        ILogger logger = this.Site.GetService<ILogger>();
                        if (logger != null)
                        {
                            logger.LogMessage("Resource", "Suspend job engine because a resource is revoked." + Environment.NewLine 
                                + string.Join(Environment.NewLine, resource.Parameters.Select(p=>p.ToString())));
                        }

                        IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
                        IResourceRequestQueue queue = this.Site.GetRequiredService<IResourceRequestQueue>();
                        queue.RegisterResourceRequest(engine.JobId, resource.Parameters);
                        Suspend();
                    }
                }
                finally
                {
                    resource.Dispose();
                }
            }
        }

        #endregion

        #region IResourceManagerHandler Members


        Guid IResourceManagerHandler.Id()
        {
           
            IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
            return engine.JobId;
            
        }

        #endregion
    }
}
