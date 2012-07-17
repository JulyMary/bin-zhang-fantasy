package fantasy.tracking;

import java.lang.ref.WeakReference;
import java.rmi.*;
import java.rmi.server.*;
import java.util.*;
import java.util.Map.Entry;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import fantasy.collections.*;

public class TrackManagerService extends UnicastRemoteObject implements ITrackManagerService, IRefreshable
{
	
	/**
	 * 
	 */
	private static final long serialVersionUID = -533843063233580786L;
	
	private HashMap<UUID, ITrackManagerServiceHandler> _serviceHandlers = new HashMap<UUID, ITrackManagerServiceHandler>();
	private Object _handlerSyncRoot = new Object();
	private Object _trackSyncRoot = new Object();
	private HashMap<UUID, WeakReference<RemoteTrack>> _remoteTracks = new HashMap<UUID, WeakReference<RemoteTrack>>();

	public TrackManagerService() throws RemoteException 
	{
		
		
	}



	public final TrackMetaData[] getActiveTrackMetaData()
	{
		TrackMetaData[] rs;
		synchronized(_trackSyncRoot)
		{
			rs = new Enumerable<WeakReference<RemoteTrack>>(_remoteTracks.values()).where(new Predicate<WeakReference<RemoteTrack>>(){

				@Override
				public boolean evaluate(WeakReference<RemoteTrack> obj) {
					return ! obj.isEnqueued();
				}}).select(new Selector<WeakReference<RemoteTrack>, TrackMetaData>(){

					@Override
					public TrackMetaData select(WeakReference<RemoteTrack> item) {
						RemoteTrack t = item.get();
						TrackMetaData rs = new TrackMetaData();
						rs.setId(t.getId());
						rs.setName(t.getName());
						rs.setCategory(t.getCategory());
						return rs;
					}}).toArrayList().toArray(new TrackMetaData[0]);
		}
		return rs;
	}


	public final boolean echo()
	{
        return true;
	}

	@Override
	public void addHandler(UUID id, ITrackManagerServiceHandler handler) {
		synchronized(_handlerSyncRoot)
		{
			_serviceHandlers.put(id, handler);
		}
		
	}

	@Override
	public void removeHandler(UUID id) {
		synchronized(_handlerSyncRoot)
		{
			_serviceHandlers.remove(id);
		}
		
	}
	
	
	private void onTrackActive(final TrackMetaData track)
	{
		
        HashMap<UUID, ITrackManagerServiceHandler> clonedHandlers; 
		
		synchronized(_handlerSyncRoot)
		{
			clonedHandlers = new HashMap<UUID, ITrackManagerServiceHandler>(this._serviceHandlers);
			
		}
		
		ExecutorService exec = Executors.newCachedThreadPool();
		try
		{
			
			
			for(final Entry<UUID, ITrackManagerServiceHandler> entry : clonedHandlers.entrySet() )
			{
				exec.execute(new Runnable(){

					@Override
					public void run() {
						try
						{
							entry.getValue().handleTrackActived(track);
						}
						catch(Exception error)
						{
							synchronized(_handlerSyncRoot)
							{
								_serviceHandlers.remove(entry.getKey());
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
	public ITrackProviderService getProvider(UUID id, String name,
			String category, HashMap<String, Object> properties) throws RemoteException {
		
		RemoteTrack rs;
		boolean isActive = false;
		synchronized(_trackSyncRoot)
		{
			WeakReference<RemoteTrack> wr = this._remoteTracks.get(id);
			if(!wr.enqueue())
			{
				
				rs = wr.get();
				
			}
			else
			{
				rs = new RemoteTrack(id, name, category, properties);
				this._remoteTracks.put(id, new WeakReference<RemoteTrack>(rs));
				isActive = true;
			}
		}
		
		if(isActive)
		{
			TrackMetaData metaData = new TrackMetaData();
			metaData.setId(id);
			metaData.setName(name);
			metaData.setCategory(category);
			this.onTrackActive(metaData);
		}
		else
		{
			rs.setName(name);
			rs.setCategory(category);
			
			for(Entry<String, Object> entry : properties.entrySet())
			{
				rs.setItem(entry.getKey(), entry.getValue());
			}
		}
		
		
		
		
		return rs;
	}



	private void refreshTracks()
	{
		synchronized(_trackSyncRoot)
		{
			HashMap<UUID, WeakReference<RemoteTrack>> clonedTracks = new HashMap<UUID, WeakReference<RemoteTrack>>(this._remoteTracks);
			
			for(Entry<UUID, WeakReference<RemoteTrack>> entry : clonedTracks.entrySet())
			{
				if(entry.getValue().isEnqueued())
				{
					this._remoteTracks.remove(entry.getKey());
				}
			}
		}
	}
	
	
	private void refreshHandlers()
	{
		
		HashMap<UUID, ITrackManagerServiceHandler> clonedHandlers; 
		
		synchronized(_handlerSyncRoot)
		{
			clonedHandlers = new HashMap<UUID, ITrackManagerServiceHandler>(this._serviceHandlers);
			
		}
		
		ExecutorService exec = Executors.newCachedThreadPool();
		try
		{
			
			
			for(final Entry<UUID, ITrackManagerServiceHandler> entry : clonedHandlers.entrySet() )
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
							synchronized(_handlerSyncRoot)
							{
								_serviceHandlers.remove(entry.getKey());
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
	public void refresh() {
		
	    this.refreshTracks();
	    this.refreshHandlers();
	}



	@Override
	public ITrackListenerService getListener(UUID id) {
		
		RemoteTrack rs = null;
		synchronized(_trackSyncRoot)
		{
			WeakReference<RemoteTrack> wr = this._remoteTracks.get(id);
			if(!wr.enqueue())
			{
				rs = wr.get();
			}
			
		}
		
		return rs;
	}


}