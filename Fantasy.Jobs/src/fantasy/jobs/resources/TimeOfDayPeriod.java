package fantasy.jobs.resources;

public final class TimeOfDayPeriod
{
	private org.joda.time.Period privateStart;
	public org.joda.time.Period getStart()
	{
		return privateStart;
	}
	public void setStart(org.joda.time.Period value)
	{
		privateStart = value;
	}

	private org.joda.time.Period privateEnd;
	public org.joda.time.Period getEnd()
	{
		return privateEnd;
	}
	public void setEnd(org.joda.time.Period value)
	{
		privateEnd = value;
	}
}