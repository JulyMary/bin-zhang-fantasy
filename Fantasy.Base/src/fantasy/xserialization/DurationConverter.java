package fantasy.xserialization;

import org.joda.time.Duration;

import fantasy.*;

@SuppressWarnings("rawtypes") 
public class DurationConverter implements ITypeConverter {

	@Override
	public Object convertFrom(Object value) throws Exception {
		return value.toString();
	}

	@Override
	public Object convertTo(Object value, Class destinationType)
			throws Exception {
		return Duration.parse((String)value);
	}

}
