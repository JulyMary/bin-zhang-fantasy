package fantasy.jobs.resources;

import org.joda.time.*;

public final class TimeOfDayPeriod
{
	private Duration privateStart;
	public Duration getStart()
	{
		return privateStart;
	}
	public void setStart(Duration value)
	{
		privateStart = value;
	}

	private Duration privateEnd;
	public Duration getEnd()
	{
		return privateEnd;
	}
	public void setEnd(Duration value)
	{
		privateEnd = value;
	}
}