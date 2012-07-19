package fantasy.jobs;

public enum WellKnownTracks
{
	Status,
	Progress,
	StartTime;

	public int getValue()
	{
		return this.ordinal();
	}

	public static WellKnownTracks forValue(int value)
	{
		return values()[value];
	}
}