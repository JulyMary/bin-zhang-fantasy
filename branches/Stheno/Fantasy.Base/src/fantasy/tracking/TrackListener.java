package fantasy.tracking;


import java.util.*;
import java.util.Map.*;
import java.util.concurrent.*;


class TrackListener extends TrackBase implements ITrackListener, IRefreshable, IRemoteTrackHandler
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 8979553077252070077L;
	private ITrackListenerService _remoteListener;
	private TrackManager _manager;

	private Object _syncRoot = new Object();
	
	private boolean _isActive = false;
	private UUID _token = UUID.randomUUID();

	public TrackListener(TrackManager manager, UUID id) throws Exception
	{
		super();
		this._manager = manager;
		
		this.setId(id);		
		ExecutorService executor = Executors.newFixedThreadPool(1);

		try
		{
			executor.execute(new Runnable(){

				@Override
				public void run() {
					tryCreateRemote(true);
					RefreshManager.register(TrackListener.this);

				}});

			executor.awaitTermination(100, TimeUnit.MILLISECONDS);
		}

		finally
		{
			executor.shutdown();
		}

	}

	private void tryCreateRemote(boolean init)
	{
		synchronized (_syncRoot)
		{

			ITrackManagerService rm = this._manager.getRemoteManager();
			if(rm != null)
			{
				
				TrackMetaData meta = null;
				HashMap<String, Object> rdata = null;



				try
				{
					this._remoteListener  = rm.getListener(this.getId());

					meta = this._remoteListener.getMetaData();

					rdata = this._remoteListener.getProperties();
					
					this._remoteListener.addHandler(this._token, this);
				}
				catch(Exception exception)
				{
					_remoteListener = null;
				}

				if(_remoteListener != null)
				{
					this.setName(meta.getName());
					this.setCategory(meta.getCategory());

					for (Entry<String, Object> prop : rdata.entrySet())
					{

						if (init)
						{
							this.Data.put(prop.getKey(), prop.getValue());
						}
						else
						{
							this.setItem(prop.getKey(), prop.getValue());
						}
					}
					this.setIsActived(true);
				}

			}

			
		}
	}



	public final void refresh()
	{
		if (this._remoteListener != null)
		{

			try
			{
				this._remoteListener.echo();
			}
			catch (Exception e)
			{
				this._remoteListener = null;
			}

		}
		if (this._remoteListener == null)
		{
			this.tryCreateRemote(false);
		}
	}

	
	private boolean _disposed = false;
	
	@Override
	public final void Dispose()
	{
		if(!_disposed)
		{
			this._disposed = true;
			RefreshManager.unregister(this);
			ITrackListenerService remote = this._remoteListener;
			if (remote != null)
			{
				try
				{

					remote.removeHandler(this._token);
				}
				catch (java.lang.Exception e)
				{
				}
				
				remote = null;
			}
			
		}
		
	}


	public final void handleChanged(String propertyName, Object value)
	{
		
		this.setItem(propertyName, value);
	}

	@Override
	protected void onChanged(TrackChangedEventObject e)
	{
		for(ITrackListenerHandler handler : this._handlers)
		{
			handler.HandleChanged(e);
		}
	}
    
   

	public final boolean echo()
	{
         return true;
	}




	public final boolean getIsActived()
	{
		return this._isActive;
	}
	
	public final void setIsActived(boolean value)
	{
		if (this._isActive != value)
		{
			this._isActive = value;
			
			synchronized(this._handlers)
			{
				
				EventObject e = new EventObject(this);
				
				for(ITrackListenerHandler handler : this._handlers)
				{
					handler.HandleActiveChanged(e);
				}
			}
		}
	}

	
	

    private HashSet<ITrackListenerHandler> _handlers = new HashSet<ITrackListenerHandler>();

	@Override
	public void addHandler(ITrackListenerHandler handler) {
		synchronized(this._handlers)
		{
			this._handlers.add(handler);
		}
	}

	@Override
	public void removeHandler(ITrackListenerHandler handler) {
		synchronized(this._handlers)
		{
			this._handlers.remove(handler);
		}
		
	}

}