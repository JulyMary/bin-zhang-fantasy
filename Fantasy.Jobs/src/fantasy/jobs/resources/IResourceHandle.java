package fantasy.jobs.resources;

import fantasy.*;

public interface IResourceHandle extends IDisposable
{
	
	void addListener(IResourceHandleListener listener);
	void removeListener(IResourceHandleListener listener);
}