package Fantasy.IO;

public class DiskspaceInfo
{
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public DiskspaceInfo(ulong freeBytesAvailable, ulong totalNumberOfBytes, ulong totalNumberOfFreeBytes)
	public DiskspaceInfo(long freeBytesAvailable, long totalNumberOfBytes, long totalNumberOfFreeBytes)
	{
		this.setFreeBytesAvailable(freeBytesAvailable);
		this.setTotalNumberOfBytes(totalNumberOfBytes);
		this.setTotalNumberOfFreeBytes(getTotalNumberOfFreeBytes());
	}

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private ulong privateTotalNumberOfFreeBytes;
	private long privateTotalNumberOfFreeBytes;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public ulong getTotalNumberOfFreeBytes()
	public final long getTotalNumberOfFreeBytes()
	{
		return privateTotalNumberOfFreeBytes;
	}
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private void setTotalNumberOfFreeBytes(ulong value)
	private void setTotalNumberOfFreeBytes(long value)
	{
		privateTotalNumberOfFreeBytes = value;
	}

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private ulong privateTotalNumberOfBytes;
	private long privateTotalNumberOfBytes;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public ulong getTotalNumberOfBytes()
	public final long getTotalNumberOfBytes()
	{
		return privateTotalNumberOfBytes;
	}
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private void setTotalNumberOfBytes(ulong value)
	private void setTotalNumberOfBytes(long value)
	{
		privateTotalNumberOfBytes = value;
	}

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private ulong privateFreeBytesAvailable;
	private long privateFreeBytesAvailable;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public ulong getFreeBytesAvailable()
	public final long getFreeBytesAvailable()
	{
		return privateFreeBytesAvailable;
	}
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private void setFreeBytesAvailable(ulong value)
	private void setFreeBytesAvailable(long value)
	{
		privateFreeBytesAvailable = value;
	}
}