package fantasy.jobs;

import java.io.*;

import fantasy.xserialization.*;

@XSerializable(name = "properties", namespaceUri=Consts.XNamespaceURI)
public class JobPropertyGroup implements IConditionalObject, Iterable<JobProperty>, Cloneable, Serializable
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -848315926536200958L;
	@XArray(serializer = JobPropertiesSerializer.class, items = { })
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

	public final java.util.Iterator<JobProperty> iterator()
	{
		return this._list.iterator();
	}

}