package fantasy.tracking;

import java.rmi.*;

public interface IRemoteTrackHandler extends Remote
{
	void handleChanged(String name, Object newValue) throws RemoteException;
	
	boolean echo() throws RemoteException;
	
	
}