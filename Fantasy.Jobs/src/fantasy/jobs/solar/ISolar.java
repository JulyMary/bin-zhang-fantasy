package fantasy.jobs.solar;

import java.rmi.Remote;
import java.util.*;

import fantasy.*;
import fantasy.jobs.management.*;
import fantasy.jobs.resources.*;



public interface ISolar extends Remote
{
	JobMetaData[] getUnterminates() throws Exception;

	JobMetaData findJobMetaDataById(UUID id) throws Exception;

	boolean isTerminated(UUID id) throws Exception;

	JobMetaData[] findTerminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception;
	JobMetaData[] findUnterminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception;


	void add(JobMetaData job) throws Exception;
	void updateState(JobMetaData job, boolean isStart) throws Exception;
	void archive(JobMetaData job) throws Exception;
	void resume(UUID id) throws Exception;
	void cancel(UUID id) throws Exception;
	void suspend(UUID id) throws Exception;
	void userPause(UUID id) throws Exception;
	
	int getTerminatedCount() throws Exception;
	
	int getUnterminatedCount() throws Exception;
	



	String[] getAllApplications() throws Exception;

	String[] getAllUsers() throws Exception;

	void registerResourceRequest(UUID jobId, ResourceParameter[] parameters) throws Exception;

	void unregisterResourceRequest(UUID jobId) throws Exception;

	void resourceAvaiable() throws Exception;

	ResourceParameter[] getRequiredResources(UUID jobId) throws Exception;
	
	void connect(String name, ISatellite satellite) throws Exception;
	
	void disconnect(String name) throws Exception;
	
	void echo() throws Exception;

}