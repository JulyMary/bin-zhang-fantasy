package fantasy.tracking;

import java.rmi.*;
import java.util.*;
import java.util.Map.Entry;
import java.util.concurrent.*;

public final class RemoteTrack extends TrackBase implements ITrackProviderService, ITrackListenerService, IRefreshable
{


	/**
	 * 
	 */
	private static final long serialVersionUID = -855997846962891701L;

	public RemoteTrack(UUID id, String name, String category, java.util.Map<String, Object> properties) throws RemoteException
	{
		super();
		this.setId(id);
		this.setName(name);
		this.setCategory(category);
		this.InitializeData(properties);
		RefreshManager.register(this);
		
	}

	private HashMap<UUID, IRemoteTrackHandler> _handlers = new HashMap<UUID, IRemoteTrackHandler>();

	public final void addHandler(UUID token, IRemoteTrackHandler handler)
	{
		synchronized (_handlers)
		{
			_handlers.put(token, handler);
		}
	}

	public final void removeHandler(UUID token)
	{
		synchronized (_handlers)
		{
			this._handlers.remove(token);
		}
	}


	@Override
	protected void onChanged(TrackChangedEventObject e)
	{
		
		final String propertyName = e.getName();
		final Object value = e.getNewValue();
		
		IRemoteTrackHandler[] list;
		synchronized(_handlers)
		{
			list = this._handlers.values().toArray(new IRemoteTrackHandler[0]);
		}
		
		ExecutorService exec = Executors.newCachedThreadPool();
		try
		{
			
			
			for(final IRemoteTrackHandler handler : list )
			{
				exec.execute(new Runnable(){

					@Override
					public void run() {
						try
						{
							handler.handleChanged(propertyName, value);
						}
						catch(Exception error)
						{
							
						}
						
					}});
			}
		}
		finally
		{
			exec.shutdown();
		}
	
	}

	@Override
	public void refresh() {
		
		HashMap<UUID, IRemoteTrackHandler> cloned;
		synchronized(_handlers)
		{
			cloned = new HashMap<UUID, IRemoteTrackHandler>(this._handlers);
		}
		
		ExecutorService exec = Executors.newCachedThreadPool();
		try
		{
			
			
			for(final Entry<UUID, IRemoteTrackHandler> entry : cloned.entrySet() )
			{
				exec.execute(new Runnable(){

					@Override
					public void run() {
						try
						{
							entry.getValue().echo();
						}
						catch(Exception error)
						{
							synchronized(_handlers)
							{
								_handlers.remove(entry.getKey());
							}
						}
						
					}});
			}
		}
		finally
		{
			exec.shutdown();
		}
		
	}

	@Override
	public HashMap<String, Object> getProperties() {
		
		HashMap<String, Object> rs;
		synchronized(this.Data)
		{
			rs = new HashMap<String, Object>(this.Data);
		}
		return rs;
	}

	

	@Override
	public boolean echo() {
		return true;
		
	}

	@Override
	public TrackMetaData getMetaData() {
		TrackMetaData rs = new TrackMetaData();
		rs.setId(this.getId());
		rs.setName(this.getName());
		rs.setCategory(this.getCategory());
		return rs;
	}

	
	




}