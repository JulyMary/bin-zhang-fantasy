package Fantasy.Jobs.Resources;

public interface IResourceService
{
	IResourceHandle Request(ResourceParameter[] parameter);

	IResourceHandle TryRequest(ResourceParameter[] parameters);

	void Release(IResourceHandle resource);
}