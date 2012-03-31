using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;

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

        public BusinessUserData User { get; set; }

        public BusinessApplication Application { get; set; }

        public static BusinessEngineContext Current
        {
            get
            {
                return ContextProvider.GetCurrent();
            }
            set
            {
                ContextProvider.SetCurrent(value);
            }
        }

        private static IBusinessEngineContextProvider _contextProvider;

        private static IBusinessEngineContextProvider ContextProvider
        {
            get
            {
                if (_contextProvider == null)
                {
                    _contextProvider = AddInTree.Tree.GetTreeNode("fantasy/businessengine/contextprovider").BuildItem<IBusinessEngineContextProvider>(null, null);

                }
                return _contextProvider;
            }
        }
    }
}
