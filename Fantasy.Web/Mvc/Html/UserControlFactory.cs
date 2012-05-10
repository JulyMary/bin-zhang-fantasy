using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.WebPages;

namespace Fantasy.Web.Mvc.Html
{


    public class UserControlFactory
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


        public virtual HtmlString ToHtmlString()
        {
            return new HtmlString(this.ToString());
        }

        public override string ToString()
        {
            StringWriter writer = new StringWriter();
            this.Render(writer);
            return writer.ToString();
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

            ViewContext viewContext = new ViewContext(this.Html.ViewContext, this.Html.ViewContext.View, dictionary, this.Html.ViewContext.TempData, writer);
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


    //public class UserControlFactory<TControl, TModel> : UserControlFactory<TControl> where TControl : UserControl<TModel>, new()
    //{

    //    public UserControlFactory(HtmlHelper htmlHelper)
    //        :base(htmlHelper)
    //    {

    //    }

    //    public new TModel Model
    //    {
    //        get
    //        {
    //            return base.Model;
    //        }
    //    }
    //}
}