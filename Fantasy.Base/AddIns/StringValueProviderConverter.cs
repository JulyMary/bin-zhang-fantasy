using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.AddIns
{
    public class StringValueProviderConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);

        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null || value is string)
            {
                return new StringValueProvider() { Value = (string)value };
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
