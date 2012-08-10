package fantasy.jobs.scheduling;



import fantasy.TimeSpan;
import fantasy.xserialization.*;

@XSerializable(name = "restart", namespaceUri = fantasy.jobs.Consts.ScheduleNamespaceURI)
public class Restart
{

	@XAttribute(name = "count")
	private int privateCount;
	public final int getCount()
	{
		return privateCount;
	}
	public final void setCount(int value)
	{
		privateCount = value;
	}

	@XAttribute(name = "duration")
	private TimeSpan privateDuration = TimeSpan.Zero;
	public final TimeSpan getDuration()
	{
		return privateDuration;
	}
	public final void setDuration(TimeSpan value)
	{
		privateDuration = value;
	}

}