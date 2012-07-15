package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("79427A2B-F895-40e0-BE79-B57DC82ED231")]
public interface IKernelTransaction
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: void GetHandle([Out] out TransactionHandle handle);
	void GetHandle(out TransactionHandle handle);
}