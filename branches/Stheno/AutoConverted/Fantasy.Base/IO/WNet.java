package Fantasy.IO;

public class WNet
{
	// Methods
	public static boolean TryAddConnection(String resourceName, String userName, String password, RefObject<String> message)
	{
		resourceName = DotNetToJavaStringHelper.trimEnd(resourceName, '/', '\\');
		message.argvalue = null;
		NETRESOURCE tempVar = new NETRESOURCE();
		tempVar.RemoteName = resourceName;
		NETRESOURCE netResource = tempVar;

		RefObject<NETRESOURCE> tempRef_netResource = new RefObject<NETRESOURCE>(netResource);
		RefObject<String> tempRef_password = new RefObject<String>(password);
		RefObject<String> tempRef_userName = new RefObject<String>(userName);
		int hr = WNetAddConnection3(IntPtr.Zero, tempRef_netResource, tempRef_password, tempRef_userName, 0);
		netResource = tempRef_netResource.argvalue;
		password = tempRef_password.argvalue;
		userName = tempRef_userName.argvalue;
		if (hr != 0)
		{
			Win32Exception ex = new Win32Exception(hr);
			message.argvalue = ex.getMessage();
		}
		return (hr == 0);
	}



	public static void AddConnection(String resourceName, String userName, String password)
	{
		resourceName = DotNetToJavaStringHelper.trimEnd(resourceName, '/', '\\');
		NETRESOURCE tempVar = new NETRESOURCE();
		tempVar.RemoteName = resourceName;
		NETRESOURCE netResource = tempVar;

		RefObject<NETRESOURCE> tempRef_netResource = new RefObject<NETRESOURCE>(netResource);
		RefObject<String> tempRef_password = new RefObject<String>(password);
		RefObject<String> tempRef_userName = new RefObject<String>(userName);
		int hr = WNetAddConnection3(IntPtr.Zero, tempRef_netResource, tempRef_password, tempRef_userName, 0);
		netResource = tempRef_netResource.argvalue;
		password = tempRef_password.argvalue;
		userName = tempRef_userName.argvalue;
		if (hr != 0)
		{
		   throw new Win32Exception(hr);
		}

	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: private static extern int WNetAddConnection3(IntPtr handler, ref NETRESOURCE netResource, [MarshalAs(UnmanagedType.VBByRefStr)] ref string password, [MarshalAs(UnmanagedType.VBByRefStr)] ref string userName, int flags);
	private static native int WNetAddConnection3(IntPtr handler, RefObject<NETRESOURCE> netResource, RefObject<String> password, RefObject<String> userName, int flags);
	static
	{
		System.loadLibrary("mpr.dll");
	}
	// Nested Types
//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: private struct NETRESOURCE
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[StructLayout(LayoutKind.Sequential)]
	private final static class NETRESOURCE
	{
		public int dwScope;
		public int dwType;
		public int dwDisplayType;
		public int dwUsage;
		public String LocalName;
		public String RemoteName;
		public String Comment;
		public String Provider;

		public NETRESOURCE clone()
		{
			NETRESOURCE varCopy = new NETRESOURCE();

			varCopy.dwScope = this.dwScope;
			varCopy.dwType = this.dwType;
			varCopy.dwDisplayType = this.dwDisplayType;
			varCopy.dwUsage = this.dwUsage;
			varCopy.LocalName = this.LocalName;
			varCopy.RemoteName = this.RemoteName;
			varCopy.Comment = this.Comment;
			varCopy.Provider = this.Provider;

			return varCopy;
		}
	}
}