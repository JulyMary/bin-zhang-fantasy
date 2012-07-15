package Fantasy.Jobs.Management;

public enum ServiceType
{
	Standalone,
	Solar,
	Satellite,
	Mesh;

	public int getValue()
	{
		return this.ordinal();
	}

	public static ServiceType forValue(int value)
	{
		return values()[value];
	}
}