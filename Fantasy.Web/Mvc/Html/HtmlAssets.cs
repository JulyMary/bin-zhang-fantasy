using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Linq;

namespace Fantasy.Web.Mvc.Html
{

    public class HtmlAssets
    {
        

        public ItemRegistrar Styles { get; private set; }
        public ItemRegistrar Scripts { get; private set; }

        public HtmlAssets()
        {
            Styles = new ItemRegistrar(ItemRegistrarFormatters.StyleFormat);
            Scripts = new ItemRegistrar(ItemRegistrarFormatters.ScriptFormat);
        }
        
    }

    public class ItemRegistrar
    {
        private readonly string _format;
        private readonly IList<string> _items;

        public ItemRegistrar(string format)
        {
            _format = format;
            _items = new List<string>();
        }

        public ItemRegistrar Add(string url)
        {
            if (!_items.Any(s=>string.Equals(url, s, System.StringComparison.OrdinalIgnoreCase)))
                _items.Insert(0, url);

            return this;
        }

        public IHtmlString Render()
        {
            var sb = new StringBuilder();

            foreach (var item in _items)
            {
                var fmt = string.Format(_format, item);
                sb.AppendLine(fmt);
            }

            return new HtmlString(sb.ToString());
        }
    }

    public class ItemRegistrarFormatters
    {
        public const string StyleFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        public const string ScriptFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
    }


    public static class HtmlAssetsExtensions
    {
        public static HtmlAssets HtmlAssets(this HtmlHelper htmlHelper)
        {
            var instanceKey = "AssetsHelperInstance";

            var context = htmlHelper.ViewContext.HttpContext;
            if (context == null) return null;

            var assetsHelper = (HtmlAssets)context.Items[instanceKey];

            if (assetsHelper == null)
                context.Items.Add(instanceKey, assetsHelper = new HtmlAssets());

            return assetsHelper;
        }
    }
}