using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.IO;
using Fantasy.Jobs.Utils;

namespace Fantasy.Jobs.Resources
{
    public class WaitForResourceProvider : ObjectWithSite, IResourceProvider
    {

        private MemoryCache _cache;

        private object _syncRoot = new object();

        private static readonly CacheItemPolicy _cachePolicy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(1, 0, 0) };

        public bool CanHandle(string name)
        {
            return string.Equals(name, "WaitFor", StringComparison.OrdinalIgnoreCase);
        }

        public void Initialize()
        {
            _cache = new MemoryCache("WaitForResourceProvider");
        }

        private string GetKey(string ids)
        {
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(ids));
            return MD5HashCodeHelper.GetMD5HashCode(stream);
        }

        public bool IsAvailable(ResourceParameter parameter)
        {

            string ids = parameter.Values.GetValueOrDefault("jobs", string.Empty);
            if (!String.IsNullOrEmpty(ids))
            {
                WaitForMode model = (WaitForMode)Enum.Parse(typeof(WaitForMode), parameter.Values.GetValueOrDefault("mode", "All"), true);
                string key = model.ToString() + this.GetKey(ids);
                List<Guid> waitList;
                lock (_syncRoot)
                {
                    waitList = (List<Guid>)this._cache[key];
                    if (waitList == null)
                    {
                        waitList = (from id in parameter.Values.GetValueOrDefault("jobs", string.Empty).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries) select new Guid(id)).ToList();
                    }

                    _cache.Add(key, waitList, _cachePolicy);
                }

                if (waitList.Count > 0)
                {
                    lock (waitList)
                    {
                        List<Guid> temp = new List<Guid>(waitList);
                        IJobQueue queue = this.Site.GetRequiredService<IJobQueue>();

                        if (model == WaitForMode.All)
                        {
                            foreach (Guid id in temp)
                            {
                                JobMetaData job = queue.FindJobMetaDataById(id);
                                if (! queue.IsTerminated(id))
                                {
                                    return false;
                                }
                                else
                                {
                                    waitList.Remove(id);
                                }
                            }
                            return true;
                        }
                        else
                        {
                            foreach (Guid id in temp)
                            {
                                if (queue.IsTerminated(id))
                                {
                                    waitList.Clear();
                                    return true;
                                }
                            }
                            return false;
                        }
                    }
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return true;
            }
            
        }

        public bool Request(ResourceParameter parameter, out object resource)
        {
            resource = null;
            return this.IsAvailable(parameter);
        }

        public void Release(object resource)
        {
            
        }

        public event EventHandler Available {add{}remove {}}

        public event EventHandler<ProviderRevokeArgs> Revoke {add{} remove {}}
    }
}
