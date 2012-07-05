using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.XSerialization
{
    public class NullableConverter : TypeConverter
    {

        private bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private TypeConverter GetInnterConverter(Type type)
        {
            Type destType = IsNullableType(type) ? Nullable.GetUnderlyingType(type): type;


            return XHelper.Default.CreateXConverter(destType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            
            if (value != null)
            {
                if (IsNullableType(value.GetType()))
                {

                    //dynamic d = value;
                    value = value.GetType().GetProperty("Value").GetValue(value, null);
                    //value = d.Value;
                }
                TypeConverter converter = GetInnterConverter(value.GetType());
                return converter.ConvertFrom(context, culture, value);  
            }
            else
            {
                return null;
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            string s = (string)value;
           
            if (!string.IsNullOrEmpty(s))
            {
                TypeConverter converter = GetInnterConverter(destinationType);
                Type underType = Nullable.GetUnderlyingType(destinationType);
                return converter.ConvertTo(context, culture, value, underType); 
            }
            else
            {
                return null;
            }
        }
    }
}
