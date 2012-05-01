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

            string rootIdPattern = @"^[A-Za-z0-9+/]+={0,2}$";
            string guidPattern = "^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$";

            BusinessApplicationRouteHandler handler = this.Site.GetRequiredService<BusinessApplicationRouteHandler>();

            RouteTable.Routes.Add("app", new Route("App/{AppName}/{ObjId}", 
                new RouteValueDictionary(new { ObjId = UrlParameter.Optional }), 
                new RouteValueDictionary(new {ObjId = guidPattern }), handler)); 
            
            RouteTable.Routes.Add("app1",  new Route("App/{AppName}/{RootId}/{ObjId}", 
                new RouteValueDictionary(new { ObjId = UrlParameter.Optional }), 
                new RouteValueDictionary(new { RootId = rootIdPattern, ObjId = guidPattern }), handler)); 


            RouteTable.Routes.Add("app2", new Route("App/{AppName}/Nav/{Action}/{ObjId}/{Property}",
                new RouteValueDictionary(new {ViewType = ViewType.Nav,
                     Action = "Default",
                     ObjId = UrlParameter.Optional,
                     Property = UrlParameter.Optional}),
                new RouteValueDictionary(new {ObjId=guidPattern}),
                handler));

            RouteTable.Routes.Add("app2", new Route("App/{AppName}/{RootId}/Nav/{Action}/{ObjId}/{Property}",


            RouteTable.Routes.Add("BusinessApplication", new Route("App/{AppName}/{ViewType}/{Action}/{ObjId}/{Property}",
                new RouteValueDictionary(),
                     this.Site.GetRequiredService<BusinessApplicationRouteHandler>()));

            return null;
        }

        #endregion
    }
}