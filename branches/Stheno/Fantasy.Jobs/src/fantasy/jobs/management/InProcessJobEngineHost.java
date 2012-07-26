package fantasy.jobs.management;

public class InProcessJobEngineHost extends MarshalByRefObject
{
	@Override
	public Object InitializeLifetimeService()
	{
		return null;
	}



	public final void Run(JobManager manager, UUID jobId)
	{

		(new JobManagerAccessor()).SetJobManager(manager);

		JobEngine engine = new JobEngine(jobId);
		engine.Initialize();
		engine.WaitForExit();
	}
}