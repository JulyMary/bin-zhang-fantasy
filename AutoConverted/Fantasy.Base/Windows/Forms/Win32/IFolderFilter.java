package Fantasy.Windows.Forms.Win32;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("9CC22886-DC8E-11d2-B1D0-00C04F8EEB3E")]
public interface IFolderFilter
{
	// Allows a client to specify which individual items should be enumerated.
	// Note: The host calls this method for each item in the folder. Return S_OK, to have the item enumerated. 
	// Return S_FALSE to prevent the item from being enumerated.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: Int32 ShouldShow([MarshalAs(UnmanagedType.Interface)]Object psf, IntPtr pidlFolder, IntPtr pidlItem);
	int ShouldShow(Object psf, IntPtr pidlFolder, IntPtr pidlItem); // The item's PIDL. -  The folder's PIDL. -  A pointer to the folder's IShellFolder interface.

	// Allows a client to specify which classes of objects in a Shell folder should be enumerated.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: Int32 GetEnumFlags([MarshalAs(UnmanagedType.Interface)]Object psf, IntPtr pidlFolder, IntPtr phwnd, out UInt32 pgrfFlags);
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
	int GetEnumFlags(Object psf, IntPtr pidlFolder, IntPtr phwnd, RefObject<Integer> pgrfFlags); // One or more SHCONTF values that specify which classes of objects to enumerate. -  A pointer to the host's window handle. -  The folder's PIDL. -  A pointer to the folder's IShellFolder interface.

}