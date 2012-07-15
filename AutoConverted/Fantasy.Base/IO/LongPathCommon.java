package Fantasy.IO;

import Fantasy.IO.Interop.*;

//     Copyright (c) Microsoft Corporation.  All rights reserved.


public final class LongPathCommon
{

	public static String NormalizeSearchPattern(String searchPattern)
	{
		if (DotNetToJavaStringHelper.isNullOrEmpty(searchPattern) || searchPattern.equals("."))
		{
			return "*";
		}

		return searchPattern;
	}

	public static String NormalizeLongPath(String path)
	{

		return NormalizeLongPath(path, "path");
	}

	// Normalizes path (can be longer than MAX_PATH) and adds \\?\ long path prefix
	public static String NormalizeLongPath(String path, String parameterName)
	{

		if (path == null)
		{
			throw new ArgumentNullException(parameterName);
		}

		if (path.length() == 0)
		{
			throw new IllegalArgumentException(String.format(CultureInfo.CurrentCulture, "'{0}' cannot be an empty string.", parameterName), parameterName);
		}

		StringBuilder buffer = new StringBuilder(path.length() + 1); // Add 1 for NULL
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: uint length = NativeMethods.GetFullPathName(path, (uint)buffer.Capacity, buffer, IntPtr.Zero);
		int length = NativeMethods.GetFullPathName(path, (int)buffer.getCapacity(), buffer, IntPtr.Zero);
		if (length > buffer.getCapacity())
		{
			// Resulting path longer than our buffer, so increase it

			buffer.setCapacity((int)length);
			length = NativeMethods.GetFullPathName(path, length, buffer, IntPtr.Zero);
		}

		if (length == 0)
		{
			throw LongPathCommon.GetExceptionFromLastWin32Error(parameterName);
		}

		if (length > NativeMethods.MAX_LONG_PATH)
		{
			throw LongPathCommon.GetExceptionFromWin32Error(NativeMethods.ERROR_FILENAME_EXCED_RANGE, parameterName);
		}

		return AddLongPathPrefix(buffer.toString());
	}

	private static boolean TryNormalizeLongPath(String path, RefObject<String> result)
	{

		try
		{

			result.argvalue = NormalizeLongPath(path);
			return true;
		}
		catch (IllegalArgumentException e)
		{
		}
		catch (PathTooLongException e2)
		{
		}

		result.argvalue = null;
		return false;
	}

	private static String AddLongPathPrefix(String path)
	{

		Uri uri = new Uri(path);
		if (uri.IsUnc)
		{
			return NativeMethods.LongUncPathPrefix + path.substring(2);
		}
		else
		{
			return NativeMethods.LongPathPrefix + path;
		}


	}

	public static String RemoveLongPathPrefix(String normalizedPath)
	{
		String rs;
		if (normalizedPath.startsWith(NativeMethods.LongUncPathPrefix, StringComparison.OrdinalIgnoreCase))
		{
			rs = "\\\\" + normalizedPath.substring(NativeMethods.LongUncPathPrefix.length());
		}
		else
		{
			rs = normalizedPath.substring(NativeMethods.LongPathPrefix.length());
		}

		return rs;
	}

	public static boolean Exists(String path, RefObject<Boolean> isDirectory)
	{

		String normalizedPath = null;
		RefObject<String> tempRef_normalizedPath = new RefObject<String>(normalizedPath);
		boolean tempVar = TryNormalizeLongPath(path, tempRef_normalizedPath);
			normalizedPath = tempRef_normalizedPath.argvalue;
		if (tempVar)
		{

			FileAttributes attributes = null;
			RefObject<FileAttributes> tempRef_attributes = new RefObject<FileAttributes>(attributes);
			int errorCode = TryGetFileAttributes(normalizedPath, tempRef_attributes);
			attributes = tempRef_attributes.argvalue;
			if (errorCode == 0)
			{
				isDirectory.argvalue = LongPathDirectory.IsDirectory(attributes);
				return true;
			}
		}

		isDirectory.argvalue = false;
		return false;
	}

	public static int TryGetDirectoryAttributes(String normalizedPath, RefObject<FileAttributes> attributes)
	{

		int errorCode = TryGetFileAttributes(normalizedPath, attributes);
		if (!LongPathDirectory.IsDirectory(attributes.argvalue))
		{
			errorCode = NativeMethods.ERROR_DIRECTORY;
		}

		return errorCode;
	}

	public static int TryGetFileAttributes(String normalizedPath, RefObject<FileAttributes> attributes)
	{
		// NOTE: Don't be tempted to use FindFirstFile here, it does not work with root directories

		int rs = 0;
		attributes.argvalue = (FileAttributes)0;

		if (getIsInTransaction())
		{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
			TransactionHandle tx = NativeMethods.GetTransactionFromDTC();
			try
			{
				WIN32_FILE_ATTRIBUTE_DATA data = new WIN32_FILE_ATTRIBUTE_DATA();
				RefObject<WIN32_FILE_ATTRIBUTE_DATA> tempRef_data = new RefObject<WIN32_FILE_ATTRIBUTE_DATA>(data);
				boolean tempVar = !NativeMethods.GetFileAttributesTransacted(normalizedPath, GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, tempRef_data, tx);
					data = tempRef_data.argvalue;
				if (tempVar)
				{
					rs = Marshal.GetLastWin32Error();
				}
				else
				{
					attributes.argvalue = data.dwFileAttributes;
				}
			}
			finally
			{
				tx.dispose();
			}
		}
		else
		{
			attributes.argvalue = NativeMethods.GetFileAttributes(normalizedPath);
			if ((int)attributes.argvalue == NativeMethods.INVALID_FILE_ATTRIBUTES)
			{
				rs = Marshal.GetLastWin32Error();
			}
		}

		return rs;
	}

	public static RuntimeException GetExceptionFromLastWin32Error()
	{
		return GetExceptionFromLastWin32Error("path");
	}

	public static RuntimeException GetExceptionFromLastWin32Error(String parameterName)
	{
		return GetExceptionFromWin32Error(Marshal.GetLastWin32Error(), parameterName);
	}

	public static RuntimeException GetExceptionFromWin32Error(int errorCode)
	{
		return GetExceptionFromWin32Error(errorCode, "path");
	}

	public static RuntimeException GetExceptionFromWin32Error(int errorCode, String parameterName)
	{

		String message = GetMessageFromErrorCode(errorCode);

		switch (errorCode)
		{

			case NativeMethods.ERROR_FILE_NOT_FOUND:
				return new FileNotFoundException(message);

			case NativeMethods.ERROR_PATH_NOT_FOUND:
				return new DirectoryNotFoundException(message);

			case NativeMethods.ERROR_ACCESS_DENIED:
				return new UnauthorizedAccessException(message);

			case NativeMethods.ERROR_FILENAME_EXCED_RANGE:
				return new PathTooLongException(message);

			case NativeMethods.ERROR_INVALID_DRIVE:
				return new DriveNotFoundException(message);

			case NativeMethods.ERROR_OPERATION_ABORTED:
				return new OperationCanceledException(message);

			case NativeMethods.ERROR_INVALID_NAME:
				return new IllegalArgumentException(message, parameterName);

			default:
				return new IOException(message, NativeMethods.MakeHRFromErrorCode(errorCode));

		}
	}

	private static String GetMessageFromErrorCode(int errorCode)
	{

		StringBuilder buffer = new StringBuilder(512);

		int bufferLength = NativeMethods.FormatMessage(NativeMethods.FORMAT_MESSAGE_IGNORE_INSERTS | NativeMethods.FORMAT_MESSAGE_FROM_SYSTEM | NativeMethods.FORMAT_MESSAGE_ARGUMENT_ARRAY, IntPtr.Zero, errorCode, 0, buffer, buffer.getCapacity(), IntPtr.Zero);

		//Contract.Assert(bufferLength != 0);

		return buffer.toString();
	}

	public static boolean getIsInTransaction()
	{
		return Environment.OSVersion.Version.Major >= 6 && Transaction.Current != null;
	}
}