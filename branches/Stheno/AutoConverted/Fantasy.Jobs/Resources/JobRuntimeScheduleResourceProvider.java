package Fantasy.Jobs.Resources;

import Fantasy.Jobs.Scheduling.*;
import Fantasy.Jobs.Management.*;

public class JobRuntimeScheduleResourceProvider extends RuntimeScheduleResourceProviderBase
{


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
		JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(JobManagerSettings_PropertyChanged);
	}

	private void JobManagerSettings_PropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if(e.PropertyName.equals("JobRuntimeSchedule"))
		{
			this.Reschedule();

		}
	}

	@Override
	protected RuntimeScheduleSetting GetSetting(String id)
	{
		return JobManagerSettings.getDefault().getJobRuntimeSchedule();
	}

	@Override
	protected String GetResourceId(ResourceParameter parameter)
	{
		return "";
	}
}