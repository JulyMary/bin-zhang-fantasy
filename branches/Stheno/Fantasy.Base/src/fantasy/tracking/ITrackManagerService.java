package fantasy.tracking;

import java.rmi.*;
import java.util.*;

public interface ITrackManagerService extends Remote
{

	TrackMetaData[] getActiveTrackMetaData() throws RemoteException;

	boolean echo() throws RemoteException;
	
	void addHandler(UUID id, ITrackManagerServiceHandler handler) throws RemoteException;
	
	void removeHandler(UUID id) throws RemoteException;
	
	ITrackProviderService getProvider(UUID id, String name, String category, HashMap<String, Object> properties) throws RemoteException;
	
	ITrackListenerService getListener(UUID id) throws RemoteException;

}