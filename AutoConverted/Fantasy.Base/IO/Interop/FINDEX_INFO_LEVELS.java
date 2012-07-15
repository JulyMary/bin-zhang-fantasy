package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

public enum FINDEX_INFO_LEVELS
{
	FindExInfoStandard,
	FindExInfoBasic,
	FindExInfoMaxInfoLevel;

	public int getValue()
	{
		return this.ordinal();
	}

	public static FINDEX_INFO_LEVELS forValue(int value)
	{
		return values()[value];
	}
}