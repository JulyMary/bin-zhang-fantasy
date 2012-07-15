package Fantasy.Windows.Forms.Win32;

//C# TO JAVA CONVERTER TODO TASK: Delegates are not available in Java:
//public delegate int BrowseFolderCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class BROWSEINFO
{
	public IntPtr Owner = new IntPtr();
	public IntPtr pidlRoot = new IntPtr();
	public IntPtr pszDisplayName = new IntPtr();
	public String Title;
	public int Flags;
	public BrowseFolderCallbackProc callback;
	public IntPtr lParam = new IntPtr();
	public int iImage;
}