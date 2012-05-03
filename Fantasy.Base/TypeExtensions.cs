using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Properties;

namespace Fantasy
{
    public static  class TypeExtensions
    {
        public static T GetCustomAttribute<T>(this Type type, bool inherit = true, bool required = true)
        {
            return required ? (T)type.GetCustomAttributes(typeof(T), inherit).Single(String.Format(Resources.CustomAttributeNotDefinedMessage, typeof(T), type))
                : (T)type.GetCustomAttributes(typeof(T), inherit).SingleOrDefault();
        }

        public static string VersionFreeTypeName(this Type type)
        {
            return String.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
        }

        
    }
}
