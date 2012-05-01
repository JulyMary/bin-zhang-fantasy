using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.Routing;
using System.Web.Mvc;
namespace Fantasy.Web.Mvc
{
    public class RegisterAppRouteCommand : ObjectWithSite, ICommand
    {

        #region ICommand Members

        public object Execute(object args)
        {
           
            AppRoute appRoute = new AppRoute(this.Site);
            RouteTable.Routes.Add("app", appRoute); 

            return null;
        }

        #endregion
    }
}