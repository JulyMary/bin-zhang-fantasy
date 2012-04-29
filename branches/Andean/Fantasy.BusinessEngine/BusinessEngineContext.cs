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

        public BusinessUser User { get; set; }

        public BusinessApplication Application
        {
            get
            {
                return _applicationStack.Peek();
            }
        }


        private Stack<BusinessApplication> _applicationStack = new Stack<BusinessApplication>();


       

        public BusinessApplication UnloadApplication()
        {
            BusinessApplication app = this._applicationStack.Peek();
            app.Unload();
            if (app is IDisposable)
            {
                ((IDisposable)app).Dispose();
            }
            return this._applicationStack.Pop();
        }

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


        private Dictionary<string, object> _items = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase); 

        public object this[string key]
        {
            get
            {
                return _items.GetValueOrDefault(key, null);
            }
            set
            {
                _items[key] = value;
            }
        }




        public void LoadApplication(BusinessApplication app)
        {
            this._applicationStack.Push(app);
            app.Load();
        }
    }
}
