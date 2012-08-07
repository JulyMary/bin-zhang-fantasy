package fantasy.jobs.resources;

import java.util.*;


import fantasy.UUIDUtils;

public class ResourceHandle implements IResourceHandle
{
	private IResourceService privateResourceService;
	public final IResourceService getResourceService()
	{
		return privateResourceService;
	}
	public final void setResourceService(IResourceService value)
	{
		privateResourceService = value;
	}

	private boolean privateSuspendEngine;
	public final boolean getSuspendEngine()
	{
		return privateSuspendEngine;
	}
	public final void setSuspendEngine(boolean value)
	{
		privateSuspendEngine = value;
	}

	private ResourceParameter[] privateParameters;
	public final ResourceParameter[] getParameters()
	{
		return privateParameters;
	}
	public final void setParameters(ResourceParameter[] value)
	{
		privateParameters = value;
	}

	private UUID privateId = UUIDUtils.Empty;
	public final UUID getId()
	{
		return privateId;
	}
	public final void setId(UUID value)
	{
		privateId = value;
	}

	private boolean _disposed = false;

	public final void dispose()
	{
		if(!_disposed)
		{
			_disposed = true;
			try {
				this.getResourceService().Release(this);
			} catch (Exception e) {
				
			}
		}
	}

	
	private HashSet<IResourceHandleListener> _listeners = new HashSet<IResourceHandleListener>();

	public void OnRevoke(RevokeArgs e)
	{
		
		for(IResourceHandleListener listener : this._listeners)
		{
			listener.revoke(e);
		}
		
	}
	@Override
	public void addListener(IResourceHandleListener listener) {
		this._listeners.add(listener);
		
	}
	@Override
	public void removeListener(IResourceHandleListener listener) {
		this._listeners.remove(listener);
	}

}