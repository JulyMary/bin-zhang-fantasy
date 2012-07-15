package Fantasy.Windows.Forms.Win32;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ComVisible(true), Guid("3766C955-DA6F-4fbc-AD36-311E342EF180")]
public class FilterByExtension implements IFolderFilter
{
	// Allows a client to specify which individual items should be enumerated.
	// Note: The host calls this method for each item in the folder. Return S_OK, to have the item enumerated. 
	// Return S_FALSE to prevent the item from being enumerated.
	public final int ShouldShow(Object psf, IntPtr pidlFolder, IntPtr pidlItem) // The item's PIDL. -  The folder's PIDL. -  A pointer to the folder's IShellFolder interface.
	{
		// check extension, and if not ok return 1 (S_FALSE)

		// get display name of item
		IShellFolder isf = (IShellFolder)psf;

		STRRET ptrDisplayName = new STRRET();
		RefObject<STRRET> tempRef_ptrDisplayName = new RefObject<STRRET>(ptrDisplayName);
		isf.getGetDisplayNameOf()(pidlItem, SHGNO.SHGDN_NORMAL.getValue() | SHGNO.SHGDN_FORPARSING.getValue(), tempRef_ptrDisplayName);
		ptrDisplayName = tempRef_ptrDisplayName.argvalue;

		String sDisplay = null;
		RefObject<STRRET> tempRef_ptrDisplayName2 = new RefObject<STRRET>(ptrDisplayName);
		RefObject<String> tempRef_sDisplay = new RefObject<String>(sDisplay);
		ShellApi.StrRetToBSTR(tempRef_ptrDisplayName2, (IntPtr)0, tempRef_sDisplay);
		ptrDisplayName = tempRef_ptrDisplayName2.argvalue;
		sDisplay = tempRef_sDisplay.argvalue;

		// check if item is file or folder
		IntPtr[] aPidl = new IntPtr[1];
		aPidl[0] = pidlItem;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: uint Attrib;
		int Attrib;
		Attrib = SFGAO.SFGAO_FOLDER.getValue();

		int temp;
		RefObject<Integer> tempRef_Attrib = new RefObject<Integer>(Attrib);
		temp = isf.getGetAttributesOf()(1, aPidl, tempRef_Attrib);
		Attrib = tempRef_Attrib.argvalue;

		// if item is a folder, accept
		if ((Attrib & SFGAO.SFGAO_FOLDER.getValue()) == SFGAO.SFGAO_FOLDER.getValue())
		{
			return 0;
		}

		// if item is file, check if it has a valid extension
		for (int i=0 ; i<ValidExtension.length ; i++)
		{
			if (sDisplay.toUpperCase().endsWith("." + ValidExtension[i].toUpperCase()))
			{
				return 0;
			}
		}

		return 1;
	}

	// Allows a client to specify which classes of objects in a Shell folder should be enumerated.
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public Int32 GetEnumFlags(Object psf, IntPtr pidlFolder, IntPtr phwnd, out UInt32 pgrfFlags)
	public final int GetEnumFlags(Object psf, IntPtr pidlFolder, IntPtr phwnd, RefObject<Integer> pgrfFlags) // One or more SHCONTF values that specify which classes of objects to enumerate. -  A pointer to the host's window handle. -  The folder's PIDL. -  A pointer to the folder's IShellFolder interface.
	{
		pgrfFlags.argvalue = SHCONTF.SHCONTF_FOLDERS.getValue() | SHCONTF.SHCONTF_NONFOLDERS.getValue();
		return 0;
	}

	public String[] ValidExtension;
}