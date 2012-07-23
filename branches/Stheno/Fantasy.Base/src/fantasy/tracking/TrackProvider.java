package fantasy.tracking;


import java.rmi.*;
import java.util.*;
import java.util.concurrent.*;



class TrackProvider extends TrackBase implements ITrackProvider, IRefreshable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 6690896643120711585L;




	private TrackManager _trackManager;
	

	

    private ITrackProviderService _remoteTrack;

	private Object _syncRoot = new Object();

	public TrackProvider(TrackManager manager, UUID id, String name, String category, java.util.Map<String, Object> values) throws Exception
	{
		super();
		this._trackManager = manager; 

		this.setId(id);
		this.setName(name);
		this.setCategory(category);
		this.InitializeData(values);

		ExecutorService executor = Executors.newFixedThreadPool(1);

		try
		{
			executor.execute(new Runnable(){

				@Override
				public void run() {
					tryCreateRemote();
					RefreshManager.register(TrackProvider.this);

				}});

			executor.awaitTermination(100, TimeUnit.MILLISECONDS);
		}

		finally
		{
			executor.shutdown();
		}
		
		
			
		
		
	}

	private void tryCreateRemote()
	{
		synchronized (_syncRoot)
		{
			try
			{

				ITrackManagerService rm = this._trackManager.getRemoteManager(); 
				if(rm != null)
				{
				this._remoteTrack = rm.getProvider(this.getId(), getName(), this.getCategory(), this.Data);
				
				
				}

			}
			catch (Exception error)
			{
				
				
				this._remoteTrack = null;
				
				
			}
		}
	}

	@Override
	protected void onChanged(final TrackChangedEventObject e)
	{

		super.onChanged(e);
		if (this._remoteTrack != null)
		{
			
			ExecutorService executor = Executors.newFixedThreadPool(1);

			try
			{
				executor.execute(new Runnable(){

					@Override
					public void run() {
						
                        try {
							TrackProvider.this._remoteTrack.setProperty(e.getName(), e.getNewValue());
						} catch (RemoteException e) {
							
							e.printStackTrace();
						}
					}});

				
			}

			finally
			{
				executor.shutdown();
			}
			
			

		}

		
	}


	@Override
	public final void refresh()
	{
		if(_disposed) return;
		
		if (this._remoteTrack != null)
		{

				try
				{
					this._remoteTrack.echo();
				}
				catch (Exception error)
				{
					
					this._remoteTrack = null;
				}



		}
		if (this._remoteTrack == null)
		{
			this.tryCreateRemote();
		}
	}

	private boolean _disposed = false;
	@Override
	public void dispose() {
		if(!_disposed)
		{
			_disposed = true;
			RefreshManager.unregister(this);
			this._remoteTrack = null;
			
		}
		
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public <T> T getProperty(String name, T defaultValue) {
		Object rs = this.getProperty(name);
		
		return rs != null ? (T)rs : defaultValue;
	}

}

