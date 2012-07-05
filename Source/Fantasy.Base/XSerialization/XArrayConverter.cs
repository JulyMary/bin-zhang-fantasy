using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fantasy.XSerialization
{
    [XConverter(TargetType = typeof(Array))]
    public class XArrayConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            StringBuilder rs = new StringBuilder();
            Type et = value.GetType().GetElementType();
            TypeConverter ec = XHelper.Default.CreateXConverter(et);
            foreach (object element in (Array)value)
            {
                if (rs.Length > 0)
                {
                    rs.Append(' ');
                }
                rs.Append(ec.ConvertFrom(element));
 
            }

            return rs.ToString();
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                return null;
            }
            
           
            Type et = destinationType.GetElementType();
            TypeConverter ec = XHelper.Default.CreateXConverter(et);
            if (string.IsNullOrEmpty((string)value))
            {
                return Array.CreateInstance(et, 0);
            }
            else
            {
                string[] strs = ((string)value).Split(' ');
                Array rs = Array.CreateInstance(et, strs.Length);
                for (int i = 0; i < strs.Length; i++)
                {
                    object element = ec.ConvertTo(strs[i], et);
                    rs.SetValue(element, i);
                }

                return rs;
            }

        }
    }
}
