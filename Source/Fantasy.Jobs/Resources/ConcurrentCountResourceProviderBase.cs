using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Resources
{
    public abstract class ConcurrentCountResourceProviderBase : ObjectWithSite, IResourceProvider
    {
        #region IResourceProvider Members


        protected object _syncRoot = new object();

        protected virtual void TryRevoke()
        {
            List<Resource> temp;
            lock (_syncRoot)
            {
                temp = new List<Resource>(this._allocated);  
            }

            var query = from res in temp group res by res.Key into g select g;

            foreach (var group in query)
            {
                int max = GetMaxCount(group.Key);
                int count = group.Count();
                if (count > max)
                {
                    foreach (Resource res in group.Reverse().Take(count - max))
                    {
                        try
                        {
                            this.OnRevoke(res);
                        }
                        finally
                        {
                            this._allocated.Remove(res);
                        }
                    }
                }
            }

        }

        private List<Resource> _allocated = new List<Resource>();
        protected List<Resource> Allocated
        {
            get
            {
                return _allocated;
            }
        }
       
        public abstract bool CanHandle(string name);

        public virtual void Initialize()
        {
           
        }

        protected virtual string GetKey(ResourceParameter parameter)
        {
            return string.Empty;
        }

        protected abstract int GetMaxCount(string key);

        private int GetAllocatedCount(string key)
        {
            var query = from r in this.Allocated where r.Key == key select r;
            int rs = query.Count();
            return rs;
        }
       

        public virtual bool IsAvailable(ResourceParameter parameter)
        {
            lock (_syncRoot)
            {
                string key = this.GetKey(parameter);
                int count = this.GetAllocatedCount(key);
                int max = this.GetMaxCount(key);

                return count < max;
            }
        }

        public virtual  bool Request(ResourceParameter parameter, out object resource)
        {
            lock (_syncRoot)
            {
                if (IsAvailable(parameter))
                {
                    Resource rs = new Resource() { Key = this.GetKey(parameter) };

                    this.Allocated.Add(rs);
                    resource = rs;
                    return true;
                }
                else
                {
                    resource = null;
                    return false;
                }
            }
        }

        public void Release(object resource)
        {
            bool available;
            lock (_syncRoot)
            {
                Resource res = (Resource)resource;
                this._allocated.Remove(res);
                int max = this.GetMaxCount(res.Key);
                if (max < Int32.MaxValue)
                {

                    int count = GetAllocatedCount(res.Key);
                    available = count < max;
                }
                else
                {
                    available = true;
                }

            }

            if (available)
            {
                this.OnAvailable();
            }
        }

        protected virtual void OnAvailable()
        {
            if (this.Available != null)
            {
                this.Available(this, EventArgs.Empty);
            }
        }

        public event EventHandler Available;

        protected virtual void OnRevoke(Resource res)
        {
            if (this.Revoke != null)
            {
                this.Revoke(this, new ProviderRevokeArgs() { Resource = res });
            }
        }

        public event EventHandler<ProviderRevokeArgs> Revoke;

        protected class Resource
        {
            public string Key;
        }

        #endregion
    }
}
