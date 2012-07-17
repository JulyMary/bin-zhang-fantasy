package fantasy.tracking;

import java.rmi.*;
import java.util.*;

public interface ITrackManagerService extends Remote
{

	TrackMetaData[] getActiveTrackMetaData();

	boolean echo();
	
	void addHandler(UUID id, ITrackManagerServiceHandler handler);
	
	void removeHandler(UUID id);
	
	ITrackProviderService getProvider(UUID id, String name, String category, HashMap<String, Object> properties) throws RemoteException;
	
	ITrackListenerService getListener(UUID id);

}