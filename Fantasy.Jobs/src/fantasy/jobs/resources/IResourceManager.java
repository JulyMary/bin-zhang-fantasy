package fantasy.jobs.resources;

import java.util.*;

public interface IResourceManager
{
	UUID Request(UUID jobId, ResourceParameter[] parameters);

	void Release(UUID id);

	boolean IsAvailable(ResourceParameter[] parameters);

	void RegisterHandler(IResourceManagerHandler handler);

	void UnregisterHandler(UUID id);

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler Available;


}