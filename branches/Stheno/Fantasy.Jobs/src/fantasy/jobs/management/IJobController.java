package fantasy.jobs.management;

import java.util.*;

import fantasy.jobs.*;

public interface IJobController
{

	void StartJob(JobMetaData job);

	void Resume(JobMetaData job);

	void Cancel(UUID id);

	void Suspend(UUID id);

	void UserPause(UUID id);




	int GetAvailableProcessCount();


	boolean IsJobProccessExisted(UUID id);

	JobMetaData[] GetRunningJobs();

	void RegisterJobEngine(IJobEngine engine);

	void SuspendAll(boolean waitForExit);
}