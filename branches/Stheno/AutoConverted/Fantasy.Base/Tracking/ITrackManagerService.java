package Fantasy.Tracking;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceContract(Namespace = Consts.NamespaceUri, SessionMode = SessionMode.Required, CallbackContract = typeof(ITrackManagerServiceHandler))]
public interface ITrackManagerService
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	TrackMetaData[] GetActivedTrackMetaData();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Echo();

}