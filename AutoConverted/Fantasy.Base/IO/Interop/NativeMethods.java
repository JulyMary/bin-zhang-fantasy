package Fantasy.IO.Interop;

import Microsoft.Win32.SafeHandles.*;

//     Copyright (c) Microsoft Corporation.  All rights reserved.
//C# TO JAVA CONVERTER NOTE: There is no Java equivalent to C# namespace aliases:
//using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;


public final class NativeMethods
{
	public static final int ERROR_FILE_NOT_FOUND = 0x2;
	public static final int ERROR_PATH_NOT_FOUND = 0x3;
	public static final int ERROR_ACCESS_DENIED = 0x5;
	public static final int ERROR_INVALID_DRIVE = 0xf;
	public static final int ERROR_NO_MORE_FILES = 0x12;
	public static final int ERROR_INVALID_NAME = 0x7B;
	public static final int ERROR_ALREADY_EXISTS = 0xB7;
	public static final int ERROR_FILENAME_EXCED_RANGE = 0xCE; // filename too long.
	public static final int ERROR_DIRECTORY = 0x10B;
	public static final int ERROR_OPERATION_ABORTED = 0x3e3;
	public static final int INVALID_FILE_ATTRIBUTES = -1;

	public static final int MAX_PATH = 260;
	// While Windows allows larger paths up to a maximum of 32767 characters, because this is only an approximation and
	// can vary across systems and OS versions, we choose a limit well under so that we can give a consistent behavior.
	public static final int MAX_LONG_PATH = 32000;
	public static final int MAX_ALTERNATE = 14;
	public static final String LongPathPrefix = "\\\\?\\";
	public static final String LongUncPathPrefix = "\\\\?\\UNC\\";

	public static final int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
	public static final int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
	public static final int FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[Flags]
	public enum EFileAccess 
	{
		GenericRead(0x80000000),
		GenericWrite(0x40000000),
		GenericExecute(0x20000000),
		GenericAll(0x10000000);

		private int intValue;
		private static java.util.HashMap<Integer, EFileAccess> mappings;
		private synchronized static java.util.HashMap<Integer, EFileAccess> getMappings()
		{
			if (mappings == null)
			{
				mappings = new java.util.HashMap<Integer, EFileAccess>();
			}
			return mappings;
		}

		private EFileAccess(int value)
		{
			intValue = value;
			EFileAccess.getMappings().put(value, this);
		}

		public int getValue()
		{
			return intValue;
		}

		public static EFileAccess forValue(int value)
		{
			return getMappings().get(value);
		}
	}

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: internal struct WIN32_FIND_DATA
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public final static class WIN32_FIND_DATA
	{
		public FileAttributes dwFileAttributes;
		public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
		public int nFileSizeHigh;
		public int nFileSizeLow;
		public int dwReserved0;
		public int dwReserved1;
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
		public String cFileName;
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
		public String cAlternate;

		public WIN32_FIND_DATA clone()
		{
			WIN32_FIND_DATA varCopy = new WIN32_FIND_DATA();

			varCopy.dwFileAttributes = this.dwFileAttributes;
			varCopy.ftCreationTime = this.ftCreationTime;
			varCopy.ftLastAccessTime = this.ftLastAccessTime;
			varCopy.ftLastWriteTime = this.ftLastWriteTime;
			varCopy.nFileSizeHigh = this.nFileSizeHigh;
			varCopy.nFileSizeLow = this.nFileSizeLow;
			varCopy.dwReserved0 = this.dwReserved0;
			varCopy.dwReserved1 = this.dwReserved1;
			varCopy.cFileName = this.cFileName;
			varCopy.cAlternate = this.cAlternate;

			return varCopy;
		}
	}

	public static int MakeHRFromErrorCode(int errorCode)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to 'unchecked' in this context:
//ORIGINAL LINE: return unchecked((int)0x80070000 | errorCode);
		return (int)0x80070000 | errorCode;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: internal static extern bool CopyFile(string src, string dst, [MarshalAs(UnmanagedType.Bool)]bool failIfExists);
	public static native boolean CopyFile(String src, String dst, boolean failIfExists);
	static
	{
		System.loadLibrary("kernel32.dll");
	}

	public static native SafeFindHandle FindFirstFile(String lpFileName, RefObject<WIN32_FIND_DATA> lpFindFileData);

	public static native IntPtr FindFirstFileEx(String lpFileName, FINDEX_INFO_LEVELS fInfoLevelId, RefObject<WIN32_FIND_DATA> lpFindFileData, FINDEX_SEARCH_OPS fSearchOp, IntPtr lpSearchFilter, int dwAdditionalFlags);




	public static native SafeFindHandle FindFirstFileTransacted(String lpFileName, FINDEX_INFO_LEVELS fInfoLevelId, RefObject<WIN32_FIND_DATA> lpFindFileData, FINDEX_SEARCH_OPS fScarchOp, IntPtr lpSearchFilter, int flags, TransactionHandle hTransaction);


	public static native boolean FindNextFile(SafeFindHandle hFindFile, RefObject<WIN32_FIND_DATA> lpFindFileData);

	public static native boolean FindClose(IntPtr hFindFile);

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: internal static extern uint GetFullPathName(string lpFileName, uint nBufferLength, StringBuilder lpBuffer, IntPtr mustBeNull);
	public static native int GetFullPathName(String lpFileName, int nBufferLength, StringBuilder lpBuffer, IntPtr mustBeNull);

	public static native boolean DeleteFile(String lpFileName);

	public static native boolean DeleteFileTransacted(String lpFileName, TransactionHandle hTransaction);

	public static native boolean RemoveDirectory(String lpPathName);

	public static native boolean RemoveDirectoryTransacted(String lpPathName, TransactionHandle hTransaction);

	public static native boolean CreateDirectory(String lpPathName, IntPtr lpSecurityAttributes);

	public static native boolean CreateDirectoryEx(String lpTemplateDirectory, String lpPathName, IntPtr lpSecurityAttributes);

	public static native boolean CreateDirectoryTransacted(String lpTemplateDirectory, String lpPathName, IntPtr lpSecurityAttributes, TransactionHandle hTranscation);

	public static native boolean MoveFile(String lpPathNameFrom, String lpPathNameTo);

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: internal static extern SafeFileHandle CreateFile(string lpFileName, EFileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
	public static native SafeFileHandle CreateFile(String lpFileName, EFileAccess dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: internal static extern SafeFileHandle CreateFileTransacted(string lpFileName, EFileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile, TransactionHandle hTransaction, TXFS_MINIVERSION pusMiniVersion, IntPtr pExtendedParameter);
	public static native SafeFileHandle CreateFileTransacted(String lpFileName, EFileAccess dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile, TransactionHandle hTransaction, TXFS_MINIVERSION pusMiniVersion, IntPtr pExtendedParameter);

	public static native FileAttributes GetFileAttributes(String lpFileName);
	public static native boolean GetFileAttributesEx(String lpFileName, GET_FILEEX_INFO_LEVELS fInfoLevelId, RefObject<WIN32_FILE_ATTRIBUTE_DATA> lpFileInformation);

	public static native boolean GetFileAttributesTransacted(String lpFileName, GET_FILEEX_INFO_LEVELS fInfoLevelId, RefObject<WIN32_FILE_ATTRIBUTE_DATA> lpFileInformation, TransactionHandle hTransaction);

	public static native int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: internal static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string path, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath, int shortPathLength);
	public static native int GetShortPathName(String path, StringBuilder shortPath, int shortPathLength);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: internal static extern int GetLongPathName([MarshalAs(UnmanagedType.LPTStr)] string path, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder longPath, int longPathLength);
	public static native int GetLongPathName(String path, StringBuilder longPath, int longPathLength);



	public static native boolean CopyFileEx(String lpExistingFileName, String lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, RefObject<Integer> pbCancel, CopyFileFlags dwCopyFlags);

	public static native boolean CopyFileTransacted(String lpExistingFileName, String lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, RefObject<Integer> pbCancel, CopyFileFlags dwCopyFlags, TransactionHandle hTransaction);



	public static native boolean MoveFileWithProgress(String lpExistingFileName, String lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, MoveFileFlags dwFlags);

	public static native boolean MoveFileTransacted(String lpExistingFileName, String lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, MoveFileFlags dwFlags, TransactionHandle hTransaction);

	public static native boolean CloseHandle(IntPtr handle);
	static
	{
		System.loadLibrary("Kernel32.dll");
	}


//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);
	public static native boolean GetDiskFreeSpaceEx(String lpDirectoryName, RefObject<Long> lpFreeBytesAvailable, RefObject<Long> lpTotalNumberOfBytes, RefObject<Long> lpTotalNumberOfFreeBytes);




	public static TransactionHandle GetTransactionFromDTC()
	{
		if (Transaction.Current != null)
		{
			IKernelTransaction tx = (IKernelTransaction)TransactionInterop.GetDtcTransaction(Transaction.Current);
			TransactionHandle rs = null;
			RefObject<TransactionHandle> tempRef_rs = new RefObject<TransactionHandle>(rs);
			tx.GetHandle(tempRef_rs);
			rs = tempRef_rs.argvalue;
			return rs;
		}
		return null;
	}




}