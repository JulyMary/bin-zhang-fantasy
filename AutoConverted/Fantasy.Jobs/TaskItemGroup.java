package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("items",NamespaceUri=Consts.XNamespaceURI)]
public class TaskItemGroup implements IConditionalObject, Iterable<TaskItem>, Cloneable, Serializable
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Serializer=typeof(ItemsSerializer))]
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

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("condition")]
	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IEnumerable Members

	private java.util.Iterator GetEnumerator()
	{
		return this.iterator();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


	private static class ItemsSerializer extends IXCollectionSerializer
	{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
			///#region IXCollectionSerializer Members

		public final void Save(IServiceProvider context, XElement element, Iterable collection)
		{

			for (TaskItem item : collection)
			{
				XElement childElement = new XElement(element.getName().Namespace + item.getCategory());
				element.Add(childElement);
				item.Save(context, childElement);
			}
		}

		public final Iterable Load(IServiceProvider context, XElement element)
		{
			java.util.ArrayList rs = new java.util.ArrayList();
			for (XElement childElement : element.Elements())
			{
				TaskItem item = new TaskItem();
				item.Load(context, childElement);
				rs.add(item);
			}

			return rs;
		}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
			///#endregion
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IEnumerable<TaskItem> Members

	public final java.util.Iterator<TaskItem> GetEnumerator()
	{
		return this._list.iterator();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}