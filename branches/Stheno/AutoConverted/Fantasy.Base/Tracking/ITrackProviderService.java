package Fantasy.Tracking;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceContract(Namespace = Consts.NamespaceUri)]
public interface ITrackProviderService
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(IsInitiating=true)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void CreateTrackProvider(Guid id, String name, String category, TrackProperty[] properties, boolean reconnect);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(IsOneWay=true)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void SetProperty(TrackProperty property);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Echo();
}