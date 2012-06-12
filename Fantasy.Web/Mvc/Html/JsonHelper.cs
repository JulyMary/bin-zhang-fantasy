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
                string rs = JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented);
                return new HtmlString(rs);
            }
            else
            {
                return new HtmlString("null");
            }
        }

        private static Type[] PrimitiveTypes = new Type[] { typeof(DBNull), typeof(string), typeof(long), typeof(int), typeof(short), typeof(sbyte), typeof(ulong), typeof(uint), typeof(ushort), typeof(byte),
        typeof(double), typeof(float), typeof(decimal), typeof(DateTime), typeof(DateTimeOffset), typeof(byte[]), typeof(bool), typeof(Guid), typeof(Uri), typeof(TimeSpan) };
        public static bool IsPrimitiveType(Type t)
        {
            return t.IsEnum || Array.IndexOf(PrimitiveTypes, t) >= 0;
        }
    }
}