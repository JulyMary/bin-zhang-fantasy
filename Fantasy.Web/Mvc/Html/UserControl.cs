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

        // Properties
        public IDictionary<string, string> Attributes { get; private set; }


      


    }
}