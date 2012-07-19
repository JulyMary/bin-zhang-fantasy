package fantasy.tracking;

public final class TrackManagerFactory {

	private TrackManagerFactory()
	{
		
	}
	
	
	public static ITrackManager createTrackManager(String uri) throws Exception
	{
		return new TrackManager(uri);
		
	}
}
