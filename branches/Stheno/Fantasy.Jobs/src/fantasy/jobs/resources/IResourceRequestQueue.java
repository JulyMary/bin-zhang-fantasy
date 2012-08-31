package fantasy.jobs.resources;

import java.util.UUID;

public interface IResourceRequestQueue
{
	void registerResourceRequest(UUID jobId, ResourceParameter[] parameters) throws Exception;
	void unregisterResourceRequest(UUID jobId) throws Exception;

	ResourceParameter[] getRequiredResources(UUID jobId) throws Exception;
}