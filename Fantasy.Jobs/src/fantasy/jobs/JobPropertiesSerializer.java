package fantasy.jobs;

import org.jdom2.*;

import fantasy.xserialization.*;
import fantasy.*;


@SuppressWarnings("rawtypes")
public class JobPropertiesSerializer implements IXCollectionSerializer
{

	public final void Save(IServiceProvider context, Element element,  Iterable collection) throws Exception
	{

		Namespace ns = element.getNamespace();

		for (Object item : collection)
		{
			
			JobProperty prop = (JobProperty)item;
			
			Element childElement = new Element(prop.getName(),ns);
			element.addContent(childElement);
			prop.Save(context, childElement);
		}
	}

	public final Iterable Load(IServiceProvider context, Element element) throws Exception
	{
		java.util.ArrayList<JobProperty> rs = new java.util.ArrayList<JobProperty>();
		for (Element childElement : element.getChildren())
		{
			JobProperty item = new JobProperty();
			item.Load(context, childElement);
			rs.add(item);
		}

		return rs;
	}
}