package fantasy.jobs;

import org.jdom2.Element;

import fantasy.xserialization.*;
import fantasy.*;

@SuppressWarnings("rawtypes")
public class StateBagItemsSerializer implements IXCollectionSerializer
{
	public final void Save(IServiceProvider context, Element element,  Iterable collection) throws Exception
	{

		for (Object o : collection)
		{
			StateBagItem item = (StateBagItem)o;
			Element childElement = new Element(item.getName(), element.getNamespace());
			element.addContent(childElement);
			((IXSerializable)item).Save(context, childElement);
		}
	}

	public final Iterable Load(IServiceProvider context, Element element) throws Exception
	{
		java.util.ArrayList<StateBagItem> rs = new java.util.ArrayList<StateBagItem>();
		for (Element childElement : element.getChildren())
		{
			StateBagItem item = new StateBagItem();
			((IXSerializable)item).Load(context, childElement);
			rs.add(item);
		}

		return rs;
	}
}