using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Linq;
using System;

namespace Fantasy.Web.Mvc.Html
{

   


    public class HtmlAssets
    {
        internal HtmlAssets(HtmlHelper html)
        {
            this._html = html;
        }


        private const string DataInstanceKey = "HtmlAssetsDataInstance";
       


        private static HtmlAssetsData GetData(HttpContextBase context)
        {
            HtmlAssetsData rs = (HtmlAssetsData)context.Items[DataInstanceKey];

            if (rs == null)
            {
                rs = new HtmlAssetsData();
                context.Items.Add(DataInstanceKey, rs);
            }

            return rs;
        }
        private HtmlAssetsData Data
        {
            get
            {

              
                HttpContextBase context = this._html.ViewContext.HttpContext;
                return GetData(context);
                   
            }
        }
       


        public void AddStyleSheet(string url)
        {
            if (!this.Data.StyleSheets.Any(s => String.Equals(s, url, StringComparison.OrdinalIgnoreCase)))
            {
                this.Data.StyleSheets.Insert(this.Data._stylePosition, url);
                this.Data._stylePosition++;
            }
        }

        public void AddScript(string url)
        {
            if (!this.Data.Scripts.Any(s => String.Equals(s, url, StringComparison.OrdinalIgnoreCase)))
            {
                this.Data.Scripts.Insert(this.Data._scriptsPosition, url);
                this.Data._scriptsPosition++;
            }
        }

        public void AddStartupScript(string content, string id = null)
        {
            

            if (string.IsNullOrEmpty(id) || !this.Data.StartupScripts.Any(s => String.Equals(s.Key, id, StringComparison.OrdinalIgnoreCase)))
            {
                this.Data.StartupScripts.Insert(this.Data._startupPosition, new KeyValuePair<string, string>(id, content));
                this.Data._startupPosition++;
            }
        }

        public bool ContainsStartupScript(string id)
        {
            return this.Data.StartupScripts.Any(s => s.Key == id);
        }

        

        private static MvcHtmlString _emptyString = new MvcHtmlString(string.Empty);

        public MvcHtmlString RenderStyleSheets()
        {
            string rs = string.Join("\r\n", this.Data.StyleSheets.Select(s => string.Format(StyleFormat, s)));
            return new MvcHtmlString(rs);
        }


        public const string StyleFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        public const string ScriptFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
        public const string ReadyFormat = @"<script type=""text/javascript"">
$(document).ready(function()
{
    *
})
</script>";

      

        public MvcHtmlString RenderScripts()
        {

            StringBuilder rs = new StringBuilder( string.Join("\r\n", this.Data.Scripts.Select(s => string.Format(ScriptFormat, s))));

            string startup = string.Join("\r\n", this.Data.StartupScripts.Select(s=>s.Value));

            rs.Append(ReadyFormat.Replace("*", startup));
            return new MvcHtmlString(rs.ToString());
        }



        internal static string RenderAjaxCallback(HttpContextBase context)
        {

            HtmlAssetsData data = (HtmlAssetsData)context.Items[DataInstanceKey];
            if (data != null)
            {

                StringBuilder rs = new StringBuilder("<script type=\"text/javascript\">\r\n");
                foreach (string s in data.StyleSheets)
                {
                    rs.AppendFormat("appendStyleSheet(\"{0}\");\r\n", s);
                }

                rs.AppendFormat("var externalScripts = {0};", JsonHelper.Serialize(data.Scripts));

                rs.AppendFormat("var startup = function(){{{0}}};", string.Join(Environment.NewLine, data.StartupScripts.Select(x => x.Value)));

                rs.Append("execute_ajax_scripts(externalScripts, startup);\r\n");

                rs.Append("</script>");

                return rs.ToString();
            }
            else
            {
                return string.Empty;
            }
           
        }

        private HtmlHelper _html { get; set; }
 

    }

    internal class HtmlAssetsData
    {
        internal List<string> StyleSheets = new List<string>();
        internal List<string> Scripts = new List<string>();

        internal List<KeyValuePair<string, string>> StartupScripts = new List<KeyValuePair<string, string>>();

        internal int _scriptsPosition = 0;
        internal int _stylePosition = 0;
        internal int _startupPosition = 0;

    }

    public static class HtmlAssetsExtensions
    {
        public static HtmlAssets HtmlAssets(this HtmlHelper htmlHelper)
        {

            var instanceKey = "HtmlAssetsInstance";

            var context = htmlHelper.ViewContext.ViewData;
            if (context == null) return null;

            var assetsHelper = (HtmlAssets)context[instanceKey];

            if (assetsHelper == null)
                context.Add(instanceKey, assetsHelper = new HtmlAssets(htmlHelper));

            return assetsHelper;
        }
    }
}