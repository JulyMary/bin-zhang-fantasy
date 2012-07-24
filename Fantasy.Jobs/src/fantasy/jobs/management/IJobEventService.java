package fantasy.jobs.management;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceContract(Namespace = Consts.JobServiceNamespaceURI, CallbackContract = typeof(IJobEventHandler), SessionMode = SessionMode.Required)]
public interface IJobEventService
{

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(IsInitiating = true)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Connect();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Echo();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(IsTerminating = true)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Disconnect();
}