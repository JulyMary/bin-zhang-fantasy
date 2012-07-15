package Fantasy.Windows.Forms.Win32;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ComImport, Guid("00000002-0000-0000-c000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMalloc
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	IntPtr Alloc(int cb);
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	IntPtr Realloc(IntPtr pv, int cb);
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void Free(IntPtr pv);
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int GetSize(IntPtr pv);
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int DidAlloc(IntPtr pv);
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	void HeapMinimize();
}