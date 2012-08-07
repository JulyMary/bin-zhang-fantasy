package fantasy.jobs.resources;

import java.util.*;

public interface IResourceManager
{
	UUID Request(UUID jobId, ResourceParameter[] parameters) throws Exception;

	void Release(UUID id) throws Exception;

	boolean IsAvailable(ResourceParameter[] parameters) throws Exception;

	void addConsumer(IResourceConsumer handler) throws Exception;

	void removeConsumer(UUID id) throws Exception;
	
	void addListener(IResourceManagerListener listener);
	void removeListener(IResourceManagerListener listener);



}