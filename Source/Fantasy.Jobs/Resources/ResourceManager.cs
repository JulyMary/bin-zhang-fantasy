using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Resources
{
    public class ResourceManager : AbstractService, IResourceManager
    {
        protected IResourceProvider[] _providers;

        private List<ResourceGroup> _allocatedResources = new List<ResourceGroup>();

        private  Thread _checkThread;

        private void CheckJobsExist()
        {
            IJobController controller = this.Site.GetRequiredService<IJobController>();
            TimeSpan timeout = new TimeSpan(0, 5, 0);
            ILogger logger = this.Site.GetService<ILogger>();
            while (true)
            {
                Thread.Sleep(15000);
                DateTime now = DateTime.Now;
                foreach (ResourceGroup resource in _allocatedResources.ToArray())
                {
                    try
                    {
                        if (!controller.IsJobProccessExisted(resource.JobId) )
                        {
                            if (logger != null)
                            {
                                logger.LogMessage("ResourceManager", "Job {0} has exited, force release resource.", resource.JobId); 
                            }

                            Release(resource);
                        }
                    }
                    catch
                    {

                    }
                }
            }

        }

        public override void InitializeService()
        {
            _providers = AddIn.CreateObjects<IResourceProvider>("jobService/resources/provider");
            foreach (IResourceProvider provider in this._providers)
            {
                if (provider is IObjectWithSite)
                {
                    ((IObjectWithSite)provider).Site = this.Site; 
                }
                provider.Available += new EventHandler(ProviderResourceAvailable);
                provider.Revoke += new EventHandler<ProviderRevokeArgs>(ProviderResourceRevoke);
                provider.Initialize();
            }

            base.InitializeService();

            _checkThread = ThreadFactory.CreateThread(this.CheckJobsExist).WithStart();
           
           
        }


        private int _availableLock = 0;
        private object _availableSyncRoot = new object();

        private void LockAvaialbe()
        {
            lock (_availableSyncRoot)
            {
                _availableLock++;
            }
        }

        private void UnlockAvaiable()
        {
            bool fire = false;
            lock (_availableSyncRoot)
            {
                if (_availableLock > 0)
                {
                    _availableLock--;
                    fire = _availableLock == 0;
                };

            }
            if (fire)
            {
                this.OnAvailable(EventArgs.Empty);
            }
        }

        void ProviderResourceRevoke(object sender, ProviderRevokeArgs e)
        {
            this.LockAvaialbe();
            try
            {
                IGrouping<Guid, ResourceGroup>[] groups;
                lock (_resSyncRoot)
                {
                    groups = (from r in this._allocatedResources where r.ContainsResource(e.Resource) group r by r.JobId into g select g).ToArray();

                }

                Parallel.ForEach(groups, g =>
                //foreach(IGrouping<Guid, ResourceGroup> g in groups)
                {
                    foreach (ResourceGroup r in g)
                    {
                        IResourceManagerHandler handler = null;
                        lock (_handlers)
                        {
                            handler = this._handlers.GetValueOrDefault(g.Key);
                        }
                        if (handler != null)
                        {
                            if (!r.Released)
                            {
                                handler.Revoke(r.Id);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                });
            }
            finally
            {
                this.UnlockAvaiable(); 
            }
            
        }

        void ProviderResourceAvailable(object sender, EventArgs e)
        {
            this.LockAvaialbe();
            this.UnlockAvaiable();
            
        }


        public event EventHandler Available;

        protected virtual void OnAvailable(EventArgs e)
        {
            if (this.Available != null)
            {
                this.Available(this, e);
            }
        }


        private void Release(ResourceGroup res)
        {
            this.LockAvaialbe();
            try
            {
                
                this._allocatedResources.Remove(res);
                res.Release();
            }
            finally
            {
                this.UnlockAvaiable();
            }
        }


        public virtual void Release(Guid id)
        {
            lock (_resSyncRoot)
            {
                ResourceGroup res = this._allocatedResources.Where(r => r.Id == id).SingleOrDefault();
                if (res != null)
                {
                    Release(res);
                }
            }
        }

        protected object _resSyncRoot = new object();

        public override void UninitializeService()
        {
            _checkThread.Abort();
            base.UninitializeService();
            foreach (IResourceProvider provider in this._providers)
            {
                if (provider is IDisposable)
                {
                    ((IDisposable)provider).Dispose();
                }
            }
        }

        public virtual  Guid Request(Guid jobId, ResourceParameter[] parameters)
        {
            Guid rs = Guid.Empty;
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");  
            }
            lock (this._resSyncRoot)
            {
                ResourceGroup group = new ResourceGroup(jobId, parameters);
                group.Site = this.Site;
                try
                {
                    
                    bool available = true;
                    var query = from param in parameters
                                from provider in this._providers where provider.CanHandle(param.Name)
                                select new { param = param, provider = provider };
                    foreach (var test in query)
                    {
                        object resource = null;
                        available = test.provider.Request(test.param, out resource);
                        if (available)
                        {
                            if (resource != null)
                            {
                                group.AddResource(test.provider, resource);
                            }
                        }
                        else
                        {
                            break;
                        }
                        
                    }
                    if (available)
                    {
                        rs = group.Id;
                       
                        this._allocatedResources.Add(group);
                    }
                    else
                    {
                        group.Release();
                    }
                }
                    
                catch(Exception error)
                {
                    if( ! (error is ThreadAbortException))
                    {
                        ILogger logger = this.Site.GetService<ILogger>();
                        if (logger != null)
                        {
                            logger.LogError("ResourceManager", error, "An error occurs when request a resource.");
                        }
                    }
                    group.Release();
                }
            }

            return rs;
        }

       

        
        public bool IsAvailable(ResourceParameter[] parameters)
        {
            lock (_resSyncRoot)
            {
                var query = from param in parameters
                            from provider in this._providers where provider.CanHandle(param.Name)
                            select new { param = param, provider = provider };
                foreach (var test in query)
                {
                    if (test.provider.IsAvailable(test.param) == false)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private Dictionary<Guid,IResourceManagerHandler> _handlers = new Dictionary<Guid,IResourceManagerHandler>();

        public void RegisterHandler(IResourceManagerHandler handler)
        {
            lock (_handlers)
            {
                _handlers.Add(handler.Id(), handler);
            }
        }

        public void UnregisterHandler(Guid id)
        {
            lock (_handlers)
            {
                _handlers.Remove(id);
            }
        }

        
    }
}
