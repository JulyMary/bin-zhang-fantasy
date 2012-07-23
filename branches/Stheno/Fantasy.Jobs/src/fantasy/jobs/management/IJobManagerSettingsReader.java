package fantasy.jobs.management;



public interface IJobManagerSettingsReader
{
	Object getSetting(String name);
	<T> T GetSetting(Class<T> type, String name);
}