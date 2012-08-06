package fantasy.jobs.tasks;

import java.text.SimpleDateFormat;
import java.util.*;

import org.joda.time.*;

import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;
import fantasy.jobs.resources.*;

@Task(name = "sleep", namespaceUri = Consts.XNamespaceURI, description="Suspend current job until specified time or duration")
public class SleepTask extends ObjectWithSite implements ITask
{

	public java.util.Date Time = new java.util.Date(Long.MIN_VALUE);
	
	@TaskMember(name = "duration", description= "Duration to sleep.")
	private Duration Duration = new Duration(0);
	
	SimpleDateFormat _format = new SimpleDateFormat();
	public void Execute() throws Exception
	{
		IResourceService ressvc = this.getSite().getService(IResourceService.class);
		ILogger logger = this.getSite().getService(ILogger.class);
		if (ressvc != null)
		{
			IJob job = this.getSite().getRequiredService(IJob.class);
			java.util.Date timeToWait = new java.util.Date(Long.MIN_VALUE);
			String s = (String)job.getRuntimeStatus().getLocal().getItem("sleep.waittime");
			if (!StringUtils2.isNullOrWhiteSpace(s))
			{
				
				timeToWait = _format.parse(s);
			}
			else
			{
				if (!this.Time.equals(new java.util.Date(Long.MIN_VALUE)))
				{
					timeToWait = this.Time;
				}
				else
				{
					timeToWait = new DateTime().plus(this.Duration).toDate();
				}
				job.getRuntimeStatus().getLocal().setItem("sleep.waittime", timeToWait.toString());
			}
			if (!timeToWait.equals(new java.util.Date(Long.MIN_VALUE)))
			{

				TreeMap<String, String> args = new TreeMap<String, String>(String.CASE_INSENSITIVE_ORDER);
				args.put("time", this._format.format(timeToWait));
				
				ResourceParameter res = new ResourceParameter("WaitTime",args);
				if (logger != null)
				{
					logger.LogMessage("Sleep", "Sleep current job till %1$s.", this._format.format(timeToWait));
				}

				IResourceHandle handler = ressvc.Request(new ResourceParameter[] { res });

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

		
	}

}