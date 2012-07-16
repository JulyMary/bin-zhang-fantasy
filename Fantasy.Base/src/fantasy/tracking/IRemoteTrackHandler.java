package fantasy.tracking;

import java.rmi.Remote;

public interface IRemoteTrackHandler extends Remote
{
	void handleChanged(String name, Object newValue);
	
	boolean echo();
}