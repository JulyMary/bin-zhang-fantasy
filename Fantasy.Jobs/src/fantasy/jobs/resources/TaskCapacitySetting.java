package fantasy.jobs.resources;

import fantasy.xserialization.*;

@XSerializable(name = "task", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class TaskCapacitySetting
{
	@XAttribute(name = "namespace")
	private String privateNamespace;
	public final String getNamespace()
	{
		return privateNamespace;
	}
	public final void setNamespace(String value)
	{
		privateNamespace = value;
	}


	@XAttribute(name = "name")
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

	@XAttribute(name = "capacity")
	private int privateCapacity = Integer.MAX_VALUE;
	public final int getCapacity()
	{
		return privateCapacity;
	}
	public final void setCapacity(int value)
	{
		privateCapacity = value;
	}

}