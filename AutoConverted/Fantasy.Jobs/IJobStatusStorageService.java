package Fantasy.Jobs;

import Fantasy.ServiceModel.*;

public interface IJobStatusStorageService
{
	void Save(Stream content);
	Stream Load();
	Stream LoadBackup();

	boolean getExists();
}