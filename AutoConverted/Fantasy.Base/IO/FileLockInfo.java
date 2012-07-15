package Fantasy.IO;

public class FileLockInfo
{

	/**  
	 Return a list of processes that hold on the given file. 
	  
	*/
	public static java.util.ArrayList<Process> GetProcessesLockingFile(String filePath)
	{
		java.util.ArrayList<Process> procs = new java.util.ArrayList<Process>();

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
		for (var process : Process.GetProcesses())
		{
			java.util.ArrayList<String> files = GetFilesLockedBy(process);
			if (files.contains(filePath))
			{
				procs.add(process);
			}
		}
		return procs;
	}

	/**  
	 Return a list of file locks held by the process. 
	  
	*/
	public static java.util.ArrayList<String> GetFilesLockedBy(Process process)
	{
		java.util.ArrayList<String> outp = new java.util.ArrayList<String>();

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		ThreadStart ts = delegate
		{
			try
			{
				outp = UnsafeGetFilesLockedBy(process);
			}
			catch (java.lang.Exception e)
			{
			}
		}


		try
		{
			Thread t = new Thread(ts);
			t.start();
			if (!t.join(250))
			{
				try
				{
					t.stop();
				}
				catch (java.lang.Exception e2)
				{
				}
			}
		}
		catch (java.lang.Exception e3)
		{
		}

		return outp;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region Inner Workings
	private static java.util.ArrayList<String> UnsafeGetFilesLockedBy(Process process)
	{
		try
		{
			java.util.ArrayList<Win32API.SYSTEM_HANDLE_INFORMATION> handles = GetHandles(process);
			java.util.ArrayList<String> files = new java.util.ArrayList<String>();

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
			for (var handle : handles)
			{
				String file = GetFilePath(handle, process);
				if (file != null)
				{
					files.add(file);
				}
			}

			return files;
		}
		catch (java.lang.Exception e)
		{
			return new java.util.ArrayList<String>();
		}
	}

	private static final int CNST_SYSTEM_HANDLE_INFORMATION = 16;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: const uint STATUS_INFO_LENGTH_MISMATCH = 0xc0000004;
	private static final int STATUS_INFO_LENGTH_MISMATCH = 0xc0000004;
	private static String GetFilePath(Win32API.SYSTEM_HANDLE_INFORMATION sYSTEM_HANDLE_INFORMATION, Process process)
	{
		IntPtr m_ipProcessHwnd = Win32API.OpenProcess(Win32API.ProcessAccessFlags.All, false, process.Id);
		IntPtr ipHandle = IntPtr.Zero;
		Win32API.OBJECT_BASIC_INFORMATION objBasic = new Win32API.OBJECT_BASIC_INFORMATION();
		IntPtr ipBasic = IntPtr.Zero;
		Win32API.OBJECT_TYPE_INFORMATION objObjectType = new Win32API.OBJECT_TYPE_INFORMATION();
		IntPtr ipObjectType = IntPtr.Zero;
		Win32API.OBJECT_NAME_INFORMATION objObjectName = new Win32API.OBJECT_NAME_INFORMATION();
		IntPtr ipObjectName = IntPtr.Zero;
		String strObjectTypeName = "";
		String strObjectName = "";
		int nLength = 0;
		int nReturn = 0;
		IntPtr ipTemp = IntPtr.Zero;

		RefObject<IntPtr> tempRef_ipHandle = new RefObject<IntPtr>(ipHandle);
		boolean tempVar = !Win32API.DuplicateHandle(m_ipProcessHwnd, sYSTEM_HANDLE_INFORMATION.Handle, Win32API.GetCurrentProcess(), tempRef_ipHandle, 0, false, Win32API.DUPLICATE_SAME_ACCESS);
			ipHandle = tempRef_ipHandle.argvalue;
		if (tempVar)
		{
			return null;
		}

		ipBasic = Marshal.AllocHGlobal(Marshal.SizeOf(objBasic.clone()));
		RefObject<Integer> tempRef_nLength = new RefObject<Integer>(nLength);
		Win32API.NtQueryObject(ipHandle, Win32API.ObjectInformationClass.ObjectBasicInformation.getValue(), ipBasic, Marshal.SizeOf(objBasic.clone()), tempRef_nLength);
		nLength = tempRef_nLength.argvalue;
		objBasic = (Win32API.OBJECT_BASIC_INFORMATION)Marshal.PtrToStructure(ipBasic, objBasic.getClass());
		Marshal.FreeHGlobal(ipBasic);


		ipObjectType = Marshal.AllocHGlobal(objBasic.TypeInformationLength);
		nLength = objBasic.TypeInformationLength;
		RefObject<Integer> tempRef_nLength2 = new RefObject<Integer>(nLength);
		boolean tempVar2 = (int)(nReturn = Win32API.NtQueryObject(ipHandle, Win32API.ObjectInformationClass.ObjectTypeInformation.getValue(), ipObjectType, nLength, tempRef_nLength2)) == Win32API.STATUS_INFO_LENGTH_MISMATCH;
			nLength = tempRef_nLength2.argvalue;
		while (tempVar2)
		{
			Marshal.FreeHGlobal(ipObjectType);
			ipObjectType = Marshal.AllocHGlobal(nLength);
			RefObject<Integer> tempRef_nLength3 = new RefObject<Integer>(nLength);
			tempVar2 = (int)(nReturn = Win32API.NtQueryObject(ipHandle, Win32API.ObjectInformationClass.ObjectTypeInformation.getValue(), ipObjectType, nLength, tempRef_nLength3)) == Win32API.STATUS_INFO_LENGTH_MISMATCH;
			nLength = tempRef_nLength3.argvalue;
		}

		objObjectType = (Win32API.OBJECT_TYPE_INFORMATION)Marshal.PtrToStructure(ipObjectType, objObjectType.getClass());
		if (Is64Bits())
		{
			ipTemp = new IntPtr(Long.parseLong(objObjectType.getName().Buffer.toString(), 10) >> 32);
		}
		else
		{
			ipTemp = objObjectType.getName().Buffer;
		}

		strObjectTypeName = Marshal.PtrToStringUni(ipTemp, objObjectType.getName().getLength() >> 1);
		Marshal.FreeHGlobal(ipObjectType);
		if (!strObjectTypeName.equals("File"))
		{
			return null;
		}

		nLength = objBasic.NameInformationLength;

		ipObjectName = Marshal.AllocHGlobal(nLength);
		RefObject<Integer> tempRef_nLength4 = new RefObject<Integer>(nLength);
		boolean tempVar3 = (int)(nReturn = Win32API.NtQueryObject(ipHandle, Win32API.ObjectInformationClass.ObjectNameInformation.getValue(), ipObjectName, nLength, tempRef_nLength4)) == Win32API.STATUS_INFO_LENGTH_MISMATCH;
			nLength = tempRef_nLength4.argvalue;
		while (tempVar3)
		{
			Marshal.FreeHGlobal(ipObjectName);
			ipObjectName = Marshal.AllocHGlobal(nLength);
			RefObject<Integer> tempRef_nLength5 = new RefObject<Integer>(nLength);
			tempVar3 = (int)(nReturn = Win32API.NtQueryObject(ipHandle, Win32API.ObjectInformationClass.ObjectNameInformation.getValue(), ipObjectName, nLength, tempRef_nLength5)) == Win32API.STATUS_INFO_LENGTH_MISMATCH;
			nLength = tempRef_nLength5.argvalue;
		}
		objObjectName = (Win32API.OBJECT_NAME_INFORMATION)Marshal.PtrToStructure(ipObjectName, objObjectName.getClass());

		if (Is64Bits())
		{
			ipTemp = new IntPtr(Long.parseLong(objObjectName.getName().Buffer.toString(), 10) >> 32);
		}
		else
		{
			ipTemp = objObjectName.getName().Buffer;
		}

		if (ipTemp != IntPtr.Zero)
		{

			byte[] baTemp = new byte[nLength];
			try
			{
				Marshal.Copy(ipTemp, baTemp, 0, nLength);

				strObjectName = Marshal.PtrToStringUni(Is64Bits() ? new IntPtr(ipTemp.ToInt64()) : new IntPtr(ipTemp.ToInt32()));
			}
			catch (AccessViolationException e)
			{
				return null;
			}
			finally
			{
				Marshal.FreeHGlobal(ipObjectName);
				Win32API.CloseHandle(ipHandle);
			}
		}

		String path = GetRegularFileNameFromDevice(strObjectName);
		try
		{
			return path;
		}
		catch (java.lang.Exception e2)
		{
			return null;
		}
	}

	private static String GetRegularFileNameFromDevice(String strRawName)
	{
		String strFileName = strRawName;
		for (String strDrivePath : Environment.GetLogicalDrives())
		{
			StringBuilder sbTargetPath = new StringBuilder(Win32API.MAX_PATH);
			if (Win32API.QueryDosDevice(strDrivePath.substring(0, 2), sbTargetPath, Win32API.MAX_PATH) == 0)
			{
				return strRawName;
			}
			String strTargetPath = sbTargetPath.toString();
			if (strFileName.startsWith(strTargetPath))
			{
				strFileName = strFileName.replace(strTargetPath, strDrivePath.substring(0, 2));
				break;
			}
		}
		return strFileName;
	}

	private static java.util.ArrayList<Win32API.SYSTEM_HANDLE_INFORMATION> GetHandles(Process process)
	{
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: uint nStatus;
		int nStatus;
		int nHandleInfoSize = 0x10000;
		IntPtr ipHandlePointer = Marshal.AllocHGlobal(nHandleInfoSize);
		int nLength = 0;
		IntPtr ipHandle = IntPtr.Zero;

		RefObject<Integer> tempRef_nLength = new RefObject<Integer>(nLength);
		boolean tempVar = (nStatus = Win32API.NtQuerySystemInformation(CNST_SYSTEM_HANDLE_INFORMATION, ipHandlePointer, nHandleInfoSize, tempRef_nLength)) == STATUS_INFO_LENGTH_MISMATCH;
			nLength = tempRef_nLength.argvalue;
		while (tempVar)
		{
			nHandleInfoSize = nLength;
			Marshal.FreeHGlobal(ipHandlePointer);
			ipHandlePointer = Marshal.AllocHGlobal(nLength);
			RefObject<Integer> tempRef_nLength2 = new RefObject<Integer>(nLength);
			tempVar = (nStatus = Win32API.NtQuerySystemInformation(CNST_SYSTEM_HANDLE_INFORMATION, ipHandlePointer, nHandleInfoSize, tempRef_nLength2)) == STATUS_INFO_LENGTH_MISMATCH;
			nLength = tempRef_nLength2.argvalue;
		}

		byte[] baTemp = new byte[nLength];
		Marshal.Copy(ipHandlePointer, baTemp, 0, nLength);

		long lHandleCount = 0;
		if (Is64Bits())
		{
			lHandleCount = Marshal.ReadInt64(ipHandlePointer);
			ipHandle = new IntPtr(ipHandlePointer.ToInt64() + 8);
		}
		else
		{
			lHandleCount = Marshal.ReadInt32(ipHandlePointer);
			ipHandle = new IntPtr(ipHandlePointer.ToInt32() + 4);
		}

		Win32API.SYSTEM_HANDLE_INFORMATION shHandle;
		java.util.ArrayList<Win32API.SYSTEM_HANDLE_INFORMATION> lstHandles = new java.util.ArrayList<Win32API.SYSTEM_HANDLE_INFORMATION>();

		for (long lIndex = 0; lIndex < lHandleCount; lIndex++)
		{
			shHandle = new Win32API.SYSTEM_HANDLE_INFORMATION();
			if (Is64Bits())
			{
				shHandle = (Win32API.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.getClass());
				ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle.clone()) + 8);
			}
			else
			{
				ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle.clone()));
				shHandle = (Win32API.SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(ipHandle, shHandle.getClass());
			}
			if (shHandle.ProcessID != process.Id)
			{
				continue;
			}
			lstHandles.add(shHandle.clone());
		}
		return lstHandles;

	}

	private static boolean Is64Bits()
	{
		return Marshal.SizeOf(IntPtr.class) == 8 ? true : false;
	}

	public static class Win32API
	{
		public static native int NtQueryObject(IntPtr ObjectHandle, int ObjectInformationClass, IntPtr ObjectInformation, int ObjectInformationLength, RefObject<Integer> returnLength);
		static
		{
			System.loadLibrary("ntdll.dll");
		}

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);
		public static native int QueryDosDevice(String lpDeviceName, StringBuilder lpTargetPath, int ucchMax);
		static
		{
			System.loadLibrary("kernel32.dll");
		}

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public static extern uint NtQuerySystemInformation(int SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, ref int returnLength);
		public static native int NtQuerySystemInformation(int SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, RefObject<Integer> returnLength);

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//ORIGINAL LINE: public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
		public static native IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, boolean bInheritHandle, int dwProcessId);
		public static native int CloseHandle(IntPtr hObject);
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, ushort hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		public static native boolean DuplicateHandle(IntPtr hSourceProcessHandle, short hSourceHandle, IntPtr hTargetProcessHandle, RefObject<IntPtr> lpTargetHandle, int dwDesiredAccess, boolean bInheritHandle, int dwOptions);
		public static native IntPtr GetCurrentProcess();

		public enum ObjectInformationClass 
		{
			ObjectBasicInformation(0),
			ObjectNameInformation(1),
			ObjectTypeInformation(2),
			ObjectAllTypesInformation(3),
			ObjectHandleInformation(4);

			private int intValue;
			private static java.util.HashMap<Integer, ObjectInformationClass> mappings;
			private synchronized static java.util.HashMap<Integer, ObjectInformationClass> getMappings()
			{
				if (mappings == null)
				{
					mappings = new java.util.HashMap<Integer, ObjectInformationClass>();
				}
				return mappings;
			}

			private ObjectInformationClass(int value)
			{
				intValue = value;
				ObjectInformationClass.getMappings().put(value, this);
			}

			public int getValue()
			{
				return intValue;
			}

			public static ObjectInformationClass forValue(int value)
			{
				return getMappings().get(value);
			}
		}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[Flags]
		public enum ProcessAccessFlags 
		{
			All(0x001F0FFF),
			Terminate(0x00000001),
			CreateThread(0x00000002),
			VMOperation(0x00000008),
			VMRead(0x00000010),
			VMWrite(0x00000020),
			DupHandle(0x00000040),
			SetInformation(0x00000200),
			QueryInformation(0x00000400),
			Synchronize(0x00100000);

			private int intValue;
			private static java.util.HashMap<Integer, ProcessAccessFlags> mappings;
			private synchronized static java.util.HashMap<Integer, ProcessAccessFlags> getMappings()
			{
				if (mappings == null)
				{
					mappings = new java.util.HashMap<Integer, ProcessAccessFlags>();
				}
				return mappings;
			}

			private ProcessAccessFlags(int value)
			{
				intValue = value;
				ProcessAccessFlags.getMappings().put(value, this);
			}

			public int getValue()
			{
				return intValue;
			}

			public static ProcessAccessFlags forValue(int value)
			{
				return getMappings().get(value);
			}
		}

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct OBJECT_BASIC_INFORMATION
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[StructLayout(LayoutKind.Sequential)]
		public final static class OBJECT_BASIC_INFORMATION
		{ // Information Class 0
			public int Attributes;
			public int GrantedAccess;
			public int HandleCount;
			public int PointerCount;
			public int PagedPoolUsage;
			public int NonPagedPoolUsage;
			public int Reserved1;
			public int Reserved2;
			public int Reserved3;
			public int NameInformationLength;
			public int TypeInformationLength;
			public int SecurityDescriptorLength;
			public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;

			public OBJECT_BASIC_INFORMATION clone()
			{
				OBJECT_BASIC_INFORMATION varCopy = new OBJECT_BASIC_INFORMATION();

				varCopy.Attributes = this.Attributes;
				varCopy.GrantedAccess = this.GrantedAccess;
				varCopy.HandleCount = this.HandleCount;
				varCopy.PointerCount = this.PointerCount;
				varCopy.PagedPoolUsage = this.PagedPoolUsage;
				varCopy.NonPagedPoolUsage = this.NonPagedPoolUsage;
				varCopy.Reserved1 = this.Reserved1;
				varCopy.Reserved2 = this.Reserved2;
				varCopy.Reserved3 = this.Reserved3;
				varCopy.NameInformationLength = this.NameInformationLength;
				varCopy.TypeInformationLength = this.TypeInformationLength;
				varCopy.SecurityDescriptorLength = this.SecurityDescriptorLength;
				varCopy.CreateTime = this.CreateTime;

				return varCopy;
			}
		}

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct OBJECT_TYPE_INFORMATION
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[StructLayout(LayoutKind.Sequential)]
		public final static class OBJECT_TYPE_INFORMATION
		{ // Information Class 2
			public UNICODE_STRING Name;
			public int ObjectCount;
			public int HandleCount;
			public int Reserved1;
			public int Reserved2;
			public int Reserved3;
			public int Reserved4;
			public int PeakObjectCount;
			public int PeakHandleCount;
			public int Reserved5;
			public int Reserved6;
			public int Reserved7;
			public int Reserved8;
			public int InvalidAttributes;
			public GENERIC_MAPPING GenericMapping;
			public int ValidAccess;
			public byte Unknown;
			public byte MaintainHandleDatabase;
			public int PoolType;
			public int PagedPoolUsage;
			public int NonPagedPoolUsage;

			public OBJECT_TYPE_INFORMATION clone()
			{
				OBJECT_TYPE_INFORMATION varCopy = new OBJECT_TYPE_INFORMATION();

				varCopy.Name = this.Name.clone();
				varCopy.ObjectCount = this.ObjectCount;
				varCopy.HandleCount = this.HandleCount;
				varCopy.Reserved1 = this.Reserved1;
				varCopy.Reserved2 = this.Reserved2;
				varCopy.Reserved3 = this.Reserved3;
				varCopy.Reserved4 = this.Reserved4;
				varCopy.PeakObjectCount = this.PeakObjectCount;
				varCopy.PeakHandleCount = this.PeakHandleCount;
				varCopy.Reserved5 = this.Reserved5;
				varCopy.Reserved6 = this.Reserved6;
				varCopy.Reserved7 = this.Reserved7;
				varCopy.Reserved8 = this.Reserved8;
				varCopy.InvalidAttributes = this.InvalidAttributes;
				varCopy.GenericMapping = this.GenericMapping.clone();
				varCopy.ValidAccess = this.ValidAccess;
				varCopy.Unknown = this.Unknown;
				varCopy.MaintainHandleDatabase = this.MaintainHandleDatabase;
				varCopy.PoolType = this.PoolType;
				varCopy.PagedPoolUsage = this.PagedPoolUsage;
				varCopy.NonPagedPoolUsage = this.NonPagedPoolUsage;

				return varCopy;
			}
		}

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct OBJECT_NAME_INFORMATION
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[StructLayout(LayoutKind.Sequential)]
		public final static class OBJECT_NAME_INFORMATION
		{ // Information Class 1
			public UNICODE_STRING Name;

			public OBJECT_NAME_INFORMATION clone()
			{
				OBJECT_NAME_INFORMATION varCopy = new OBJECT_NAME_INFORMATION();

				varCopy.Name = this.Name.clone();

				return varCopy;
			}
		}

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct UNICODE_STRING
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public final static class UNICODE_STRING
		{
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public ushort Length;
			public short Length;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public ushort MaximumLength;
			public short MaximumLength;
			public IntPtr Buffer = new IntPtr();

			public UNICODE_STRING clone()
			{
				UNICODE_STRING varCopy = new UNICODE_STRING();

				varCopy.Length = this.Length;
				varCopy.MaximumLength = this.MaximumLength;
				varCopy.Buffer = this.Buffer;

				return varCopy;
			}
		}

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct GENERIC_MAPPING
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[StructLayout(LayoutKind.Sequential)]
		public final static class GENERIC_MAPPING
		{
			public int GenericRead;
			public int GenericWrite;
			public int GenericExecute;
			public int GenericAll;

			public GENERIC_MAPPING clone()
			{
				GENERIC_MAPPING varCopy = new GENERIC_MAPPING();

				varCopy.GenericRead = this.GenericRead;
				varCopy.GenericWrite = this.GenericWrite;
				varCopy.GenericExecute = this.GenericExecute;
				varCopy.GenericAll = this.GenericAll;

				return varCopy;
			}
		}

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: public struct SYSTEM_HANDLE_INFORMATION
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
		//[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public final static class SYSTEM_HANDLE_INFORMATION
		{ // Information Class 16
			public int ProcessID;
			public byte ObjectTypeNumber;
			public byte Flags; // 0x01 = PROTECT_FROM_CLOSE, 0x02 = INHERIT
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public ushort Handle;
			public short Handle;
			public int Object_Pointer;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public UInt32 GrantedAccess;
			public int GrantedAccess;

			public SYSTEM_HANDLE_INFORMATION clone()
			{
				SYSTEM_HANDLE_INFORMATION varCopy = new SYSTEM_HANDLE_INFORMATION();

				varCopy.ProcessID = this.ProcessID;
				varCopy.ObjectTypeNumber = this.ObjectTypeNumber;
				varCopy.Flags = this.Flags;
				varCopy.Handle = this.Handle;
				varCopy.Object_Pointer = this.Object_Pointer;
				varCopy.GrantedAccess = this.GrantedAccess;

				return varCopy;
			}
		}

		public static final int MAX_PATH = 260;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: public const uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;
		public static final int STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;
		public static final int DUPLICATE_SAME_ACCESS = 0x2;
	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}