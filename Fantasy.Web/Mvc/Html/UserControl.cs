using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Fantasy.Web.Properties;
using System.Globalization;
using System.IO;
using System.Web.Routing;
using System.Web.WebPages;

namespace Fantasy.Web.Mvc.Html
{
    public class UserControl : WebViewPage
    {
        // Methods
        public UserControl()
        {
            
            this.Attributes = new SortedDictionary<string, string>(StringComparer.Ordinal);
        }

        protected HtmlString WriteAttributes()
        {
            StringBuilder output = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in this.Attributes)
            {
                string key = pair.Key;
                string str2 = HttpUtility.HtmlAttributeEncode(pair.Value);
                output.Append(" ").Append(key).Append("=\"").Append(str2).Append('"');
               
            }
            return new HtmlString(output.ToString());
        }

        // Properties
        public IDictionary<string, string> Attributes { get; private set; }


        public override void ExecutePageHierarchy()
        {
            this.PreExecute();
            base.ExecutePageHierarchy();
        }

        protected virtual void PreExecute()
        {
        }

        public override void Execute()
        {
            
        }


        protected void MergeAttribute(string key, string value, bool replaceExisting = true)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(String.Format(Resources.ArgumentNullOrEmptyStringText, "key"), "key");
            }
            if (replaceExisting || !this.Attributes.ContainsKey(key))
            {
                this.Attributes[key] = value;
            }
        }

        protected void RemoveAttribute(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(String.Format(Resources.ArgumentNullOrEmptyStringText, "key"), "key");
            }
            if (this.Attributes.ContainsKey(key))
            {
                this.Attributes.Remove(key);
            }
        }


        protected virtual void AddClass(string value)

        {

            string str;
            if (this.Attributes.TryGetValue("class", out str))
            {
                this.Attributes["class"] = value + " " + str;
            }
            else
            {
                this.Attributes["class"] = value;
            }
        }

        protected virtual void RemoveClass(string value)
           
        {
            string @class = this.Attributes.GetValueOrDefault("class", string.Empty);
            List<string> values = @class.Split(' ').ToList();

            int index;
            do
            {
                index = values.IndexOfBy(value, comparer: StringComparer.OrdinalIgnoreCase);

                if (index >= 0)
                {
                    values.RemoveAt(index);
                }
            } while (index >= 0);

            this.Attributes["class"] = String.Join(" ", values);
            
        }
      


    }
}