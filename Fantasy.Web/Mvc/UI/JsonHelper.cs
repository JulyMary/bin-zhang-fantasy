using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;

namespace Fantasy.Web.Mvc.UI
{
    public class JsonHelper
    {
        public static HtmlString Serialize(object o)
        {
            if (o != null)
            {
                string rs = new JavaScriptSerializer().Serialize(o);
                return new HtmlString(rs);
            }
            else
            {
                return new HtmlString("null");
            }
        }
    }
}