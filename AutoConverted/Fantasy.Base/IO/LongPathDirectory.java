package Fantasy.IO;

import Fantasy.IO.Interop.*;
import Fantasy.Properties.*;

//     Copyright (c) Microsoft Corporation.  All rights reserved.


/** 
	 Provides methods for creating, deleting, moving and enumerating directories and 
	 subdirectories with long paths, that is, paths that exceed 259 characters.
 
*/
public final class LongPathDirectory
{

	/** 
		 Creates the specified directory.
	 
	 @param path
		 A <see cref="String"/> containing the path of the directory to create.
	 
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
		 <paramref name="path"/> contains one or more directories that could not be
		 found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
	 
	 @exception IOException
		 <paramref name="path"/> is a file.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	 
		 Note: Unlike <see cref="Directory.CreateDirectory(System.String)"/>, this method only creates 
		 the last directory in <paramref name="path"/>.
	 
	*/
	public static void Create(String path)
	{

		String parent = LongPath.GetDirectoryName(path);
		if (!LongPathDirectory.Exists(parent))
		{
			LongPathDirectory.Create(parent);
		}

		String normalizedPath = LongPathCommon.NormalizeLongPath(path);
		boolean success = false;
		if (LongPathCommon.getIsInTransaction())
		{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
			TransactionHandle tx = NativeMethods.GetTransactionFromDTC();
			try
			{
				success = NativeMethods.CreateDirectoryTransacted(null, normalizedPath,IntPtr.Zero, tx);

			}
			finally
			{
				tx.dispose();
			}
		}
		else
		{
			success = NativeMethods.CreateDirectory(normalizedPath, IntPtr.Zero);

		}

		if (!success)
		{

			// To mimic Directory.CreateDirectory, we don't throw if the directory (not a file) already exists
			int errorCode = Marshal.GetLastWin32Error();
			if (errorCode != NativeMethods.ERROR_ALREADY_EXISTS || !LongPathDirectory.Exists(path))
			{
				throw LongPathCommon.GetExceptionFromWin32Error(errorCode);
			}
		}
	}

	/** 
		 Deletes the specified empty directory.
	 
	 @param path
		  A <see cref="String"/> containing the path of the directory to delete.
	 
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
		 <paramref name="path"/> could not be found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> refers to a directory that is read-only.
	 
	 @exception IOException
		 <paramref name="path"/> is a file.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> refers to a directory that is not empty.
		 <p>
			 -or-    
		 </p>
		 <paramref name="path"/> refers to a directory that is in use.
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
				success = NativeMethods.RemoveDirectoryTransacted(normalizedPath, tx);
			}
			finally
			{
				tx.dispose();
			}
		}
		else
		{
			success = NativeMethods.RemoveDirectory(normalizedPath);
		}
		if (!success)
		{
			throw LongPathCommon.GetExceptionFromLastWin32Error();
		}
	}

	public static void Delete(String path, boolean recursive)
	{
		if (recursive)
		{
			for (String f : EnumerateFileSystemEntries(path).toArray())
			{
				if (Exists(f))
				{
					Delete(f, true);
				}
				else
				{
					LongPathFile.Delete(f);
				}
			}
		}
		Delete(path);
	}

	/** 
		 Returns a value indicating whether the specified path refers to an existing directory.
	 
	 @param path
		 A <see cref="String"/> containing the path to check.
	 
	 @return 
		 <see langword="true"/> if <paramref name="path"/> refers to an existing directory; 
		 otherwise, <see langword="false"/>.
	 
	 
		 Note that this method will return false if any error occurs while trying to determine 
		 if the specified directory exists. This includes situations that would normally result in 
		 thrown exceptions including (but not limited to); passing in a directory name with invalid 
		 or too many characters, an I/O error such as a failing or missing disk, or if the caller
		 does not have Windows or Code Access Security (CAS) permissions to to read the directory.
	 
	*/
	public static boolean Exists(String path)
	{

		boolean isDirectory = false;
		RefObject<Boolean> tempRef_isDirectory = new RefObject<Boolean>(isDirectory);
		boolean tempVar = LongPathCommon.Exists(path, tempRef_isDirectory);
			isDirectory = tempRef_isDirectory.argvalue;
		if (tempVar)
		{

			return isDirectory;
		}

		return false;
	}

	/** 
		 Returns a enumerable containing the directory names of the specified directory.
	 
	 @param path
		 A <see cref="String"/> containing the path of the directory to search.
	 
	 @return 
		 A <see cref="IEnumerable{T}"/> containing the directory names within <paramref name="path"/>.
	 
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
		 <paramref name="path"/> contains one or more directories that could not be
		 found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
	 
	 @exception IOException
		 <paramref name="path"/> is a file.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static Iterable<String> EnumerateDirectories(String path)
	{

		return EnumerateDirectories(path, (String)null);
	}

	/** 
		 Returns a enumerable containing the directory names of the specified directory that 
		 match the specified search pattern.
	 
	 @param path
		 A <see cref="String"/> containing the path of the directory to search.
	 
	 @param searchPattern
		 A <see cref="String"/> containing search pattern to match against the names of the 
		 directories in <paramref name="path"/>, otherwise, <see langword="null"/> or an empty 
		 string ("") to use the default search pattern, "*".
	 
	 @return 
		 A <see cref="IEnumerable{T}"/> containing the directory names within <paramref name="path"/>
		 that match <paramref name="searchPattern"/>.
	 
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
		 <paramref name="path"/> contains one or more directories that could not be
		 found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
	 
	 @exception IOException
		 <paramref name="path"/> is a file.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static Iterable<String> EnumerateDirectories(String path, String searchPattern)
	{

		return EnumerateFileSystemEntries(path, searchPattern, true, false);
	}

	/** 
		 Returns a enumerable containing the file names of the specified directory.
	 
	 @param path
		 A <see cref="String"/> containing the path of the directory to search.
	 
	 @return 
		 A <see cref="IEnumerable{T}"/> containing the file names within <paramref name="path"/>.
	 
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
		 <paramref name="path"/> contains one or more directories that could not be
		 found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
	 
	 @exception IOException
		 <paramref name="path"/> is a file.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static Iterable<String> EnumerateFiles(String path)
	{

		return EnumerateFiles(path, (String)null);
	}


//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public static IEnumerable<string> EnumerateAllFiles(string path, string searchPattern = null)
	public static Iterable<String> EnumerateAllFiles(String path, String searchPattern)
	{
		for (String file : EnumerateFiles(path, searchPattern))
		{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
			yield return file;
		}

		for (String subDir : EnumerateDirectories(path))
		{
			for (String file : EnumerateAllFiles(subDir, searchPattern))
			{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
				yield return file;
			}
		}
	}

	/** 
		 Returns a enumerable containing the file names of the specified directory that 
		 match the specified search pattern.
	 
	 @param path
		 A <see cref="String"/> containing the path of the directory to search.
	 
	 @param searchPattern
		 A <see cref="String"/> containing search pattern to match against the names of the 
		 files in <paramref name="path"/>, otherwise, <see langword="null"/> or an empty 
		 string ("") to use the default search pattern, "*".
	 
	 @return 
		 A <see cref="IEnumerable{T}"/> containing the file names within <paramref name="path"/>
		 that match <paramref name="searchPattern"/>.
	 
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
		 <paramref name="path"/> contains one or more directories that could not be
		 found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
	 
	 @exception IOException
		 <paramref name="path"/> is a file.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static Iterable<String> EnumerateFiles(String path, String searchPattern)
	{

		return EnumerateFileSystemEntries(path, searchPattern, false, true);
	}

	/** 
		 Returns a enumerable containing the file and directory names of the specified directory.
	 
	 @param path
		 A <see cref="String"/> containing the path of the directory to search.
	 
	 @return 
		 A <see cref="IEnumerable{T}"/> containing the file and directory names within 
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
		 <paramref name="path"/> contains one or more directories that could not be
		 found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
	 
	 @exception IOException
		 <paramref name="path"/> is a file.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static Iterable<String> EnumerateFileSystemEntries(String path)
	{

		return EnumerateFileSystemEntries(path, (String)null);
	}

	/** 
		 Returns a enumerable containing the file and directory names of the specified directory 
		 that match the specified search pattern.
	 
	 @param path
		 A <see cref="String"/> containing the path of the directory to search.
	 
	 @param searchPattern
		 A <see cref="String"/> containing search pattern to match against the names of the 
		 files and directories in <paramref name="path"/>, otherwise, <see langword="null"/> 
		 or an empty string ("") to use the default search pattern, "*".
	 
	 @return 
		 A <see cref="IEnumerable{T}"/> containing the file and directory names within 
		 <paramref name="path"/>that match <paramref name="searchPattern"/>.
	 
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
		 <paramref name="path"/> contains one or more directories that could not be
		 found.
	 
	 @exception UnauthorizedAccessException
		 The caller does not have the required access permissions.
	 
	 @exception IOException
		 <paramref name="path"/> is a file.
		 <p>
			 -or-
		 </p>
		 <paramref name="path"/> specifies a device that is not ready.
	 
	*/
	public static Iterable<String> EnumerateFileSystemEntries(String path, String searchPattern)
	{

		return EnumerateFileSystemEntries(path, searchPattern, true, true);
	}

	private static Iterable<String> EnumerateFileSystemEntries(String path, String searchPattern, boolean includeDirectories, boolean includeFiles)
	{

		String normalizedSearchPattern = LongPathCommon.NormalizeSearchPattern(searchPattern);
		String normalizedPath = LongPathCommon.NormalizeLongPath(path);

		// First check whether the specified path refers to a directory and exists
		FileAttributes attributes = null;
		RefObject<FileAttributes> tempRef_attributes = new RefObject<FileAttributes>(attributes);
		int errorCode = LongPathCommon.TryGetDirectoryAttributes(normalizedPath, tempRef_attributes);
		attributes = tempRef_attributes.argvalue;
		if (errorCode != 0)
		{
			throw LongPathCommon.GetExceptionFromWin32Error(errorCode);
		}

		return EnumerateFileSystemIterator(normalizedPath, normalizedSearchPattern, includeDirectories, includeFiles);
	}

	private static Iterable<String> EnumerateFileSystemIterator(String normalizedPath, String normalizedSearchPattern, boolean includeDirectories, boolean includeFiles)
	{
		// NOTE: Any exceptions thrown from this method are thrown on a call to IEnumerator<string>.MoveNext()

		String path = LongPathCommon.RemoveLongPathPrefix(normalizedPath);

		TransactionHandle tx = null;
		if (LongPathCommon.getIsInTransaction())
		{
			tx = NativeMethods.GetTransactionFromDTC();
		}
		try
		{
			NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (SafeFindHandle handle = BeginFind(Path.Combine(normalizedPath, normalizedSearchPattern), out findData, tx))

			RefObject<NativeMethods.WIN32_FIND_DATA> tempRef_findData = new RefObject<NativeMethods.WIN32_FIND_DATA>(findData);            SafeFindHandle handle = BeginFind(Path.Combine(normalizedPath, normalizedSearchPattern), tempRef_findData, tx);
				findData = tempRef_findData.argvalue;
			try
			{
				if (handle == null)
				{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
					yield break;
				}

				do
				{
					String currentFileName = findData.cFileName;

					if (IsDirectory(findData.dwFileAttributes))
					{
						if (includeDirectories && !IsCurrentOrParentDirectory(currentFileName))
						{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
							yield return Path.Combine(path, currentFileName);
						}
					}
					else
					{
						if (includeFiles)
						{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
							yield return Path.Combine(path, currentFileName);
						}
					}
				RefObject<NativeMethods.WIN32_FIND_DATA> tempRef_findData = new RefObject<NativeMethods.WIN32_FIND_DATA>(findData);
				} while (NativeMethods.FindNextFile(handle, tempRef_findData));
				findData = tempRef_findData.argvalue;

				int errorCode = Marshal.GetLastWin32Error();
				if (errorCode != NativeMethods.ERROR_NO_MORE_FILES)
				{
					throw LongPathCommon.GetExceptionFromWin32Error(errorCode);
				}
			}
			finally
			{
				handle.dispose();
			}
		}
		finally
		{
			if (tx != null)
			{
				tx.dispose();
			}
		}
	}

	private static SafeFindHandle BeginFind(String normalizedPathWithSearchPattern, RefObject<NativeMethods.WIN32_FIND_DATA> findData, TransactionHandle tx)
	{

		SafeFindHandle handle;
		if(tx != null)
		{
			handle = NativeMethods.FindFirstFileTransacted(normalizedPathWithSearchPattern, FINDEX_INFO_LEVELS.FindExInfoStandard, findData, FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, 0, tx);

		}
		else
		{
			handle = NativeMethods.FindFirstFile(normalizedPathWithSearchPattern, findData);
		}

		if (handle.IsInvalid)
		{

			int errorCode = Marshal.GetLastWin32Error();
			if (errorCode != NativeMethods.ERROR_FILE_NOT_FOUND)
			{
				throw LongPathCommon.GetExceptionFromWin32Error(errorCode);
			}

			return null;
		}

		return handle;
	}

	public static boolean IsDirectory(FileAttributes attributes)
	{

		return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
	}

	private static boolean IsCurrentOrParentDirectory(String directoryName)
	{

		return directoryName.equals(".", StringComparison.OrdinalIgnoreCase) || directoryName.equals("..", StringComparison.OrdinalIgnoreCase);
	}

	public static DiskspaceInfo GetDiskspaceInfo(String path)
	{
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: ulong freeBytesAvailable;
		long freeBytesAvailable;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: ulong totalNumberOfBytes;
		long totalNumberOfBytes;
//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: ulong totalNumberOfFreeBytes;
		long totalNumberOfFreeBytes;

		if (DotNetToJavaStringHelper.isNullOrEmpty(path))
		{
			throw new IllegalArgumentException(Resources.getArgumentCannotBeNullOrEmptyText(), path);
		}

		if (path.length() > 255)
		{
			path = LongPath.GetShortPathName(path);
		}

		RefObject<Long> tempRef_freeBytesAvailable = new RefObject<Long>(freeBytesAvailable);
		RefObject<Long> tempRef_totalNumberOfBytes = new RefObject<Long>(totalNumberOfBytes);
		RefObject<Long> tempRef_totalNumberOfFreeBytes = new RefObject<Long>(totalNumberOfFreeBytes);
		boolean tempVar = NativeMethods.GetDiskFreeSpaceEx(path, tempRef_freeBytesAvailable, tempRef_totalNumberOfBytes, tempRef_totalNumberOfFreeBytes);
			freeBytesAvailable = tempRef_freeBytesAvailable.argvalue;
		totalNumberOfBytes = tempRef_totalNumberOfBytes.argvalue;
		totalNumberOfFreeBytes = tempRef_totalNumberOfFreeBytes.argvalue;
		if (tempVar)
		{
			return new DiskspaceInfo(freeBytesAvailable, totalNumberOfBytes, totalNumberOfFreeBytes);
		}
		else
		{
			throw new Win32Exception();
		}
	}
}