package fantasy.jobs.management;

import java.util.*;




public interface IJobQueue
{
	Iterable<JobMetaData> getUnterminates() throws Exception;

	

	JobMetaData findJobMetaDataById(UUID id) throws Exception;

	boolean isTerminated(UUID id) throws Exception;

	List<JobMetaData> findTerminated(String filter, String order, int skip, int take) throws Exception;
	List<JobMetaData> findUnterminated(String filter, String order, int skip, int take) throws Exception;
	
    int getTerminatedCount() throws Exception;
	
	int getUnterminatedCount() throws Exception;



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
	
	
	



	String[] getAllApplications() throws Exception;

	String[] getAllUsers() throws Exception;
}