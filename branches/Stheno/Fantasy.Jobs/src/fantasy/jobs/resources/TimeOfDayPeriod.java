package fantasy.jobs.resources;

import fantasy.*;

public final class TimeOfDayPeriod
{
	private TimeSpan privateStart;
	public TimeSpan getStart()
	{
		return privateStart;
	}
	public void setStart(TimeSpan value)
	{
		privateStart = value;
	}

	private TimeSpan privateEnd;
	public TimeSpan getEnd()
	{
		return privateEnd;
	}
	public void setEnd(TimeSpan value)
	{
		privateEnd = value;
	}
}