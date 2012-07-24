package fantasy.jobs.management;

public class JobThread
{
	public JobThread(JobMetaData job, Thread thread, boolean isResume)
	{
		this.setThread(thread);
		this.setJob(job);
		this.setIsResume(isResume);
		this.setExitEvent(new ManualResetEvent(false));
	}

	private Thread privateThread;
	public final Thread getThread()
	{
		return privateThread;
	}
	private void setThread(Thread value)
	{
		privateThread = value;
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


	private ManualResetEvent privateExitEvent;
	public final ManualResetEvent getExitEvent()
	{
		return privateExitEvent;
	}
	private void setExitEvent(ManualResetEvent value)
	{
		privateExitEvent = value;
	}


	private boolean privateIsResume;
	public final boolean getIsResume()
	{
		return privateIsResume;
	}
	private void setIsResume(boolean value)
	{
		privateIsResume = value;
	}

	private IJobEngine privateEngine;
	public final IJobEngine getEngine()
	{
		return privateEngine;
	}
	public final void setEngine(IJobEngine value)
	{
		privateEngine = value;
	}
}