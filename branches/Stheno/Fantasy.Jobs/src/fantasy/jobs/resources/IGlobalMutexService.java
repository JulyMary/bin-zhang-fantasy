package fantasy.jobs.resources;

import java.rmi.*;

public interface IGlobalMutexService extends Remote
{
	boolean isAvaiable(String key) throws Exception;

	boolean request(String key, long timeout) throws Exception;

	void release(String key) throws Exception;
 }