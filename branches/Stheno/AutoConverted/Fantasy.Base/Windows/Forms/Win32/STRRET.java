package Fantasy.Windows.Forms.Win32;

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct STRRET
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[StructLayout(LayoutKind.Explicit)]
public final class STRRET
{
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public UInt32 uType;
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[FieldOffset(0)]
	public int uType; // One of the STRRET_* values

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[FieldOffset(4)]
	public IntPtr pOleStr = new IntPtr(); // must be freed by caller of GetDisplayNameOf

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[FieldOffset(4)]
	public IntPtr pStr = new IntPtr(); // NOT USED

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public UInt32 uOffset;
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[FieldOffset(4)]
	public int uOffset; // Offset into SHITEMID

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[FieldOffset(4)]
	public IntPtr cStr = new IntPtr(); // Buffer to fill in (ANSI)

	public STRRET clone()
	{
		STRRET varCopy = new STRRET();

		varCopy.uType = this.uType;
		varCopy.pOleStr = this.pOleStr;
		varCopy.pStr = this.pStr;
		varCopy.uOffset = this.uOffset;
		varCopy.cStr = this.cStr;

		return varCopy;
	}
}