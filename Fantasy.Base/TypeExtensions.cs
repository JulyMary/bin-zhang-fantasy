using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy
{
    public static  class TypeExtensions
    {
        public static T GetCustomAttribute<T>(this Type type, bool inherit)
        {
            return (T)type.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
        }

        
    }
}
