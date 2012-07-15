package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

public enum FINDEX_SEARCH_OPS
{
	FindExSearchNameMatch,
	FindExSearchLimitToDirectories,
	FindExSearchLimitToDevices;

	public int getValue()
	{
		return this.ordinal();
	}

	public static FINDEX_SEARCH_OPS forValue(int value)
	{
		return values()[value];
	}
}