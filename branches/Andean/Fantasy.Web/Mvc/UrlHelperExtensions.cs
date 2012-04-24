using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string ImageList(this UrlHelper url, string key)
        {
            return url.Content("~/_ImageList/" + key);
        }
    }
}
