using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Fantasy.Web.Mvc.UI
{
    public static class ApplicationExtensions
    {
        public static MvcHtmlString ApplicationLink(this ViewComponentFactory businessEngine, string linkText, string appName, object routeValues)
        {
            RouteValueDictionary values2 = new RouteValueDictionary(routeValues);
            values2.Add("AppName",appName);
            return businessEngine.HtmlHelper.RouteLink(linkText, values2); 
        }
    }
}