package fantasy.jobs.resources;

import fantasy.*;

public interface IResourceProvider
{
	boolean canHandle(String name)  throws Exception;

	void initialize()  throws Exception;

	boolean isAvailable(ResourceParameter parameter)  throws Exception;

	boolean request(ResourceParameter parameter, RefObject<Object> resource)  throws Exception;

	void release(Object resource) throws Exception;
	
	void addListener(IResourceProviderListener listener);
	void removeListener(IResourceProviderListener listener);

}