package Fantasy.Jobs.Resources;

public interface IResourceManager
{
	Guid Request(Guid jobId, ResourceParameter[] parameters);

	void Release(Guid id);

	boolean IsAvailable(ResourceParameter[] parameters);

	void RegisterHandler(IResourceManagerHandler handler);

	void UnregisterHandler(Guid id);

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler Available;


}