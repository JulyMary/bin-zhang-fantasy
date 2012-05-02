using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Web.Properties;
using System.Web.WebPages;
using System.Reflection;
using System.Web.Routing;
using System.Text;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;

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


        public static string ApplicationUrl (this UrlHelper url, string appName = null, Guid? rootId = null, ViewType? viewType = null, 
            Guid? objectId = null, string action = null, string property = null,object routeValues = null)
        {

            RouteValueDictionary values2 = MergeRouteValue(appName, rootId, viewType, objectId, action, property, routeValues);

            //Inherit appName, rootId, viewType if appName is not assigned
            if (!values2.ContainsKey("AppName"))
            {
                AppRouteValue current = AppRouteStack.Current.Peek();
                values2.Add("AppName", current.AppName);
                if (current.RootId != null && !values2.ContainsKey("RootId"))
                {
                    values2.Add("RootId", (Guid)current.RootId);
                }
                if (current.ViewType != null && !values2.ContainsKey("ViewType"))
                {
                    values2.Add("ViewType", (ViewType)current.ViewType); 
                }
            }

            return url.RouteUrl(values2);
        }

       

        private static RouteValueDictionary MergeRouteValue(string appName, Guid? rootId, ViewType? viewType, Guid? objectId, string action, string property, object routeValues)
        {
            RouteValueDictionary values2 = new RouteValueDictionary(routeValues);
            if (appName != null)
            {
                values2.Add("AppName", appName);
            }
            if (rootId != null)
            {
                values2.Add("RootId", rootId);
            }
            if (viewType != null)
            {
                values2.Add("ViewType", viewType);
            }
            if (objectId != null)
            {
                values2.Add("ObjId", objectId);
            }
            if (action != null)
            {
                values2.Add("action", action);
            }
            if (property != null)
            {
                values2.Add("Property", property);
            }
            return values2;
        }

        private static bool PageExists(string path)
        {
            
            object manager = typeof(VirtualPathFactoryManager).GetProperty("Instance", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            bool rs = (bool)typeof(VirtualPathFactoryManager).InvokeMember("Exists", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, manager, new object[] { path});
            return rs;
            
        }
    }
}
