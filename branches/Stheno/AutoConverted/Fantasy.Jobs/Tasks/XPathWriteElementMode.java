package Fantasy.Jobs.Tasks;

import Fantasy.IO.*;

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