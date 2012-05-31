using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Web.Mvc
{
    public class AppRoute : RouteBase
    {


        public AppRoute(IServiceProvider services)
        {
            this._handler = services.GetRequiredService<BusinessApplicationRouteHandler>();
            this._applicationService = services.GetRequiredService<IBusinessApplicationService>();
            
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteValueDictionary values = this.ParseHttpContext(httpContext);
            if (values != null)
            {
                RouteData rs = new RouteData(this, this._handler);
                foreach (KeyValuePair<string, object> pair in values)
                {
                    rs.Values.Add(pair.Key, pair.Value);
                }

                return rs;
            }
            else
            {
                return null;
            }
        }

        private IRouteHandler _handler;
        private IBusinessApplicationService _applicationService;
        private string[] _routeSegmentNames = new string[] { "appname", "rootid", "viewtype", "action", "objid", "property", "controller", "obj" }; 
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (values.ContainsKey("AppName"))
            {
                StringBuilder path = new StringBuilder(1024);
                path.AppendFormat("App/{0}", values["AppName"]);

                if (values.ContainsKey("RootId"))
                {
                    Guid rootId = (Guid)values["RootId"];
                    string cipter = _applicationService.EncryptRootId((string)values["AppName"], rootId);
                    path.AppendFormat("/{0}", cipter);
                }
                if (values.ContainsKey("ViewType"))
                {
                    ViewType vt = (ViewType)values["ViewType"];
                    if (vt != ViewType.Nav)
                    {
                        path.AppendFormat("/{0}", vt);
                    }
                }

                if (values.ContainsKey("Action"))
                {
                    string action = (string)values["Action"];
                    if (!string.Equals(action, "default", StringComparison.OrdinalIgnoreCase))
                    {
                        path.AppendFormat("/{0}", action);
                    }
                }

                if (values.ContainsKey("ObjId"))
                {
                    Guid objId = (Guid)values["ObjId"];
                    path.AppendFormat("/{0}", objId);

                    if (values.ContainsKey("Property"))
                    {
                        string property = (string)values["Property"];
                        path.AppendFormat("/{0}", property);
                    }
                }

                var query = from p in values where !_routeSegmentNames.Any(s=>string.Equals(s, p.Key, StringComparison.OrdinalIgnoreCase)) && p.Value != null
                    select string.Format("{0}={1}", p.Key, HttpUtility.UrlEncode(p.Value.ToString()));

                string queryString = string.Join("&", query);
                if(!String.IsNullOrEmpty(queryString))
                {
                    path.Append("?");
                    path.Append(queryString);
                }

                VirtualPathData rs = new VirtualPathData(this, path.ToString());
               
               
                return rs;

            }
            else
            {
                return null;
            }
        }



        private class EnumeratorHelper : IDisposable
        {
            public EnumeratorHelper(IEnumerator<string> enumerator)
            {
                this._enumerator = enumerator;
            }

            IEnumerator<string> _enumerator;

            public bool MoveNext()
            {
                bool rs = this._enumerator.MoveNext();
                this.IsEnd = !rs;
                return rs;
            }

            public bool IsEnd { get; private set; }


            public string Current
            {
                get
                {
                    return this._enumerator.Current;
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                this._enumerator.Dispose();
            }

            #endregion
        }


        private bool ParseApp(EnumeratorHelper enumerator, RouteValueDictionary value)
        {
            bool rs = false;
            if (!enumerator.IsEnd && string.Equals(enumerator.Current, "App", StringComparison.OrdinalIgnoreCase))
            {
                //Get AppName

                if (enumerator.MoveNext())
                {
                    
                    value.Add("AppName", enumerator.Current);
                    enumerator.MoveNext();
                    rs = true;
                    
                }
            }
            return rs;
        }


        private void ParseRoot(EnumeratorHelper enumerator, RouteValueDictionary value)
        {

            Guid rootId;
            string appName = (string)value["appName"];
            if (!enumerator.IsEnd && _applicationService.TryDecryptRootId(appName, enumerator.Current, out rootId))
            {
                
                value.Add("RootId", rootId);
                enumerator.MoveNext();
            }
        }

        private void ParseViewType(EnumeratorHelper enumerator, RouteValueDictionary value)
        {
            ViewType viewType;
            if(!enumerator.IsEnd && Enum.TryParse<ViewType>(enumerator.Current, out viewType))
            {
                value.Add("ViewType", viewType);
                enumerator.MoveNext();
            }
            else
            {
                value.Add("ViewType", ViewType.Nav);
            }
        }


        private void ParseAction(EnumeratorHelper enumerator, RouteValueDictionary value)
        {
            if (!enumerator.IsEnd && !Regex.IsMatch(enumerator.Current, guidPattern))
            {
                value.Add("Action", enumerator.Current);
                enumerator.MoveNext();
            }
            else
            {
                value.Add("Action", "Default");
            }
        }

        private bool ParseObjId(EnumeratorHelper enumerator, RouteValueDictionary value)
        {
            if (!enumerator.IsEnd && Regex.IsMatch(enumerator.Current, guidPattern))
            {
                value.Add("ObjId", new Guid(enumerator.Current));
                enumerator.MoveNext();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private void ParseProperty(EnumeratorHelper enumerator, RouteValueDictionary value)
        {
            if (!enumerator.IsEnd)
            {
                value.Add("Property", enumerator.Current);
                enumerator.MoveNext();
            }
        }


       
        string guidPattern = "^[A-Za-z0-9]{8}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{4}-[A-Za-z0-9]{12}$";

        private RouteValueDictionary ParseHttpContext(HttpContextBase httpContext)
        {
            // App/{AppName}/{ObjId}

            // App/{AppName}/{RootId}/{ObjId}

            // App/{AppName}/{ViewType}/{Action}/{ObjId}/{Property}

            // App/{AppName}/{RootId}/{ViewType}/{Action}/{ObjId}/{Property}
            RouteValueDictionary rs = null;
            RouteValueDictionary value = new RouteValueDictionary();
            
            string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;
            IEnumerable<string> segments = virtualPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries); 
            using (EnumeratorHelper enumerator = new EnumeratorHelper(segments.GetEnumerator()))
            {
                enumerator.MoveNext();

                if(ParseApp(enumerator, value))
                {
                    rs = value;
                    ParseRoot(enumerator, value);
                    ParseViewType(enumerator, value);
                    ParseAction(enumerator, value);
                    if (ParseObjId(enumerator, value))
                    {
                        ParseProperty(enumerator, value);
                    }
                }
                
               
            }


            return rs;

        }

       
    }
}