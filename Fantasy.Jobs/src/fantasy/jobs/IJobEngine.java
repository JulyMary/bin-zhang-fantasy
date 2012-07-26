package fantasy.jobs;

import java.util.UUID;

import org.joda.time.Duration;

import fantasy.*;

public interface IJobEngine extends IServiceProvider
{
	UUID getJobId() throws Exception;
	String getJobDirectory() throws Exception;
	void Start(JobStartInfo startInfo) throws Exception;
	void Resume(JobStartInfo startInfo) throws Exception;
	void Terminate() throws Exception;
	void Suspend() throws Exception;
	void UserPause() throws Exception;
	void Fail() throws Exception;
	void Sleep(Duration timeToSleep) throws Exception;
	void AddHandler(IJobEngineEventHandler handler) throws Exception;
	void RemoveHandler(IJobEngineEventHandler handler) throws Exception;

	void SaveStatusForError(Exception error) throws Exception;
	void SaveStatus() throws Exception;
}