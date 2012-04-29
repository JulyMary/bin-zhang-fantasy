using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{
    public class RegisterAspNetMvcDefualtRouteCommand : ObjectWithSite, ICommand
    {

        #region ICommand Members

        public object Execute(object args)
        {
            RouteTable.Routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}",
               defaults: new { controller = "Home", action = "Index"}
           );

            return null;
        }

        #endregion
    }
}