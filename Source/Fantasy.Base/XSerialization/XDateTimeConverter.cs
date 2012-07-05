using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Fantasy.XSerialization
{
    [XConverter(TargetType = typeof(DateTime))]
    public class XDateTimeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {

            return ((DateTime)value).ToString("O");
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            DateTime rs = DateTime.MinValue;
            string text = (string)value;
            if (!string.IsNullOrEmpty(text))
            {
                if(!DateTime.TryParseExact(text, "O", null, DateTimeStyles.None, out rs))
                {
                    rs = DateTime.Parse(text);
                }
                
            }
            return rs;
        }
    }
}
