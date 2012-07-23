package fantasy.jobs.resources;

import java.util.UUID;

public interface IResourceRequestQueue
{
	void RegisterResourceRequest(UUID jobId, ResourceParameter[] parameters);
	void UnregisterResourceRequest(UUID jobId);

	ResourceParameter[] GetRequiredResources(UUID jobId);
}