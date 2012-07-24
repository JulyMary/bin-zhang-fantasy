package fantasy.jobs.management;

import Fantasy.Jobs.Properties.*;

public class JobQueueEventArgs extends EventArgs
{
	public JobQueueEventArgs()
	{

	}

	public JobQueueEventArgs(JobMetaData job)
	{
		this.setJob(job);
	}

	private JobMetaData privateJob;
	public final JobMetaData getJob()
	{
		return privateJob;
	}
	private void setJob(JobMetaData value)
	{
		privateJob = value;
	}
}