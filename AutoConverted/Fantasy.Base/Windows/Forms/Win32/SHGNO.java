package Fantasy.Windows.Forms.Win32;

public enum SHGNO
{
	SHGDN_NORMAL(0x0000), // default (display purpose)
	SHGDN_INFOLDER(0x0001), // displayed under a folder (relative)
	SHGDN_FOREDITING(0x1000), // for in-place editing
	SHGDN_FORADDRESSBAR(0x4000), // UI friendly parsing name (remove ugly stuff)
	SHGDN_FORPARSING(0x8000); // parsing name for ParseDisplayName()

	private int intValue;
	private static java.util.HashMap<Integer, SHGNO> mappings;
	private synchronized static java.util.HashMap<Integer, SHGNO> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, SHGNO>();
		}
		return mappings;
	}

	private SHGNO(int value)
	{
		intValue = value;
		SHGNO.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static SHGNO forValue(int value)
	{
		return getMappings().get(value);
	}
}