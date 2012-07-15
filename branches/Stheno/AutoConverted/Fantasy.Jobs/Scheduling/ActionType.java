package Fantasy.Jobs.Scheduling;

public enum ActionType
{
	Template,
	Inline,
	Custom;

	public int getValue()
	{
		return this.ordinal();
	}

	public static ActionType forValue(int value)
	{
		return values()[value];
	}
}