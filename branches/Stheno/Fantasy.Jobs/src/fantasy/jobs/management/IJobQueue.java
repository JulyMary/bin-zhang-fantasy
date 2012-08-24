package fantasy.jobs.management;

import java.util.*;

import fantasy.*;


public interface IJobQueue
{
	Iterable<JobMetaData> getUnterminates();

	

	JobMetaData findJobMetaDataById(UUID id) throws Exception;

	boolean isTerminated(UUID id) throws Exception;

	List<JobMetaData> findTerminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception;
	List<JobMetaData> findUnterminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception;



	JobMetaData createJobMetaData();

	void add(JobMetaData job) throws Exception;
	void updateState(JobMetaData job, boolean isStart) throws Exception;
	void archive(JobMetaData job) throws Exception;
	void resume(UUID id) throws Exception;
	void cancel(UUID id) throws Exception;
	void suspend(UUID id) throws Exception;
	void userPause(UUID id) throws Exception;
	
	
	void addListener(IJobQueueListener listener);
	void removeListener(IJobQueueListener listener);
	
	int getTerminatedCount() throws Exception;
	
	int getUnterminatedCount() throws Exception;
	



	String[] getAllApplications();

	String[] getAllUsers();
}