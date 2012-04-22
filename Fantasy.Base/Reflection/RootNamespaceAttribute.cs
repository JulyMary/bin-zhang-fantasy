using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false)] 
    public class RootNamespaceAttribute : Attribute
    {
        public RootNamespaceAttribute(string rootNamespace)
        {
            this.RootNamespace = rootNamespace;
        }

        public string RootNamespace { get; private set; }
    }
}
