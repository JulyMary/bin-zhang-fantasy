package Fantasy.Jobs;

import Fantasy.XSerialization.*;

public class StateBagItemsSerializer extends IXCollectionSerializer
{
	public final void Save(IServiceProvider context, XElement element, Iterable collection)
	{

		for (StateBagItem item : collection)
		{
			XElement childElement = new XElement(element.getName().Namespace + item.getName());
			element.Add(childElement);
			((IXSerializable)item).Save(context, childElement);
		}
	}

	public final Iterable Load(IServiceProvider context, XElement element)
	{
		java.util.ArrayList rs = new java.util.ArrayList();
		for (XElement childElement : element.Elements())
		{
			StateBagItem item = new StateBagItem();
			((IXSerializable)item).Load(context, childElement);
			rs.add(item);
		}

		return rs;
	}
}