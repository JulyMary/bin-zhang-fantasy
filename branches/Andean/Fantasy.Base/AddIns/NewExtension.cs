using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Fantasy.AddIns
{
    public class NewExtension : TypeExtension
    {
        public NewExtension(string typeName) : base(typeName)
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            System.Type type = (System.Type)base.ProvideValue(serviceProvider);
            return Activator.CreateInstance(type);
        }
    }
}
