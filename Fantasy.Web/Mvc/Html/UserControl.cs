using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Fantasy.Web.Properties;
using System.Globalization;
using System.IO;
using System.Web.Routing;
using System.Web.WebPages;

namespace Fantasy.Web.Mvc.Html
{
    public class UserControl : WebViewPage
    {
        // Methods
        public UserControl()
        {
            
            this.Attributes = new SortedDictionary<string, string>(StringComparer.Ordinal);
        }

        protected HtmlString WriteAttributes()
        {
            StringBuilder output = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in this.Attributes)
            {
                string key = pair.Key;
                string str2 = HttpUtility.HtmlAttributeEncode(pair.Value);
                output.Append(" ").Append(key).Append("=\"").Append(str2).Append('"');
               
            }
            return new HtmlString(output.ToString());
        }


        internal object Model2 { get; set; }

        internal HtmlHelper Html2 { get; set; }

        // Properties
        public IDictionary<string, string> Attributes { get; private set; }


        public override void Execute()
        {
            
        }


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
            this.Render(this.Html2.ViewContext.Writer);
        }

        public virtual void Render(TextWriter writer)
        {

            ViewDataDictionary dictionary = null;

            if (this.Model2 == null)
            {
                dictionary = new ViewDataDictionary(this.Html2.ViewData);
            }
            else
            {
                dictionary = new ViewDataDictionary(this.Model2); 
            }

            ViewContext viewContext = new ViewContext(this.Html2.ViewContext, this.Html2.ViewContext.View, dictionary, this.Html2.ViewContext.TempData, writer);

            string _virtualPath = this.GetType().GetCustomAttributes(typeof(PageVirtualPathAttribute), true).Cast<PageVirtualPathAttribute>().Single().VirtualPath;

            this.VirtualPath = _virtualPath;
            this.ViewContext = viewContext;
            this.ViewData = viewContext.ViewData;
            this.InitHelpers();
            var pageContext = new WebPageContext(viewContext.HttpContext, this, this.Model2);
            this.ExecutePageHierarchy(pageContext, writer);

           
        }
    }
}