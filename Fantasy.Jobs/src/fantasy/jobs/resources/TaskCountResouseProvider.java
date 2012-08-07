package fantasy.jobs.resources;

import fantasy.jobs.management.*;

public class TaskCountResouseProvider extends CapacityResourceProviderBase
{
	@Override
	public boolean CanHandle(String name)
	{
		return String.equals("RunTask", name, StringComparison.OrdinalIgnoreCase);
	}

	@Override
	protected String GetKey(ResourceParameter parameter)
	{
		return parameter.getValues().get("namespace") + "|" + parameter.getValues().get("taskname");
	}

	@Override
	public void Initialize()
	{
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsPropertyChanged);
	}

	private void SettingsPropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName.equals("ConcurrentTaskCount"))
		{
			this.TryRevoke();
		}

		this.OnAvailable();
	}


	@Override
	protected int GetMaxCount(String key)
	{
		String[] strs = key.split(new char[] {'|'}, 2);

		String ns = strs[0];
		String name = strs[1];
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from s in JobManagerSettings.getDefault().getConcurrentTaskCount().getTasks() where ns.equals(s.Namespace) && name.equals(s.getName()) select s;
		TaskCountSetting setting = query.SingleOrDefault();

		return setting != null ? setting.getCount() : JobManagerSettings.getDefault().getConcurrentTaskCount().getCount();
	}

}