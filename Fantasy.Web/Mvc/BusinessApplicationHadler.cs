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
using Fantasy.Web.Mvc.Html;

namespace Fantasy.Web.Mvc
{
    public class BusinessApplicationHadler : /*IHttpAsyncHandler,*/ IHttpHandler, IRequiresSessionState
    {

        public BusinessApplicationHadler(RequestContext requestContext )
        {
            this.RequestContext = requestContext;
        }

        public RequestContext RequestContext { get; private set; }

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

      

        protected virtual internal void ProcessRequest(HttpContextBase httpContext)
        {
            BusinessApplication app = GetApplication(); 
            IController controller = null; 
            try
            {
                controller = ProcessRequestInit(httpContext, app);
                controller.Execute(this.RequestContext);
                if (httpContext.Request.IsAjaxRequest()&&!((ControllerBase)controller).ControllerContext.IsChildAction)
                {
                    string script = HtmlAssets.RenderAjaxCallback(httpContext);

                    httpContext.Response.Write(script);
                }
            }
            finally
            {
                
                if(controller is IDisposable )
                {
                    ((IDisposable)controller).Dispose();
                }

                ReleaseApplication(app);
                
            }

        }

        protected virtual void ReleaseApplication(BusinessApplication app)
        {
            if (app != null)
            {
               
                BusinessEngineContext.Current.UnloadApplication();
            }
        }

        protected virtual BusinessApplication GetApplication()
        {
            RouteData routeData = this.RequestContext.RouteData;
            string appName = this.RequestContext.RouteData.GetRequiredString("AppName");
            IBusinessApplicationService appSvc = BusinessEngineContext.Current.GetRequiredService<IBusinessApplicationService>();


            BusinessApplication app = appSvc.CreateByName(appName);

            if (routeData.Values.ContainsKey("RootId"))
            {

                Guid rootId = (Guid)routeData.Values["RootId"];
                BusinessObject entryObj = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get<BusinessObject>(rootId);
                app.EntryObject = entryObj;
            }

            BusinessEngineContext.Current.LoadApplication(app);
          
            return app;
        }

        private IController ProcessRequestInit(HttpContextBase httpContext, BusinessApplication app)
        {

            IController rs = null;

            ViewType type = (ViewType)this.RequestContext.RouteData.Values["ViewType"];

            switch (type)
            {
                case ViewType.Nav:
                    rs = app.GetNaviationView();
                    break;
                case ViewType.Obj:
                    {
                        BusinessObject obj;
                        if (string.Equals((string)this.RequestContext.RouteData.Values["action"], "create", StringComparison.OrdinalIgnoreCase))
                        {
                            object o = this.RequestContext.RouteData.Values["classId"];
                            if (o == null)
                            {
                                o = this.RequestContext.HttpContext.Request["classId"];
                            }
                            Guid classId = o is Guid ? (Guid)o : new Guid((string)o);
                            IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
                            BusinessClass @class = oms.FindBusinessClass(classId);
                            rs = app.GetScalarView(@class);
                        }
                        else
                        {

                            obj = GetBusinessObject(app);
                            rs = app.GetScalarView(obj);
                        }
                      
                    }

                    break;
                case ViewType.Col:
                    {
                        BusinessObject obj = GetBusinessObject(app);
                        string prop = this.RequestContext.RouteData.GetRequiredString("Property");
           
                        rs = app.GetCollectionView(obj, prop);

                    }
                    break;
                default:
                    throw new Exception("Impossible");

            }


            SessionStateAttribute attr = rs.GetType().GetCustomAttribute<SessionStateAttribute>(true, false);
            SessionStateBehavior behavior = attr != null ? attr.Behavior : SessionStateBehavior.Default;
            this.RequestContext.HttpContext.SetSessionStateBehavior(behavior);

            if (!this.DisableMvcResponseHeader && !MvcHandler.DisableMvcResponseHeader)
            {
                string mvcVersion = (string)typeof(MvcHandler).InvokeMember("MvcVersion", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Static, null, null, null);
                httpContext.Response.AppendHeader(MvcHandler.MvcVersionHeaderName, mvcVersion);

            }

            this.RemoveOptionalRoutingParameters();

            string controllerName = rs.GetType().Name;
            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            this.RequestContext.RouteData.Values["controller"] = controllerName;

            return rs;
        }


        private bool _disableMvcResponseHeader = false;
        public bool DisableMvcResponseHeader
        {
            get
            {
                return _disableMvcResponseHeader;
            }
            set
            {
                _disableMvcResponseHeader = value; 
            }
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
          

            
            BusinessObject obj;
            if (this.RequestContext.RouteData.Values.ContainsKey("ObjId"))
            {
                Guid id = (Guid)this.RequestContext.RouteData.Values["ObjId"];
                obj = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get<BusinessObject>(id);
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