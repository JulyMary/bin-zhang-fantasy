package fantasy.tracking;

import java.rmi.Remote;

public interface IRemoteTrackHandler extends Remote
{
	void HandleChanged(TrackChangedEventArgs e);
}