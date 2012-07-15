package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Flags]
public enum MoveFileFlags
{
	MOVEFILE_REPLACE_EXISTING(0x00000001),
	MOVEFILE_COPY_ALLOWED(0x00000002),
	MOVEFILE_DELAY_UNTIL_REBOOT(0x00000004),
	MOVEFILE_WRITE_THROUGH(0x00000008),
	MOVEFILE_CREATE_HARDLINK(0x00000010),
	MOVEFILE_FAIL_IF_NOT_TRACKABLE(0x00000020);

	private int intValue;
	private static java.util.HashMap<Integer, MoveFileFlags> mappings;
	private synchronized static java.util.HashMap<Integer, MoveFileFlags> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, MoveFileFlags>();
		}
		return mappings;
	}

	private MoveFileFlags(int value)
	{
		intValue = value;
		MoveFileFlags.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static MoveFileFlags forValue(int value)
	{
		return getMappings().get(value);
	}
}