package Fantasy.Windows.Forms.Win32;

public class ShellApi
{


	// Contains parameters for the SHBrowseForFolder function and receives information about the folder selected 


	// Retrieves a pointer to the Shell's IMalloc interface.
	public static native int SHGetMalloc(RefObject<IntPtr> hObject);
	static
	{
		System.loadLibrary("shell32.dll");
	}

	// Retrieves the path of a folder as an PIDL.
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public static extern Int32 SHGetFolderLocation(IntPtr hwndOwner, Int32 nFolder, IntPtr hToken, UInt32 dwReserved, out IntPtr ppidl);
	public static native int SHGetFolderLocation(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwReserved, RefObject<IntPtr> ppidl);
								// specifying the folder's location relative to the root of the namespace 
								// (the desktop). 

	// Converts an item identifier list to a file system path. 
								// relative to the root of the namespace (the desktop). 
	public static native int SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);


	// Takes the CSIDL of a folder and returns the pathname.
									// the folder associated with a CSIDL may be moved or renamed by the user. 
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public static extern Int32 SHGetFolderPath(IntPtr hwndOwner, Int32 nFolder, IntPtr hToken, UInt32 dwFlags, StringBuilder pszPath);
	public static native int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder pszPath);

	// Translates a Shell namespace object's display name into an item identifier list and returns the attributes 
	// of the object. This function is the preferred method to convert a string to a pointer to an item 
	// identifier list (PIDL). 
									// to parse. 
									// is normally set to NULL.
									// identifier list for the object.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: public static extern Int32 SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] String pszName, IntPtr pbc, out IntPtr ppidl, UInt32 sfgaoIn, out UInt32 psfgaoOut);
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
	public static native int SHParseDisplayName(String pszName, IntPtr pbc, RefObject<IntPtr> ppidl, int sfgaoIn, RefObject<Integer> psfgaoOut);
									// object and were requested in sfgaoIn will be set. 


	// Retrieves the IShellFolder interface for the desktop folder, which is the root of the Shell's namespace. 
	public static native int SHGetDesktopFolder(RefObject<IntPtr> ppshf);
									// desktop folder.

	// This function takes the fully-qualified pointer to an item identifier list (PIDL) of a namespace object, 
	// and returns a specified interface pointer on the parent object.
								// you are finished. 
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: public static extern Int32 SHBindToParent(IntPtr pidl, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IntPtr ppv, ref IntPtr ppidlLast);
	public static native int SHBindToParent(IntPtr pidl, Guid riid, RefObject<IntPtr> ppv, RefObject<IntPtr> ppidlLast);
								// of the methods supported by the parent folder's interfaces. If you set ppidlLast 
								// to NULL, the PIDL will not be returned. 

	// Accepts a STRRET structure returned by IShellFolder::GetDisplayNameOf that contains or points to a 
	// string, and then returns that string as a BSTR.
								// to the parent folder.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: public static extern Int32 StrRetToBSTR(ref STRRET pstr, IntPtr pidl, [MarshalAs(UnmanagedType.BStr)] out String pbstr);
	public static native int StrRetToBSTR(RefObject<STRRET> pstr, IntPtr pidl, RefObject<String> pbstr);
	static
	{
		System.loadLibrary("shlwapi.dll");
	}

	// Takes a STRRET structure returned by IShellFolder::GetDisplayNameOf, converts it to a string, and 
	// places the result in a buffer. 
								// longer be valid.
								// string. If cchBuf is too small, the name will be truncated to fit. 
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public static extern Int32 StrRetToBuf(ref STRRET pstr, IntPtr pidl, StringBuilder pszBuf, UInt32 cchBuf);
	public static native int StrRetToBuf(RefObject<STRRET> pstr, IntPtr pidl, StringBuilder pszBuf, int cchBuf);
								// truncated to fit. 



	// Displays a dialog box that enables the user to select a Shell folder. 
	public static native IntPtr SHBrowseForFolder(RefObject<BROWSEINFO> lbpi);
								// the dialog box. 



	public static short GetHResultCode(int hr)
	{
		hr = hr & 0x0000ffff;
		return (short)hr;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: public static extern IntPtr SHBrowseForFolder([In] BROWSEINFO lpbi);
	public static native IntPtr SHBrowseForFolder(BROWSEINFO lpbi);
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: public static extern int SHGetMalloc([Out, MarshalAs(UnmanagedType.LPArray)] IMalloc[] ppMalloc);
	public static native int SHGetMalloc(IMalloc[] ppMalloc);
	public static native boolean SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);
	public static native int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, RefObject<IntPtr> ppidl);

}