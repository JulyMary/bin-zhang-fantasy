using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Web.Properties;
using System.Web.WebPages;
using System.Reflection;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string ImageList(this UrlHelper url, string key)
        {
            return url.Content("~/_ImageList/" + key);
        }

        public static string ThemedContent(this UrlHelper url, string contentPath)
        {
            string rs = string.Format(contentPath, Settings.Default.Theme);
            if (!PageExists(rs))
            {

                rs = string.Format(contentPath, Settings.Default.DefaultTheme);
            }

            return url.Content(rs);

        }


        public static string ApplicationUrl (this UrlHelper url, string appName, object routeValues)
        {
            RouteValueDictionary values2 = new RouteValueDictionary(routeValues);
            values2.Add("AppName",appName);
            
        }

        private static bool PageExists(string path)
        {
            
            object manager = typeof(VirtualPathFactoryManager).GetProperty("Instance", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            bool rs = (bool)typeof(VirtualPathFactoryManager).InvokeMember("Exists", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, manager, new object[] { path});
            return rs;
            
        }
    }
}
