package fantasy.tracking;

public interface ITrackManager
{

	TrackFactory getConnection();

	TrackMetaData[] GetActivedTrackMetaData();

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<TrackActivedEventArgs> TrackActived;

}