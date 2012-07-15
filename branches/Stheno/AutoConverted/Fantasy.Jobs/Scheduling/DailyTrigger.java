package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract(Name="daily"), XSerializable("dailyTrigger", NamespaceUri=Consts.ScheduleNamespaceURI)]
public class DailyTrigger extends Trigger
{
	public DailyTrigger()
	{
		setDaysInterval(1);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("daysInterval")]
	private int privateDaysInterval;
	public final int getDaysInterval()
	{
		return privateDaysInterval;
	}
	public final void setDaysInterval(int value)
	{
		privateDaysInterval = value;
	}

	@Override
	public TriggerType getType()
	{
		return TriggerType.Daily;
	}

	@Override
	protected String getTriggerTimeDescription()
	{
		java.util.Date time = new java.util.Date(this.getStartTime().Ticks);
//C# TO JAVA CONVERTER TODO TASK: The 't' format specifier is not converted to Java:
//ORIGINAL LINE: return string.Format("At {0:t} {1}, starting {2:g}.", time, DaysInterval > 1 ? string.Format("every {0} days", DaysInterval) : "everyday", this.StartBoundary);
		return String.format("At %0s %2$s, starting %2s.", time, getDaysInterval() > 1 ? String.format("every %1$s days", getDaysInterval()) : "everyday", this.getStartBoundary());
	}
}