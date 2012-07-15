package Fantasy.Windows.Forms.Win32;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214E6-0000-0000-C000-000000000046")]
public interface IShellFolder
{
	// Translates a file object's or folder's display name into an item identifier list.
	// Return value: error code, if any
		// normally set to NULL. 
		// display name that was parsed.
		// the object.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: Int32 ParseDisplayName(IntPtr hwnd, IntPtr pbc, [MarshalAs(UnmanagedType.LPWStr)] String pszDisplayName, ref UInt32 pchEaten, out IntPtr ppidl, ref UInt32 pdwAttributes);
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
	int ParseDisplayName(IntPtr hwnd, IntPtr pbc, String pszDisplayName, RefObject<Integer> pchEaten, RefObject<IntPtr> ppidl, RefObject<Integer> pdwAttributes); // Optional parameter that can be used to query for file attributes. -  Pointer to an ITEMIDLIST pointer that receives the item identifier list for -  Pointer to a ULONG value that receives the number of characters of the -  Null-terminated UNICODE string with the display name. -  Optional bind context that controls the parsing operation. This parameter is -  Optional window handle
	// this can be values from the SFGAO enum

	// Allows a client to determine the contents of a folder by creating an item identifier enumeration object 
	// and returning its IEnumIDList interface.
	// Return value: error code, if any
		// should be used by the enumeration object as the parent window to take 
		// user input.
		// of possible values, see the SHCONTF enum. 
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int EnumObjects(IntPtr hwnd, int grfFlags, RefObject<IntPtr> ppenumIDList); // Address that receives a pointer to the IEnumIDList interface of the -  Flags indicating which items to include in the enumeration. For a list -  If user input is required to perform the enumeration, this window handle
	// enumeration object created by this method. 

	// Retrieves an IShellFolder object for a subfolder.
	// Return value: error code, if any
		// used during this operation.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int BindToObject(IntPtr pidl, IntPtr pbc, Guid riid, RefObject<IntPtr> ppv); // Address that receives the interface pointer. -  Identifier of the interface to return. -  Optional address of an IBindCtx interface on a bind context object to be -  Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.

	// Requests a pointer to an object's storage interface. 
	// Return value: error code, if any
		// to its parent folder. 
		// used during this operation.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int BindToStorage(IntPtr pidl, IntPtr pbc, Guid riid, RefObject<IntPtr> ppv); // Address that receives the interface pointer specified by riid. -  Interface identifier (IID) of the requested storage interface. -  Optional address of an IBindCtx interface on a bind context object to be -  Address of an ITEMIDLIST structure that identifies the subfolder relative

	// Determines the relative order of two file objects or folders, given their item identifier lists.
	// Return value: If this method is successful, the CODE field of the HRESULT contains one of the following 
	// values (the code can be retrived using the helper function GetHResultCode):
	// Negative A negative return value indicates that the first item should precede the second (pidl1 < pidl2). 
	// Positive A positive return value indicates that the first item should follow the second (pidl1 > pidl2). 
	// Zero A return value of zero indicates that the two items are the same (pidl1 = pidl2). 
		// sixteen bits of lParam define the sorting rule. The upper sixteen bits of 
		// lParam are used for flags that modify the sorting rule. values can be from 
		// the SHCIDS enum
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int CompareIDs(int lParam, IntPtr pidl1, IntPtr pidl2); // Pointer to the second item's ITEMIDLIST structure. -  Pointer to the first item's ITEMIDLIST structure. -  Value that specifies how the comparison should be performed. The lower

	// Requests an object that can be used to obtain information from or interact with a folder object.
	// Return value: error code, if any
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	int CreateViewObject(IntPtr hwndOwner, Guid riid, RefObject<IntPtr> ppv); // Address of a pointer to the requested interface. -  Identifier of the requested interface. -  Handle to the owner window.

	// Retrieves the attributes of one or more file objects or subfolders. 
	// Return value: error code, if any
		// uniquely identifies a file object relative to the parent folder.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: Int32 GetAttributesOf(UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IntPtr[] apidl, ref UInt32 rgfInOut);
	int GetAttributesOf(int cidl, IntPtr[] apidl, RefObject<Integer> rgfInOut); // Address of a single ULONG value that, on entry, contains the attributes that -  Address of an array of pointers to ITEMIDLIST structures, each of which -  Number of file objects from which to retrieve attributes.
	// the caller is requesting. On exit, this value contains the requested 
	// attributes that are common to all of the specified objects. this value can
	// be from the SFGAO enum

	// Retrieves an OLE interface that can be used to carry out actions on the specified file objects or folders.
	// Return value: error code, if any
		// a dialog box or message box.
		// uniquely identifies a file object or subfolder relative to the parent folder.
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: Int32 GetUIObjectOf(IntPtr hwndOwner, UInt32 cidl, IntPtr[] apidl, Guid riid, ref UInt32 rgfReserved, out IntPtr ppv);
	int GetUIObjectOf(IntPtr hwndOwner, int cidl, IntPtr[] apidl, Guid riid, RefObject<Integer> rgfReserved, RefObject<IntPtr> ppv); // Pointer to the requested interface. -  Reserved. -  Identifier of the COM interface object to return. -  Address of an array of pointers to ITEMIDLIST structures, each of which -  Number of file objects or subfolders specified in the apidl parameter. -  Handle to the owner window that the client should specify if it displays

	// Retrieves the display name for the specified file object or subfolder. 
	// Return value: error code, if any
		// object or subfolder relative to the parent folder. 
		// possible values, see the SHGNO enum. 
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: Int32 GetDisplayNameOf(IntPtr pidl, UInt32 uFlags, out STRRET pName);
	int GetDisplayNameOf(IntPtr pidl, int uFlags, RefObject<STRRET> pName); // Address of a STRRET structure in which to return the display name. -  Flags used to request the type of display name to return. For a list of -  Address of an ITEMIDLIST structure (PIDL) that uniquely identifies the file

	// Sets the display name of a file object or subfolder, changing the item identifier in the process.
	// Return value: error code, if any
		// displays.
		// or subfolder relative to the parent folder. 
		// a list of possible values, see the description of the SHGNO enum. 
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[PreserveSig]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: Int32 SetNameOf(IntPtr hwnd, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] String pszName, UInt32 uFlags, out IntPtr ppidlOut);
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
	int SetNameOf(IntPtr hwnd, IntPtr pidl, String pszName, int uFlags, RefObject<IntPtr> ppidlOut); // Address of a pointer to an ITEMIDLIST structure which receives the new ITEMIDLIST. -  Flags indicating the type of name specified by the lpszName parameter. For -  Pointer to a null-terminated string that specifies the new display name. -  Pointer to an ITEMIDLIST structure that uniquely identifies the file object -  Handle to the owner window of any dialog or message boxes that the client
}