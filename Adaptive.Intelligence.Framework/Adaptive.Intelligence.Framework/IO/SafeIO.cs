using Adaptive.Intelligence.Common;
using Adaptive.Intelligence.Converters;
using Adaptive.Intelligence.Enumerations;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides static methods and functions for performing standard IO operations with
    /// exception capturing and logging.
    /// </summary>
    public static class SafeIO
    {
        #region Private Win32 API Declarations		
        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="handle">
        /// An <see cref="IntPtr"/> containing the specified handle.
        /// </param>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern void CloseHandle(IntPtr handle);

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the name of the file.</param>
        /// <param name="access">The access.</param>
        /// <param name="share">The share.</param>
        /// <param name="securityAttributes">The security attributes.</param>
        /// <param name="creationDisposition">The creation disposition.</param>
        /// <param name="flagsAndAttributes">The flags and attributes.</param>
        /// <param name="templateFile">The template file.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFile(string fileName, FileAccess access, FileShare share, IntPtr securityAttributes, FileMode creationDisposition, FileAttributes flagsAndAttributes, IntPtr templateFile);

        /// <summary>
        /// Gets the file size ex.
        /// </summary>
        /// <param name="fileHandle">The file handle.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern long GetFileSizeEx(IntPtr fileHandle, out long fileSize);
        #endregion

        #region Public Static Methods / Functions		
        /// <summary>
        /// Copies the specified file to the specified location.
        /// </summary>
        /// <remarks>
        /// This method requires that <i>newFile</i> does NOT already exist, but 
        /// <i>originalFile</i> does and is not in use by another process.
        /// </remarks>
        /// <param name="originalFile">
        /// A string containing the fully-qualified path and name of the original file.
        /// </param>
        /// <param name="newFile">
        /// A string containing the fully-qualified path and name of the new file to be created.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public static bool CopyFile(string originalFile, string newFile)
        {
            bool success = false;

            if (!FileExists(newFile) && FileExists(originalFile))
            {
                try
                {
                    File.Copy(originalFile, newFile);
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return success;
        }

        /// <summary>
        /// Creates the file for an exclusive write operation.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file to be created.
        /// </param>
        /// <returns>
        /// The new <see cref="FileStream"/> instance if successful; otherwise, returns
        /// <b>null</b>.
        /// </returns>
        public static FileStream? CreateFileForExclusiveWrite(string fileName)
        {
            FileStream? stream;

            if (FileExists(fileName))
            {
                DeleteFile(fileName);
            }

            try
            {
                stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                stream = null;
            }

            return stream;
        }

        /// <summary>
        /// Decompresses the GZ file.
        /// </summary>
        /// <param name="inputFile">
        /// The fully-qualified path and name of the input file.
        /// </param>
        /// <param name="outputFile">
        /// The fully-qualified path and name of the output file.
        /// </param>
        /// <param name="deleteOriginal">
        /// <b>true</b> to delete the original file on successful completion;
        /// otherwise, <b>false</b>.
        /// </param>
        /// <param name="token">
        /// An optional <see cref="CancellationToken"/> that can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public static async Task<bool> DecompressGZFileAsync(string inputFile, string outputFile, bool deleteOriginal, CancellationToken token = default)
        {
            FileStream? inStream = null;
            FileStream? outStream = null;

            bool success = false;

            try
            {
                inStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            if (inStream != null)
            {
                DeleteFile(outputFile);
                try
                {
                    outStream = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }

                if (outStream != null)
                {
                    GZipStream decompressStream = new(inStream, CompressionMode.Decompress);

                    try
                    {
                        Memory<byte> buffer = new byte[524288];
                        int readCount;
                        do
                        {
                            readCount = await decompressStream.ReadAsync(buffer, token).ConfigureAwait(false);
                            if (readCount > 0 && !token.IsCancellationRequested)
                            {
                                await outStream.WriteAsync(buffer[..readCount], token).ConfigureAwait(false);
                            }
                        } while (readCount > 0);

                        success = true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }

                    await outStream.FlushAsync(token).ConfigureAwait(false);
                    decompressStream.Close();
                    await outStream.DisposeAsync();
                }
                await inStream.DisposeAsync();

                if (success && deleteOriginal)
                {
                    DeleteFile(inputFile);
                }
                else if (!success)
                {
                    DeleteFile(outputFile);
                }
            }
            return success;
        }

        /// <summary>
        /// Deletes all the files in the specified directory.
        /// </summary>
        /// <param name="pathName">
        /// The name of the path.</param>
        /// <returns>
        /// <b>true</b> if all files are deleted; <b>false</b> if an error occurs accessing
        /// the directory or deleting the files.
        /// </returns>
        public static bool DeleteAllFilesInDirectory(string pathName)
        {
            bool success = false;
            int deleteCount = 0;

            if (!string.IsNullOrEmpty(pathName) && DirectoryExists(pathName))
            {
                string[]? files = GetFilesInPath(pathName);
                if (files != null && files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        success = DeleteFile(file);
                        if (success)
                        {
                            deleteCount++;
                        }
                    }
                }
                if (files != null)
                {
                    success = deleteCount == files.Length;
                }
            }
            return success;
        }

        /// <summary>
        /// Delete the files that match the wild card.
        /// </summary>
        /// <param name="pathName">
        /// A string containing the fully-qualified path in which to delete files.
        /// </param>
        /// <param name="wildCard">
        /// A string containing the wild-card specification for the files to be deleted.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public static bool DeleteAllFilesInDirectory(string pathName, string wildCard)
        {
            bool success = false;
            int deleteCount = 0;

            if (!string.IsNullOrEmpty(pathName) && DirectoryExists(pathName))
            {
                string[]? files = GetFilesInPath(pathName, wildCard);
                if (files != null && files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        success = DeleteFile(file);
                        if (success)
                        {
                            deleteCount++;
                        }
                    }
                }
                if (files != null)
                {
                    success = deleteCount == files.Length;
                }
            }
            return success;
        }

        /// <summary>
        /// Attempts to delete the specified file.
        /// </summary>
        /// <param name="pathAndFileName">
        /// The fully-qualified path and name of the file to delete.</param>
        /// <returns>
        /// <b>true</b> if the delete is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public static bool DeleteFile(string pathAndFileName)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(pathAndFileName))
            {

                try
                {
                    if (File.Exists(pathAndFileName))
                    {
                        File.Delete(pathAndFileName);
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                catch (DirectoryNotFoundException dirNotFoundEx)
                {
                    System.Diagnostics.Trace.TraceError(dirNotFoundEx.Message);
                }
                catch (PathTooLongException pathEx)
                {
                    System.Diagnostics.Trace.TraceError(pathEx.Message);
                }
                catch (IOException ioEx)
                {
                    System.Diagnostics.Trace.TraceError(ioEx.Message);
                }
                catch (SecurityException securityEx)
                {
                    System.Diagnostics.Trace.TraceError(securityEx.Message);
                }
                catch (NotSupportedException noSupportEx)
                {
                    System.Diagnostics.Trace.TraceError(noSupportEx.Message);
                }
                catch (UnauthorizedAccessException accessEx)
                {
                    System.Diagnostics.Trace.TraceError(accessEx.Message);
                }

            }
            return success;
        }

        /// <summary>
        /// Attempts to delete the specified file.
        /// </summary>
        /// <param name="fileInstance">
        /// A <see cref="FileInfo"/> instance representing the specified file.
        /// </param>
        /// <returns>
        /// <b>true</b> if the delete is successful; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        public static bool DeleteFile(FileInfo? fileInstance)
        {
            bool success = false;
            if (fileInstance != null)
            {
                try
                {
                    fileInstance.Delete();
                    success = true;
                }
                catch (IOException ioEx)
                {
                    System.Diagnostics.Trace.TraceError(ioEx.Message);
                }
                catch (SecurityException securityEx)
                {
                    System.Diagnostics.Trace.TraceError(securityEx.Message);
                }
                catch (UnauthorizedAccessException accessEx)
                {
                    System.Diagnostics.Trace.TraceError(accessEx.Message);
                }
            }
            return success;
        }

        /// <summary>
        /// Attempts to delete the specified file.
        /// </summary>
        /// <param name="fileName">
        /// The fully-qualified path and name of the file to be deleted.
        /// </param>
        /// <returns>
        /// An <see cref="OperationResult"/> containing the result of the operation.
        /// </returns>
        public static OperationResult DeleteFileWithResult(string fileName)
        {
            OperationResult result;

            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    result = new OperationResult(true);
                }
                else
                {
                    result = new OperationResult(false, "The specified file does not exist.");
                }
            }
            catch (Exception ex)
            {
                result = new OperationResult(ex);
            }
            return result;
        }

        /// <summary>
        /// Determines the type of the file.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the name of the file with extension.
        /// </param>
        /// <returns>
        /// A <see cref="FileFormats"/> enumerated value indicating the file type.
        /// </returns>
        public static FileFormats DetermineFileFormat(string? fileName)
        {
            FileFormats format = FileFormats.NotSpecified;

            if (!string.IsNullOrEmpty(fileName))
            {
                string ext = Path.GetExtension(fileName);
                if (!string.IsNullOrWhiteSpace(ext))
                {
                    FileFormatConverter converter = new();
                    format = converter.ConvertBack(ext);
                }
            }
            return format;
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="pathName">
        /// The path to test.
        /// </param>
        /// <returns>
        /// <b>true</b> if <i>pathName</i> refers to an existing directory;
        /// <b>false</b> if the directory does not exist or an error occurs when trying to
        /// determine if the specified directory exists.
        /// </returns>
        public static bool DirectoryExists(string? pathName)
        {
            bool isPresent = false;

            if (pathName != null)
            {

                try
                {
                    isPresent = Directory.Exists(pathName);
                }
                catch (SecurityException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return isPresent;
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="pathAndFileName">
        /// The fully-qualified path and name of the file.</param>
        /// <returns>
        /// <b>true</b> if the caller has the required permissions and <i>pathAndFileName</i> contains the name of
        /// an existing file; otherwise, <b>false</b>. This method also returns <b>false</b> if
        /// <i>pathAndFileName</i> is <b>null</b>, an invalid path, or a zero-length string.
        /// If the caller does not have sufficient permissions to read the specified file, no
        /// exception is thrown and the method returns false regardless of the existence of
        /// <i>pathAndFileName</i>.
        /// </returns>
        public static bool FileExists(string pathAndFileName)
        {
            bool isPresent = false;

            try
            {
                isPresent = File.Exists(pathAndFileName);
            }
            catch (SecurityException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }
            catch (IOException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            return isPresent;
        }

        /// <summary>
        /// Attempts to locate the first USB drive.
        /// </summary>
        /// <returns>
        /// A <see cref="DirectoryInfo"/> instance containing the directory information
        /// for the drive, or <b>null</b> if not found.
        /// </returns>
        public static DirectoryInfo? FindUSBDrive()
        {
            DirectoryInfo? pathInfo = null;
            DriveInfo[]? driveList = null;

            try
            {
                driveList = DriveInfo.GetDrives();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            if (driveList != null)
            {
                foreach (DriveInfo drive in driveList)
                {
                    if (drive.DriveType == DriveType.Removable && drive.IsReady)
                    {
                        pathInfo = drive.RootDirectory;
                    }
                }
            }
            return pathInfo;
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="fileName">
        /// The fully-qualified path and name of the file to be tested for.
        /// </param>
        /// <returns>
        /// An <see cref="OperationResult{T}"/> instance indicating the result
        /// of the operation where the <see cref="OperationResult{T}.DataContent"/>
        /// property contains the indicator as to whether the file exists if the operation
        /// is successful.
        /// </returns>
        public static OperationResult<bool> FileExistsWithResult(string fileName)
        {
            OperationResult<bool> result;

            try
            {
                bool exists = File.Exists(fileName);
                result = new OperationResult<bool>(true)
                {
                    DataContent = exists
                };
            }
            catch (Exception ex)
            {
                result = new OperationResult<bool>(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the application path.
        /// </summary>
        /// <returns>
        /// A string containing the path the current application or module is running from.
        /// </returns>
        public static string? GetAppPath()
        {
            string? path = null;

            try
            {
                System.Diagnostics.Process? selfProcess = System.Diagnostics.Process.GetCurrentProcess();
                if (selfProcess != null)
                {
                    if (selfProcess.MainModule != null)
                    {
                        path = selfProcess.MainModule.FileName;
                        path = System.IO.Path.GetDirectoryName(path);
                    }
                    selfProcess.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }
            return path;
        }

        /// <summary>
        /// Returns the names of subdirectories (including their paths) in the specified directory.
        /// </summary>
        /// <param name="pathName">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <returns>
        /// Type: An <see cref="Array"/> of <see cref="string"/>.
        /// An array of the full names (including paths) of subdirectories in the specified path, or an empty array if no directories are found.
        /// </returns>
        public static string[]? GetDirectories(string pathName)
        {
            string[]? subDirList = null;

            if (!string.IsNullOrEmpty(pathName))
            {
                try
                {
                    subDirList = Directory.GetDirectories(pathName);
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (PathTooLongException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return subDirList;
        }

        /// <summary>
        /// Gets the list of all files in the specified path.
        /// </summary>
        /// <param name="pathName">
        /// The fully-qualified path name.</param>
        /// <returns>
        /// An array of <see cref="string"/> containing the
        /// file names, or <b>null</b> if an error occurs reading the file list.
        /// </returns>
        public static string[]? GetFilesInPath(string pathName)
        {
            string[]? list = null;

            if ((!string.IsNullOrEmpty(pathName)) && DirectoryExists(pathName))
            {
                try
                {
                    list = Directory.GetFiles(pathName);
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (PathTooLongException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets the list of all files in the specified path.
        /// </summary>
        /// <param name="pathName">
        /// The fully-qualified path name.
        /// </param>
        /// <param name="wildCard">
        /// A string containing the wildcard to search on.
        /// </param>
        /// <returns>
        /// An array of <see cref="string"/> containing the
        /// file names, or <b>null</b> if an error occurs reading the file list.
        /// </returns>
        public static string[]? GetFilesInPath(string? pathName, string? wildCard)
        {
            string[]? list = null;

            if ((!string.IsNullOrEmpty(pathName)) && DirectoryExists(pathName) && (wildCard != null))
            {
                try
                {
                    list = Directory.GetFiles(pathName, wildCard);
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (PathTooLongException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        /// <returns>
        /// An integer indicating the size of the file, or zero (0) if the operation fails.
        /// </returns>
        public static int GetFileSize(string? fileName)
        {
            int size = 0;

            if (!string.IsNullOrEmpty(fileName))
            {
                if (FileExists(fileName))
                {
                    try
                    {
                        FileInfo info = new(fileName);
                        size = (int)info.Length;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }
                }
            }

            return size;
        }

        /// <summary>
        /// Gets the size of the file using the Win32 API.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        /// <returns>
        /// A <see cref="long"/> value containing the file size, or -1 if the operation fails.
        /// </returns>
        public static long GetFileSizeNative(string fileName)
        {
            long size = -1;

            if (!string.IsNullOrEmpty(fileName))
            {
                if (FileExists(fileName))
                {
                    try
                    {
                        IntPtr fileHandle = CreateFile(fileName, FileAccess.Read, FileShare.Read, IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero);
                        if (fileHandle > 0)
                        {
                            GetFileSizeEx(fileHandle, out size);
                            CloseHandle(fileHandle);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }
                }
            }

            return size;
        }

        /// <summary>
        /// Opens the file for an exclusive read operation.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file to be created.
        /// </param>
        /// <returns>
        /// The new <see cref="FileStream"/> instance if successful; otherwise, returns
        /// <b>null</b>.
        /// </returns>
        public static FileStream? OpenFileForExclusiveRead(string fileName)
        {
            FileStream? stream = null;

            if (FileExists(fileName))
            {
                try
                {
                    stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                    stream = null;
                }
            }
            return stream;
        }

        /// <summary>
        /// Opens the file for an exclusive write operation.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file to be created.
        /// </param>
        /// <returns>
        /// The new <see cref="FileStream"/> instance if successful; otherwise, returns
        /// <b>null</b>.
        /// </returns>
        public static FileStream? OpenFileForExclusiveWrite(string fileName)
        {
            FileStream? stream;

            if (FileExists(fileName))
            {
                DeleteFile(fileName);
            }

            try
            {
                stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                stream = null;
            }

            return stream;
        }

        /// <summary>
        /// Attempts to read the specified file into a byte array.
        /// </summary>
        /// <param name="pathAndFileName">
        /// The fully-qualified path and name of the file to be read.</param>
        /// <returns>
        /// An array of bytes containing the file content if successful; otherwise,
        /// returns <b>null</b>.
        /// </returns>
        public static byte[]? ReadBytesFromFile(string pathAndFileName)
        {
            byte[]? data = null;
            OperationResult<byte[]> result = ReadBytesFromFileWithResult(pathAndFileName);
            if (result.Success)
            {
                if (result.DataContent != null)
                {
                    data = new byte[result.DataContent.Length];
                    Array.Copy(result.DataContent, data, data.Length);
                }
            }
            return data;
        }

        /// <summary>
        /// Attempts to read the specified file into a byte array.
        /// </summary>
        /// <param name="pathAndFileName">
        /// The fully-qualified path and name of the file to be read.</param>
        /// <returns>
        /// An array of bytes containing the file content if successful; otherwise,
        /// returns <b>null</b>.
        /// </returns>
        public static async Task<byte[]?> ReadBytesFromFileAsync(string pathAndFileName)
        {
            byte[]? data = null;
            OperationResult<byte[]> result = await ReadBytesFromFileWithResultAsync(pathAndFileName).ConfigureAwait(false);
            if (result.Success)
            {
                if (result.DataContent != null)
                {
                    data = new byte[result.DataContent.Length];
                    Array.Copy(result.DataContent, data, data.Length);
                }
            }
            return data;
        }

        /// <summary>
        /// Attempts to read the specified file into a byte array.
        /// </summary>
        /// <param name="pathAndFileName">
        /// The fully-qualified path and name of the file to be read.
        /// </param>
        /// <returns>
        /// An <see cref="OperationResult{T}"/> containing the result of the
        /// operation.
        /// </returns>
        public static OperationResult<byte[]> ReadBytesFromFileWithResult(string pathAndFileName)
        {
            OperationResult<byte[]> result = new();

            // Determine if the file is present.
            OperationResult<bool> existsResult = FileExistsWithResult(pathAndFileName);
            if (!existsResult.Success || !existsResult.DataContent)
            {
                result.Success = false;
                result.Message = $"The specified file {pathAndFileName} does not exist;";
            }
            else
            {

                FileStream? inStream = null;
                try
                {
                    inStream = new FileStream(pathAndFileName, FileMode.Open, FileAccess.Read);
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                    result.Success = false;
                    result.Exceptions?.Add(ex);
                }

                if (inStream != null)
                {
                    int length = (int)inStream.Length;
                    try
                    {
                        byte[] buffer = new byte[length];
                        int totalRead = inStream.Read(buffer, 0, length);
                        byte[] dataContent = new byte[totalRead];
                        Array.Copy(buffer, 0, dataContent, 0, totalRead);
                        Array.Clear(buffer, 0, length);
                        result.DataContent = dataContent;
                        result.Success = true;
                    }
                    catch (IOException ex)
                    {
                        result.Success = false;
                        result.Exceptions?.Add(ex);
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }
                    inStream.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// Attempts to read the specified file into a byte array.
        /// </summary>
        /// <param name="pathAndFileName">
        /// The fully-qualified path and name of the file to be read.
        /// </param>
        /// <param name="token">
        /// An optional <see cref="CancellationToken"/> that can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// An <see cref="OperationResult{T}"/> containing the result of the
        /// operation.
        /// </returns>
        public static async Task<OperationResult<byte[]>> ReadBytesFromFileWithResultAsync(string pathAndFileName,
            CancellationToken token = default)
        {
            OperationResult<byte[]> result = new();

            // Determine if the file is present.
            OperationResult<bool> existsResult = FileExistsWithResult(pathAndFileName);
            if (!existsResult.Success || !existsResult.DataContent)
            {
                result.Success = false;
                result.Message = $"The specified file {pathAndFileName} does not exist;";
            }
            else
            {

                FileStream? inStream = null;
                try
                {
                    inStream = new FileStream(pathAndFileName, FileMode.Open, FileAccess.Read);
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                    result.Success = false;
                    result.Exceptions?.Add(ex);
                }

                if (inStream != null)
                {
                    int length = (int)inStream.Length;
                    try
                    {
                        Memory<byte> buffer = new byte[length];
                        int totalRead = await inStream.ReadAsync(buffer, token).ConfigureAwait(false);
                        byte[] dataContent = new byte[totalRead];
                        buffer.CopyTo(dataContent);
                        result.DataContent = dataContent;
                        result.Success = true;
                    }
                    catch (IOException ex)
                    {
                        result.Success = false;
                        result.Exceptions?.Add(ex);
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }
                    inStream.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// Attempts to read the binary data from the specified stream.
        /// </summary>
        /// <param name="sourceStream">
        /// An open <see cref="Stream"/> implementation for reading.
        /// </param>
        /// <returns>
        /// A byte array containing the content of the stream, or <b>null</b>.
        /// </returns>
        public static byte[]? ReadBytesFromStream(Stream? sourceStream)
        {
            byte[]? data = null;

            if (sourceStream != null)
            {
                BinaryReader reader = new(sourceStream);
                try
                {
                    sourceStream.Seek(0, SeekOrigin.Begin);
                    data = reader.ReadBytes((int)sourceStream.Length);

                    // Do not close the stream; it may be in use elsewhere.
                    sourceStream.Seek(0, SeekOrigin.Begin);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                    data = null;
                }
            }
            return data;
        }

        /// <summary>
        /// Attempts to read the binary data from the specified stream.
        /// </summary>
        /// <param name="sourceStream">
        /// An open <see cref="Stream"/> implementation for reading.
        /// </param>
        /// <param name="token">
        /// An optional <see cref="CancellationToken"/> that can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A byte array containing the content of the stream, or <b>null</b>.
        /// </returns>
        public static async Task<byte[]?> ReadBytesFromStreamAsync(Stream? sourceStream,
            CancellationToken token = default)
        {
            byte[]? data = null;

            if (sourceStream != null && sourceStream.CanRead)
            {
                try
                {
                    int length = (int)sourceStream.Length;
                    Memory<byte> buffer = new byte[length];
                    int totalRead = await sourceStream.ReadAsync(buffer, token).ConfigureAwait(false);
                    data = new byte[totalRead];
                    buffer.CopyTo(data);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                    data = null;
                }
            }
            return data;
        }

        /// <summary>
        /// Attempts to read the specified file into a string.
        /// </summary>
        /// <param name="pathAndFileName">
        /// The fully-qualified path and name of the file to be read.
        /// </param>
        /// <param name="isUnicode">
        /// A value indicating whether to use Unicode text encoding.  If
        /// <b>false</b>, the method uses ASCII encoding.
        /// </param>
        /// <returns>
        /// A string containing the file content if successful; otherwise,
        /// returns <b>null</b>.
        /// </returns>
        public static string? ReadTextFromFile(string pathAndFileName, bool isUnicode)
        {
            string? text = null;
            OperationResult<byte[]> result = ReadBytesFromFileWithResult(pathAndFileName);
            if (result.Success && result.DataContent != null)
            {
                if (!isUnicode)
                {
                    text = System.Text.Encoding.ASCII.GetString(result.DataContent);
                }
                else
                {
                    text = System.Text.Encoding.Unicode.GetString(result.DataContent);
                }
            }
            return text;
        }

        /// <summary>
        /// Attempts to write the binary data the specified file.
        /// </summary>
        /// <remarks>
        /// This method will delete the file if it already exists and create a new
        /// one in its place.
        /// </remarks>
        /// <param name="fileName">
        /// The fully-qualified path and name of the file to be created and written to.</param>
        /// <param name="dataContent">
        /// An array of bytes containing the data to be written.
        /// </param>
        /// <returns>
        /// An <see cref="OperationResult"/> containing the result of the operation.
        /// </returns>
        public static OperationResult WriteBytesToFile(string fileName, byte[] dataContent)
        {
            OperationResult? result = null;

            // Determine if the file is present.
            OperationResult<bool> existsResult = FileExistsWithResult(fileName);
            if (!existsResult.Success)
            {
                result = new OperationResult(false);
                if (existsResult.Exceptions != null && existsResult.Exceptions.Count > 0)
                {
                    result.Exceptions?.AddRange(existsResult.Exceptions);
                }
            }
            else
            {
                // If present, delete the file.
                if (existsResult.DataContent)
                {
                    OperationResult deleteResult = DeleteFileWithResult(fileName);
                    if (!deleteResult.Success)
                    {
                        result = new OperationResult(false);
                        if (existsResult.Exceptions != null && existsResult.Exceptions.Count > 0)
                        {
                            result.Exceptions?.AddRange(existsResult.Exceptions);
                        }
                    }
                }

                // If no errors have occurred so far, continue...
                if (result is null)
                {
                    try
                    {
                        FileStream stream = new(fileName, FileMode.CreateNew, FileAccess.Write);
                        stream.Write(dataContent, 0, dataContent.Length);
                        stream.Flush();
                        stream.Dispose();
                        result = new OperationResult(true);
                    }
                    catch (Exception ex)
                    {
                        result = new OperationResult(ex);
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
