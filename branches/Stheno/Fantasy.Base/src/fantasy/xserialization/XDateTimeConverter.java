package fantasy.xserialization;

import java.text.*;

import fantasy.*;

@SuppressWarnings("rawtypes") 
public class XDateTimeConverter implements ITypeConverter
{
	
	
	private static SimpleDateFormat _roundTripFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSSSSS"); 
	private static SimpleDateFormat _localFormat = new SimpleDateFormat();
	
	@Override
	public Object convertFrom(Object value)
	{
		
		return _roundTripFormat.format(value);
	}

	@Override
	public Object convertTo(Object value, java.lang.Class destinationType) throws Exception
	{
		java.util.Date rs = null;
		String text = (String)value;
		
		if (!StringUtils2.isNullOrEmpty(text))
		{
			boolean parsed = false;
			try
			{
			    rs = _roundTripFormat.parse(text);
			    parsed = true;
			}
			catch(ParseException error)
			{
				
			}
			
			if(!parsed)
			{
				rs = _localFormat.parse(text);
			}
			
			
			

		}
		return rs;
	}
}