package Fantasy.Tracking;

public final class TrackingConfiguration
{

	private static java.util.ArrayList<ServiceHost> _hosts = new java.util.ArrayList<ServiceHost>();

	public static void StartTrackingService()
	{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Thread thread = ThreadFactory.CreateThread(()=>{
			_hosts.addAll(new ServiceHost[] { new ServiceHost(TrackProviderService.class), new ServiceHost(TrackManagerService.class), new ServiceHost(TrackListenerService.class) });
			for (ServiceHost host : _hosts)
			{
				host.Open();
			}
	}
   ).WithStart();


		thread.Join();

	}

	public static void CloseTrackingService()
	{
		for (ServiceHost host : _hosts)
		{
			host.Close();
		}
	}
}