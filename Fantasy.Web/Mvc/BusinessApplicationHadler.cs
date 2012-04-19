using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Web.Routing;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Web.Mvc
{
    public class BusinessApplicationHadler : IHttpAsyncHandler, IHttpHandler, IRequiresSessionState
    {

        public BusinessApplicationHadler(RequestContext requestContext )
        {

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


            string appName = this.RequestContext.RouteData.GetRequiredString("AppName");
            BusinessApplication app = BusinessEngineContext.Current.GetRequiredService<IBusinessApplicationService>().CreateByName(appName);
            BusinessEngineContext.Current.Application = app;
            app.Load();
            ControllerType type = (ControllerType)Enum.Parse(typeof(ControllerType), this.RequestContext.RouteData.GetRequiredString("controllerType"));
            IController controller;
            switch (type)
            {
                case ControllerType.Nav:
                    controller = app.GetNaviationView();
                    break;
                case ControllerType.Obj:
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
                    

                    controller = app

                    break;
                case ControllerType.Col:
                    break;
                default:
                    break;
            }


            this.ProcessRequestInit(httpContext, out controller, out factory);
            try
            {
                controller.Execute(this.RequestContext);
            }
            finally
            {
                factory.ReleaseController(controller);
            }

        }

        

        #endregion

       

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            this.ProcessRequest(context);
        }

        
    }
}