package fantasy.jobs.resources;

import fantasy.xserialization.*;



@XSerializable(name="TaskRunningTimeSettings", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class TaskRunningTimeSettings extends RunningTimeSetting
{

	@XArray(items=@XArrayItem(name="task", type=TaskRunningTimeSetting.class))
	private java.util.ArrayList<TaskRunningTimeSetting> _tasks = new java.util.ArrayList<TaskRunningTimeSetting>();
	public final java.util.ArrayList<TaskRunningTimeSetting> getTasks()
	{
		return _tasks;
	}
}