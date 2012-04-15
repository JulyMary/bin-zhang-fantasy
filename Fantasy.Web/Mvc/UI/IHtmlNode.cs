using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Fantasy.Web.Mvc.UI
{
    public interface IHtmlNode
    {

        void Write(TextWriter output);
    }
}