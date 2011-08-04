using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Fantasy.AddIns
{
    public class BindingExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new MemberValueProvider() { Member = Path };
        }

        public string Path { get; set; }
    }
}
