package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

public enum GET_FILEEX_INFO_LEVELS
{
	GetFileExInfoStandard,
	GetFileExMaxInfoLevel;

	public int getValue()
	{
		return this.ordinal();
	}

	public static GET_FILEEX_INFO_LEVELS forValue(int value)
	{
		return values()[value];
	}
}