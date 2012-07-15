package fantasy.xserialization;

import fantasy.*;

public class PrimitiveConverter implements ITypeConverter {

	@Override
	public Object convertFrom(Object value) throws Exception {
		
		return value.toString();
		
		
	}

	@Override
	public Object convertTo(Object value, @SuppressWarnings("rawtypes") Class t)
			throws Exception {
		
		String s = (String)value;
		
		if(t == int.class || t == Integer.class)
		{
			return Integer.parseInt((String)value);
		}
		else if(t==byte.class || t == Byte.class)
		{
			return Byte.parseByte(s);
		}
		else if(t == short.class || t == Short.class)	
		{
			return Short.parseShort(s);
		}
		else if(t == long.class || t == Long.class)
		{
		    return Long.parseLong(s);
		}
		else if(t == float.class || t == Float.class)
		{
			return Float.parseFloat(s); 
				
		}
		else if(t == double.class || t == Double.class)
		{
			return Double.parseDouble(s);
		}
		else if(t == boolean.class || t == Boolean.class)
		{
			return Boolean.parseBoolean(s);
		}
		else if(t == char.class || t == Character.class)
		{
			
			return s.length() > 0 ? s.charAt(0) : null;
		}
		else if(t == String.class)
		{
			return s;
		}
		
		throw new IllegalArgumentException("t");
		
	}

}
