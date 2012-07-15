package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

//     Copyright (c) Microsoft Corporation.  All rights reserved.

public final class SafeFindHandle extends SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeFindHandle()
	{
		super(true);
	}

	@Override
	protected boolean ReleaseHandle()
	{
		return NativeMethods.FindClose(super.handle);
	}
}