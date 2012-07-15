package Fantasy.Jobs.Management;

public class JobManagerAccessor extends MarshalByRefObject
{

	private static IJobManager _manager;

	public final IJobManager GetJobManager()
	{
		return (_manager != null) ? _manager : JobManager.getDefault();
	}

	public final void SetJobManager(JobManager manager)
	{
		_manager = manager;
	}
}