using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Fantasy.ComponentModel
{
    public class CaptionAttribute : Attribute
    {
        public CaptionAttribute(string caption)
        {
            this.Caption = caption;
        }

        public string Caption { get; private set; }
    }


    public class ResourceCaptionAttribute : CaptionAttribute
    {
        public ResourceCaptionAttribute(Type type, string key)
            : base((string)Invoker.InvokeStatic(type, key))
        {
           
            
        }

        public ResourceCaptionAttribute(string typeName, string key)
            : this(Type.GetType(typeName, true), key)
        {
          
        }
    }
}
