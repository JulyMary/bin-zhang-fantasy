package fantasy.jobs.management;

import java.util.*;

import fantasy.jobs.*;

public interface IJobController
{

	void StartJob(JobMetaData job) throws Exception;

	void Resume(JobMetaData job) throws Exception;

	void Cancel(UUID id) throws Exception;

	void Suspend(UUID id) throws Exception;

	void UserPause(UUID id) throws Exception;




	int GetAvailableProcessCount() throws Exception;


	boolean IsJobProccessExisted(UUID id) throws Exception;

	JobMetaData[] GetRunningJobs() throws Exception;

	void RegisterJobEngine(IJobEngine engine) throws Exception; 

	void SuspendAll(boolean waitForExit) throws Exception; 
}