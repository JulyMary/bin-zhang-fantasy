package fantasy.tracking;

import java.rmi.*;
import java.util.*;

public interface ITrackManagerService extends Remote
{

	TrackMetaData[] GetActivedTrackMetaData();

	void Echo();
	
	void AddHanlder(UUID id, ITrackManagerServiceHandler handler);
	
	void RemoveHandler(UUID id);
	
	ITrackProviderService GetProvider(UUID id, String name, String category, TrackProperty[] properties);

}