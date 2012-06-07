using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace Fantasy.ComponentModel
{
    

    public class IconAttribute : ReferenceTypeAttributeBase
    {

        public IconAttribute(Type type, string key)
            : base(type)
        {
          
            this.ResourceKey = key;
        }

        public IconAttribute(string typeName, string key)
            : base(typeName)
        {
            
            this.ResourceKey = key;
        }

        public String ResourceKey { get; private set; }

        public Image Icon
        {
            get
            {
                return (Image)Invoker.InvokeStatic(this.ReferencedType, this.ResourceKey);

            }
        }
    }
}
