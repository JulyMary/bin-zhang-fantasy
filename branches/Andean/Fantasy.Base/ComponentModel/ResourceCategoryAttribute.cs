using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Fantasy.ComponentModel
{
    public class ResourceCategoryAttribute : CategoryAttribute
    {
        public ResourceCategoryAttribute(Type type, string key)
            : base((string)Invoker.InvokeStatic(type, key))
        {
           
            
        }

        public ResourceCategoryAttribute(string typeName, string key)
            : this(Type.GetType(typeName, true), key)
        {
          
        }

        
    }
}
