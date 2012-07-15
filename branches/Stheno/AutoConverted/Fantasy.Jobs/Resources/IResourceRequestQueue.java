package Fantasy.Jobs.Resources;

public interface IResourceRequestQueue
{
	void RegisterResourceRequest(Guid jobId, ResourceParameter[] parameters);
	void UnregisterResourceRequest(Guid jobId);

	ResourceParameter[] GetRequiredResources(Guid jobId);
}