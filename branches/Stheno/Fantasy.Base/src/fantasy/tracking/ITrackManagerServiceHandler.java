package fantasy.tracking;

import java.rmi.*; 

public interface ITrackManagerServiceHandler extends Remote
{
	void handleTrackActived(TrackMetaData track) throws RemoteException;
	boolean echo() throws RemoteException;
}