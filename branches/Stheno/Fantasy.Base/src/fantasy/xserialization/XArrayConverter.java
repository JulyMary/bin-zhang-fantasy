package fantasy.xserialization;

import java.lang.reflect.*;

import org.apache.commons.codec.binary.Base64;

import fantasy.*;

@SuppressWarnings("rawtypes") 
public class XArrayConverter implements ITypeConverter
{
	@Override
	public Object convertFrom(Object value) throws Exception
	{

		java.lang.Class et = value.getClass().getComponentType();
		if(et == byte.class || et == Byte.class)
		{
			return Base64.encodeBase64String((byte[])value);
		}
		else
		{

			StringBuilder rs = new StringBuilder();

			ITypeConverter ec = XHelper.getDefault().CreateXConverter(et);
			for (int i = 0; i < Array.getLength(value); i ++)
			{
				Object element = Array.get(value, i);
				
				if (rs.length() > 0)
				{
					rs.append(' ');
				}
				rs.append(ec.convertFrom(element));

			}

			return rs.toString();
		}
	}

	@Override
	public Object convertTo(Object value,Class destinationType) throws Exception
	{



		if (value == null)
		{
			return null;
		}


		java.lang.Class et = destinationType.getComponentType();

		if(et == byte.class || et ==Byte.class)
		{
			return Base64.decodeBase64((String)value);
		}
		else
		{

			ITypeConverter ec = XHelper.getDefault().CreateXConverter(et);
			if (StringUtils2.isNullOrEmpty((String)value))
			{
				return Array.newInstance(et, 0);
			}
			else
			{
				String[] strs = ((String)value).split("\\s", -1);
				Object rs = Array.newInstance(et, strs.length);
				for (int i = 0; i < strs.length; i++)
				{
					Object element = ec.convertTo(strs[i], et);
					Array.set(rs, i, element);
				}

				return rs;
			}
		}

	}
}