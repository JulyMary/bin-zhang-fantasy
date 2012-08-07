package fantasy.jobs.resources;

import Fantasy.Jobs.Management.*;

public class TaskRuntimeScheduleResourceProvider extends RuntimeScheduleResourceProviderBase
{
	@Override
	protected boolean InternalCanHandle(String name)
	{
		return String.equals("RunTask", name, StringComparison.OrdinalIgnoreCase);
	}

	@Override
	public void Initialize()
	{
		super.Initialize();
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsPropertyChanged);
	}


	private void SettingsPropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName.equals("TaskRuntimeSchedule"))
		{
			this.Reschedule();
		}
	}

	@Override
	protected RuntimeScheduleSetting GetSetting(String id)
	{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		RuntimeScheduleSetting rs = JobManagerSettings.getDefault().getTaskRuntimeSchedule().getTasks().Find(setting => String.equals(String.format("%1$s|%2$s", setting.Namespace, setting.getName()), id, StringComparison.OrdinalIgnoreCase));
		if (rs == null)
		{
			rs = JobManagerSettings.getDefault().getTaskRuntimeSchedule();
		}
		return rs;
	}

	@Override
	protected String GetResourceId(ResourceParameter parameter)
	{
		return String.format("%1$s|%2$s", parameter.getValues().get("namespace"), parameter.getValues().get("taskname"));
	}
}