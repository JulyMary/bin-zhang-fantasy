package fantasy.jobs.management;



public interface IJobManagerSettingsReader
{
	
	<T> T GetSetting(Class<T> type, String name);
}