package Fantasy.IO;

import Fantasy.ServiceModel.*;
import Fantasy.IO.Interop.*;

public class CopyProgressRoutineHelper
{
	private IProgressMonitor _owner;

	private CopyProgressRoutineHelper(IProgressMonitor owner)
	{
		if (owner == null)
		{
			throw new ArgumentNullException("owner");
		}
		this._owner = owner;
	}


//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
	private CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, int dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
	{
		if (dwCallbackReason == CopyProgressCallbackReason.CALLBACK_STREAM_SWITCH)
		{
			this._owner.setMinimum(0);
			this._owner.setMaximum(100);
			this._owner.setValue(0);
		}

		else
		{
			int value = (int)(Math.round((double)TotalBytesTransferred / (double)TotalFileSize)) * 100;
			this._owner.setValue(value);
		}
		return CopyProgressResult.PROGRESS_CONTINUE;
	}


	public static CopyProgressRoutine Create(IProgressMonitor monitor)
	{
		CopyProgressRoutineHelper helper = new CopyProgressRoutineHelper(monitor);
		CopyProgressRoutine rs = new CopyProgressRoutine(helper.CopyProgressRoutine);
		return rs;

	}


}