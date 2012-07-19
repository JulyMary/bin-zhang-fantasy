package fantasy.jobs;

import java.util.UUID;

import fantasy.*;

public interface IJobEngine extends IServiceProvider
{
	UUID getJobId();
	String getJobDirectory();
	void Start(JobStartInfo startInfo);
	void Resume(JobStartInfo startInfo);
	void Terminate();
	void Suspend();
	void UserPause();
	void Fail();
	void Sleep(long timeToSleep);
	void AddHandler(IJobEngineEventHandler handler);
	void RemoveHandler(IJobEngineEventHandler handler);

	void SaveStatusForError(RuntimeException error);
	void SaveStatus();
}