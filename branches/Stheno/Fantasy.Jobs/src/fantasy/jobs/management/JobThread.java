package fantasy.jobs.management;

import fantasy.jobs.*;

public class JobThread
{
	public JobThread(JobMetaData job, Thread thread, boolean isResume)
	{
		this.setThread(thread);
		this.setJob(job);
		this.setIsResume(isResume);
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


	private Object privateExitEvent = new Object();
	public final Object getExitEvent()
	{
		return privateExitEvent;
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