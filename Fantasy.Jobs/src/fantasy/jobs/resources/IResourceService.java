package fantasy.jobs.resources;

public interface IResourceService
{
	IResourceHandle Request(ResourceParameter[] parameter);

	IResourceHandle TryRequest(ResourceParameter[] parameters);

	void Release(IResourceHandle resource);
}