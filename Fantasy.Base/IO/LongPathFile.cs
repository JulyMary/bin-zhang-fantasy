//     Copyright (c) Microsoft Corporation.  All rights reserved.
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Fantasy.IO.Interop;
using Microsoft.Win32.SafeHandles;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.IO {

    /// <summary>
    ///     Provides static methods for creating, copying, deleting, moving, and opening of files
    ///     with long paths, that is, paths that exceed 259 characters.
    /// </summary>
    public static class LongPathFile {

        /// <summary>
        ///     Returns a value indicating whether the specified path refers to an existing file.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="String"/> containing the path to check.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="path"/> refers to an existing file; 
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        ///     Note that this method will return false if any error occurs while trying to determine 
        ///     if the specified file exists. This includes situations that would normally result in 
        ///     thrown exceptions including (but not limited to); passing in a file name with invalid 
        ///     or too many characters, an I/O error such as a failing or missing disk, or if the caller
        ///     does not have Windows or Code Access Security (CAS) permissions to to read the file.
        /// </remarks>
        public static bool Exists(string path) {

            bool isDirectory;
            if (LongPathCommon.Exists(path, out isDirectory)) {

                return !isDirectory;
            }

            return false;
        }

        /// <summary>
        ///     Deletes the specified file.
        /// </summary>
        /// <param name="path">
        ///      A <see cref="String"/> containing the path of the file to delete.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white 
        ///     space, or contains one or more invalid characters as defined in 
        ///     <see cref="Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based 
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length. 
        ///     For example, on Windows-based platforms, paths must not exceed 
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        ///     <paramref name="path"/> could not be found.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     One or more directories in <paramref name="path"/> could not be found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> refers to a file that is read-only.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> is a directory.
        /// </exception>
        /// <exception cref="IOException">
        ///     <paramref name="path"/> refers to a file that is in use.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static void Delete(string path) {

            string normalizedPath = LongPathCommon.NormalizeLongPath(path);
            bool success;
            if (LongPathCommon.IsInTransaction)
            {
                using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
                {
                    success = NativeMethods.DeleteFileTransacted(normalizedPath, tx);
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

   

        public static void Move(string sourcePath, string destinationPath, bool overwrite = false, IProgressMonitor progress = null)
        {
            string normalizedSourcePath = LongPathCommon.NormalizeLongPath(sourcePath, "sourcePath");
            string normalizedDestinationPath = LongPathCommon.NormalizeLongPath(destinationPath, "destinationPath");
            CopyProgressRoutine routine = progress != null ? CopyProgressRoutineHelper.Create(progress) : null;
            MoveFileFlags flags = MoveFileFlags.MOVEFILE_COPY_ALLOWED | MoveFileFlags.MOVEFILE_WRITE_THROUGH;
            if (overwrite)
            {
                flags |= MoveFileFlags.MOVEFILE_REPLACE_EXISTING; 
            }

            bool success;
            if (LongPathCommon.IsInTransaction)
            {
                using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
                {
                    success = NativeMethods.MoveFileTransacted(normalizedSourcePath, normalizedDestinationPath, routine, IntPtr.Zero, flags, tx);
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



        public static void Copy(string sourcePath, string destinationPath, bool overwrite = false, IProgressMonitor progress = null)
        {
            string normalizedSourcePath = LongPathCommon.NormalizeLongPath(sourcePath, "sourcePath");
            string normalizedDestinationPath = LongPathCommon.NormalizeLongPath(destinationPath, "destinationPath");

            CopyProgressRoutine routine = progress != null ? CopyProgressRoutineHelper.Create(progress) : null;

            CopyFileFlags flags = overwrite ? CopyFileFlags.COPY_FILE_NONE : CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS;
            int cancel = 0;

            bool success;
            if (LongPathCommon.IsInTransaction)
            {
                using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
                {
                    success = NativeMethods.CopyFileTransacted(normalizedSourcePath, normalizedDestinationPath, routine, IntPtr.Zero, ref cancel, flags, tx);
                }
            }
            else
            {
                success = NativeMethods.CopyFileEx(normalizedSourcePath, normalizedDestinationPath, routine, IntPtr.Zero, ref cancel, flags);
            }


            if (!success)
            {
                throw LongPathCommon.GetExceptionFromLastWin32Error();
            }

        }

        

        /// <summary>
        ///     Opens the specified file.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="String"/> containing the path of the file to open.
        /// </param>
        /// <param name="access">
        ///     One of the <see cref="FileAccess"/> value that specifies the operations that can be 
        ///     performed on the file. 
        /// </param>
        /// <param name="mode">
        ///     One of the <see cref="FileMode"/> values that specifies whether a file is created
        ///     if one does not exist, and determines whether the contents of existing files are 
        ///     retained or overwritten.
        /// </param>
        /// <returns>
        ///     A <see cref="FileStream"/> that provides access to the file specified in 
        ///     <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white 
        ///     space, or contains one or more invalid characters as defined in 
        ///     <see cref="Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based 
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length. 
        ///     For example, on Windows-based platforms, paths must not exceed 
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     One or more directories in <paramref name="path"/> could not be found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> refers to a file that is read-only and <paramref name="access"/>
        ///     is not <see cref="FileAccess.Read"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> is a directory.
        /// </exception>
        /// <exception cref="IOException">
        ///     <paramref name="path"/> refers to a file that is in use.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static FileStream Open(string path, FileMode mode, FileAccess access) {

            return Open(path, mode, access, FileShare.None, 0, FileOptions.None);
        }

        /// <summary>
        ///     Opens the specified file.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="String"/> containing the path of the file to open.
        /// </param>
        /// <param name="access">
        ///     One of the <see cref="FileAccess"/> value that specifies the operations that can be 
        ///     performed on the file. 
        /// </param>
        /// <param name="mode">
        ///     One of the <see cref="FileMode"/> values that specifies whether a file is created
        ///     if one does not exist, and determines whether the contents of existing files are 
        ///     retained or overwritten.
        /// </param>
        /// <param name="share">
        ///     One of the <see cref="FileShare"/> values specifying the type of access other threads 
        ///     have to the file. 
        /// </param>
        /// <returns>
        ///     A <see cref="FileStream"/> that provides access to the file specified in 
        ///     <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white 
        ///     space, or contains one or more invalid characters as defined in 
        ///     <see cref="Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based 
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length. 
        ///     For example, on Windows-based platforms, paths must not exceed 
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     One or more directories in <paramref name="path"/> could not be found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> refers to a file that is read-only and <paramref name="access"/>
        ///     is not <see cref="FileAccess.Read"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> is a directory.
        /// </exception>
        /// <exception cref="IOException">
        ///     <paramref name="path"/> refers to a file that is in use.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share) {

            return Open(path, mode, access, share, 0, FileOptions.None);
        }

        /// <summary>
        ///     Opens the specified file.
        /// </summary>
        /// <param name="path">
        ///     A <see cref="String"/> containing the path of the file to open.
        /// </param>
        /// <param name="access">
        ///     One of the <see cref="FileAccess"/> value that specifies the operations that can be 
        ///     performed on the file. 
        /// </param>
        /// <param name="mode">
        ///     One of the <see cref="FileMode"/> values that specifies whether a file is created
        ///     if one does not exist, and determines whether the contents of existing files are 
        ///     retained or overwritten.
        /// </param>
        /// <param name="share">
        ///     One of the <see cref="FileShare"/> values specifying the type of access other threads 
        ///     have to the file. 
        /// </param>
        /// <param name="bufferSize">
        ///     An <see cref="Int32"/> containing the number of bytes to buffer for reads and writes
        ///     to the file, or 0 to specified the default buffer size, 1024.
        /// </param>
        /// <param name="options">
        ///     One or more of the <see cref="FileOptions"/> values that describes how to create or 
        ///     overwrite the file.
        /// </param>
        /// <returns>
        ///     A <see cref="FileStream"/> that provides access to the file specified in 
        ///     <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="path"/> is an empty string (""), contains only white 
        ///     space, or contains one or more invalid characters as defined in 
        ///     <see cref="Path.GetInvalidPathChars()"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> contains one or more components that exceed
        ///     the drive-defined maximum length. For example, on Windows-based 
        ///     platforms, components must not exceed 255 characters.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="bufferSize"/> is less than 0.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     <paramref name="path"/> exceeds the system-defined maximum length. 
        ///     For example, on Windows-based platforms, paths must not exceed 
        ///     32,000 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     One or more directories in <paramref name="path"/> could not be found.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the required access permissions.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> refers to a file that is read-only and <paramref name="access"/>
        ///     is not <see cref="FileAccess.Read"/>.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> is a directory.
        /// </exception>
        /// <exception cref="IOException">
        ///     <paramref name="path"/> refers to a file that is in use.
        ///     <para>
        ///         -or-
        ///     </para>
        ///     <paramref name="path"/> specifies a device that is not ready.
        /// </exception>
        public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options) {

            bool isAppend = false;
            if (FileMode.Append == mode)
            {
                mode = FileMode.OpenOrCreate;
                isAppend = true;
            }

            const int DefaultBufferSize = 1024;

            if (bufferSize == 0)
                bufferSize = DefaultBufferSize;

            string normalizedPath =  LongPathCommon.NormalizeLongPath(path);

            SafeFileHandle handle = GetFileHandle(normalizedPath, mode, access, share, options);

            FileStream rs = new FileStream(handle, access, bufferSize, (options & FileOptions.Asynchronous) == FileOptions.Asynchronous);
            if (isAppend)
            {
                rs.Seek(0, SeekOrigin.End);
            }

            return rs;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification="False positive")]
        private static SafeFileHandle GetFileHandle(string normalizedPath, FileMode mode, FileAccess access, FileShare share, FileOptions options) {

         

           

            NativeMethods.EFileAccess underlyingAccess = GetUnderlyingAccess(access);
           
            SafeFileHandle handle;
            if (LongPathCommon.IsInTransaction)
            {
                using (TransactionHandle tx = NativeMethods.GetTransactionFromDTC())
                {
                    handle = NativeMethods.CreateFileTransacted(normalizedPath, underlyingAccess, (uint)share, IntPtr.Zero, (uint)mode, (uint)options, IntPtr.Zero, tx, TXFS_MINIVERSION.TXFS_MINIVERSION_COMMITTED_VIEW, IntPtr.Zero); 
                }
            }
            else
            {

                handle = NativeMethods.CreateFile(normalizedPath, underlyingAccess, (uint)share, IntPtr.Zero, (uint)mode, (uint)options, IntPtr.Zero);
            }
            if (handle.IsInvalid)
                throw LongPathCommon.GetExceptionFromLastWin32Error();

            return handle;
        }

        private static NativeMethods.EFileAccess GetUnderlyingAccess(FileAccess access) {

            switch (access) {
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

        public static void AppendAllText(string path, string text)
        {
            FileStream fs = Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Seek(0, SeekOrigin.End);  
            try
            {
                if (text != null)
                {
                    byte[] bytes = Encoding.Default.GetBytes(text);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            finally
            {
                fs.Close();
            }
        }


        public static void WriteAllText(string path, string text, Encoding encoding)
        {
            FileStream fs = Open(path, FileMode.Create, FileAccess.Write);
            try
            {
                if (text != null)
                {
                    byte[] bytes = encoding.GetBytes(text);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            finally
            {
                fs.Close();
            }
        }

        public static void WriteAllText(string path, string text)
        {
            WriteAllText(path, text, Encoding.Default);
        }

        public static string ReadAllText(string path)
        {
            string rs = null;
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
}
