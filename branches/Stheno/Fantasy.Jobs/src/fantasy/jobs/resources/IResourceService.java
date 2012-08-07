package fantasy.jobs.resources;

public interface IResourceService
{
	IResourceHandle Request(ResourceParameter[] parameter) throws Exception;

	IResourceHandle TryRequest(ResourceParameter[] parameters) throws Exception;

	void Release(IResourceHandle resource) throws Exception;
}