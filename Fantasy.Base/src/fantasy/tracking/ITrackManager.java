package fantasy.tracking;

import fantasy.IDisposable;

public interface ITrackManager extends IDisposable
{

	TrackMetaData[] getActiveTrackMetaData();
	
	
	void addHandler(ITrackActiveEventListener handler);
	void removeHandler(ITrackActiveEventListener handler);

}