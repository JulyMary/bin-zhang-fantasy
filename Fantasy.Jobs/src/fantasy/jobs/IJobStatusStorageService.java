package fantasy.jobs;

import fantasy.servicemodel.*;

public interface IJobStatusStorageService
{
	void Save(Stream content);
	Stream Load();
	Stream LoadBackup();

	boolean getExists();
}