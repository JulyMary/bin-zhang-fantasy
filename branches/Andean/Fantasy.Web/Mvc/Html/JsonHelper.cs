using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Fantasy.Web.Mvc.Html
{
    public class JsonHelper
    {
        public static HtmlString Serialize(object o)
        {
            if (o != null)
            {
                string rs = JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.None);
                return new HtmlString(rs);
            }
            else
            {
                return new HtmlString("null");
            }
        }
    }
}