package fantasy.jobs.tasks;

public enum XPathWriteElementMode
{
	Add,
	AddFirst,
	AddAfterSelf,
	AddBeforeSelf,
	ReplaceWith,
	ReplaceAll;

	public int getValue()
	{
		return this.ordinal();
	}

	public static XPathWriteElementMode forValue(int value)
	{
		return values()[value];
	}
}