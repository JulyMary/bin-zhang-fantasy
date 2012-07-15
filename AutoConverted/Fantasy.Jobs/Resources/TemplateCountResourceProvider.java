package Fantasy.Jobs.Resources;

import Fantasy.Jobs.Management.*;

public class TemplateCountResourceProvider extends ConcurrentCountResourceProviderBase
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
		JobManagerSettings.Default.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsPropertyChanged);
	}

	@Override
	protected int GetMaxCount(String key)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from t in JobManagerSettings.getDefault().getConcurrentTemplateCount().getTemplates() where key.equals(t.getName()) select t;
		TemplateCountSetting setting = query.SingleOrDefault();

		return setting != null ? setting.getCount() : JobManagerSettings.getDefault().getConcurrentTemplateCount().getCount();

	}

	private void SettingsPropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName.equals("ConcurrentTemplateCount"))
		{
			this.TryRevoke();
		}

		this.OnAvailable();
	}

	@Override
	protected String GetKey(ResourceParameter parameter)
	{
		return parameter.getValues().get("template");
	}



}