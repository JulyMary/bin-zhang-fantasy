package fantasy.xserialization;
import fantasy.*;

@SuppressWarnings("rawtypes") 
public class XUUIDConverter implements ITypeConverter
{
	@Override
	public Object convertFrom(Object value)
	{

		return value.toString();
	}

	@Override
	public Object convertTo(Object value, java.lang.Class destinationType)
	{
		return java.util.UUID.fromString((String)value);

	}
}