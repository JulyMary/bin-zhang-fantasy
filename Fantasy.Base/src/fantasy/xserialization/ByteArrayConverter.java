package fantasy.xserialization;

import fantasy.*;

import org.apache.commons.codec.binary.Base64;
@SuppressWarnings("rawtypes")
public class ByteArrayConverter implements ITypeConverter {

	@Override
	public Object convertFrom(Object value) throws Exception {
		byte[] bytes = (byte[])value;
		
		return Base64.encodeBase64String(bytes);
		
	}

	@Override
	public Object convertTo(Object value,  Class destinationType)
			throws Exception {
		if(value == null)
		{
			return null;
		}
		else
		{
			return Base64.decodeBase64((String)value);
		}
	}

}
