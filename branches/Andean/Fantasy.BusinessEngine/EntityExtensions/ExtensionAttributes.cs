using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace Fantasy.BusinessEngine.EntityExtensions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class ExtensionUsageAttribute : Attribute
    {
        public bool AllowMultiple { get; set; }
    }

    public class ExtensionIconAttribute : Attribute
    {
        public Type ResourceType { get; set; }

        public String ResourceKey { get; set; }

        public Image Icon
        {
            get
            {
                PropertyInfo pi = ResourceType.GetProperty(ResourceKey, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
                return (Image)pi.GetValue(null, null);
            }
        }
    }

      


}
