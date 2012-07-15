package Fantasy.Jobs.Tasks;

import Fantasy.Jobs.Resources.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("sleep", Consts.XNamespaceURI, Description="Suspend current job until specified time or duration")]
public class SleepTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("time", Description="Absolute time to sleep to.")]
	private java.util.Date privateTime = new java.util.Date(0);
	public final java.util.Date getTime()
	{
		return privateTime;
	}
	public final void setTime(java.util.Date value)
	{
		privateTime = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("duration", Description= "Duration to sleep.")]
	private TimeSpan privateDuration = new TimeSpan();
	public final TimeSpan getDuration()
	{
		return privateDuration;
	}
	public final void setDuration(TimeSpan value)
	{
		privateDuration = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		IResourceService ressvc = this.Site.<IResourceService>GetService();
		ILogger logger = this.Site.<ILogger>GetService();
		if (ressvc != null)
		{
			IJob job = this.Site.<IJob>GetRequiredService();
			java.util.Date timeToWait = java.util.Date.getMinValue();
			String s = (String)job.getRuntimeStatus().getLocal().getItem("sleep.waittime");
			if (!String.IsNullOrWhiteSpace(s))
			{
				timeToWait = new java.util.Date(java.util.Date.parse(s));
			}
			else
			{
				if (!this.getTime().equals(new java.util.Date(0)))
				{
					timeToWait = this.getTime();
				}
				else
				{
					timeToWait = new java.util.Date() + this.getDuration();
				}
				job.getRuntimeStatus().getLocal().setItem("sleep.waittime", timeToWait.toString());
			}
			if (!timeToWait.equals(java.util.Date.getMinValue()))
			{
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
				ResourceParameter res = new ResourceParameter("WaitTime", new { time = timeToWait });
				if (logger != null)
				{
					logger.LogMessage("Sleep", "Sleep current job till {0}.", timeToWait);
				}

				IResourceHandle handler = ressvc.Request(new ResourceParameter[] { res });

				// if request succeed, release resource immediately, otherwise, job engine will block executing.
				if (handler != null)
				{
					handler.dispose();
				}


			}
		}
		else
		{
			if (logger != null)
			{
				logger.LogMessage("Sleep", "IResourceService is not available on this system, this task is skipped.");
			}
		}

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}