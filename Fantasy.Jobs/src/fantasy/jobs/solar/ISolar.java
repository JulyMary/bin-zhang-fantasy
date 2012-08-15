package fantasy.jobs.solar;

import java.util.UUID;

import fantasy.*;
import fantasy.jobs.management.*;
import fantasy.jobs.resources.*;



public interface ISolar
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region operators for IJobQueue
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	JobMetaData[] Unterminates(int skip, int take);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	JobMetaData[] Terminates(int skip, int take);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	JobMetaData FindJobMetaDataById(UUID id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	boolean IsTermianted(UUID id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	JobMetaData[] FindTerminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	JobMetaData[] FindUnterminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void ApplyChange(JobMetaData job);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Resume(UUID id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Cancel(UUID id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Suspend(UUID id);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void UserPause(UUID id);

	String[] GetAllApplications();

	String[] GetAllUsers();
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region operatiors for resource management

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void RegisterResourceRequest(UUID jobId, ResourceParameter[] parameters);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void UnregisterResourceRequest(UUID jobId);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void ResourceAvaiable();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[OperationContract]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	ResourceParameter[] GetRequiredResources(UUID jobId);

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


}