package fantasy.tracking;

import java.lang.ref.WeakReference;
import java.rmi.*;
import java.rmi.server.*;
import java.util.*;

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



	public final TrackMetaData[] GetActivedTrackMetaData()
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
	public void addHanlder(UUID id, ITrackManagerServiceHandler handler) {
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

	@Override
	public ITrackProviderService getProvider(UUID id, String name,
			String category, HashMap<String, Object> properties) throws RemoteException {
		
		RemoteTrack rs;
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
			}
		}
		
		return rs;
	}




	@Override
	public void refresh() {
		// TODO Auto-generated method stub
		
	}



	@Override
	public ITrackListenerService getListener(UUID id) {
		// TODO Auto-generated method stub
		return null;
	}


}