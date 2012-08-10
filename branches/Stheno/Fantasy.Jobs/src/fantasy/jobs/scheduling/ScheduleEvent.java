package fantasy.jobs.scheduling;

import java.util.*;

import fantasy.xserialization.*;

@XSerializable(name = "event", namespaceUri=fantasy.jobs.Consts.ScheduleNamespaceURI)
public class ScheduleEvent
{
	@XAttribute(name = "exectionTime")
	private java.util.Date privateExecutionTime;
	public final java.util.Date getExecutionTime()
	{
		return privateExecutionTime;
	}
	public final void setExecutionTime(java.util.Date value)
	{
		privateExecutionTime = value;
	}

	@XAttribute(name = "scheduleTime")
	private java.util.Date privateScheduleTime = new java.util.Date(Long.MIN_VALUE);
	public final java.util.Date getScheduleTime()
	{
		return privateScheduleTime;
	}
	public final void setScheduleTime(java.util.Date value)
	{
		privateScheduleTime = value;
	}


	@XArray(items=@XArrayItem(name="job",type=UUID.class))
	private UUID[] privateCreatedJobs;
	public final UUID[] getCreatedJobs()
	{
		return privateCreatedJobs;
	}
	public final void setCreatedJobs(UUID[] value)
	{
		privateCreatedJobs = value;
	}
}