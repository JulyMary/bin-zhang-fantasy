package fantasy.xserialization;



import fantasy.*;

@SuppressWarnings("rawtypes") 
public class TimeSpanConverter implements ITypeConverter {

	@Override
	public Object convertFrom(Object value) throws Exception {
		return value.toString();
	}

	@Override
	public Object convertTo(Object value, Class destinationType)
			throws Exception {
		return TimeSpan.parse((String)value);
	}

}
