using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{
    public class CompiledContentRoute : RouteBase, IObjectWithSite
    {
        private string _basePath;

        public CompiledContentRoute(string basePath, IRouteHandler handler)
        {
            this._basePath = basePath;
            this._routeHandler = handler;
        }

        private IRouteHandler _routeHandler;

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {


            string virtualPath;
            if(CanHandle(httpContext, out virtualPath))
            {
                 return new RouteData(this, this._routeHandler);
            }
            else
            {
                return null;
            }
        }



        private bool CanHandle(HttpContextBase httpContext, out string virtualPath )
        {

            bool rs = false;

            virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;
            if (virtualPath.StartsWith(this._basePath, StringComparison.OrdinalIgnoreCase))
            {
                if (this.virtualPathFactory.Exists("~/" + virtualPath))
                {
                    rs = true;
                }

            }

            return rs;

        }
        


        private CompiledContentHandlerFactory _virtualPathFactory;
        private CompiledContentHandlerFactory virtualPathFactory
        {
            get
            {
                if (_virtualPathFactory == null)
                {
                    _virtualPathFactory = this.Site.GetRequiredService<CompiledContentHandlerFactory>();
                }
                return _virtualPathFactory;
            }
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {

            string virtualPath;
            if(CanHandle(requestContext.HttpContext, out virtualPath))
            {
                 return new VirtualPathData(this, virtualPath); 
            }
            else
            {
                return null;
            }

        }

        public IServiceProvider Site { get; set; }
    }
}