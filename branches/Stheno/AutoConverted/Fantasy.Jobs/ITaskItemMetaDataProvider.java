package Fantasy.Jobs;

import Fantasy.IO.*;

public interface ITaskItemMetaDataProvider
{
	String[] GetNames(TaskItem item);

	String GetData(TaskItem item, String name);
}