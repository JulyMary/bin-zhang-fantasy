package fantasy.jobs;

import java.io.*;

public interface IJobStatusStorageService
{
	void Save(InputStream stream );
	InputStream Load();
	InputStream LoadBackup();

	boolean getExists();
}