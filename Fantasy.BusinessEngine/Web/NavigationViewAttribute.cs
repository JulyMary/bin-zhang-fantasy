using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Web
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NavigationViewAttribute : Attribute
    {
        public bool EnableCustomizeDetailView { get; set; }
    }
}
