﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{
    public class ImageListRoutHandler : ObjectWithSite, IRouteHandler
    {

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ImageListHandler(requestContext);
        }
    }
}