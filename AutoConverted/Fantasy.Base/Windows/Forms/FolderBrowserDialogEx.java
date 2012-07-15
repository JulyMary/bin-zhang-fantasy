package Fantasy.Windows.Forms;

import Fantasy.Windows.Forms.Win32.*;

// FolderBrowserDialogEx.cs
//
// A replacement for the builtin System.Windows.Forms.FolderBrowserDialog class.
// This one includes an edit box, and also displays the full path in the edit box. 
//
// based on code from http://support.microsoft.com/default.aspx?scid=kb;[LN];306285 
// 
// 20 Feb 2009
//
// ========================================================================================
// Example usage:
// 
// string _folderName = "c:\\dinoch";
// private void button1_Click(object sender, EventArgs e)
// {
//     _folderName = (System.IO.Directory.Exists(_folderName)) ? _folderName : "";
//     var dlg1 = new Ionic.Utils.FolderBrowserDialogEx
//     {
//         Description = "Select a folder for the extracted files:",
//         ShowNewFolderButton = true,
//         ShowEditBox = true,
//         //NewStyle = false,
//         SelectedPath = _folderName,
//         ShowFullPathInEditBox= false,
//     };
//     dlg1.RootFolder = System.Environment.SpecialFolder.MyComputer;
// 
//     var result = dlg1.ShowDialog();
// 
//     if (result == DialogResult.OK)
//     {
//         _folderName = dlg1.SelectedPath;
//         this.label1.Text = "The folder selected was: ";
//         this.label2.Text = _folderName;
//     }
// }
//



//[Designer("System.Windows.Forms.Design.FolderBrowserDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), DefaultEvent("HelpRequest"), SRDescription("DescriptionFolderBrowserDialog"), DefaultProperty("SelectedPath")]
public class FolderBrowserDialogEx extends System.Windows.Forms.CommonDialog
{
	private static final int MAX_PATH = 260;

	// Fields
	private BrowseFolderCallbackProc _callback;
	private String _descriptionText;
	private Environment.SpecialFolder _rootFolder;
	private String _selectedPath;
	private boolean _selectedPathNeedsCheck;
	private boolean _showNewFolderButton;
	private boolean _showEditBox;
	private boolean _showBothFilesAndFolders;
	private boolean _newStyle = true;
	private boolean _showFullPathInEditBox = true;
	private boolean _dontIncludeNetworkFoldersBelowDomainLevel;
	private int _uiFlags;
	private IntPtr _hwndEdit = new IntPtr();
	private IntPtr _rootFolderLocation = new IntPtr();

	// Events
	//[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public new event EventHandler HelpRequest
//		{
//			add
//			{
//				super.HelpRequest += value;
//			}
//			remove
//			{
//				super.HelpRequest -= value;
//			}
//		}

	// ctor
	public FolderBrowserDialogEx()
	{
		this.Reset();
	}

	// Factory Methods
	public static FolderBrowserDialogEx PrinterBrowser()
	{
		FolderBrowserDialogEx x = new FolderBrowserDialogEx();
		// avoid MBRO comppiler warning when passing _rootFolderLocation as a ref:
		x.BecomePrinterBrowser();
		return x;
	}

	public static FolderBrowserDialogEx ComputerBrowser()
	{
		FolderBrowserDialogEx x = new FolderBrowserDialogEx();
		// avoid MBRO comppiler warning when passing _rootFolderLocation as a ref:
		x.BecomeComputerBrowser();
		return x;
	}


	// Helpers
	private void BecomePrinterBrowser()
	{
		_uiFlags += BrowseFlags.BIF_BROWSEFORPRINTER;
		setDescription("Select a printer:");
		RefObject<IntPtr> tempRef__rootFolderLocation = new RefObject<IntPtr>(this._rootFolderLocation);
		ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.PRINTERS, tempRef__rootFolderLocation);
		this._rootFolderLocation = tempRef__rootFolderLocation.argvalue;
		setShowNewFolderButton(false);
		setShowEditBox(false);
	}

	private void BecomeComputerBrowser()
	{
		_uiFlags += BrowseFlags.BIF_BROWSEFORCOMPUTER;
		setDescription("Select a computer:");
		RefObject<IntPtr> tempRef__rootFolderLocation = new RefObject<IntPtr>(this._rootFolderLocation);
		ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.NETWORK, tempRef__rootFolderLocation);
		this._rootFolderLocation = tempRef__rootFolderLocation.argvalue;
		setShowNewFolderButton(false);
		setShowEditBox(false);
	}


	private static class CSIDL
	{
		public static final int PRINTERS = 4;
		public static final int NETWORK = 0x12;
	}

	private static class BrowseFlags
	{
		public static final int BIF_DEFAULT = 0x0000;
		public static final int BIF_BROWSEFORCOMPUTER = 0x1000;
		public static final int BIF_BROWSEFORPRINTER = 0x2000;
		public static final int BIF_BROWSEINCLUDEFILES = 0x4000;
		public static final int BIF_BROWSEINCLUDEURLS = 0x0080;
		public static final int BIF_DONTGOBELOWDOMAIN = 0x0002;
		public static final int BIF_EDITBOX = 0x0010;
		public static final int BIF_NEWDIALOGSTYLE = 0x0040;
		public static final int BIF_NONEWFOLDERBUTTON = 0x0200;
		public static final int BIF_RETURNFSANCESTORS = 0x0008;
		public static final int BIF_RETURNONLYFSDIRS = 0x0001;
		public static final int BIF_SHAREABLE = 0x8000;
		public static final int BIF_STATUSTEXT = 0x0004;
		public static final int BIF_UAHINT = 0x0100;
		public static final int BIF_VALIDATE = 0x0020;
		public static final int BIF_NOTRANSLATETARGETS = 0x0400;
	}

	private static class BrowseForFolderMessages
	{
		// messages FROM the folder browser
		public static final int BFFM_INITIALIZED = 1;
		public static final int BFFM_SELCHANGED = 2;
		public static final int BFFM_VALIDATEFAILEDA = 3;
		public static final int BFFM_VALIDATEFAILEDW = 4;
		public static final int BFFM_IUNKNOWN = 5;

		// messages TO the folder browser
		public static final int BFFM_SETSTATUSTEXT = 0x464;
		public static final int BFFM_ENABLEOK = 0x465;
		public static final int BFFM_SETSELECTIONA = 0x466;
		public static final int BFFM_SETSELECTIONW = 0x467;
	}

	private int FolderBrowserCallback(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData)
	{
		switch (msg)
		{
			case BrowseForFolderMessages.BFFM_IUNKNOWN:
					{
						this.SetExtensionFilter(lParam);
					}
					break;

			case BrowseForFolderMessages.BFFM_INITIALIZED:
				if (this._selectedPath.length() != 0)
				{
					User32.SendMessage(new HandleRef(null, hwnd), BrowseForFolderMessages.BFFM_SETSELECTIONW, 1, this._selectedPath);
					if (this._showEditBox && this._showFullPathInEditBox)
					{
						// get handle to the Edit box inside the Folder Browser Dialog
						_hwndEdit = User32.FindWindowEx(new HandleRef(null, hwnd), IntPtr.Zero, "Edit", null);
						User32.SetWindowText(_hwndEdit, this._selectedPath);
					}
				}
				break;

			case BrowseForFolderMessages.BFFM_SELCHANGED:
				IntPtr pidl = lParam;
				if (pidl != IntPtr.Zero)
				{
					if (((_uiFlags & BrowseFlags.BIF_BROWSEFORPRINTER) == BrowseFlags.BIF_BROWSEFORPRINTER) || ((_uiFlags & BrowseFlags.BIF_BROWSEFORCOMPUTER) == BrowseFlags.BIF_BROWSEFORCOMPUTER))
					{
						// we're browsing for a printer or computer, enable the OK button unconditionally.
						User32.SendMessage(new HandleRef(null, hwnd), BrowseForFolderMessages.BFFM_ENABLEOK, 0, 1);
					}
					else
					{
						IntPtr pszPath = Marshal.AllocHGlobal(MAX_PATH * Marshal.SystemDefaultCharSize);
						boolean haveValidPath = ShellApi.SHGetPathFromIDList(pidl, pszPath);
						String displayedPath = Marshal.PtrToStringAuto(pszPath);
						Marshal.FreeHGlobal(pszPath);
						// whether to enable the OK button or not. (if file is valid)
						User32.SendMessage(new HandleRef(null, hwnd), BrowseForFolderMessages.BFFM_ENABLEOK, 0, haveValidPath ? 1 : 0);

						// Maybe set the Edit Box text to the Full Folder path
						if (haveValidPath && !DotNetToJavaStringHelper.isNullOrEmpty(displayedPath))
						{
							if (_showEditBox && _showFullPathInEditBox)
							{
								if (_hwndEdit != IntPtr.Zero)
								{
									User32.SetWindowText(_hwndEdit, displayedPath);
								}
							}

							if ((_uiFlags & BrowseFlags.BIF_STATUSTEXT) == BrowseFlags.BIF_STATUSTEXT)
							{
								User32.SendMessage(new HandleRef(null, hwnd), BrowseForFolderMessages.BFFM_SETSTATUSTEXT, 0, displayedPath);
							}
						}
					}
				}
				break;
		}
		return 0;
	}

	private void SetExtensionFilter(IntPtr iunknown)
	{
		if (this.getExtensions() != null && this.getExtensions().length > 0 && iunknown != IntPtr.Zero)
		{
			IntPtr iFolderFilterSite = new IntPtr();

			Guid iid_IFolderFilterSite = IFolderFilterSite.class.GUID;

			RefObject<Guid> tempRef_iid_IFolderFilterSite = new RefObject<Guid>(iid_IFolderFilterSite);
			RefObject<IntPtr> tempRef_iFolderFilterSite = new RefObject<IntPtr>(iFolderFilterSite);
			Marshal.QueryInterface(iunknown, tempRef_iid_IFolderFilterSite, tempRef_iFolderFilterSite);
			iid_IFolderFilterSite = tempRef_iid_IFolderFilterSite.argvalue;
			iFolderFilterSite = tempRef_iFolderFilterSite.argvalue;

			Object obj = Marshal.GetTypedObjectForIUnknown(iFolderFilterSite, IFolderFilterSite.class);
			IFolderFilterSite folderFilterSite = (IFolderFilterSite)obj;

			FilterByExtension filter = new FilterByExtension();
			filter.ValidExtension = this.getExtensions();

			folderFilterSite.getSetFilter()(filter);
		}
	}

	private static IMalloc GetSHMalloc()
	{
		IMalloc[] ppMalloc = new IMalloc[1];
		ShellApi.SHGetMalloc(ppMalloc);
		return ppMalloc[0];
	}

	@Override
	public void Reset()
	{
		this._rootFolder = (Environment.SpecialFolder)0;
		this._descriptionText = "";
		this._selectedPath = "";
		this._selectedPathNeedsCheck = false;
		this._showNewFolderButton = true;
		this._showEditBox = true;
		this._newStyle = true;
		this._dontIncludeNetworkFoldersBelowDomainLevel = false;
		this._hwndEdit = IntPtr.Zero;
		this._rootFolderLocation = IntPtr.Zero;
	}

	@Override
	protected boolean RunDialog(IntPtr hWndOwner)
	{
		boolean result = false;
		if (_rootFolderLocation == IntPtr.Zero)
		{
			RefObject<IntPtr> tempRef__rootFolderLocation = new RefObject<IntPtr>(_rootFolderLocation);
			ShellApi.SHGetSpecialFolderLocation(hWndOwner, (int)this._rootFolder, tempRef__rootFolderLocation);
			_rootFolderLocation = tempRef__rootFolderLocation.argvalue;
			if (_rootFolderLocation == IntPtr.Zero)
			{
				RefObject<IntPtr> tempRef__rootFolderLocation2 = new RefObject<IntPtr>(_rootFolderLocation);
				ShellApi.SHGetSpecialFolderLocation(hWndOwner, 0, tempRef__rootFolderLocation2);
				_rootFolderLocation = tempRef__rootFolderLocation2.argvalue;
				if (_rootFolderLocation == IntPtr.Zero)
				{
					throw new InvalidOperationException("FolderBrowserDialogNoRootFolder");
				}
			}
		}
		_hwndEdit = IntPtr.Zero;
		//_uiFlags = 0;
		if (_dontIncludeNetworkFoldersBelowDomainLevel)
		{
			_uiFlags += BrowseFlags.BIF_DONTGOBELOWDOMAIN;
		}
		if (this._newStyle)
		{
			_uiFlags += BrowseFlags.BIF_NEWDIALOGSTYLE;
		}
		if (!this._showNewFolderButton)
		{
			_uiFlags += BrowseFlags.BIF_NONEWFOLDERBUTTON;
		}
		if (this._showEditBox)
		{
			_uiFlags += BrowseFlags.BIF_EDITBOX;
		}
		if (this._showBothFilesAndFolders)
		{
			_uiFlags += BrowseFlags.BIF_BROWSEINCLUDEFILES;
		}


		if (Control.CheckForIllegalCrossThreadCalls && (Application.OleRequired() != ApartmentState.STA))
		{
			throw new ThreadStateException("DebuggingException: ThreadMustBeSTA");
		}
		IntPtr pidl = IntPtr.Zero;
		IntPtr hglobal = IntPtr.Zero;
		IntPtr pszPath = IntPtr.Zero;
		try
		{
			BROWSEINFO browseInfo = new BROWSEINFO();
			hglobal = Marshal.AllocHGlobal(MAX_PATH * Marshal.SystemDefaultCharSize);
			pszPath = Marshal.AllocHGlobal(MAX_PATH * Marshal.SystemDefaultCharSize);
			this._callback = new BrowseFolderCallbackProc(this.FolderBrowserCallback);
			browseInfo.pidlRoot = _rootFolderLocation;
			browseInfo.Owner = hWndOwner;
			browseInfo.pszDisplayName = hglobal;
			browseInfo.Title = this._descriptionText;
			browseInfo.Flags = _uiFlags;
			browseInfo.callback = this._callback;
			browseInfo.lParam = IntPtr.Zero;
			browseInfo.iImage = 0;
			pidl = ShellApi.SHBrowseForFolder(browseInfo);
			if (((_uiFlags & BrowseFlags.BIF_BROWSEFORPRINTER) == BrowseFlags.BIF_BROWSEFORPRINTER) || ((_uiFlags & BrowseFlags.BIF_BROWSEFORCOMPUTER) == BrowseFlags.BIF_BROWSEFORCOMPUTER))
			{
				this._selectedPath = Marshal.PtrToStringAuto(browseInfo.pszDisplayName);
				result = true;
			}
			else
			{
				if (pidl != IntPtr.Zero)
				{
					ShellApi.SHGetPathFromIDList(pidl, pszPath);
					this._selectedPathNeedsCheck = true;
					this._selectedPath = Marshal.PtrToStringAuto(pszPath);
					result = true;
				}
			}
		}
		finally
		{
			IMalloc sHMalloc = GetSHMalloc();
			sHMalloc.Free(_rootFolderLocation);
			_rootFolderLocation = IntPtr.Zero;
			if (pidl != IntPtr.Zero)
			{
				sHMalloc.Free(pidl);
			}
			if (pszPath != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pszPath);
			}
			if (hglobal != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(hglobal);
			}
			this._callback = null;
		}
		return result;
	}

	// Properties
	//[SRDescription("FolderBrowserDialogDescription"), SRCategory("CatFolderBrowsing"), Browsable(true), DefaultValue(""), Localizable(true)]

	/** 
	 This description appears near the top of the dialog box, providing direction to the user.
	 
	*/
	public final String getDescription()
	{
		return this._descriptionText;
	}
	public final void setDescription(String value)
	{
		this._descriptionText = (value == null) ? "" : value;
	}

	//[Localizable(false), SRCategory("CatFolderBrowsing"), SRDescription("FolderBrowserDialogRootFolder"), TypeConverter(typeof(SpecialFolderEnumConverter)), Browsable(true), DefaultValue(0)]
	public final Environment.SpecialFolder getRootFolder()
	{
		return this._rootFolder;
	}
	public final void setRootFolder(Environment.SpecialFolder value)
	{
		if (!Enum.IsDefined(Environment.SpecialFolder.class, value))
		{
			throw new InvalidEnumArgumentException("value", (int)value, Environment.SpecialFolder.class);
		}
		this._rootFolder = value;
	}

	//[Browsable(true), SRDescription("FolderBrowserDialogSelectedPath"), SRCategory("CatFolderBrowsing"), DefaultValue(""), Editor("System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Localizable(true)]

	/** 
	 Set or get the selected path.  
	 
	*/
	public final String getSelectedPath()
	{
		if (((this._selectedPath != null) && (this._selectedPath.length() != 0)) && this._selectedPathNeedsCheck)
		{
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this._selectedPath).Demand();
			this._selectedPathNeedsCheck = false;
		}
		return this._selectedPath;
	}
	public final void setSelectedPath(String value)
	{
		this._selectedPath = (value == null) ? "" : value;
		this._selectedPathNeedsCheck = true;
	}

	//[SRDescription("FolderBrowserDialogShowNewFolderButton"), Localizable(false), Browsable(true), DefaultValue(true), SRCategory("CatFolderBrowsing")]

	/** 
	 Enable or disable the "New Folder" button in the browser dialog.
	 
	*/
	public final boolean getShowNewFolderButton()
	{
		return this._showNewFolderButton;
	}
	public final void setShowNewFolderButton(boolean value)
	{
		this._showNewFolderButton = value;
	}

	/** 
	 Show an "edit box" in the folder browser.
	 
	 
	 The "edit box" normally shows the name of the selected folder.  
	 The user may also type a pathname directly into the edit box.  
	 
	 {@link ShowFullPathInEditBox}
	*/
	public final boolean getShowEditBox()
	{
		return this._showEditBox;
	}
	public final void setShowEditBox(boolean value)
	{
		this._showEditBox = value;
	}

	/** 
	 Set whether to use the New Folder Browser dialog style.
	 
	 
	 The new style is resizable and includes a "New Folder" button.
	 
	*/
	public final boolean getNewStyle()
	{
		return this._newStyle;
	}
	public final void setNewStyle(boolean value)
	{
		this._newStyle = value;
	}


	private String[] privateExtensions;
	public final String[] getExtensions()
	{
		return privateExtensions;
	}
	public final void setExtensions(String[] value)
	{
		privateExtensions = value;
	}



	public final boolean getDontIncludeNetworkFoldersBelowDomainLevel()
	{
		return _dontIncludeNetworkFoldersBelowDomainLevel;
	}
	public final void setDontIncludeNetworkFoldersBelowDomainLevel(boolean value)
	{
		_dontIncludeNetworkFoldersBelowDomainLevel = value;
	}

	/** 
	 Show the full path in the edit box as the user selects it. 
	 
	 
	 This works only if ShowEditBox is also set to true. 
	 
	*/
	public final boolean getShowFullPathInEditBox()
	{
		return _showFullPathInEditBox;
	}
	public final void setShowFullPathInEditBox(boolean value)
	{
		_showFullPathInEditBox = value;
	}

	public final boolean getShowBothFilesAndFolders()
	{
		return _showBothFilesAndFolders;
	}
	public final void setShowBothFilesAndFolders(boolean value)
	{
		_showBothFilesAndFolders = value;
	}
}