using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using System.IO;
using System.Reflection;
using System.Web.Mvc.Ajax;

namespace Fantasy.Web.Mvc.Html
{
    public static class BusinessApplicationExtensions
    {
        public static MvcHtmlString ScalarViewLink(this AjaxHelper ajaxHelper, string linkText, Guid objId, string action = "Default", RouteValueDictionary routeValues = null, object htmlAttributes = null, AjaxOptions ajaxOptions = null)
        {

            UrlHelper urlHelper = new UrlHelper(ajaxHelper.ViewContext.RequestContext);

            IDictionary<string, object> attrs = urlHelper.GetSclarViewLinkAttributes(objId, action, routeValues, ajaxOptions); 

            TagBuilder rs = new TagBuilder("a");

           
            attrs = MergeDictionaries(attrs, CreateRouteValueDictionary(htmlAttributes));
            

            rs.MergeAttributes(attrs);
            rs.SetInnerText(linkText);

            return new MvcHtmlString(rs.ToString(TagRenderMode.Normal));
            
        }


        private static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            if (values is RouteValueDictionary)
            {
                return (RouteValueDictionary)values; 
            }
            else if (values != null)
            {
                return new RouteValueDictionary(values);
            }
            else
            {
                return new RouteValueDictionary();
            }
        }


        private static  IDictionary<string, object> GetAjaxHtmlAttributes(AjaxOptions ajaxOptions = null)
        {
            if (ajaxOptions == null)
            {
                ajaxOptions = new AjaxOptions();
            }
            IDictionary<string, object> rs = ajaxOptions.ToUnobtrusiveHtmlAttributes();

           
           

            return rs;

        }

        public static MvcForm BeginAppViewForm(this AjaxHelper ajaxHelper, AjaxOptions ajaxOptions = null, string appName = null, Guid? rootId = null, ViewType? viewType = null,
                  Guid? objectId = null, string action = null, string property = null, object routeValues = null, object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(ajaxHelper.ViewContext.RequestContext);
            string formAction = urlHelper.ApplicationUrl(appName, rootId, viewType, objectId, action, property, routeValues);

            RouteValueDictionary htmlAttributes2 = CreateRouteValueDictionary(htmlAttributes);

            return ajaxHelper.FormHelper(formAction, ajaxOptions, htmlAttributes2);

        }

       
        private static MvcForm FormHelper(this AjaxHelper ajaxHelper, string formAction, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("form");
            builder.MergeAttributes<string, object>(htmlAttributes);
            builder.MergeAttribute("action", formAction);
            builder.MergeAttribute("method", "post");
            ajaxOptions = ajaxOptions ?? new AjaxOptions();
           
            builder.MergeAttributes<string, object>(ajaxOptions.ToUnobtrusiveHtmlAttributes());
            
            if (ajaxHelper.ViewContext.ClientValidationEnabled)
            {
                Func<string> generator = (Func<string>)Invoker.Invoke(ajaxHelper.ViewContext, "FormIdGenerator"); 

                builder.GenerateId(generator());
            }
            ajaxHelper.ViewContext.Writer.Write(builder.ToString(TagRenderMode.StartTag));
            MvcForm form = new MvcForm(ajaxHelper.ViewContext);
            if (ajaxHelper.ViewContext.ClientValidationEnabled)
            {
                ajaxHelper.ViewContext.FormContext.FormId = builder.Attributes["id"];
            }
            return form;
        }

 

 

      

        public  static IDictionary<string, object> GetSclarViewLinkAttributes(this UrlHelper urlHelper, Guid objId, string action = "Default", RouteValueDictionary routeValues = null, AjaxOptions ajaxOptions = null)
        {

            RouteValueDictionary dictionary = CreateRouteValueDictionary(routeValues);
            routeValues = MergeDictionaries(dictionary, urlHelper.RequestContext.RouteData.Values);

            routeValues["action"] = action;

            routeValues["ObjId"] = objId;

            routeValues["ViewType"] = ViewType.Obj;

            routeValues.Remove("Property");

            IDictionary<string, object> rs = GetAjaxHtmlAttributes(ajaxOptions);
           
            string url = urlHelper.RouteUrl(routeValues);
            rs["href"] = url;
            return rs;
        }


        public static MvcHtmlString ScalarView(this HtmlHelper htmlHelper, Guid objId, string action = "Default", RouteValueDictionary routeValues = null)
        {

            StringWriter textWriter = new StringWriter();

            RouteValueDictionary dictionary = CreateRouteValueDictionary(routeValues);

            routeValues = MergeDictionaries(dictionary, htmlHelper.ViewContext.RouteData.Values);

            routeValues["action"] = action;

            routeValues["ObjId"] = objId;

            routeValues["ViewType"] = ViewType.Obj;

            RouteData routeData = new RouteData();
            foreach (KeyValuePair<string, object> pair in routeValues)
            {
                routeData.Values.Add(pair.Key, pair.Value);
            }

            foreach (KeyValuePair<string, object> pair in htmlHelper.ViewContext.RouteData.DataTokens)
            {
                routeData.DataTokens.Add(pair.Key, pair.Value);
            }

            routeData.Route = htmlHelper.ViewContext.RouteData.Route;

            routeData.DataTokens["ParentActionViewContext"] = htmlHelper.ViewContext;
            HttpContextBase httpContext = htmlHelper.ViewContext.HttpContext;
            RequestContext context = new RequestContext(httpContext, routeData);

            CurrentBusinessApplicationHandler httpHandler = new CurrentBusinessApplicationHandler(context);

            MethodInfo switchWriter = typeof(HttpResponse).GetMethod("SwitchWriter", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic);
            TextWriter writer2 = (TextWriter)switchWriter.Invoke(HttpContext.Current.Response, new object[] {textWriter});
            try
            {
                httpHandler.ProcessRequest(httpContext); 
            }
            finally
            {
                switchWriter.Invoke(HttpContext.Current.Response, new object[] { writer2 });
            }
           
            return new MvcHtmlString(textWriter.ToString());
        }


        private static RouteValueDictionary MergeDictionaries(params IDictionary<string, object>[] dictionaries)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            foreach (IDictionary<string, object> dictionary2 in from d in dictionaries
                                                         where d != null
                                                         select d)
            {
                foreach (KeyValuePair<string, object> pair in dictionary2)
                {
                    if (!dictionary.ContainsKey(pair.Key))
                    {
                        dictionary.Add(pair.Key, pair.Value);
                    }
                }
            }
            return dictionary;
        }

        private static RouteData CreateRouteData(RouteBase route, RouteValueDictionary routeValues, RouteValueDictionary dataTokens, ViewContext parentViewContext)
        {
            RouteData data = new RouteData();
            foreach (KeyValuePair<string, object> pair in routeValues)
            {
                data.Values.Add(pair.Key, pair.Value);
            }
            foreach (KeyValuePair<string, object> pair2 in dataTokens)
            {
                data.DataTokens.Add(pair2.Key, pair2.Value);
            }
            data.Route = route;
            data.DataTokens["ParentActionViewContext"] = parentViewContext;
            return data;
        }

        public static string ApplicationUrl(this UrlHelper url, string appName = null, Guid? rootId = null, ViewType? viewType = null,
                  Guid? objectId = null, string action = null, string property = null, object routeValues = null)
        {

            RouteValueDictionary values2 = MergeRouteValue(appName, rootId, viewType, objectId, action, property, routeValues);

            //Inherit appName, rootId, viewType if appName is not assigned
            if (!values2.ContainsKey("AppName"))
            {
                //RouteValueDictionary current = url.RequestContext.RouteData.Values;
              
                //values2.Add("AppName", current["AppName"]);
                //if (current.ContainsKey("RootId") && !values2.ContainsKey("RootId"))
                //{
                //    values2.Add("RootId", current["RootId"]);
                //}
                //if (current.ContainsKey("ViewType") && !values2.ContainsKey("ViewType"))
                //{
                //    values2.Add("ViewType", current["ViewType"]);
                //}
                values2 = MergeDictionaries(values2, url.RequestContext.RouteData.Values);
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


    }
}