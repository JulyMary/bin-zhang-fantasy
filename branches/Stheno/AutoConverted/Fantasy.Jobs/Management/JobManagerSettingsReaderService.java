package Fantasy.Jobs.Management;

import Fantasy.ServiceModel.*;

public class JobManagerSettingsReaderService extends AbstractService implements IJobManagerSettingsReader
{

	@Override
	public void InitializeService()
	{
		super.InitializeService();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobManagerSettingsReader Members

	public final Object GetSetting(String name)
	{
		java.lang.Class t = JobManagerSettings.class;
		PropertyInfo prop = t.GetProperty(name);
		Object rs = prop.GetValue(JobManagerSettings.getDefault(), null);
		//object rs = t.InvokeMember(name, System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase, null, 
		//    JobManagerSettings.Default, new object[] {});
		return rs;
	}

	public final <T> T GetSetting(String name)
	{
		return (T)this.GetSetting(name);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}