package Fantasy.Jobs;

public interface IJobEngine extends IServiceProvider
{
	Guid getJobId();
	String getJobDirectory();
	void Start(JobStartInfo startInfo);
	void Resume(JobStartInfo startInfo);
	void Terminate();
	void Suspend();
	void UserPause();
	void Fail();
	void Sleep(TimeSpan timeToSleep);
	void AddHandler(IJobEngineEventHandler handler);
	void RemoveHandler(IJobEngineEventHandler handler);

	void SaveStatusForError(RuntimeException error);
	void SaveStatus();
}