using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.UI
{
    public class HtmlLiteral : IHtmlNode
    {
        #region IHtmlNode Members

        public void Write(System.IO.TextWriter output)
        {
            output.Write(Html);
        }

        #endregion

        public string Html { get; set; }
    }
}