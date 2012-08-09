package fantasy.jobs.scheduling;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("weeklyTrigger", NamespaceUri = Consts.ScheduleNamespaceURI)]
public class WeeklyTrigger extends Trigger
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("daysOfWeek")]
	private DayOfWeek[] privateDaysOfWeek;
	public final DayOfWeek[] getDaysOfWeek()
	{
		return privateDaysOfWeek;
	}
	public final void setDaysOfWeek(DayOfWeek[] value)
	{
		privateDaysOfWeek = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("weeksInterval")]
	private int privateWeeksInterval;
	public final int getWeeksInterval()
	{
		return privateWeeksInterval;
	}
	public final void setWeeksInterval(int value)
	{
		privateWeeksInterval = value;
	}

	public WeeklyTrigger()
	{
		this.setDaysOfWeek(new DayOfWeek[0]);
		setWeeksInterval(1);
	}

	@Override
	public TriggerType getType()
	{
		return TriggerType.Weekly;
	}

	@Override
	protected String getTriggerTimeDescription()
	{
		 DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
		 java.util.Date time = new java.util.Date(this.getStartTime().Ticks);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
//C# TO JAVA CONVERTER TODO TASK: The 't' format specifier is not converted to Java:
//ORIGINAL LINE: return string.Format("At {0:t} every {1} of every {2} {3}, starting {4:g}.",time, string.Join(",", DaysOfWeek.Select(d=>info.GetDayName(d))), WeeksInterval, WeeksInterval > 1 ? "weeks" : "week", this.StartBoundary);
		return String.format("At %0s every %2$s of every %3$s %4$s, starting %4s.",time, DotNetToJavaStringHelper.join(",", getDaysOfWeek().Select(d=>info.GetDayName(d))), getWeeksInterval(), getWeeksInterval() > 1 ? "weeks" : "week", this.getStartBoundary());
	}
}