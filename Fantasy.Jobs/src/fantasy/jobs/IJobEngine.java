package fantasy.jobs;

import java.util.UUID;

import org.joda.time.Duration;

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
	void Sleep(Duration timeToSleep);
	void AddHandler(IJobEngineEventHandler handler);
	void RemoveHandler(IJobEngineEventHandler handler);

	void SaveStatusForError(RuntimeException error);
	void SaveStatus();
}