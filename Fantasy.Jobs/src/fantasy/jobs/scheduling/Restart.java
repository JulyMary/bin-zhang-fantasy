package fantasy.jobs.scheduling;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("restart", NamespaceUri = Consts.ScheduleNamespaceURI)]
public class Restart
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("count")]
	private int privateCount;
	public final int getCount()
	{
		return privateCount;
	}
	public final void setCount(int value)
	{
		privateCount = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("duration")]
	private TimeSpan privateDuration = new TimeSpan();
	public final TimeSpan getDuration()
	{
		return privateDuration;
	}
	public final void setDuration(TimeSpan value)
	{
		privateDuration = value;
	}

}