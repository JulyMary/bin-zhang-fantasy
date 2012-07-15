package Fantasy.Windows.Forms.Win32;

public enum SHCIDS 
{
	SHCIDS_ALLFIELDS(0x80000000), // Compare all the information contained in the ITEMIDLIST
	// structure, not just the display names
	SHCIDS_CANONICALONLY(0x10000000), // When comparing by name, compare the system names but not the
	// display names. 
	SHCIDS_BITMASK(0xFFFF0000),
	SHCIDS_COLUMNMASK(0x0000FFFF);

	private int intValue;
	private static java.util.HashMap<Integer, SHCIDS> mappings;
	private synchronized static java.util.HashMap<Integer, SHCIDS> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, SHCIDS>();
		}
		return mappings;
	}

	private SHCIDS(int value)
	{
		intValue = value;
		SHCIDS.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static SHCIDS forValue(int value)
	{
		return getMappings().get(value);
	}
}