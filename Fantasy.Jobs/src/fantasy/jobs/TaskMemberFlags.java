package fantasy.jobs;



public enum TaskMemberFlags
{
	Input(1),
	Output(2),
	Required(4),
	Inline(8);

	private int intValue;
	private static java.util.HashMap<Integer, TaskMemberFlags> mappings;
	private synchronized static java.util.HashMap<Integer, TaskMemberFlags> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, TaskMemberFlags>();
		}
		return mappings;
	}

	private TaskMemberFlags(int value)
	{
		intValue = value;
		TaskMemberFlags.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static TaskMemberFlags forValue(int value)
	{
		return getMappings().get(value);
	}
}