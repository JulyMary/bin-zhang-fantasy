package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Flags]
public enum CopyFileFlags 
{
	COPY_FILE_NONE(0),
	COPY_FILE_FAIL_IF_EXISTS(0x00000001),
	COPY_FILE_RESTARTABLE(0x00000002),
	COPY_FILE_OPEN_SOURCE_FOR_WRITE(0x00000004),
	COPY_FILE_ALLOW_DECRYPTED_DESTINATION(0x00000008),
	COPY_FILE_COPY_SYMLINK(0x00000800); //NT 6.0+

	private int intValue;
	private static java.util.HashMap<Integer, CopyFileFlags> mappings;
	private synchronized static java.util.HashMap<Integer, CopyFileFlags> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, CopyFileFlags>();
		}
		return mappings;
	}

	private CopyFileFlags(int value)
	{
		intValue = value;
		CopyFileFlags.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static CopyFileFlags forValue(int value)
	{
		return getMappings().get(value);
	}
}