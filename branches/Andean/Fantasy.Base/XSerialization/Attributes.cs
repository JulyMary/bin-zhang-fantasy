using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.XSerialization
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]  
    public abstract class XMemberAttribute : Attribute
    {
        public XMemberAttribute()
        {
            this.Order = Int32.MaxValue;
           
        }
        public int Order { get; set; }
        public string Name { get; set; }
        public string NamespaceUri { get; set; }
        public Type XConverter { get; set; }
        
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class XValueAttribute : XMemberAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class XElementAttribute : XMemberAttribute
    {
        public XElementAttribute(string name)
        {
            this.Name = name;
        }
       
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false)]
    public class XAttributeAttribute : XMemberAttribute
    {
        public XAttributeAttribute(string name)
        {
            this.Name = name;
           
        } 
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class XNamespaceAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class XArrayAttribute : XMemberAttribute
    {
        public Type Serializer { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)] 
    public class XArrayItemAttribute : Attribute
    {
        public XArrayItemAttribute()
        {
           
        }
        public string Name { get; set; }
        public string NamespaceUri { get; set; }
        public Type Type { get; set; }
        public Type XConverter { get; set; }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class XSerializableAttribute : Attribute
    {
        
        public XSerializableAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public string NamespaceUri { get; set; }

        public Type XConverter { get; set; }

    }


    [AttributeUsage(AttributeTargets.Class)] 
    public class XConverterAttribute : Attribute
    {
        public Type TargetType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple=true)]  
    public class XPrefixAttribute : Attribute 
    {
        public XPrefixAttribute(string prefix, string @namespace)
        {
            this.Prefix = prefix;
            this.Namesapce = @namespace;
        }
        public string Prefix { get; private set; }
        public string Namesapce { get; private set; }
    }
}
