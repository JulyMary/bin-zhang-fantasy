package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct WIN32_FILE_ATTRIBUTE_DATA
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[StructLayout(LayoutKind.Sequential)]
public final class WIN32_FILE_ATTRIBUTE_DATA
{
	public FileAttributes dwFileAttributes;
	public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
	public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
	public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public uint nFileSizeHigh;
	public int nFileSizeHigh;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public uint nFileSizeLow;
	public int nFileSizeLow;

	public WIN32_FILE_ATTRIBUTE_DATA clone()
	{
		WIN32_FILE_ATTRIBUTE_DATA varCopy = new WIN32_FILE_ATTRIBUTE_DATA();

		varCopy.dwFileAttributes = this.dwFileAttributes;
		varCopy.ftCreationTime = this.ftCreationTime;
		varCopy.ftLastAccessTime = this.ftLastAccessTime;
		varCopy.ftLastWriteTime = this.ftLastWriteTime;
		varCopy.nFileSizeHigh = this.nFileSizeHigh;
		varCopy.nFileSizeLow = this.nFileSizeLow;

		return varCopy;
	}
}