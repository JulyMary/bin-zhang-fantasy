using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public class BusinessDataTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }


        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is BusinessClass)
            {
                return ((BusinessClass)value).FullName;
            }
            else if (value is BusinessEnum)
            {
                return ((BusinessEnum)value).FullName;
            }
            else if (value is BusinessDataType)
            {
                return ((BusinessDataType)value).Name;
            }
            else
            {
                return null;
            }

        }
    }
}
