package Fantasy.Windows.Forms.Win32;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("C0A651F5-B48B-11d2-B5ED-006097C686F6")]
public interface IFolderFilterSite
{
	// Exposed by a host to allow clients to pass the host their IUnknown interface pointers.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: Int32 SetFilter([MarshalAs(UnmanagedType.Interface)]Object punk);
	int SetFilter(Object punk); // A pointer to the client's IUnknown interface. To notify the host to terminate
	// filtering and stop calling your IFolderFilter interface, set this parameter to NULL. 
}