package Fantasy.Windows.Forms.Win32;

public enum SHGFP_TYPE
{
	SHGFP_TYPE_CURRENT(0), // current value for user, verify it exists
	SHGFP_TYPE_DEFAULT(1); // default value, may not exist

	private int intValue;
	private static java.util.HashMap<Integer, SHGFP_TYPE> mappings;
	private synchronized static java.util.HashMap<Integer, SHGFP_TYPE> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, SHGFP_TYPE>();
		}
		return mappings;
	}

	private SHGFP_TYPE(int value)
	{
		intValue = value;
		SHGFP_TYPE.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static SHGFP_TYPE forValue(int value)
	{
		return getMappings().get(value);
	}
}