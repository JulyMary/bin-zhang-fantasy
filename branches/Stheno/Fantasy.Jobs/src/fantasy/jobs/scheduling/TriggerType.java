package fantasy.jobs.scheduling;

public enum TriggerType
{
	Time,
	Daily,
	Weekly,
	Monthly,
	MonthlyDayOfWeek;

	public int getValue()
	{
		return this.ordinal();
	}

	public static TriggerType forValue(int value)
	{
		return values()[value];
	}
}