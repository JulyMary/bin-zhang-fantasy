package fantasy.jobs.management;

import java.util.*;

import fantasy.*;


public interface IJobQueue
{
	Iterable<JobMetaData> getUnterminates();

	

	JobMetaData FindJobMetaDataById(UUID id) throws Exception;

	boolean IsTerminated(UUID id) throws Exception;

	List<JobMetaData> FindTerminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception;
	List<JobMetaData> FindUnterminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception;



	JobMetaData CreateJobMetaData();

	void Add(JobMetaData job) throws Exception;
	void UpdateState(JobMetaData job, boolean isStart) throws Exception;
	void Archive(JobMetaData job) throws Exception;
	void Resume(UUID id) throws Exception;
	void Cancel(UUID id) throws Exception;
	void Suspend(UUID id) throws Exception;
	void UserPause(UUID id) throws Exception;
	
	
	void addListener(IJobQueueListener listener);
	void removeListener(IJobQueueListener listener);



	String[] GetAllApplications();

	String[] GetAllUsers();
}