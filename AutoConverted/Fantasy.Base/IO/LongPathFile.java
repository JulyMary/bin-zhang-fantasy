package Fantasy.IO;

import Fantasy.IO.Interop.*;
import Microsoft.Win32.SafeHandles.*;
import Fantasy.ServiceModel.*;

//     Copyright (c) Microsoft Corporation.  All rights reserved.


/** 
	 Provides static methods for creating, copying, deleting, moving, and opening of files
	 with long paths, that is, paths that exceed 259 characters.
 
*/
public final class LongPathFile
{

	/** 
		 Returns a value indicating whether the specified path refers to an existing file.
	 
	 @param path
		 A <see cref="String"/> containing the path to check.
	 
	 @return 
		 <see langword="true"/> if <paramref name="path"/> refers to an existing file; 
		 otherwise, <see langword="false"/>.
	 
	 
		 Note that this method will return false if any error occurs while trying to determine 
		 if the specified file exists. This includes situations that would normally result in 
		 thrown exceptions including (but not limited to); passing in a file name with invalid 
		 or too many characters, an I/O error such as a failing or missing disk, or if the caller
		 does not have Windows or Code Access Security (CAS) permissions to to read the file.
	 
	*/
	public static boolean Exists(String path)
	{

		boolean isDirectory = false;
		RefObject<Boolean> tempRef_isDirectory = new RefObject<Boolean>(isDirectory);
		boolean tempVar = LongPathCommon.Exists(path, tempRef_isDirectory);
			isDirectory = tempRef_isDirectory.argvalue;
		if (tempVar)
		{

			return !isDirectory;
		}

		return false;
	}

	/** 
		 Deletes the specified file.
	 
	 @param path
		  A <see cref="String"/> containing the path of the file to delete.
	 
	 @exception ArgumentNullException
		 <paramref name="path"/> is <see langword="null"/>.
	 
	 @exception ArgumentException
		 <paramref name="path"/> is an empty string (""), contains only white 
		 space, or contains one or more invalid characters as defined in 
		 <see cref="Path.GetInvalidPathChars()"/>.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> contains one or more components that exceed
		 the drive-defined maximum length. For example, on Windows-based 
		 platforms, components must not exceed 255 characters.
	 
	 @exception PathTooLongException
		 <paramref name="path"/> exceeds the system-defined maximum length. 
		 For example, on Windows-based platforms, paths must not exceed 
		 32,000 characters.
	 
	 @exception FileNotFoundException
		 <paramref name="path"/> could not be found.
	 
	 @exception DirectoryNotFoundException
		 One or more directories in <paramref name="path"/> could not be found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> refers to a file that is read-only.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> is a directory.
	 
	 @exception IOException
		 <paramref name="path"/> refers to a file that is in use.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static void Delete(String path)
	{

		String normalizedPath = LongPathCommon.NormalizeLongPath(path);
		boolean success;
		if (LongPathCommon.getIsInTransaction())
		{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
			TransactionHandle tx = NativeMethods.GetTransactionFromDTC();
			try
			{
				success = NativeMethods.DeleteFileTransacted(normalizedPath, tx);
			}
			finally
			{
				tx.dispose();
			}
		}
		else
		{
			success = NativeMethods.DeleteFile(normalizedPath);
		}

		if (!success)
		{
			throw LongPathCommon.GetExceptionFromLastWin32Error();
		}
	}



//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public static void Move(string sourcePath, string destinationPath, bool overwrite = false, IProgressMonitor progress = null)
	public static void Move(String sourcePath, String destinationPath, boolean overwrite, IProgressMonitor progress)
	{
		String normalizedSourcePath = LongPathCommon.NormalizeLongPath(sourcePath, "sourcePath");
		String normalizedDestinationPath = LongPathCommon.NormalizeLongPath(destinationPath, "destinationPath");
		CopyProgressRoutine routine = progress != null ? CopyProgressRoutineHelper.Create(progress) : null;
		MoveFileFlags flags = MoveFileFlags.MOVEFILE_COPY_ALLOWED | MoveFileFlags.MOVEFILE_WRITE_THROUGH;
		if (overwrite)
		{
			flags |= MoveFileFlags.MOVEFILE_REPLACE_EXISTING;
		}

		boolean success;
		if (LongPathCommon.getIsInTransaction())
		{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
			TransactionHandle tx = NativeMethods.GetTransactionFromDTC();
			try
			{
				success = NativeMethods.MoveFileTransacted(normalizedSourcePath, normalizedDestinationPath, routine, IntPtr.Zero, flags, tx);
			}
			finally
			{
				tx.dispose();
			}
		}
		else
		{
			success = NativeMethods.MoveFileWithProgress(normalizedSourcePath, normalizedDestinationPath, routine, IntPtr.Zero, flags);
		}


		if(!success)
		{
			throw LongPathCommon.GetExceptionFromLastWin32Error();
		}
	}



//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public static void Copy(string sourcePath, string destinationPath, bool overwrite = false, IProgressMonitor progress = null)
	public static void Copy(String sourcePath, String destinationPath, boolean overwrite, IProgressMonitor progress)
	{
		String normalizedSourcePath = LongPathCommon.NormalizeLongPath(sourcePath, "sourcePath");
		String normalizedDestinationPath = LongPathCommon.NormalizeLongPath(destinationPath, "destinationPath");

		CopyProgressRoutine routine = progress != null ? CopyProgressRoutineHelper.Create(progress) : null;

		CopyFileFlags flags = overwrite ? CopyFileFlags.COPY_FILE_NONE : CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS;
		int cancel = 0;

		boolean success;
		if (LongPathCommon.getIsInTransaction())
		{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
			TransactionHandle tx = NativeMethods.GetTransactionFromDTC();
			try
			{
				RefObject<Integer> tempRef_cancel = new RefObject<Integer>(cancel);
				success = NativeMethods.CopyFileTransacted(normalizedSourcePath, normalizedDestinationPath, routine, IntPtr.Zero, tempRef_cancel, flags, tx);
				cancel = tempRef_cancel.argvalue;
			}
			finally
			{
				tx.dispose();
			}
		}
		else
		{
			RefObject<Integer> tempRef_cancel2 = new RefObject<Integer>(cancel);
			success = NativeMethods.CopyFileEx(normalizedSourcePath, normalizedDestinationPath, routine, IntPtr.Zero, tempRef_cancel2, flags);
			cancel = tempRef_cancel2.argvalue;
		}


		if (!success)
		{
			throw LongPathCommon.GetExceptionFromLastWin32Error();
		}

	}



	/** 
		 Opens the specified file.
	 
	 @param path
		 A <see cref="String"/> containing the path of the file to open.
	 
	 @param access
		 One of the <see cref="FileAccess"/> value that specifies the operations that can be 
		 performed on the file. 
	 
	 @param mode
		 One of the <see cref="FileMode"/> values that specifies whether a file is created
		 if one does not exist, and determines whether the contents of existing files are 
		 retained or overwritten.
	 
	 @return 
		 A <see cref="FileStream"/> that provides access to the file specified in 
		 <paramref name="path"/>.
	 
	 @exception ArgumentNullException
		 <paramref name="path"/> is <see langword="null"/>.
	 
	 @exception ArgumentException
		 <paramref name="path"/> is an empty string (""), contains only white 
		 space, or contains one or more invalid characters as defined in 
		 <see cref="Path.GetInvalidPathChars()"/>.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> contains one or more components that exceed
		 the drive-defined maximum length. For example, on Windows-based 
		 platforms, components must not exceed 255 characters.
	 
	 @exception PathTooLongException
		 <paramref name="path"/> exceeds the system-defined maximum length. 
		 For example, on Windows-based platforms, paths must not exceed 
		 32,000 characters.
	 
	 @exception DirectoryNotFoundException
		 One or more directories in <paramref name="path"/> could not be found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> refers to a file that is read-only and <paramref name="access"/>
		 is not <see cref="FileAccess.Read"/>.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> is a directory.
	 
	 @exception IOException
		 <paramref name="path"/> refers to a file that is in use.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static FileStream Open(String path, FileMode mode, FileAccess access)
	{

		return Open(path, mode, access, FileShare.None, 0, FileOptions.None);
	}

	/** 
		 Opens the specified file.
	 
	 @param path
		 A <see cref="String"/> containing the path of the file to open.
	 
	 @param access
		 One of the <see cref="FileAccess"/> value that specifies the operations that can be 
		 performed on the file. 
	 
	 @param mode
		 One of the <see cref="FileMode"/> values that specifies whether a file is created
		 if one does not exist, and determines whether the contents of existing files are 
		 retained or overwritten.
	 
	 @param share
		 One of the <see cref="FileShare"/> values specifying the type of access other threads 
		 have to the file. 
	 
	 @return 
		 A <see cref="FileStream"/> that provides access to the file specified in 
		 <paramref name="path"/>.
	 
	 @exception ArgumentNullException
		 <paramref name="path"/> is <see langword="null"/>.
	 
	 @exception ArgumentException
		 <paramref name="path"/> is an empty string (""), contains only white 
		 space, or contains one or more invalid characters as defined in 
		 <see cref="Path.GetInvalidPathChars()"/>.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> contains one or more components that exceed
		 the drive-defined maximum length. For example, on Windows-based 
		 platforms, components must not exceed 255 characters.
	 
	 @exception PathTooLongException
		 <paramref name="path"/> exceeds the system-defined maximum length. 
		 For example, on Windows-based platforms, paths must not exceed 
		 32,000 characters.
	 
	 @exception DirectoryNotFoundException
		 One or more directories in <paramref name="path"/> could not be found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> refers to a file that is read-only and <paramref name="access"/>
		 is not <see cref="FileAccess.Read"/>.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> is a directory.
	 
	 @exception IOException
		 <paramref name="path"/> refers to a file that is in use.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static FileStream Open(String path, FileMode mode, FileAccess access, FileShare share)
	{

		return Open(path, mode, access, share, 0, FileOptions.None);
	}

	/** 
		 Opens the specified file.
	 
	 @param path
		 A <see cref="String"/> containing the path of the file to open.
	 
	 @param access
		 One of the <see cref="FileAccess"/> value that specifies the operations that can be 
		 performed on the file. 
	 
	 @param mode
		 One of the <see cref="FileMode"/> values that specifies whether a file is created
		 if one does not exist, and determines whether the contents of existing files are 
		 retained or overwritten.
	 
	 @param share
		 One of the <see cref="FileShare"/> values specifying the type of access other threads 
		 have to the file. 
	 
	 @param bufferSize
		 An <see cref="Int32"/> containing the number of bytes to buffer for reads and writes
		 to the file, or 0 to specified the default buffer size, 1024.
	 
	 @param options
		 One or more of the <see cref="FileOptions"/> values that describes how to create or 
		 overwrite the file.
	 
	 @return 
		 A <see cref="FileStream"/> that provides access to the file specified in 
		 <paramref name="path"/>.
	 
	 @exception ArgumentNullException
		 <paramref name="path"/> is <see langword="null"/>.
	 
	 @exception ArgumentException
		 <paramref name="path"/> is an empty string (""), contains only white 
		 space, or contains one or more invalid characters as defined in 
		 <see cref="Path.GetInvalidPathChars()"/>.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> contains one or more components that exceed
		 the drive-defined maximum length. For example, on Windows-based 
		 platforms, components must not exceed 255 characters.
	 
	 @exception ArgumentOutOfRangeException
		 <paramref name="bufferSize"/> is less than 0.
	 
	 @exception PathTooLongException
		 <paramref name="path"/> exceeds the system-defined maximum length. 
		 For example, on Windows-based platforms, paths must not exceed 
		 32,000 characters.
	 
	 @exception DirectoryNotFoundException
		 One or more directories in <paramref name="path"/> could not be found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> refers to a file that is read-only and <paramref name="access"/>
		 is not <see cref="FileAccess.Read"/>.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> is a directory.
	 
	 @exception IOException
		 <paramref name="path"/> refers to a file that is in use.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static FileStream Open(String path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
	{

		boolean isAppend = false;
		if (FileMode.Append == mode)
		{
			mode = FileMode.OpenOrCreate;
			isAppend = true;
		}

		final int DefaultBufferSize = 1024;

		if (bufferSize == 0)
		{
			bufferSize = DefaultBufferSize;
		}

		String normalizedPath = LongPathCommon.NormalizeLongPath(path);

		SafeFileHandle handle = GetFileHandle(normalizedPath, mode, access, share, options);

		FileStream rs = new FileStream(handle, access, bufferSize, (options & FileOptions.Asynchronous) == FileOptions.Asynchronous);
		if (isAppend)
		{
			rs.Seek(0, SeekOrigin.End);
		}

		return rs;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification="False positive")]
	private static SafeFileHandle GetFileHandle(String normalizedPath, FileMode mode, FileAccess access, FileShare share, FileOptions options)
	{





		NativeMethods.EFileAccess underlyingAccess = GetUnderlyingAccess(access);

		SafeFileHandle handle;
		if (LongPathCommon.getIsInTransaction())
		{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
			TransactionHandle tx = NativeMethods.GetTransactionFromDTC();
			try
			{
				handle = NativeMethods.CreateFileTransacted(normalizedPath, underlyingAccess, (int)share, IntPtr.Zero, (int)mode, (int)options, IntPtr.Zero, tx, TXFS_MINIVERSION.TXFS_MINIVERSION_COMMITTED_VIEW, IntPtr.Zero);
			}
			finally
			{
				tx.dispose();
			}
		}
		else
		{

			handle = NativeMethods.CreateFile(normalizedPath, underlyingAccess, (int)share, IntPtr.Zero, (int)mode, (int)options, IntPtr.Zero);
		}
		if (handle.IsInvalid)
		{
			throw LongPathCommon.GetExceptionFromLastWin32Error();
		}

		return handle;
	}

	private static NativeMethods.EFileAccess GetUnderlyingAccess(FileAccess access)
	{

		switch (access)
		{
			case FileAccess.Read:
				return NativeMethods.EFileAccess.GenericRead;

			case FileAccess.Write:
				return NativeMethods.EFileAccess.GenericWrite;

			case FileAccess.ReadWrite:
				return NativeMethods.EFileAccess.GenericRead | NativeMethods.EFileAccess.GenericWrite;

			default:
				throw new ArgumentOutOfRangeException("access");
		}
	}

	public static void AppendAllText(String path, String text)
	{
		AppendAllText(path, text, Encoding.Default);
	}

	public static void AppendAllText(String path, String text, Encoding encoding)
	{
		FileStream fs = Open(path, FileMode.OpenOrCreate, FileAccess.Write);
		fs.Seek(0, SeekOrigin.End);
		try
		{
			if (text != null)
			{
				byte[] bytes = encoding.GetBytes(text);
				fs.Write(bytes, 0, bytes.length);
			}
		}
		finally
		{
			fs.Close();
		}
	}

	public static void WriteAllText(String path, String text)
	{
		WriteAllText(path, text, Encoding.Default);
	}
	public static void WriteAllText(String path, String text, Encoding encoding)
	{
		FileStream fs = Open(path, FileMode.Create, FileAccess.Write);
		try
		{
			if (text != null)
			{
				byte[] bytes = encoding.GetBytes(text);
				fs.Write(bytes, 0, bytes.length);
			}
		}
		finally
		{
			fs.Close();
		}
	}

	public static String ReadAllText(String path)
	{
		String rs = null;
		FileStream fs = Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		StreamReader sr = new StreamReader(fs);
		try
		{
			rs = sr.ReadToEnd();
		}
		finally
		{
			fs.Close();
		}
		return rs;
	}
}