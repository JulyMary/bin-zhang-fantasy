package fantasy.jobs.resources;

import java.util.*;

import fantasy.*;

public abstract class ResourceProvider extends ObjectWithSite implements IResourceProvider {

	
	protected HashSet<IResourceProviderListener> _listeners = new HashSet<IResourceProviderListener>();
	
	@Override
	public void addListener(IResourceProviderListener listener)
	{
		this._listeners.add(listener);
	}
	
	@Override
	public void removeListener(IResourceProviderListener listener)
	{
		this._listeners.remove(listener);
	}
	
	protected void onAvailable()
	{
		EventObject e = new EventObject(this);
		for(IResourceProviderListener listener : this._listeners)
		{
			listener.Available(e);
		}
	}
	
	protected void onRevoke(ProviderRevokeArgs e) throws Exception
	{
		for(IResourceProviderListener listener : this._listeners)
		{
			listener.Revoke(e);
		}
	}
}
