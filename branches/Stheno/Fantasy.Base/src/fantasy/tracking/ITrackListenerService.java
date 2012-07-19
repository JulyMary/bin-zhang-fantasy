package fantasy.tracking;

import java.rmi.*;
import java.util.*;

public interface ITrackListenerService extends Remote
{

	

	public HashMap<String, Object> getProperties() throws RemoteException;

	public TrackMetaData getMetaData() throws RemoteException;

	public boolean echo() throws RemoteException;
	
	public void addHandler(UUID token, IRemoteTrackHandler handler) throws RemoteException;
	
	public void removeHandler(UUID token) throws RemoteException;


}