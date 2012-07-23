package fantasy.jobs;

import java.io.Serializable;
import org.jdom2.Element;

import fantasy.xserialization.*;
import fantasy.*;

@XSerializable(name = "items",namespaceUri=Consts.XNamespaceURI)
@SuppressWarnings("rawtypes") 
public class TaskItemGroup implements IConditionalObject, Iterable<TaskItem>, Cloneable, Serializable
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 7185313619078834413L;
	@XArray(serializer=ItemsSerializer.class, items={})
	private java.util.ArrayList<TaskItem> _list = new java.util.ArrayList<TaskItem>();

	public final TaskItem[] toArray()
	{
		return this._list.toArray(new TaskItem[]{});
	}

	public final int getCount()
	{
		return this._list.size();
	}

	public final TaskItem AddNewItem(String name, String category)
	{
		TaskItem tempVar = new TaskItem();
		tempVar.setName(name);
		tempVar.setCategory(category);
		TaskItem rs = tempVar;
		this._list.add(rs);
		return rs;
	}

	public final void Clear()
	{
		this._list.clear();
	}

	public final Object clone()
	{
		TaskItemGroup rs = new TaskItemGroup();
		rs.setCondition(this.getCondition());
		for (TaskItem item : this._list)
		{
			rs._list.add((TaskItem)item.clone());
		}
		return rs;
	}

	public final void RemoveItem(TaskItem itemToRemove)
	{
		this._list.remove(itemToRemove);
	}

	public final void RemoveAt(int index)
	{
		this._list.remove(index);
	}

	public final int IndexOf(TaskItem item)
	{
		return _list.indexOf(item);
	}

	@XAttribute(name = "condition")
	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}


	private static class ItemsSerializer implements IXCollectionSerializer
	{


		public final void Save(IServiceProvider context, Element element, Iterable collection)
		{

			for (Object o : collection)
			{
				TaskItem item = (TaskItem)o;
				Element childElement = new Element(item.getCategory(), element.getNamespace());
				element.addContent(childElement);
				item.Save(context, childElement);
			}
		}

		public final Iterable Load(IServiceProvider context, Element element)
		{
			java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
			for (Element childElement : element.getChildren())
			{
				TaskItem item = new TaskItem();
				item.Load(context, childElement);
				rs.add(item);
			}

			return rs;
		}

	}


	@Override
	public final java.util.Iterator<TaskItem> iterator()
	{
		return this._list.iterator();
	}

	
	

}