package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("event", NamespaceUri=Consts.ScheduleNamespaceURI)]
public class ScheduleEvent
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("exectionTime")]
	private java.util.Date privateExecutionTime = new java.util.Date(0);
	public final java.util.Date getExecutionTime()
	{
		return privateExecutionTime;
	}
	public final void setExecutionTime(java.util.Date value)
	{
		privateExecutionTime = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("scheduleTime")]
	private java.util.Date privateScheduleTime = new java.util.Date(0);
	public final java.util.Date getScheduleTime()
	{
		return privateScheduleTime;
	}
	public final void setScheduleTime(java.util.Date value)
	{
		privateScheduleTime = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XArray, XArrayItem(Name="job", java.lang.Class = typeof(Guid))]
	private Guid[] privateCreatedJobs;
	public final Guid[] getCreatedJobs()
	{
		return privateCreatedJobs;
	}
	public final void setCreatedJobs(Guid[] value)
	{
		privateCreatedJobs = value;
	}
}