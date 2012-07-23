package fantasy.jobs;

import java.io.*;

public interface IJobStatusStorageService
{
	void Save(InputStream stream ) throws Exception;
	InputStream Load() throws Exception;;
	InputStream LoadBackup() throws Exception;;

	boolean getExists() throws Exception;
}