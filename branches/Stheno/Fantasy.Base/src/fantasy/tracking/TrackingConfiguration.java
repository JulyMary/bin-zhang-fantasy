package fantasy.tracking;


import fantasy.rmi.RmiUtils;


public final class TrackingConfiguration
{

	private static TrackManagerService _service;

	
	
	public static void StartTrackingService() throws Exception
	{
		   _service = new TrackManagerService();
	       RmiUtils.bind(_service);
	}

	public static void CloseTrackingService() throws Exception
	{
		  RmiUtils.unbind(_service);
	}
}