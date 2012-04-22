using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.UI
{
    public static class HtmlHelperExtensions
    {
        public static ViewComponentFactory BusinessEngine(this HtmlHelper helper)
        {
            ViewContext viewContext = helper.ViewContext;
            HttpContextBase httpContext = viewContext.HttpContext;

            ViewComponentFactory rs = (ViewComponentFactory)httpContext.Items[ViewComponentFactoryKey];

            if (rs == null)
            {
                rs = new ViewComponentFactory(helper);
            }

            return rs;
        }


        private static readonly string ViewComponentFactoryKey = typeof(ViewComponentFactory).AssemblyQualifiedName; 
    }
}