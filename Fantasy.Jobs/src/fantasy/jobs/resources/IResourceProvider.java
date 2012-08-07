package fantasy.jobs.resources;

import fantasy.*;

public interface IResourceProvider
{
	boolean CanHandle(String name)  throws Exception;

	void Initialize()  throws Exception;

	boolean IsAvailable(ResourceParameter parameter)  throws Exception;

	boolean Request(ResourceParameter parameter, RefObject<Object> resource)  throws Exception;

	void Release(Object resource) throws Exception;
	
	void addListener(IResourceProviderListener listener);
	void removeListener(IResourceProviderListener listener);

}