package fantasy.jobs;


public interface ITaskItemMetaDataProvider
{
	String[] GetNames(TaskItem item);

	String GetData(TaskItem item, String name);
}