using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;
using System.Web.Routing;

namespace Fantasy.Web.Mvc
{
    internal class AppRouteStack : Stack<AppRouteValue>
    {

        private static readonly string AppRouteStackKey = typeof(AppRouteStack).FullName;
        public static AppRouteStack Current
        {
            get
            {
                BusinessEngineContext context = BusinessEngineContext.Current;
                AppRouteStack rs = (AppRouteStack)context[AppRouteStackKey];
                if (rs == null)
                {
                    rs = new AppRouteStack();
                    context[AppRouteStackKey] = rs; 
                }
                return rs;
            }
        }


    }


    internal class AppRouteValue
    {

        public AppRouteValue()
        {

        }

        public AppRouteValue(RouteValueDictionary values)
        {
            if (values.ContainsKey("AppName"))
            {
                AppName = (string)values["AppName"];
            }
            if (values.ContainsKey("RootId"))
            {
                this.RootId = (Guid)values["RootId"];
            }
            if (values.ContainsKey("ViewType"))
            {
                this.ViewType = (ViewType)values["ViewType"];
            }
            if (values.ContainsKey("ObjId"))
            {
                this.ObjId = (Guid)values["ObjId"];
            }
            if (values.ContainsKey("Action"))
            {
                this.Action = (string)values["Action"];
            }
            if (values.ContainsKey("Property"))
            {
                this.Action = (string)values["Property"];
            }

        }

        public string AppName { get; set; }

        public Guid? RootId { get; set; }

        public ViewType? ViewType { get; set; }

        public Guid? ObjId { get; set; }

        public string Action { get; set; }

        public string Property { get; set; }
    }
}