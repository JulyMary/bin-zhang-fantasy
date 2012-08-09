package fantasy.jobs.scheduling;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Flags]
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