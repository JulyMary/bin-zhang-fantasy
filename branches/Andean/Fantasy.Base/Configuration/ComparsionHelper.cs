using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections;

namespace Fantasy.Configuration
{
    public static class ComparsionHelper
    {
        public static bool DeepEquals(object a, object b)
        {
            if (a != null && b != null)
            {
                if (a.GetType() == b.GetType())
                {
                    return CompareProperties(a, b);
                }
                else
                {
                    return false;
                }
            }
            else if (a == null && b == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareProperties(object a, object b)
        {
            Type t = a.GetType();
            var query = from prop in t.GetProperties() where prop.GetIndexParameters().Length == 0 && ! prop.IsDefined(typeof(XmlIgnoreAttribute), true) 
                            && (prop.CanWrite || typeof(ICollection).IsAssignableFrom(prop.PropertyType)) select prop;
            foreach (PropertyInfo prop in query)
            {
                object v1 = prop.GetValue(a, null);
                object v2 = prop.GetValue(b, null);
                if (typeof(ICollection).IsAssignableFrom(prop.PropertyType))
                {
                    if (!CollectionEquals((ICollection)v1, (ICollection)v2))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!Object.Equals(v1, v2))
                    {
                        return false;
                    }
                }
            }

            foreach (FieldInfo field in t.GetFields())
            {
                if (!field.IsDefined(typeof(XmlIgnoreAttribute), true))
                {
                    object v1 = field.GetValue(a);
                    object v2 = field.GetValue(b);
                    if (typeof(ICollection).IsAssignableFrom(field.FieldType))
                    {
                        if (!CollectionEquals((ICollection)v1, (ICollection)v2))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!Object.Equals(v1, v2))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
           
        }

        private static bool CollectionEquals(ICollection a, ICollection b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            IEnumerator e1 = a.GetEnumerator();
            IEnumerator e2 = b.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext())
            {
                if (!Object.Equals(e1.Current, e2.Current))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
