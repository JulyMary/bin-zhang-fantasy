package fantasy.tracking;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, Namespace=Consts.NamespaceUri)]
public class TrackManagerService implements ITrackManagerService
{
	private ITrackManagerServiceHandler handler;
	public TrackManagerService()
	{
		handler = OperationContext.Current.<ITrackManagerServiceHandler>GetCallbackChannel();
		RemoteTrackManager.getManager().AddHandler(handler);
	}

	protected void finalize() throws Throwable
	{
		RemoteTrackManager.getManager().RemoveHandler(handler);
	}

	public final TrackMetaData[] GetActivedTrackMetaData()
	{
		return RemoteTrackManager.getManager().GetAllTrackMetaData();
	}


	public final void Echo()
	{

	}


}