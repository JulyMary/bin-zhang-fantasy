using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Reflection
{
    public static class MemberInfoExtension
    {
        public static T[] GetCustomAttributes<T>(this MemberInfo mi, bool inherit)
        {
            return mi.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }
    }
}
