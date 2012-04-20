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
            //URL Pattern: ~/App/{AppName}/{ViewType}/{Action}/{ObjId}/{Property}
             //RouteTable.Routes.Add("BusinessApplication", "App/{AppName}/{ViewType}/{Action}/{ObjId}/{Property}",
             //    new {
             //        
             //    }, );
            RouteTable.Routes.Add("BusinessApplication", new Route("App/{AppName}/{ViewType}/{Action}/{ObjId}/{Property}",
                new RouteValueDictionary(new {ViewType = ViewType.Nav,
                     Action = "Default",
                     ObjId = UrlParameter.Optional,
                     Property = UrlParameter.Optional}),
                     this.Site.GetRequiredService<BusinessApplicationRouteHandler>()));

            return null;
        }

        #endregion
    }
}