using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Web.Routing;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using System.Reflection;

namespace Fantasy.Web.Mvc
{
    public class BusinessApplicationHadler : /*IHttpAsyncHandler,*/ IHttpHandler, IRequiresSessionState
    {

        public BusinessApplicationHadler(RequestContext requestContext )
        {
            this.RequestContext = requestContext;
        }


        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            throw new NotImplementedException();
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        protected virtual void ProcessRequest(HttpContext context)
        {
            HttpContextBase base2 = new HttpContextWrapper(context);
            this.ProcessRequest(base2);
        }


        public RequestContext RequestContext { get; private set; }
        
        
       

        protected internal void ProcessRequest(HttpContextBase httpContext)
        {

            BusinessApplication app;
            IController controller;
            ProcessRequestInit(httpContext, out app, out controller);
            try
            {
                controller.Execute(this.RequestContext);
            }
            finally
            {
                if(controller is IDisposable )
                {
                    ((IDisposable)controller).Dispose();
                }
                if(app is IDisposable)
                {
                    ((IDisposable)app).Dispose();
                }
            }

        }

        private void ProcessRequestInit(HttpContextBase httpContext, out BusinessApplication app, out IController controller)
        {
            //URL Pattern: ~/App/{AppName}/{ViewType}/{Action}/{ObjId}/{Property}
            string appName = this.RequestContext.RouteData.GetRequiredString("AppName");
            app = BusinessEngineContext.Current.GetRequiredService<IBusinessApplicationService>().CreateByName(appName);
            BusinessEngineContext.Current.Application = app;
            app.Load();
            ViewType type = (ViewType)this.RequestContext.RouteData.Values["ViewType"];

            switch (type)
            {
                case ViewType.Nav:
                    controller = app.GetNaviationView();
                    break;
                case ViewType.Obj:
                    {
                        BusinessObject obj = GetBusinessObject(app);
                        controller = app.GetScalarView(obj);
                    }

                    break;
                case ViewType.Col:
                    {
                        BusinessObject obj = GetBusinessObject(app);
                        string prop = this.RequestContext.RouteData.GetRequiredString("Property");
                        IEnumerable<BusinessObject> collection = (IEnumerable<BusinessObject>)obj.GetType().GetProperty(prop, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(obj, null);
                        controller = app.GetCollectionView(obj, prop, collection);

                    }
                    break;
                default:
                    throw new Exception("Impossible");

            }


            SessionStateAttribute attr = controller.GetType().GetCustomAttribute<SessionStateAttribute>(true);
            SessionStateBehavior behavior = attr != null ? attr.Behavior : SessionStateBehavior.Default;
            this.RequestContext.HttpContext.SetSessionStateBehavior(behavior);

            if (!MvcHandler.DisableMvcResponseHeader)
            {
                string mvcVersion = (string)typeof(MvcHandler).InvokeMember("MvcVersion", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Static, null, null, null);
                httpContext.Response.AppendHeader(MvcHandler.MvcVersionHeaderName, mvcVersion);

            }

            this.RemoveOptionalRoutingParameters();

            string controllerName = controller.GetType().Name;
            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            this.RequestContext.RouteData.Values.Add("controller", controllerName);
        }

        private void RemoveOptionalRoutingParameters()
        {
            RouteValueDictionary values = this.RequestContext.RouteData.Values;
            foreach (string str in (from entry in values
                                    where entry.Value == UrlParameter.Optional
                                    select entry.Key).ToArray<string>())
            {
                values.Remove(str);
            }
        }



        private BusinessObject GetBusinessObject(BusinessApplication app)
        {
            Guid objectId = new Guid(this.RequestContext.RouteData.GetRequiredString("ObjId"));
            BusinessObject obj;
            if (objectId != Guid.Empty)
            {
                obj = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get<BusinessObject>(objectId);
            }
            else
            {
                obj = app.EntryObject;
            }
            return obj;
        }

        

        #endregion

       

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            this.ProcessRequest(context);
        }

        
    }
}