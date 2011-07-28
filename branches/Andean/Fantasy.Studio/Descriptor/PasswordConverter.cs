using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Fantasy.Studio.Descriptor
{
	public class PasswordConverter : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value != null)
			{
				return new string('\u25cf', ((string)value).Length);
			}
			else
			{
				return value;
			}
		}
	}
}
