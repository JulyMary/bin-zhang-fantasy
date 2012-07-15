package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

//[StructLayout(LayoutKind.Sequential)]
//public struct SECURITY_ATTRIBUTES
//{
//    int nLength;
//    IntPtr lpSecurityDescriptor;
//    int bInheritHandle;
//}

public enum TXFS_MINIVERSION 
{
	TXFS_MINIVERSION_COMMITTED_VIEW(0x0000),
	TXFS_MINIVERSION_DIRTY_VIEW(0xFFFF),
	TXFS_MINIVERSION_DEFAULT_VIEW(0xFFFE);

	private int intValue;
	private static java.util.HashMap<Integer, TXFS_MINIVERSION> mappings;
	private synchronized static java.util.HashMap<Integer, TXFS_MINIVERSION> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, TXFS_MINIVERSION>();
		}
		return mappings;
	}

	private TXFS_MINIVERSION(int value)
	{
		intValue = value;
		TXFS_MINIVERSION.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static TXFS_MINIVERSION forValue(int value)
	{
		return getMappings().get(value);
	}
}