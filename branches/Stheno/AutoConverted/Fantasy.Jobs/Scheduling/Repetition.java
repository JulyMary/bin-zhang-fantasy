package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("repetition", NamespaceUri=Consts.ScheduleNamespaceURI)]
public class Repetition
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("duration")]
	private TimeSpan privateDuration;
	public final TimeSpan getDuration()
	{
		return privateDuration;
	}
	public final void setDuration(TimeSpan value)
	{
		privateDuration = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("interval")]
	private TimeSpan privateInterval = new TimeSpan();
	public final TimeSpan getInterval()
	{
		return privateInterval;
	}
	public final void setInterval(TimeSpan value)
	{
		privateInterval = value;
	}
}