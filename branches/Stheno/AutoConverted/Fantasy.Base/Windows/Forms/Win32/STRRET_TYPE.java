package Fantasy.Windows.Forms.Win32;

public enum STRRET_TYPE
{
	STRRET_WSTR(0x0000), // Use STRRET.pOleStr
	STRRET_OFFSET(0x0001), // Use STRRET.uOffset to Ansi
	STRRET_CSTR(0x0002); // Use STRRET.cStr

	private int intValue;
	private static java.util.HashMap<Integer, STRRET_TYPE> mappings;
	private synchronized static java.util.HashMap<Integer, STRRET_TYPE> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, STRRET_TYPE>();
		}
		return mappings;
	}

	private STRRET_TYPE(int value)
	{
		intValue = value;
		STRRET_TYPE.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static STRRET_TYPE forValue(int value)
	{
		return getMappings().get(value);
	}
}