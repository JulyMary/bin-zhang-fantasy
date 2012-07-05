using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.XSerialization
{
    [XConverter(TargetType=typeof(Object))]
    public class XObjectConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                return Convert.ChangeType(value, destinationType);
            }
            else
            {
                return null;
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return Convert.ChangeType(value, typeof(string));
            }
        }
    }
}
