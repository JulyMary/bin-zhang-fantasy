package fantasy.jobs.management;

import fantasy.*;
import fantasy.jobs.properties.*;

public interface IJobQueue
{
	Iterable<JobMetaData> getUnterminates();

	Iterable<JobMetaData> getTerminates();

	JobMetaData FindJobMetaDataById(UUID id);

	boolean IsTerminated(UUID id);

	Iterable<JobMetaData> FindTerminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take);
	Iterable<JobMetaData> FindUnterminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take);

	Iterable<JobMetaData> FindAll();

	JobMetaData CreateJobMetaData();

	void ApplyChange(JobMetaData job);
	void Resume(UUID id);
	void Cancel(UUID id);
	void Suspend(UUID id);
	void UserPause(UUID id);

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