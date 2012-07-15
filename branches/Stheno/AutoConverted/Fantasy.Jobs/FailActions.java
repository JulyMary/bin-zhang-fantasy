package Fantasy.Jobs;

public enum FailActions
{
	Throw,
	Continue,
	Terminate;

	public int getValue()
	{
		return this.ordinal();
	}

	public static FailActions forValue(int value)
	{
		return values()[value];
	}
}