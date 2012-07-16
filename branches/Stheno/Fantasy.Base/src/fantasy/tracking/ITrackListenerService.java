package fantasy.tracking;

import java.util.*;

public interface ITrackListenerService
{

	

	TrackProperty[] GetProperties();


	void Echo();
	
	void AddHandler(UUID token, ITrackListenerService handler);
	
	void RemoveHandler(UUID token);


}