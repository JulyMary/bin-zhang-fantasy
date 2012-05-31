using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{
    public class CurrentBusinessApplicationHandler : BusinessApplicationHadler
    {
        public CurrentBusinessApplicationHandler(RequestContext requestContext)
            : base(requestContext)
        {
            this.DisableMvcResponseHeader = true;
        }


        protected internal override void ProcessRequest(HttpContextBase httpContext)
        {
            base.ProcessRequest(httpContext);
        }

        protected override BusinessEngine.BusinessApplication GetApplication()
        {
            return BusinessEngineContext.Current.Application;
        }

        protected override void ReleaseApplication(BusinessApplication app)
        {
            
        }
    }
}