package fantasy.jobs.resources;

import Fantasy.Configuration.*;

public class TaskCountSettings
{
	public TaskCountSettings()
	{
		setCount(Integer.MAX_VALUE);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlAttribute("count")]
	private int privateCount;
	public final int getCount()
	{
		return privateCount;
	}
	public final void setCount(int value)
	{
		privateCount = value;
	}


	private java.util.ArrayList<TaskCountSetting> _tasks = new java.util.ArrayList<TaskCountSetting>();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlArray("tasks"), XmlArrayItem(java.lang.Class=typeof(TaskCountSetting), ElementName="task")]
	public final java.util.ArrayList<TaskCountSetting> getTasks()
	{
		return _tasks;
	}

	@Override
	public boolean equals(Object obj)
	{
		return ComparsionHelper.DeepEquals(this, obj);
	}

	@Override
	public int hashCode()
	{
		return super.hashCode();
	}
}