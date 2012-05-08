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
    public static class UserControlFactoryExtensions
    {

        public static T Attr<T, TControl>(this T factory, object htmlAttribute, bool replaceExisting = true)
            where T : UserControlFactory<TControl>
            where TControl : UserControl, new()
        {

            RouteValueDictionary attrs2 = new RouteValueDictionary(htmlAttribute);


            foreach (KeyValuePair<string, object> pair in attrs2)
            {
                string key = Convert.ToString(pair.Key, CultureInfo.InvariantCulture);
                string str2 = Convert.ToString(pair.Value, CultureInfo.InvariantCulture);
                MergeAttribute(factory.Control, key, str2, replaceExisting);
            }
            
            return factory;
        }

        public static T Attr<T, TControl>(this T factory, RouteValueDictionary htmlAttributes, bool replaceExisting = true)
            where T : UserControlFactory<TControl>
            where TControl : UserControl, new()
        {

            if (htmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> pair in htmlAttributes)
                {
                    string key = Convert.ToString(pair.Key, CultureInfo.InvariantCulture);
                    string str2 = Convert.ToString(pair.Value, CultureInfo.InvariantCulture);
                    MergeAttribute(factory.Control, key, str2, replaceExisting);
                }
            }
            return factory;
        }

        public static T RemoveAttr<T, TControl>(this T factory, string attrName)
            where T : UserControlFactory<TControl>
            where TControl : UserControl, new()
        {
            factory.Control.Attributes.Remove(attrName);
            return factory;
        }

        public static T AddClass<T, TControl>(this T factory, string value)
            where T : UserControlFactory<TControl>
            where TControl : UserControl, new()
        {

            string str;
            if (factory.Control.Attributes.TryGetValue("class", out str))
            {
                factory.Control.Attributes["class"] = value + " " + str;
            }
            else
            {
                factory.Control.Attributes["class"] = value;
            }

         
            return factory;
        }

        public static T RemoveClass<T, TControl>(this T factory, string value) 
            where T : UserControlFactory<TControl> 
            where TControl : UserControl, new()
        {
            string @class = factory.Control.Attributes.GetValueOrDefault("class", string.Empty);
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

            factory.Control.Attributes["class"] = String.Join(" ", values);
            return factory;
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


        public static T BindModel<T, TControl>(this T factory, object model)
            where T : UserControlFactory<TControl>
            where TControl : UserControl, new()
        {

            factory.Model = model;
            return factory;
        }


        public static T BindModel<T, TControl, TModel>(this T factory, TModel model)
            where T : ControlFactory<TControl, TModel>
            where TControl: UserControl<TModel>, new()
        {

            ((UserControlFactory<TControl>)factory).Model = model;
            return factory;
        }
    
        

 
    }


}