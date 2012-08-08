package fantasy.jobs.resources;

import fantasy.xserialization.*;


@XSerializable(name="tasks", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class TaskCapacitySettings
{
	
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


	@XArray(items=@XArrayItem(name="task", type=TaskCapacitySetting.class))
	private java.util.ArrayList<TaskCapacitySetting> _tasks = new java.util.ArrayList<TaskCapacitySetting>();

	public final java.util.ArrayList<TaskCapacitySetting> getTasks()
	{
		return _tasks;
	}

	
}