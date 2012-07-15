package Fantasy.Tracking;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceContract(Namespace = Consts.NamespaceUri, CallbackContract = typeof(ITrackListenerServiceHandler), SessionMode = SessionMode.Required)]
public interface ITrackListenerService
{



//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract()]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	TrackProperty[] GetProperties();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Echo();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(IsInitiating=true)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	TrackMetaData GetMetaData(Guid id);

}