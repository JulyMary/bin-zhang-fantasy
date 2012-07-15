package Fantasy.Jobs.Scheduling;

public enum InstancesPolicy
{
	Parallel,
	Queue,
	IgnoreNew,
	StopExisting;

	public int getValue()
	{
		return this.ordinal();
	}

	public static InstancesPolicy forValue(int value)
	{
		return values()[value];
	}
}