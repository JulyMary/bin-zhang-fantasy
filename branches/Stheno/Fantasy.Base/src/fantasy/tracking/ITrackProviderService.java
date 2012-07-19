package fantasy.tracking;

import java.rmi.*;

public interface ITrackProviderService extends Remote
{
	

	void setItem(String name, Object value) throws RemoteException;

	boolean echo() throws RemoteException;
}