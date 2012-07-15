package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

public enum CopyProgressCallbackReason 
{
	CALLBACK_CHUNK_FINISHED(0x00000000),
	CALLBACK_STREAM_SWITCH(0x00000001);

	private int intValue;
	private static java.util.HashMap<Integer, CopyProgressCallbackReason> mappings;
	private synchronized static java.util.HashMap<Integer, CopyProgressCallbackReason> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, CopyProgressCallbackReason>();
		}
		return mappings;
	}

	private CopyProgressCallbackReason(int value)
	{
		intValue = value;
		CopyProgressCallbackReason.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static CopyProgressCallbackReason forValue(int value)
	{
		return getMappings().get(value);
	}
}