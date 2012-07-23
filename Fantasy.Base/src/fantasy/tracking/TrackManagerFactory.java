package fantasy.tracking;

public final class TrackManagerFactory {

	private TrackManagerFactory()
	{
		
	}
	
	
	public static ITrackManager createTrackManager(String uri) throws Exception
	{
		return new TrackManager(uri);
		
	}


	public static ITrackManager createTrackManager() {
		// TODO Auto-generated method stub
		return null;
	}
}
