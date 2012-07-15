package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XConverter(TargetType = typeof(Guid))]
public class XGuidConverter extends TypeConverter
{
	@Override
	public Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
	{

		return value.toString();
	}

	@Override
	public Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, java.lang.Class destinationType)
	{
		return new Guid((String)value);

	}
}