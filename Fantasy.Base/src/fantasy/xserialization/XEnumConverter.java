package fantasy.xserialization;

import fantasy.*;

@SuppressWarnings("rawtypes") 
public class XEnumConverter implements ITypeConverter
{
	@Override
	public Object convertFrom(Object value)
	{

		return value.toString();
	}

	@SuppressWarnings("unchecked")
	@Override
	public Object convertTo(Object value, java.lang.Class destinationType)
	{
		return Enum.valueOf(destinationType, (String)value);
		

	}
}