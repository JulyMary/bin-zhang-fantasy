package fantasy.jobs.resources;

import java.rmi.*;

import fantasy.*;

public interface IGlobalMutexService extends Remote
{
	boolean isAvaiable(String key) throws Exception;

	boolean request(String key, TimeSpan timeout) throws Exception;

	void release(String key) throws Exception;
 }