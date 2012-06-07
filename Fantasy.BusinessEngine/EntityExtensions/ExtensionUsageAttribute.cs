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

    

      


}
