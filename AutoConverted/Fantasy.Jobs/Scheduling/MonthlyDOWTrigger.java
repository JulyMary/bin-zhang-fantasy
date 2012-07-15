package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("monthlyDOWTrigger", NamespaceUri=Consts.ScheduleNamespaceURI)]
public class MonthlyDOWTrigger extends Trigger
{



	@Override
	public TriggerType getType()
	{
		return TriggerType.MonthlyDayOfWeek;
	}

	public MonthlyDOWTrigger()
	{
		this.setMonthsOfYear(new int[0]);
		this.setWeeksOfMonth(new WeekOfMonth[0]);
		this.setDaysOfWeek(new DayOfWeek[0]);
	}

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
	//[DataMember, XAttribute("monthsOfYear")]
	private int[] privateMonthsOfYear;
	public final int[] getMonthsOfYear()
	{
		return privateMonthsOfYear;
	}
	public final void setMonthsOfYear(int[] value)
	{
		privateMonthsOfYear = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("runOnLastWeekOfMonth")]
	private boolean privateRunOnLastWeekOfMonth;
	public final boolean getRunOnLastWeekOfMonth()
	{
		return privateRunOnLastWeekOfMonth;
	}
	public final void setRunOnLastWeekOfMonth(boolean value)
	{
		privateRunOnLastWeekOfMonth = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("weeksOfMonth")]
	private WeekOfMonth[] privateWeeksOfMonth;
	public final WeekOfMonth[] getWeeksOfMonth()
	{
		return privateWeeksOfMonth;
	}
	public final void setWeeksOfMonth(WeekOfMonth[] value)
	{
		privateWeeksOfMonth = value;
	}


	@Override
	protected String getTriggerTimeDescription()
	{
		DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
		java.util.Date time = new java.util.Date(this.getStartTime().Ticks);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
//C# TO JAVA CONVERTER TODO TASK: The 't' format specifier is not converted to Java:
//ORIGINAL LINE: return string.Format("At {0:t} on the {1} {2} each {3}, starting {4:g}.",time, string.Join(",", WeeksOfMonth.Select(w=>w.ToString())), string.Join(",", DaysOfWeek.Select(d=>info.GetDayName(d))), string.Join(",", MonthsOfYear.Select(m=>info.GetMonthName(m))), this.StartBoundary);
		return String.format("At %0s on the %2$s %3$s each %4$s, starting %4s.",time, DotNetToJavaStringHelper.join(",", getWeeksOfMonth().Select(w=>w.toString())), DotNetToJavaStringHelper.join(",", getDaysOfWeek().Select(d=>info.GetDayName(d))), DotNetToJavaStringHelper.join(",", getMonthsOfYear().Select(m=>info.GetMonthName(m))), this.getStartBoundary());
	}
}