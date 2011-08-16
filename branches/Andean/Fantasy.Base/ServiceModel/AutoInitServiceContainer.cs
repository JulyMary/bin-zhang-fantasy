using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Fantasy.ServiceModel
{
    public class AutoInitServiceContainer : IServiceProvider
    {
        private IServiceProvider _parentProvider;

        public AutoInitServiceContainer()
        {

        }

        private bool _initialized = false;
        private List<object> _list = new List<object>();

        public void InitializeServices(object[] services)
        {
            this.InitializeServices(null, services);
        }

        public virtual void InitializeServices(IServiceProvider parentProvider, object[] services)
        {
            this._parentProvider = parentProvider;

            this._list.AddRange(services);

            ILogger logger = this.GetService<ILogger>();
           

            foreach (object o in this._list)
            {
                if (o is IObjectWithSite)
                {
                    ((IObjectWithSite)o).Site = this;
                }
                if (o is IService)
                {
                    try
                    {
                        ((IService)o).InitializeService();
                    }
                    catch(Exception error)
                    {
                        logger.SafeLogError("Services", error, "Service {0} failed to initialize.", o.GetType().FullName);
                        throw;
                    }

                    if (logger != null)
                    {
                        logger.SafeLogMessage("Services", "Service {0} initialized", o.GetType().FullName);  
                    }
                }
            }
            this._initialized = true;
        }

        public virtual void AddService(object service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service"); 
            }



            this._list.Add(service);
            if (service is IObjectWithSite)
            {
                ((IObjectWithSite)service).Site = this;
            }
            if (service is IService && this._initialized)
            {
                ((IService)service).InitializeService();
            }
            
        }

        public virtual void RemoveService(object service)
        {
            int index = this._list.IndexOf(service);
            if (index >= 0)
            {
                this._list.RemoveAt(index);
                if (service is IService)
                {
                    ((IService)service).UninitializeService(); 
                }
            }


        }

        public virtual void UninitializeServices()
        {
            ILogger logger = this.GetService<ILogger>();
            for (int i = this._list.Count - 1; i >= 0; i--)
            {
                object o = this._list[i];
                if (o is IService)
                {
                    if (logger != null)
                    {
                        logger.SafeLogMessage("Services", "Service {0} uninitialized", o.GetType().FullName);
                    }
                    ((IService)o).UninitializeService();
                    
                }
            }
        }

        public T GetService<T>()
        {
            return (T)this.GetService(typeof(T));
        }

        #region IServiceProvider Members

        public virtual object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return new ArgumentNullException("serviceType");
            }
            foreach (object o in this._list)
            {
                if (serviceType.IsInstanceOfType(o))
                {
                    return o;
                }
            }

            if (this._parentProvider != null)
            {
                return this._parentProvider.GetService(serviceType);
            }

            return null;
        }

        #endregion
    }
}
