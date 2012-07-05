using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Jobs.Web.Controllers
{
    public static class JobStateImageHelper
    {
        public static string GetImageUrl(UrlHelper url, int state)
        {
            return url.Content(String.Format("~/Content/JobState/{0}.png", JobState.ToString(state))); 
        }
    }
}