package fantasy.jobs.management;



public interface IJobManagerSettingsReader
{
	
	<T> T getSetting(Class<T> type, String name);
}