package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XConverter(TargetType = typeof(Array))]
public class XArrayConverter extends TypeConverter
{
	@Override
	public Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
	{
		StringBuilder rs = new StringBuilder();
		java.lang.Class et = value.getClass().GetElementType();
		TypeConverter ec = XHelper.getDefault().CreateXConverter(et);
		for (Object element : (Array)value)
		{
			if (rs.length() > 0)
			{
				rs.append(' ');
			}
			rs.append(ec.ConvertFrom(element));

		}

		return rs.toString();
	}

	@Override
	public Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, java.lang.Class destinationType)
	{
		if (value == null)
		{
			return null;
		}


		java.lang.Class et = destinationType.GetElementType();
		TypeConverter ec = XHelper.getDefault().CreateXConverter(et);
		if (DotNetToJavaStringHelper.isNullOrEmpty((String)value))
		{
			return Array.CreateInstance(et, 0);
		}
		else
		{
			String[] strs = ((String)value).split("[ ]", -1);
			Array rs = Array.CreateInstance(et, strs.length);
			for (int i = 0; i < strs.length; i++)
			{
				Object element = ec.ConvertTo(strs[i], et);
				rs.SetValue(element, i);
			}

			return rs;
		}

	}
}