package fantasy.jobs;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("properties", NamespaceUri=Consts.XNamespaceURI)]
public class JobPropertyGroup implements IConditionalObject, Iterable, Cloneable, Serializable
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Serializer = typeof(JobPropertiesSerializer))]
	private java.util.ArrayList<JobProperty> _list = new java.util.ArrayList<JobProperty>();

	public final JobProperty[] toArray()
	{
		return this._list.toArray(new JobProperty[]{});
	}

	public final int getCount()
	{
		return this._list.size();
	}

	public final JobProperty AddNewItem(String name)
	{
		JobProperty tempVar = new JobProperty();
		tempVar.setName(name);
		JobProperty rs = tempVar;
		this._list.add(rs);
		return rs;
	}

	public final void Clear()
	{
		this._list.clear();
	}

	public final Object clone()
	{
		JobPropertyGroup rs = new JobPropertyGroup();
		rs.setCondition(this.getCondition());
		for (JobProperty prop : this._list)
		{
			this._list.add((JobProperty)prop.clone());
		}

		return rs;
	}

	public final void RemoveItem(JobProperty itemToRemove)
	{
		this._list.remove(itemToRemove);
	}

	public final void RemoveAt(int index)
	{
		this._list.remove(index);
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

	public final java.util.Iterator GetEnumerator()
	{
		return this._list.iterator();
	}

}