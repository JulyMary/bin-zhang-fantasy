package Fantasy.XSerialization;

public class NullableConverter extends TypeConverter
{

	private boolean IsNullableType(java.lang.Class t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == .class;
	}

	private TypeConverter GetInnterConverter(java.lang.Class type)
	{
		java.lang.Class destType = IsNullableType(type) ? Nullable.GetUnderlyingType(type): type;


		return XHelper.getDefault().CreateXConverter(destType);
	}

	@Override
	public Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
	{

		if (value != null)
		{
			if (IsNullableType(value.getClass()))
			{

				//dynamic d = value;
				value = value.getClass().GetProperty("Value").GetValue(value, null);
				//value = d.Value;
			}
			TypeConverter converter = GetInnterConverter(value.getClass());
			return converter.ConvertFrom(context, culture, value);
		}
		else
		{
			return null;
		}
	}

	@Override
	public Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, java.lang.Class destinationType)
	{
		String s = (String)value;

		if (!DotNetToJavaStringHelper.isNullOrEmpty(s))
		{
			TypeConverter converter = GetInnterConverter(destinationType);
			java.lang.Class underType = Nullable.GetUnderlyingType(destinationType);
			return converter.ConvertTo(context, culture, value, underType);
		}
		else
		{
			return null;
		}
	}
}