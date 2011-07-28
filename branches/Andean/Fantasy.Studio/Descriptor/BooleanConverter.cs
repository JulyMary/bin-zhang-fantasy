using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using Fantasy.Studio.Properties;

namespace Fantasy.Studio.Descriptor
{
	public class BooleanConverter : TypeConverter
	{


		internal static readonly string TrueText;
		internal static readonly string FalseText;
		
		static BooleanConverter()
		{
            TrueText = Resources.TrueText;
            FalseText = Resources.FalseText;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return (string)value == TrueText;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return (bool)value ? TrueText : FalseText;
		}
	}
}
