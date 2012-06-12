using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Fantasy.Web.Mvc
{
    [JsonConverter(typeof(JavaStringStringJsonConverter))]
    public class JavaScriptString : HtmlString
    {
        // Fields
        private readonly string _value;

        public static readonly JavaScriptString Empty = Create(string.Empty);

        // Methods
        public JavaScriptString(string value)
            : base(value ?? string.Empty)
        {
            this._value = value ?? string.Empty;
        }

        public static JavaScriptString Create(string value)
        {
            return new JavaScriptString(value);
        }

        public static bool IsNullOrEmpty(JavaScriptString value)
        {
            if (value != null)
            {
                return (value._value.Length == 0);
            }
            return true;
        }

    }

    
}