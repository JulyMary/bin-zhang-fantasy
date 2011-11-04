using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessEngineContext : IServiceProvider
    {
        private IServiceProvider _services;

        public BusinessEngineContext(IServiceProvider services)
        {
            this._services = services;
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return this._services.GetService(serviceType);
        }

        #endregion

        public BusinessUser User { get; set; }

        public BusinessApplication Application { get; set; }

        public static BusinessEngineContext Current { get; set; }
    }
}
