package Fantasy.Jobs.Management;

public class JobProcess
{
	public JobProcess(JobMetaData job, Process process, boolean isResume)
	{
		this.setProcess(process);
		this.setJob(job);
		this.setIsResume(isResume);
		this.setExitEvent(new ManualResetEvent(false));

	}

	private Process privateProcess;
	public final Process getProcess()
	{
		return privateProcess;
	}
	private void setProcess(Process value)
	{
		privateProcess = value;
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