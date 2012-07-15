package Fantasy.Windows.Forms.Win32;

public enum SHCONTF
{
	SHCONTF_FOLDERS(0x0020), // only want folders enumerated (SFGAO_FOLDER)
	SHCONTF_NONFOLDERS(0x0040), // include non folders
	SHCONTF_INCLUDEHIDDEN(0x0080), // show items normally hidden
	SHCONTF_INIT_ON_FIRST_NEXT(0x0100), // allow EnumObject() to return before validating enum
	SHCONTF_NETPRINTERSRCH(0x0200), // hint that client is looking for printers
	SHCONTF_SHAREABLE(0x0400), // hint that client is looking sharable resources (remote shares)
	SHCONTF_STORAGE(0x0800); // include all items with accessible storage and their ancestors

	private int intValue;
	private static java.util.HashMap<Integer, SHCONTF> mappings;
	private synchronized static java.util.HashMap<Integer, SHCONTF> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, SHCONTF>();
		}
		return mappings;
	}

	private SHCONTF(int value)
	{
		intValue = value;
		SHCONTF.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static SHCONTF forValue(int value)
	{
		return getMappings().get(value);
	}
}