package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("monthlyTrigger", NamespaceUri=Consts.ScheduleNamespaceURI)]
public class MonthlyTrigger extends Trigger
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("daysOfMonth")]
	private int[] privateDaysOfMonth;
	public final int[] getDaysOfMonth()
	{
		return privateDaysOfMonth;
	}
	public final void setDaysOfMonth(int[] value)
	{
		privateDaysOfMonth = value;
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
	//[DataMember, XAttribute("runOnLastDayOfMonth")]
	private boolean privateRunOnLastDayOfMonth;
	public final boolean getRunOnLastDayOfMonth()
	{
		return privateRunOnLastDayOfMonth;
	}
	public final void setRunOnLastDayOfMonth(boolean value)
	{
		privateRunOnLastDayOfMonth = value;
	}


	public MonthlyTrigger()
	{
		this.setDaysOfMonth(new int[0]);
		this.setMonthsOfYear(new int[0]);
	}


	@Override
	public TriggerType getType()
	{
		return TriggerType.Monthly;
	}


	private Iterable<String> DayNames()
	{
		for (int d : getDaysOfMonth())
		{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
			yield return (new Integer(d)).toString();
		}

		if (this.getRunOnLastDayOfMonth())
		{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
			yield return "last";
		}
	}

	private Iterable<String> MonthNames()
	{
		DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
		for (int m : getMonthsOfYear())
		{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
			yield return info.GetMonthName(m);
		}
	}

	@Override
	protected String getTriggerTimeDescription()
	{
		java.util.Date time = new java.util.Date(this.getStartTime().Ticks);
//C# TO JAVA CONVERTER TODO TASK: The 't' format specifier is not converted to Java:
//ORIGINAL LINE: return string.Format("At {0:t} on day {1} of {2}, starting {3:g}.",time, string.Join(",", DayNames()),string.Join(",", MonthNames()), this.StartBoundary);
		return String.format("At %0s on day %2$s of %3$s, starting %3s.",time, DotNetToJavaStringHelper.join(",", DayNames()),DotNetToJavaStringHelper.join(",", MonthNames()), this.getStartBoundary());
	}
}