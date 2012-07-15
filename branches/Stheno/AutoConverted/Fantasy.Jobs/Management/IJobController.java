package Fantasy.Jobs.Management;

public interface IJobController
{

	void StartJob(JobMetaData job);

	void Resume(JobMetaData job);

	void Cancel(Guid id);

	void Suspend(Guid id);

	void UserPause(Guid id);




	int GetAvailableProcessCount();


	boolean IsJobProccessExisted(Guid id);

	JobMetaData[] GetRunningJobs();

	void RegisterJobEngine(IJobEngine engine);

	void SuspendAll(boolean waitForExit);
}