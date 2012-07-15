package Fantasy.Jobs.Management;

import Fantasy.Jobs.Properties.*;

public interface IJobQueue
{
	Iterable<JobMetaData> getUnterminates();

	Iterable<JobMetaData> getTerminates();

	JobMetaData FindJobMetaDataById(Guid id);

	boolean IsTerminated(Guid id);

	Iterable<JobMetaData> FindTerminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take);
	Iterable<JobMetaData> FindUnterminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take);

	Iterable<JobMetaData> FindAll();

	JobMetaData CreateJobMetaData();

	void ApplyChange(JobMetaData job);
	void Resume(Guid id);
	void Cancel(Guid id);
	void Suspend(Guid id);
	void UserPause(Guid id);

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<JobQueueEventArgs> Changed;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<JobQueueEventArgs> Added;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<JobQueueEventArgs> RequestCancel;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<JobQueueEventArgs> RequestSuspend;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<JobQueueEventArgs> RequestUserPause;


	String[] GetAllApplications();

	String[] GetAllUsers();
}