package fantasy.jobs.resources;

import Fantasy.Configuration.*;

public class TaskRuntimeScheduleSettings extends RuntimeScheduleSetting
{

	private java.util.ArrayList<TaskRuntimeScheduleSetting> _tasks = new java.util.ArrayList<TaskRuntimeScheduleSetting>();
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	 //[XmlArray("tasks"), XmlArrayItem(ElementName = "task", java.lang.Class = typeof(TaskRuntimeScheduleSetting))]
	 public final java.util.ArrayList<TaskRuntimeScheduleSetting> getTasks()
	 {
		 return _tasks;
	 }
}