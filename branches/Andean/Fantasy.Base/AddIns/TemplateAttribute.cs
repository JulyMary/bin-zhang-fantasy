using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TemplateAttribute : Attribute 
    {
        public TemplateAttribute(string buildMember)
        {
            BuildMember = buildMember;
        }

        public string BuildMember { get; private set; }
    }
}
