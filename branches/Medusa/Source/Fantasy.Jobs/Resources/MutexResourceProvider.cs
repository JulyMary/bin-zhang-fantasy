using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public class MutexResourceProvider : ObjectWithSite, IResourceProvider
    {

        private List<Resource> _allocated = new List<Resource>();
        private object _syncRoot = new object();
        private IGlobalMutexService _globalSvc;

        public bool CanHandle(string name)
        {
            return String.Equals(name, "mutex", StringComparison.OrdinalIgnoreCase); 
        }

        public void Initialize()
        {
            _globalSvc = this.Site.GetService<IGlobalMutexService>();
        }

        public bool IsAvailable(ResourceParameter parameter)
        {
            string key = parameter.Values["key"];
            bool isGlobal = Boolean.Parse(parameter.Values.GetValueOrDefault("global", "true"));

            bool globalAvaile;

            if(isGlobal && _globalSvc != null)
            {
                globalAvaile = _globalSvc.IsAvaiable(key);
            }
            else
            {
                globalAvaile = true;
            }

            if (globalAvaile)
            {
                lock (_syncRoot)
                {
                    return _allocated.BinarySearchBy(key, r => r.Key, StringComparer.OrdinalIgnoreCase) < 0;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Request(ResourceParameter parameter, out object resource)
        {
            resource = null;
            string key = parameter.Values["key"];

            
            lock (_syncRoot)
            {
                int n = _allocated.BinarySearchBy(key, r => r.Key, StringComparer.OrdinalIgnoreCase);
                if (n < 0)
                {

                    bool isGlobal = Boolean.Parse(parameter.Values.GetValueOrDefault("global", "true"));

                    bool globalAvaile;

                    TimeSpan timeout = TimeSpan.Parse(parameter.Values.GetValueOrDefault("timeout", "00:15:00"));

                    if (isGlobal && _globalSvc != null)
                    {
                        globalAvaile = _globalSvc.Request(key, timeout);
                    }
                    else
                    {
                        globalAvaile = true;
                    }

                    if (globalAvaile)
                    {
                        Resource res = new Resource() { Key = key, IsGlobal = isGlobal };
                        _allocated.Insert(~n, res);
                        resource = res;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    
                }
                else
                {
                    return false;
                }
            }
        }

        public void Release(object resource)
        {
            Resource res = (Resource)resource;
            bool available = false;
            lock (_syncRoot)
            {
                int n = _allocated.BinarySearchBy(res.Key, r => r.Key, StringComparer.OrdinalIgnoreCase);
                if (n >= 0)
                {
                    _allocated.RemoveAt(n);

                    if (res.IsGlobal && _globalSvc != null)
                    {
                        _globalSvc.Release(res.Key);
                    }
                    available = true;
                }

            }

            if (available)
            {
                this.OnAvailable(EventArgs.Empty);
            }
        }


        public event EventHandler Available;

        protected virtual void OnAvailable(EventArgs e)
        {
            if (this.Available != null)
            {
                this.Available(this, e);
            }
        }


        public event EventHandler<ProviderRevokeArgs> Revoke;

        protected virtual void OnRevoke(ProviderRevokeArgs e)
        {
            if (this.Revoke != null)
            {
                this.Revoke(this, e);
            }
        }

        private class Resource
        {
            public string Key { get; set; }

            public bool IsGlobal { get; set; }
        }
       
    }
}
