package fantasy.jobs.scheduling;


import fantasy.*;
import fantasy.xserialization.*;

@XSerializable(name = "repetition", namespaceUri=fantasy.jobs.Consts.ScheduleNamespaceURI)
public class Repetition
{
    @XAttribute(name = "duration")
	private TimeSpan privateDuration;
	public final TimeSpan getDuration()
	{
		return privateDuration;
	}
	public final void setDuration(TimeSpan value)
	{
		privateDuration = value;
	}

    @XAttribute(name = "interval")
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