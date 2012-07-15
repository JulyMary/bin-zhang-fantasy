package Fantasy.Jobs.Resources;

import Fantasy.Jobs.Management.*;

public class JobHostCountResourceProvider extends ConcurrentCountResourceProviderBase
{


	@Override
	public boolean CanHandle(String name)
	{
		return String.equals("RunJob", name, StringComparison.OrdinalIgnoreCase);
	}

	@Override
	public void Initialize()
	{

//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsChanged);
	}

	private void SettingsChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName.equals("JobProcessCount"))
		{
			this.TryRevoke();
		}
		this.OnAvailable();
	}


	@Override
	protected int GetMaxCount(String key)
	{
		 return JobManagerSettings.getDefault().getJobProcessCount();
	}

	@Override
	protected String GetKey(ResourceParameter parameter)
	{
		return "";
	}



}