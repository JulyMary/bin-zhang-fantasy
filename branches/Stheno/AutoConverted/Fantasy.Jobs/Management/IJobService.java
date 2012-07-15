package Fantasy.Jobs.Management;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceContract(Namespace = Consts.JobServiceNamespaceURI)]
public interface IJobService
{

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract, WebGet]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	String Version();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract, WebGet]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	JobMetaData StartJob(String startInfo);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(Name="ResumeById")]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Resume(Guid id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(Name = "CancelById")]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Cancel(Guid id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(Name = "PauseById")]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Pause(Guid id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(Name="ResumeByIds")]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Resume(Guid[] ids);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(Name="CancelByIds")]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Cancel(Guid[] ids);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract(Name="PauseByIds")]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Pause(Guid[] ids);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract, WebInvoke(RequestFormat=WebMessageFormat.Xml, BodyStyle=WebMessageBodyStyle.WrappedRequest)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: void ResumeByFilter(string filter, string[] args = null);
	void ResumeByFilter(String filter, String[] args);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract, WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: void CancelByFilter(string filter, string[] args = null);
	void CancelByFilter(String filter, String[] args);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract, WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: void PauseByFilter(string filter, string[] args = null);
	void PauseByFilter(String filter, String[] args);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	JobMetaData FindJobById(Guid id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract, WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: JobMetaData[] FindUnterminatedJob(out int totalCount, string filter = "", string[] args = null, string order = "", int skip = 0, int take = Int32.MaxValue);
	JobMetaData[] FindUnterminatedJob(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract, WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat=WebMessageFormat.Xml)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: JobMetaData[] FindTerminatedJob(out int totalCount, string filter = "", string[] args = null, string order = "", int skip = 0, int take = Int32.MaxValue);
	JobMetaData[] FindTerminatedJob(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int GetTerminatedCount();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int GetUnterminatedCount();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	String GetJobLog(Guid id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	String GetManagerLog(java.util.Date date);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	java.util.Date[] GetManagerLogAvaiableDates();


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	JobTemplate[] GetJobTemplates();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	String GetJobScript(Guid id);


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	String[] GetAllApplications();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	String[] GetAllUsers();


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	String GetSettings(String typeName);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract, WebInvoke(BodyStyle=WebMessageBodyStyle.WrappedRequest)]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void SetSettings(String typeName, String xml);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	String GetLocation();

}