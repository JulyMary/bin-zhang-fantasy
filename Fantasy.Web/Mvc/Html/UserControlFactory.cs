using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.WebPages;
using System.Web.Routing;

namespace Fantasy.Web.Mvc.Html
{


    public class UserControlFactory : IHtmlString
    {
        public UserControlFactory(HtmlHelper htmlHelper, Type controlType)
        {
            this.Html = htmlHelper;
            this.Control = (UserControl)Activator.CreateInstance(controlType);
            string _virtualPath = controlType.GetCustomAttributes(typeof(PageVirtualPathAttribute), true).Cast<PageVirtualPathAttribute>().Single().VirtualPath;
            this.Control.VirtualPath = _virtualPath;
        }

        public HtmlHelper Html { get; private set; }

        public dynamic Model { get; internal set; }

        protected internal UserControl Control { get; private set; }


        public virtual string ToHtmlString()
        {

            StringWriter writer = new StringWriter();
            this.Render(writer);
           

            return writer.ToString();
        }

        public override string ToString()
        {
            return this.ToHtmlString().ToString();
        }

        public void Render()
        {
            this.Render(this.Html.ViewContext.Writer);
        }

        public virtual void Render(TextWriter writer)
        {

            ViewDataDictionary dictionary = null;

            if (this.Model == null)
            {
                dictionary = new ViewDataDictionary(this.Html.ViewData);
            }
            else
            {
                dictionary = new ViewDataDictionary(this.Model);
            }

            RouteData routeData = new RouteData();

            foreach (KeyValuePair<string, object> pair in this.Html.ViewContext.RouteData.Values)
            {
                routeData.Values.Add(pair.Key, pair.Value);
            }
           

            foreach (KeyValuePair<string, object> pair in this.Html.ViewContext.RouteData.DataTokens)
            {
                routeData.DataTokens.Add(pair.Key, pair.Value);
            }

            routeData.Route = this.Html.ViewContext.RouteData.Route;

            routeData.DataTokens["ParentActionViewContext"] = this.Html.ViewContext;

            ViewContext viewContext = new ViewContext(this.Html.ViewContext, this.Html.ViewContext.View, dictionary, this.Html.ViewContext.TempData, writer);
            viewContext.RouteData = routeData;
            this.Control.ViewContext = viewContext;
            Control.ViewData = viewContext.ViewData;
           
            Control.InitHelpers();
            var pageContext = new WebPageContext(viewContext.HttpContext, this.Control, this.Model);
            Control.ExecutePageHierarchy(pageContext, writer);


        }

       
    }


    public class UserControlFactory<TControl> : UserControlFactory where TControl : UserControl , new()
    {


        public UserControlFactory(HtmlHelper htmlHelper)
            : base(htmlHelper, typeof(TControl))
        {
           
        }

        protected internal new TControl Control
        {
            get
            {
                return (TControl)base.Control;
            }
        }
        
    }


   
}