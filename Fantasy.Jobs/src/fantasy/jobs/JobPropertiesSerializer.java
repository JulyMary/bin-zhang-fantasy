package fantasy.jobs;

import fantasy.xserialization.*;

public class JobPropertiesSerializer extends IXCollectionSerializer
{

	public final void Save(IServiceProvider context, XElement element, Iterable collection)
	{


		for (JobProperty item : collection)
		{
			XElement childElement = new XElement(element.getName().Namespace + item.getName());
			element.Add(childElement);
			item.Save(context, childElement);
		}
	}

	public final Iterable Load(IServiceProvider context, XElement element)
	{
		java.util.ArrayList rs = new java.util.ArrayList();
		for (XElement childElement : element.Elements())
		{
			JobProperty item = new JobProperty();
			item.Load(context, childElement);
			rs.add(item);
		}

		return rs;
	}
}