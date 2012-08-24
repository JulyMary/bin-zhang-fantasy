package fantasy.jobs.resources;

import java.util.UUID;

public interface IResourceRequestQueue
{
	void registerResourceRequest(UUID jobId, ResourceParameter[] parameters);
	void unregisterResourceRequest(UUID jobId);

	ResourceParameter[] getRequiredResources(UUID jobId);
}