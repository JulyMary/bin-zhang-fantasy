package fantasy.tracking;


import java.rmi.*;


public final class TrackingConfiguration
{

	private static TrackManagerService _service;

	public static void StartTrackingService(String uri) throws Exception
	{
		
          _service = new TrackManagerService();
          Naming.bind(uri, _service);
          _uri = uri;

          
	}

	
	private static String _uri = null;
	
	public static void CloseTrackingService() throws Exception
	{
		if(_uri != null)
		Naming.unbind(_uri);
	}
}