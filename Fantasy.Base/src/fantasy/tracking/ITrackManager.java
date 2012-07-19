package fantasy.tracking;

import java.util.HashMap;
import java.util.UUID;

import fantasy.IDisposable;

public interface ITrackManager extends IDisposable
{

	TrackMetaData[] getActiveTrackMetaData();
	
	
	void addHandler(ITrackActiveEventListener handler);
	void removeHandler(ITrackActiveEventListener handler);
	

	ITrackProvider getProvider(UUID id, String name, String category, HashMap<String, Object> properties) throws Exception;
    ITrackListener getListener(UUID id) throws Exception;
	  

}