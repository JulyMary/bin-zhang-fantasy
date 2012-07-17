package fantasy.tracking;

import java.util.*;

public interface ITrackListenerService
{

	

	public HashMap<String, Object> getProperties();

	public TrackMetaData getMetaData();

	public boolean echo();
	
	public void addHandler(UUID token, IRemoteTrackHandler handler);
	
	public void removeHandler(UUID token);


}