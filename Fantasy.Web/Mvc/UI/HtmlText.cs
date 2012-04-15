using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.UI
{
    public class HtmlText : IHtmlNode
    {

        public string Text { get; set; }



        #region IHtmlNode Members

        public void Write(System.IO.TextWriter output)
        {
            output.Write(HttpUtility.HtmlEncode(this.Text));
        }

        #endregion
    }
}