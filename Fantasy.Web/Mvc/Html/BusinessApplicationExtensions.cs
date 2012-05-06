using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Web.Mvc.Html
{
    public static class BusinessApplicationExtensions
    {
        public static MvcHtmlString ApplicationLink(this HtmlHelper htmlHelper, string linkText, string appName, object routeValues)
        {
            RouteValueDictionary values2 = new RouteValueDictionary(routeValues);
            values2.Add("AppName",appName);
            return htmlHelper.RouteLink(linkText, values2); 
        }



        public MvcHtmlString ScalarView(this HtmlHelper helper, Guid objectId)
        {

             IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();
             BusinessObject obj = es.Get<BusinessObject>(objectId);
             IScalarViewController controller = BusinessEngineContext.Current.Application.GetScalarView(obj);

             throw new NotImplementedException();
        }
    }
}