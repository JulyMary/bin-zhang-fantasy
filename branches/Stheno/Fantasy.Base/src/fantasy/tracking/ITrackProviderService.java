package fantasy.tracking;

import java.rmi.*;

public interface ITrackProviderService extends Remote
{
	

	void setProperty(String name, Object value) throws RemoteException;

	boolean echo() throws RemoteException;
}