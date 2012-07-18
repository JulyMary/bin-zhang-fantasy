package fantasy.servicemodel;

public enum MessageImportance
{
	High,
	Normal,
	Low;

	public int getValue()
	{
		return this.ordinal();
	}

	public static MessageImportance forValue(int value)
	{
		return values()[value];
	}
}