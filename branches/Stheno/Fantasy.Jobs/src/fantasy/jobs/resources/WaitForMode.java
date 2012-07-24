package fantasy.jobs.resources;

public enum WaitForMode
{
	All(0),
	Any(1);

	private int intValue;
	private static java.util.HashMap<Integer, WaitForMode> mappings;
	private synchronized static java.util.HashMap<Integer, WaitForMode> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, WaitForMode>();
		}
		return mappings;
	}

	private WaitForMode(int value)
	{
		intValue = value;
		WaitForMode.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static WaitForMode forValue(int value)
	{
		return getMappings().get(value);
	}
}