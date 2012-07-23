package fantasy.jobs.resources;

import fantasy.*;

public interface IResourceProvider
{
	boolean CanHandle(String name);

	void Initialize();

	boolean IsAvailable(ResourceParameter parameter);

	boolean Request(ResourceParameter parameter, RefObject<Object> resource);

	void Release(Object resource);

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler Available;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<ProviderRevokeArgs> Revoke;
}