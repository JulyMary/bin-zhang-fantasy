package fantasy.jobs.resources;

import Fantasy.Jobs.Management.*;

public class TemplateRuntimeScheduleResourceProvider extends RuntimeScheduleResourceProviderBase
{
	public TemplateRuntimeScheduleResourceProvider()
	{

	}
	@Override
	protected boolean InternalCanHandle(String name)
	{
		return String.equals("RunJob", name, StringComparison.OrdinalIgnoreCase);
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
		if (e.PropertyName.equals("TemplateRuntimeSchedule"))
		{
			this.Reschedule();
		}
	}

	@Override
	protected RuntimeScheduleSetting GetSetting(String id)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		RuntimeScheduleSetting rs = JobManagerSettings.getDefault().getTemplateRuntimeSchedule().getTemplates().Find(s => String.equals(s.getName(), id, StringComparison.OrdinalIgnoreCase));
		if (rs == null)
		{
			rs = JobManagerSettings.getDefault().getTemplateRuntimeSchedule();
		}

		return rs;
	}

	@Override
	protected String GetResourceId(ResourceParameter parameter)
	{
		return parameter.getValues().get("template");
	}
}