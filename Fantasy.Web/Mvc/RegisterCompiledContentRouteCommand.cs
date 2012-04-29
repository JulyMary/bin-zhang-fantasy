using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{
    public class RegisterCompiledContentRouteCommand : ObjectWithSite, ICommand
    {

        #region ICommand Members

        public object Execute(object args)
        {

            foreach (string s in new string[] {"Content", "Images", "Scripts" })
            {
               RouteTable.Routes.Add(s, new CompiledContentRoute(s,
               this.Site.GetRequiredService<CompiledContentRouteHandler>()) { Site = this.Site});
            }

           

            return null;
        }

        #endregion
    }
}