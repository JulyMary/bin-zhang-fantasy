package fantasy.jobs.scheduling;


public enum WeekOfMonth
{
	First,
	Second,
	Third,
	Forth,
	Fifth,
	Sixth;

	public int getValue()
	{
		return this.ordinal();
	}

	public static WeekOfMonth forValue(int value)
	{
		return values()[value];
	}
}