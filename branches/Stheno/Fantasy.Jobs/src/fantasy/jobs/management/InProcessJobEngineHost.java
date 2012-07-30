package fantasy.jobs.management;

import java.util.UUID;

import fantasy.jobs.JobEngine;

public class InProcessJobEngineHost
{
	
	public final void Run(JobManager manager, UUID jobId) throws Exception
	{

		(new JobManagerAccessor()).SetJobManager(manager);

		JobEngine engine = new JobEngine(jobId);
		engine.Initialize();
		engine.WaitForExit();
	}
}