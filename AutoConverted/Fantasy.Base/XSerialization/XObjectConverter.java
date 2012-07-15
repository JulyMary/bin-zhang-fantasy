package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XConverter(TargetType=typeof(Object))]
public class XObjectConverter extends TypeConverter
{
	@Override
	public Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, java.lang.Class destinationType)
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

	@Override
	public Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
	{
		if (value == null)
		{
			return null;
		}
		else
		{
			return Convert.ChangeType(value, String.class);
		}
	}
}