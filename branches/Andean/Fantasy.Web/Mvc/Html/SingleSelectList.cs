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
            if (this.OptionsCaption != null)
            {
                rs.AppendFormat(", optionsCaption: '{0}'", this.OptionsCaption);
            }
            if(!string.IsNullOrEmpty(this.OptionsText))
            {
                rs.AppendFormat(", optionsText: '{0}'", this.OptionsText);
            }
            if (!string.IsNullOrEmpty(this.OptionsValue))
            {
                rs.AppendFormat(", optionsValue: '{0}'", this.OptionsValue); 
            }
            return new MvcHtmlString(rs.ToString());
        }

        public string CreateEntityExtension()
        {
            JObject ex = new JObject();
            ex.Add(new JProperty("Id", this.Model.Object.Id));
            ex.Add(new JProperty(String.Format("{0}$Options", this.PropertyName), JArray.FromObject(this.Items)));
            ex.Add(new JProperty(String.Format("{0}$Display", this.PropertyName), new JRaw(GetDisplayFunc())));
            string rs = JsonConvert.SerializeObject(ex, Formatting.Indented);
            return rs; 
        }

       

        private IEnumerable<JToken> JItems()
        {
            foreach (object o in this.Items)
            {
                yield return JToken.FromObject(o);
            }
        }

        private string GetDisplayFunc()
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
            var items = this.{2}$Options();
            var item = itemByValue(this.{2}(), items);
            var rs = displayByItem(item);
            return rs;
}}", itemByValueFunc, displayByItemFunc, this.PropertyName);
            return displayFunc;
        }

        private const string DisplayByItemFunctionTemplate = @"function displayByItem(item){{
              if(item == undefined)
              {{
                  return undefined;
              }}
              return $.isFunction(item.{0}) ? item.{0}() : item.{0};
             
}}";

        private const string NoDisplayMemberDisplayByItemFunctionTemplate = @"function displayByItem(item){{
              return item;
             
}}";
        private const string ItemByValueFunctionTemplate = @"function itemByValue(val, items){{
            
            
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

        private const string NoValueMemberItemByValueFunctionTemplate = @"function itemByValue(val, items){{
          
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