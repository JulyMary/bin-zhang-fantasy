package fantasy.jobs.management;

import fantasy.xserialization.*;
import fantasy.*;
import org.jdom2.*;
import org.jdom2.output.XMLOutputter;

@SuppressWarnings("unchecked")
public final class JobServiceUtils
{

	

	
	public static <T> T GetSettings(IJobService service, Class<T> type) throws Exception
	{
		
		XSerializer ser = new XSerializer(type);
		
		
		
		String xml = service.GetSettings(type.getName());
		Element ele = JDomUtils.parseElement(xml);
		
		return (T)ser.deserialize(ele);
	}

	public static void SetSettings(IJobService service, Object settings) throws Exception
	{
		if(settings == null)
		{
			throw new IllegalArgumentException("settings");
		}
		
		XSerializer ser = new XSerializer(settings.getClass());
		
		Element ele = ser.serialize(settings);
		
		String xml = new XMLOutputter().outputString(ele);
	
		
		service.SetSettings(settings.getClass().getName(), xml);
	}
}