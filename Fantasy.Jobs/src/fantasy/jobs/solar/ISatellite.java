package fantasy.jobs.solar;

import java.rmi.Remote;
import java.util.UUID;

import fantasy.jobs.management.*;
import fantasy.jobs.resources.*;


public interface ISatellite extends Remote
{

	void echo() throws Exception;

	void onJobAdded(JobMetaData job) throws Exception;

	void onJobChanged(JobMetaData job) throws Exception;


	boolean isResourceAvailable(ResourceParameter[] parameters) throws Exception;

	double getLoadFactor()  throws Exception;

	void requestStartJob(JobMetaData job)  throws Exception;


	void requestResume(JobMetaData job) throws Exception;


	void requestCancel(UUID id) throws Exception;

    void requestSuspend(UUID id) throws Exception;

	void requestUserPause(UUID id) throws Exception;

	void requestSuspendAll() throws Exception;


	boolean isRunning(UUID id) throws Exception;


	
	void addListener(ISatelliteListener listener);
	
	void removeListener(ISatelliteListener listener);

	String getName();

}