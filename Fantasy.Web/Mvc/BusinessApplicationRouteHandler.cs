using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{

    

    public class BusinessApplicationRouteHandler  : ObjectWithSite, IRouteHandler
    {
        #region IRouteHandler Members

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
           
            return new BusinessApplicationHadler(requestContext);

        }

       

        #endregion
    }
}