using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.Routing;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc
{
    public class IgnoreSystemRouteCommand : ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("favicon.ico");

            return null;
        }

        #endregion
    }
}