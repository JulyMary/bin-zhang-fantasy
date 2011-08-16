using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.ServiceModel
{
    public class ServiceContainer : IServiceProvider
    {

        public ServiceContainer()
        {

        }

        public ServiceContainer(IServiceProvider parentServiceProvider)
        {
            this._parentServices = parentServiceProvider;

        }


        private object _syncRoot = new object();

        private IServiceProvider _parentServices = null;

        private List<object> _services = new List<object>();

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            object rs;
            lock (_syncRoot)
            {
                rs = this._services.SingleOrDefault(svc=>serviceType.IsInstanceOfType(svc)); 
            }

            if(rs == null && _parentServices != null)
            {
                rs = this._parentServices.GetService(serviceType);
            }

            return rs;
        }


        public void AddService(object instance)
        {
            this._services.Add(instance);
        }

        public void RemoveService(object instance)
        {
            this._services.Remove(instance);
        }

        #endregion
    }
}
