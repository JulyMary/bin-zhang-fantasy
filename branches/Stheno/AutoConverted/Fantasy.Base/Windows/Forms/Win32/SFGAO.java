package Fantasy.Windows.Forms.Win32;

public enum SFGAO 
{
	SFGAO_CANCOPY(0x00000001), // Objects can be copied
	SFGAO_CANMOVE(0x00000002), // Objects can be moved
	SFGAO_CANLINK(0x00000004), // Objects can be linked
	SFGAO_STORAGE(0x00000008), // supports BindToObject(IID_IStorage)
	SFGAO_CANRENAME(0x00000010), // Objects can be renamed
	SFGAO_CANDELETE(0x00000020), // Objects can be deleted
	SFGAO_HASPROPSHEET(0x00000040), // Objects have property sheets
	SFGAO_DROPTARGET(0x00000100), // Objects are drop target
	SFGAO_CAPABILITYMASK(0x00000177), // This flag is a mask for the capability flags.
	SFGAO_ENCRYPTED(0x00002000), // object is encrypted (use alt color)
	SFGAO_ISSLOW(0x00004000), // 'slow' object
	SFGAO_GHOSTED(0x00008000), // ghosted icon
	SFGAO_LINK(0x00010000), // Shortcut (link)
	SFGAO_SHARE(0x00020000), // shared
	SFGAO_READONLY(0x00040000), // read-only
	SFGAO_HIDDEN(0x00080000), // hidden object
	SFGAO_DISPLAYATTRMASK(0x000FC000), // This flag is a mask for the display attributes.
	SFGAO_FILESYSANCESTOR(0x10000000), // may contain children with SFGAO_FILESYSTEM
	SFGAO_FOLDER(0x20000000), // support BindToObject(IID_IShellFolder)
	SFGAO_FILESYSTEM(0x40000000), // is a win32 file system object (file/folder/root)
	SFGAO_HASSUBFOLDER(0x80000000), // may contain children with SFGAO_FOLDER
	SFGAO_CONTENTSMASK(0x80000000), // This flag is a mask for the contents attributes.
	SFGAO_VALIDATE(0x01000000), // invalidate cached information
	SFGAO_REMOVABLE(0x02000000), // is this removeable media?
	SFGAO_COMPRESSED(0x04000000), // Object is compressed (use alt color)
	SFGAO_BROWSABLE(0x08000000), // supports IShellFolder, but only implements CreateViewObject() (non-folder view)
	SFGAO_NONENUMERATED(0x00100000), // is a non-enumerated object
	SFGAO_NEWCONTENT(0x00200000), // should show bold in explorer tree
	SFGAO_CANMONIKER(0x00400000), // defunct
	SFGAO_HASSTORAGE(0x00400000), // defunct
	SFGAO_STREAM(0x00400000), // supports BindToObject(IID_IStream)
	SFGAO_STORAGEANCESTOR(0x00800000), // may contain children with SFGAO_STORAGE or SFGAO_STREAM
	SFGAO_STORAGECAPMASK(0x70C50008); // for determining storage capabilities, ie for open/save semantics

	private int intValue;
	private static java.util.HashMap<Integer, SFGAO> mappings;
	private synchronized static java.util.HashMap<Integer, SFGAO> getMappings()
	{
		if (mappings == null)
		{
			mappings = new java.util.HashMap<Integer, SFGAO>();
		}
		return mappings;
	}

	private SFGAO(int value)
	{
		intValue = value;
		SFGAO.getMappings().put(value, this);
	}

	public int getValue()
	{
		return intValue;
	}

	public static SFGAO forValue(int value)
	{
		return getMappings().get(value);
	}
}