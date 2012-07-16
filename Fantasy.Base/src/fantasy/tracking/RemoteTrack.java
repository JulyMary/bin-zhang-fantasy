package fantasy.tracking;

import java.rmi.server.*;
import java.rmi.*;
import java.util.*;
import java.util.Map.Entry;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import org.apache.commons.lang3.ObjectUtils;

public final class RemoteTrack extends UnicastRemoteObject implements ITrackProviderService, ITrackListenerService, IRefreshable
{


	/**
	 * 
	 */
	private static final long serialVersionUID = -855997846962891701L;

	public RemoteTrack(UUID id, String name, String category, java.util.Map<String, Object> values) throws RemoteException
	{
		this.setId(id);
		this.setName(name);
		this.setCategory(category);
		this.InitializeData(values);
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


	
	private HashMap<String, Object> Data = new HashMap<String, Object>();

	private UUID privateId = UUID.randomUUID();
	public UUID getId()
	{
		return privateId;
	}
	public void setId(UUID value)
	{
		privateId = value;
	}

	private String privateName;
	public String getName()
	{
		return privateName;
	}
	public void setName(String value)
	{
		privateName = value;
	}

	private String privateCategory;
	public String getCategory()
	{
		return privateCategory;
	}
	public void setCategory(String value)
	{
		privateCategory = value;
	}



	private void InitializeData(java.util.Map<String, Object> values)
	{
		if (values != null)
		{
			for (java.util.Map.Entry<String, Object> pair : values.entrySet())
			{
				this.Data.put(pair.getKey(), pair.getValue());
			}
		}
	}

	public Object getItem(String name)
	{
		synchronized(this.Data)
		{
			Object rs = null;
			rs = this.Data.get(name);
			return rs;
		}
	}
	public void setItem(String name, Object value)
	{
		Object oldValue = null;
		boolean changed = false;
		synchronized (this.Data)
		{
			oldValue = this.Data.get(name);

			
			if (ObjectUtils.equals(oldValue, value))
			{
				changed = true;
				this.Data.put(name, value);
			}
		}

		if (changed)
		{
			this.OnChanged(name, value);
		}
	}


	
	
	private void OnChanged(final String propertyName, final Object value)
	{
		
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

	
	




}