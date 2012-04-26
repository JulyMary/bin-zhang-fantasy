using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Web.Properties;
using System.Web.WebPages;
using System.Reflection;

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

        private static bool PageExists(string path)
        {
            
            object manager = typeof(VirtualPathFactoryManager).GetProperty("Instance", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            bool rs = (bool)typeof(VirtualPathFactoryManager).InvokeMember("PageExists", BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, manager, new object[] { path, true });
            return rs;
        }
    }
}
