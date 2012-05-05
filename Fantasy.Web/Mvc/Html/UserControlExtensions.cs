using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Fantasy.Web.Properties;
using System.Globalization;

namespace Fantasy.Web.Mvc.Html
{
    public static class UserControlExtensions
    {


        public static T Control<T>(this HtmlHelper htmlHelper) where T : UserControl, new()
        {
            return new T { Html2 = htmlHelper };
        }


        //public static UserControl Control(this HtmlHelper htmlHelper, Type controlType)
        //{
        //    UserControl rs = (UserControl)Activator.CreateInstance(controlType);
        //    rs.Html2 = htmlHelper;
        //    return rs;
        //}

        public static T Attr<T>(this T control, object htmlAttributes, bool replaceExisting = true) where T : UserControl
        {
            RouteValueDictionary htmlAttributes2 = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            Attr(control, htmlAttributes2, replaceExisting); 

            return control;
        }

        public static T Attr<T>(this T control, RouteValueDictionary htmlAttributes, bool replaceExisting = true) where T : UserControl
        {

            if (htmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> pair in htmlAttributes)
                {
                    string key = Convert.ToString(pair.Key, CultureInfo.InvariantCulture);
                    string str2 = Convert.ToString(pair.Value, CultureInfo.InvariantCulture);
                    MergeAttribute(control, key, str2, replaceExisting);
                }
            }
            return control;
        }

        public static T RemoveAttr<T>(this T control, string attrName) where T : UserControl
        {
            control.Attributes.Remove(attrName);
            return control;
        }

        public static T AddClass<T>(this T control, string value) where T : UserControl
        {

            string str;
            if (control.Attributes.TryGetValue("class", out str))
            {
                control.Attributes["class"] = value + " " + str;
            }
            else
            {
                control.Attributes["class"] = value;
            }

         
            return control;
        }

        public static T RemoveClass<T>(this T control, string value) where T : UserControl
        {
            string @class = control.Attributes.GetValueOrDefault("class", string.Empty);
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

            control.Attributes["class"] = String.Join(" ", values);
            return control;
        }

        public static T Model<T, TModel>(this T control, TModel model) where T : UserControl<TModel>
        {
            control.Model2 = model;
            return control;
        }

        public static T Model<T>(this T control, object model) where T : UserControl
        {
            control.Model2 = model;
            return control;

        }


        private static void MergeAttribute(UserControl control, string key, string value, bool replaceExisting = true)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "key");
            }
            if (replaceExisting || !control.Attributes.ContainsKey(key))
            {
                control.Attributes[key] = value;
            }
        }

    
       

 
    }


}