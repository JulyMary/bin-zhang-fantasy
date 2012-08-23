package fantasy.jobs.solar;

import java.rmi.Remote;
import java.util.*;

import fantasy.*;
import fantasy.jobs.management.*;
import fantasy.jobs.resources.*;



public interface ISolar extends Remote
{
    Iterable<JobMetaData> getUnterminates();
    
	JobMetaData FindJobMetaDataById(UUID id) throws Exception;

	boolean IsTerminated(UUID id) throws Exception;

	List<JobMetaData> FindTerminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception;
	List<JobMetaData> FindUnterminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception;

	void Add(JobMetaData job) throws Exception;
	void UpdateState(JobMetaData job, boolean isStart) throws Exception;
	void Archive(JobMetaData job) throws Exception;
	void Resume(UUID id) throws Exception;
	void Cancel(UUID id) throws Exception;
	void Suspend(UUID id) throws Exception;
	void UserPause(UUID id) throws Exception;
	
	
	int getTerminatedCount() throws Exception;
	
	int getUnterminatedCount() throws Exception;
	



	String[] GetAllApplications();

	String[] GetAllUsers();

	void RegisterResourceRequest(UUID jobId, ResourceParameter[] parameters);

	void UnregisterResourceRequest(UUID jobId);

	void ResourceAvaiable();

	ResourceParameter[] GetRequiredResources(UUID jobId);
	
	void connect(ISatellite satellite);
	
	void disconnect(UUID cookie);
	
	void echo();

}