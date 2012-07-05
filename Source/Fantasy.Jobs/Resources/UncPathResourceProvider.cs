using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using System.Threading;
using System.ComponentModel;

namespace Fantasy.Jobs.Resources
{
    public class UncPathResourceProvider : ObjectWithSite, IResourceProvider
    {

        private class PathInfo
        {
            public string Path { get; set; }
            public string User  { get; set; }
            public string Password  { get; set; }
        }

        private Thread _refreshThread;

        private List<PathInfo> _requestedPaths = new List<PathInfo>();

        private object _syncRoot = new object();

        #region IResourceProvider Members

        public bool CanHandle(string name)
        {
            return String.Equals(name, "unc", StringComparison.OrdinalIgnoreCase); 
        }

        public void Initialize()
        {
            _refreshThread = ThreadFactory.CreateThread(Refresh).WithStart();
            
        }

        private void Refresh()
        {
            while (true)
            {
                Thread.Sleep(60 * 1000);
                List<string> connects = new List<string>();
                List<PathInfo> temp;
                lock (_syncRoot)
                {
                    temp = new List<PathInfo>(this._requestedPaths);
                }

                foreach (PathInfo pi in temp)
                {
                    if (!connects.Any(s => string.Equals(pi.Path, pi.Path, StringComparison.OrdinalIgnoreCase)))
                    {
                        try
                        {
                            WNet.AddConnection(pi.Path, pi.User, pi.Password);
                            connects.Add(pi.Path);
                        }
                        catch
                        {
                        }
                    }
                }

                lock (_syncRoot)
                {
                    var removing = from pi in temp where connects.Any(s => string.Equals(pi.Path, s, StringComparison.OrdinalIgnoreCase)) select pi;
                    foreach (PathInfo pi in removing)
                    {
                        _requestedPaths.Remove(pi);
                    }
                }

                if (connects.Count > 0)
                {
                    this.OnAvailable(EventArgs.Empty);
                }


            }
        }

        public bool IsAvailable(ResourceParameter parameter)
        {
            string path = parameter.Values.GetValueOrDefault("path");
            bool isUnc = false;
            if (!string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    Uri uri = new Uri(path);
                    isUnc = uri.IsUnc;
                }
                catch
                {

                }

                if (isUnc)
                {
                    bool connected = false;
                    try
                    {
                        connected = LongPathDirectory.Exists(path);
                    }
                    catch
                    {
                    }

                    if (!connected)
                    {
                        string user = parameter.Values.GetValueOrDefault("user", string.Empty);
                        string password = parameter.Values.GetValueOrDefault("password", string.Empty);
                        password = Encryption.Decrypt(password);
                        try
                        {
                           
                            WNet.AddConnection(path, user, password);
                            connected = true;
                        }
                        catch(Win32Exception)
                        {
                            lock(_syncRoot)
                            {
                                if(! _requestedPaths.Any(pi=> String.Equals(path, pi.Path, StringComparison.OrdinalIgnoreCase)
                                    && String.Equals(user, pi.User, StringComparison.OrdinalIgnoreCase) 
                                    && String.Equals(password, pi.Password, StringComparison.OrdinalIgnoreCase)))
                                {
                                    this._requestedPaths.Add(new PathInfo{ Path = path, User = user, Password = password});
                                }
                            }
                        }
                       

                    }

                    return connected;

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
            return IsAvailable(parameter);
        }

        public void Release(object resource)
        {
            
        }

        public event EventHandler Available;

        protected virtual void OnAvailable(EventArgs e)
        {
            if (this.Available != null)
            {
                this.Available(this, e);
            }
        }

        event EventHandler<ProviderRevokeArgs> IResourceProvider.Revoke { add { } remove { } }

        #endregion
    }
}
