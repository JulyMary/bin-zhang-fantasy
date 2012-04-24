using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{
    public class RegisterImageListRouteCommand : ObjectWithSite, ICommand
    {
        public object Execute(object args)
        {

            RouteTable.Routes.Add("_ImageList", new Route("_ImageList/{key}",
                     new RouteValueDictionary(),
               this.Site.GetRequiredService<ImageListRoutHandler>()));

            return null;
        }
    }
}