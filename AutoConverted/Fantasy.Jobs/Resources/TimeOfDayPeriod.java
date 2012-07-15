package Fantasy.Jobs.Resources;

import Fantasy.Configuration.*;

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct TimeOfDayPeriod
public final class TimeOfDayPeriod
{
	private TimeSpan privateStart = new TimeSpan();
	public TimeSpan getStart()
	{
		return privateStart;
	}
	public void setStart(TimeSpan value)
	{
		privateStart = value;
	}

	private TimeSpan privateEnd = new TimeSpan();
	public TimeSpan getEnd()
	{
		return privateEnd;
	}
	public void setEnd(TimeSpan value)
	{
		privateEnd = value;
	}
}