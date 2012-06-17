using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Mvc.Html
{
    public partial class FlagsEnumEditor
    {

        public string OptionsCaption { get; set; }

        private MvcHtmlString AdditionalOptions()
        {
            StringBuilder rs = new StringBuilder();
            if (this.OptionsCaption != null)
            {
                rs.AppendFormat(", optionsCaption: '{0}'", this.OptionsCaption);
            }
           
            rs.Append(", optionsText: 'Text'");
            rs.Append(", optionsValue: 'Value'");
            
            return new MvcHtmlString(rs.ToString());
        }

        public string CreateEntityExtension()
        {
            JObject ex = new JObject();
            ex.Add(new JProperty("Id", this.Model.Object.Id));
            ex.Add(new JProperty(String.Format("{0}$Options", this.PropertyName), JArray.FromObject(this.Items())));
            ex.Add(new JProperty(String.Format("{0}$Display", this.PropertyName), new JRaw(string.Format(DisplayFunctionTemplate, this.PropertyName))));
            string rs = JsonConvert.SerializeObject(ex, Formatting.Indented);
            return rs;
        }

        private IEnumerable<object> Items()
        {
            foreach (BusinessEnumValue val in this.PropertyDescriptor.ReferencedEnum.EnumValues)
            {
                yield return new { Value = "_" + val.Value.ToString(), Text = val.Name };
            }
        }

        private const string DisplayFunctionTemplate = @"function (){{
            var value = this.{0}();
            if(value == undefined)
            {{
                return '';
            }}
            var items = this.{0}$Options();
            var rs = new Array();
            for(var item in items)
            {{
                var v2 = item.Value;
                if(value & v2 == v2)
                {{
                    rs.push(item.Text);
                }}
            }}
              
            return rs.join();
             
        }}";


       
    }
}