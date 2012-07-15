package Fantasy.Jobs.Management;

public class InProcessJobEngineHost extends MarshalByRefObject
{
	@Override
	public Object InitializeLifetimeService()
	{
		return null;
	}



	public final void Run(JobManager manager, Guid jobId)
	{

		(new JobManagerAccessor()).SetJobManager(manager);

		JobEngine engine = new JobEngine(jobId);
		engine.Initialize();
		engine.WaitForExit();
	}
}