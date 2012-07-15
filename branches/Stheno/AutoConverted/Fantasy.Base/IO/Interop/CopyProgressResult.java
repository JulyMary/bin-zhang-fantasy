package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

//C# TO JAVA CONVERTER TODO TASK: Delegates are not available in Java:
//delegate CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

public enum CopyProgressResult 
{
	PROGRESS_CONTINUE(0),
	PROGRESS_CANCEL(1),
	PROGRESS_STOP(2),
	PROGRESS_QUIET(3);

	private int intValue;
	private static java.util.HashMap<Integer, CopyProgressResult> mappings;
	private synchronized static java.util.HashMap<Integer, CopyProgressResult> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, CopyProgressResult>();
		}
		return mappings;
	}

	private CopyProgressResult(int value)
	{
		intValue = value;
		CopyProgressResult.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static CopyProgressResult forValue(int value)
	{
		return getMappings().get(value);
	}
}