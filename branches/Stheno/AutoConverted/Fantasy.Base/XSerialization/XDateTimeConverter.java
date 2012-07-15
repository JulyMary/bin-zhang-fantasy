package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XConverter(TargetType = typeof(java.util.Date))]
public class XDateTimeConverter extends TypeConverter
{
	@Override
	public Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
	{

		return ((java.util.Date)value).ToString("O");
	}

	@Override
	public Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, java.lang.Class destinationType)
	{
		java.util.Date rs = java.util.Date.getMinValue();
		String text = (String)value;
		if (!DotNetToJavaStringHelper.isNullOrEmpty(text))
		{
			RefObject<java.util.Date> tempRef_rs = new RefObject<java.util.Date>(rs);
			boolean tempVar = !java.util.Date.TryParseExact(text, "O", null, DateTimeStyles.None, tempRef_rs);
				rs = tempRef_rs.argvalue;
			if (tempVar)
			{
				rs = new java.util.Date(java.util.Date.parse(text));
			}

		}
		return rs;
	}
}