package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

public class TransactionHandle extends SafeHandleZeroOrMinusOneIsInvalid
{


	public TransactionHandle()
	{
		super(true);

	}



	@Override
	protected boolean ReleaseHandle()
	{
		return NativeMethods.CloseHandle(super.handle);
	}
}