package Fantasy.Jobs.Client;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;
import Fantasy.Tracking.*;

public class JobListenerJobEventArgs extends EventArgs
{

	public JobListenerJobEventArgs(JobMetaData job)
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