package Fantasy.Windows.Forms.Win32;

public final class User32
{
	public static native IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, String lParam);
	static
	{
		System.loadLibrary("user32.dll");
	}

	public static native IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

	//public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
	//public static extern IntPtr FindWindowEx(HandleRef hwndParent, HandleRef hwndChildAfter, string lpszClass, string lpszWindow);
	public static native IntPtr FindWindowEx(HandleRef hwndParent, IntPtr hwndChildAfter, String lpszClass, String lpszWindow);

	public static native boolean SetWindowText(IntPtr hWnd, String text);
}