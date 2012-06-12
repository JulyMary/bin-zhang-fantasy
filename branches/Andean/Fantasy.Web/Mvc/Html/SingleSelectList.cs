using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Fantasy.Web.Mvc.Html
{
    partial class SingleSelectList
    {

        private List<object> _items = new List<object>();
        public List<object> Items
        {
            get
            {
                return _items;
            }
        }


        public string OptionsText { get; set; }

        public string OptionsValue { get; set; }

        public string OptionsCaption { get; set; }

        private MvcHtmlString AdditionalOptions()
        {
            StringBuilder rs = new StringBuilder();
            if (!string.IsNullOrEmpty(this.OptionsCaption))
            {
                rs.AppendFormat(", optionsCaption: {0}", this.OptionsCaption);
            }
            if(!string.IsNullOrEmpty(this.OptionsText))
            {
                rs.AppendFormat(", optionsText: {0}", this.OptionsText);
            }
            if (!string.IsNullOrEmpty(this.OptionsValue))
            {
                rs.AppendFormat(", optionsValue: {0}", this.OptionsValue); 
            }
            return new MvcHtmlString(rs.ToString());
        }

        public string CreateEntityExtension()
        {
            JObject ex = new JObject();
            ex.Add(new JProperty("Id", this.Model.Object.Id));
            ex.Add(new JProperty(String.Format("{0}$Options", this.PropertyName),new JArray(this.Items)));
            ex.Add(new JProperty(String.Format("{0}$Display", this.PropertyName),  GetDisplayFunc()));
            string rs = JsonConvert.SerializeObject(ex, Formatting.None);
            return rs; 
        }

        private JavaScriptString GetDisplayFunc()
        {
            string itemByValueFunc;
            if (string.IsNullOrEmpty(this.OptionsValue))
            {
                itemByValueFunc = String.Format(NoValueMemberItemByValueFunctionTemplate, this.PropertyName);
            }
            else
            {
                itemByValueFunc = string.Format(ItemByValueFunctionTemplate, this.PropertyName, this.OptionsValue);
            }

            string displayByItemFunc;
            if (string.IsNullOrEmpty(this.OptionsText))
            {
                displayByItemFunc = NoDisplayMemberDisplayByItemFunctionTemplate;
            }
            else
            {
                displayByItemFunc = string.Format(DisplayByItemFunctionTemplate, this.OptionsText);
            }

            string displayFunc = string.Format(@"function(){{
            {0}
            {1}
            var item = itemByValue(this.{2}());
            var rs = displayByItem(item);
            return rs;
}}", itemByValueFunc, displayByItemFunc, this.PropertyName);
            return new JavaScriptString(displayFunc);
        }


//        private const string PostValueByItemFunctionTemplate = @"function postValueByItem(item){{
//              return $.isFunction(item.{0})) ? item.{0}() : item.{0};
//             
//}}";

//        private const string NoPostValueMemberDisplayByItemFunctionTemplate = @"function postValueByItem(item){{
//              return item;
//             
//}}";


        private const string DisplayByItemFunctionTemplate = @"function displayByItem(item){{
              return $.isFunction(item.{0})) ? item.{0}() : item.{0};
             
}}";

        private const string NoDisplayMemberDisplayByItemFunctionTemplate = @"function displayByItem(item){{
              return item;
             
}}";
        private const string ItemByValueFunctionTemplate = @"function itemByValue(val){{
            var items = this.{0}$Options();
            
            for(var i = 0; i < items.length; i ++)
            {{
                var item = items[i];
                var found = false;
                if($.isFunction(item.{1}))
                {{
                    found = item.{1}() == val;
                }}
                else
                {{
                    found = item.{1} == val;
                }}
                if(found)
                {{
                   return item;
                }}
            }}
            return undefined;
        }}";

        private const string NoValueMemberItemByValueFunctionTemplate = @"function itemByValue(val){{
            var items = this.{0}$Options();
            
            for(var i = 0; i < items.length; i ++)
            {{
                var item = items[i]; 
                if(item == val)
                {{
                   return item;
                }}
            }}
            return undefined;
        }}";
    }
}