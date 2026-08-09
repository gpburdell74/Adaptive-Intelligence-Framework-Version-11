using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;

using Adaptive.Intelligence.Common;
using Adaptive.Intelligence.Enumerations;
using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests;

public class SafeIOTests
{
    [Fact]
    public void CopyFile_SourceExistsAndDestinationMissing_CopiesAndReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string sourceFile = Path.Combine(tempDirectory, "source.txt");
            string destinationFile = Path.Combine(tempDirectory, "destination.txt");
            const string content = "copy-content";

            File.WriteAllText(sourceFile, content);

            bool result = SafeIO.CopyFile(sourceFile, destinationFile);

            Assert.True(result);
            Assert.True(File.Exists(destinationFile));
            Assert.Equal(content, File.ReadAllText(destinationFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void CopyFile_DestinationAlreadyExists_ReturnsFalseWithoutChangingDestination()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string sourceFile = Path.Combine(tempDirectory, "source.txt");
            string destinationFile = Path.Combine(tempDirectory, "destination.txt");

            File.WriteAllText(sourceFile, "source-content");
            File.WriteAllText(destinationFile, "existing-content");

            bool result = SafeIO.CopyFile(sourceFile, destinationFile);

            Assert.False(result);
            Assert.Equal("existing-content", File.ReadAllText(destinationFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void CopyFile_DestinationDirectoryDoesNotExist_ReturnsFalse()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string sourceFile = Path.Combine(tempDirectory, "source.txt");
            string missingDirectory = Path.Combine(tempDirectory, "missing");
            string destinationFile = Path.Combine(missingDirectory, "destination.txt");

            File.WriteAllText(sourceFile, "source-content");

            bool result = SafeIO.CopyFile(sourceFile, destinationFile);

            Assert.False(result);
            Assert.False(File.Exists(destinationFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void CreateFileForExclusiveWrite_NewFilePath_ReturnsWritableStream()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "exclusive.txt");

            using FileStream? stream = SafeIO.CreateFileForExclusiveWrite(filePath);

            Assert.NotNull(stream);
            Assert.True(stream.CanWrite);
            Assert.True(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void CreateFileForExclusiveWrite_FileAlreadyExists_DeletesAndRecreatesFile()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "exclusive.txt");
            File.WriteAllText(filePath, "existing-content");

            using FileStream? stream = SafeIO.CreateFileForExclusiveWrite(filePath);

            Assert.NotNull(stream);
            Assert.True(File.Exists(filePath));

            FileInfo info = new(filePath);
            Assert.Equal(0, info.Length);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void CreateFileForExclusiveWrite_MissingParentDirectory_ReturnsNull()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string missingDirectory = Path.Combine(tempDirectory, "missing");
            string filePath = Path.Combine(missingDirectory, "exclusive.txt");

            using FileStream? stream = SafeIO.CreateFileForExclusiveWrite(filePath);

            Assert.Null(stream);
            Assert.False(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public async Task DecompressGZFileAsync_InputFileMissing_ReturnsFalseAndLeavesOutputUnchanged()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string inputFile = Path.Combine(tempDirectory, "missing.gz");
            string outputFile = Path.Combine(tempDirectory, "output.txt");
            File.WriteAllText(outputFile, "existing-output");

            bool result = await SafeIO.DecompressGZFileAsync(inputFile, outputFile, false);

            Assert.False(result);
            Assert.True(File.Exists(outputFile));
            Assert.Equal("existing-output", File.ReadAllText(outputFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public async Task DecompressGZFileAsync_ValidGZipAndDeleteOriginal_ProducesOutputAndDeletesInput()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string inputFile = Path.Combine(tempDirectory, "input.gz");
            string outputFile = Path.Combine(tempDirectory, "output.txt");
            const string expectedText = "decompressed text";

            CreateGZipFile(inputFile, expectedText);
            File.WriteAllText(outputFile, "stale-output");

            bool result = await SafeIO.DecompressGZFileAsync(inputFile, outputFile, true);

            Assert.True(result);
            Assert.True(File.Exists(outputFile));
            Assert.Equal(expectedText, File.ReadAllText(outputFile));
            Assert.False(File.Exists(inputFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public async Task DecompressGZFileAsync_ValidGZipWithoutDeleteOriginal_ProducesOutputAndKeepsInput()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string inputFile = Path.Combine(tempDirectory, "input.gz");
            string outputFile = Path.Combine(tempDirectory, "output.txt");
            const string expectedText = "another decompressed value";

            CreateGZipFile(inputFile, expectedText);

            bool result = await SafeIO.DecompressGZFileAsync(inputFile, outputFile, false);

            Assert.True(result);
            Assert.True(File.Exists(outputFile));
            Assert.Equal(expectedText, File.ReadAllText(outputFile));
            Assert.True(File.Exists(inputFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public async Task DecompressGZFileAsync_InvalidGZipContent_ReturnsFalseAndDeletesOutput()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string inputFile = Path.Combine(tempDirectory, "input.gz");
            string outputFile = Path.Combine(tempDirectory, "output.txt");

            File.WriteAllText(inputFile, "not-a-gzip-payload");
            File.WriteAllText(outputFile, "old-output");

            bool result = await SafeIO.DecompressGZFileAsync(inputFile, outputFile, false);

            Assert.False(result);
            Assert.True(File.Exists(inputFile));
            Assert.False(File.Exists(outputFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public async Task DecompressGZFileAsync_OutputDirectoryMissing_ReturnsFalse()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string inputFile = Path.Combine(tempDirectory, "input.gz");
            string outputFile = Path.Combine(tempDirectory, "missing", "output.txt");

            CreateGZipFile(inputFile, "content");

            bool result = await SafeIO.DecompressGZFileAsync(inputFile, outputFile, false);

            Assert.False(result);
            Assert.True(File.Exists(inputFile));
            Assert.False(File.Exists(outputFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteAllFilesInDirectory_EmptyPath_ReturnsFalse()
    {
        bool result = SafeIO.DeleteAllFilesInDirectory(string.Empty);

        Assert.False(result);
    }

    [Fact]
    public void DeleteAllFilesInDirectory_ExistingEmptyDirectory_ReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            bool result = SafeIO.DeleteAllFilesInDirectory(tempDirectory);

            Assert.True(result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteAllFilesInDirectory_FilesExist_DeletesAllAndReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string firstFile = Path.Combine(tempDirectory, "one.txt");
            string secondFile = Path.Combine(tempDirectory, "two.txt");
            File.WriteAllText(firstFile, "a");
            File.WriteAllText(secondFile, "b");

            bool result = SafeIO.DeleteAllFilesInDirectory(tempDirectory);

            Assert.True(result);
            Assert.False(File.Exists(firstFile));
            Assert.False(File.Exists(secondFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteAllFilesInDirectory_WithWildcardAndEmptyPath_ReturnsFalse()
    {
        bool result = SafeIO.DeleteAllFilesInDirectory(string.Empty, "*.txt");

        Assert.False(result);
    }

    [Fact]
    public void DeleteAllFilesInDirectory_WithWildcardNoMatches_ReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            File.WriteAllText(Path.Combine(tempDirectory, "one.bin"), "a");

            bool result = SafeIO.DeleteAllFilesInDirectory(tempDirectory, "*.txt");

            Assert.True(result);
            Assert.True(File.Exists(Path.Combine(tempDirectory, "one.bin")));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteAllFilesInDirectory_WithWildcardMatches_DeletesMatchingFilesAndReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string matchOne = Path.Combine(tempDirectory, "one.txt");
            string matchTwo = Path.Combine(tempDirectory, "two.txt");
            string keepFile = Path.Combine(tempDirectory, "keep.bin");

            File.WriteAllText(matchOne, "a");
            File.WriteAllText(matchTwo, "b");
            File.WriteAllText(keepFile, "c");

            bool result = SafeIO.DeleteAllFilesInDirectory(tempDirectory, "*.txt");

            Assert.True(result);
            Assert.False(File.Exists(matchOne));
            Assert.False(File.Exists(matchTwo));
            Assert.True(File.Exists(keepFile));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteAllFilesInDirectory_LockedFile_WindowsReturnsFalse()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string lockedFile = Path.Combine(tempDirectory, "locked.txt");
            string deletableFile = Path.Combine(tempDirectory, "deletable.txt");
            File.WriteAllText(lockedFile, "locked");
            File.WriteAllText(deletableFile, "free");

            using FileStream lockStream = new(lockedFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            bool result = SafeIO.DeleteAllFilesInDirectory(tempDirectory);

            if (OperatingSystem.IsWindows())
            {
                Assert.False(result);
                Assert.True(File.Exists(lockedFile));
            }
            else
            {
                Assert.True(result);
            }
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteAllFilesInDirectory_WithWildcardAndLockedFile_WindowsReturnsFalse()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string lockedFile = Path.Combine(tempDirectory, "locked.txt");
            string deletableFile = Path.Combine(tempDirectory, "deletable.txt");
            File.WriteAllText(lockedFile, "locked");
            File.WriteAllText(deletableFile, "free");

            using FileStream lockStream = new(lockedFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            bool result = SafeIO.DeleteAllFilesInDirectory(tempDirectory, "*.txt");

            if (OperatingSystem.IsWindows())
            {
                Assert.False(result);
                Assert.True(File.Exists(lockedFile));
            }
            else
            {
                Assert.True(result);
            }
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }


    [Fact]
    public void DeleteFile_PathAndFileExists_DeletesFileAndReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "delete-me.txt");
            File.WriteAllText(filePath, "content");

            bool result = SafeIO.DeleteFile(filePath);

            Assert.True(result);
            Assert.False(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteFile_PathIsNull_ReturnsFalse()
    {
        bool result = SafeIO.DeleteFile((string)null!);

        Assert.False(result);
    }

    [Fact]
    public void DeleteFile_PathIsEmpty_ReturnsFalse()
    {
        bool result = SafeIO.DeleteFile(string.Empty);

        Assert.False(result);
    }

    [Fact]
    public void DeleteFile_PathDoesNotExist_ReturnsFalse()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing.txt");

        bool result = SafeIO.DeleteFile(missingFilePath);

        Assert.False(result);
    }

    [Fact]
    public void DeleteFile_PathIsLockedFile_WindowsReturnsFalseOtherwiseTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "locked-delete.txt");
            File.WriteAllText(filePath, "content");

            using FileStream lockStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            bool result = SafeIO.DeleteFile(filePath);

            if (OperatingSystem.IsWindows())
            {
                Assert.False(result);
                Assert.True(File.Exists(filePath));
            }
            else
            {
                Assert.True(result);
                Assert.False(File.Exists(filePath));
            }
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteFile_FileInstanceIsNull_ReturnsFalse()
    {
        bool result = SafeIO.DeleteFile((FileInfo?)null);

        Assert.False(result);
    }

    [Fact]
    public void DeleteFile_FileInstanceExists_DeletesAndReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "instance-delete.txt");
            File.WriteAllText(filePath, "content");
            FileInfo fileInfo = new(filePath);

            bool result = SafeIO.DeleteFile(fileInfo);

            Assert.True(result);
            Assert.False(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteFile_FileInstanceLocked_WindowsReturnsFalseOtherwiseTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "instance-locked.txt");
            File.WriteAllText(filePath, "content");
            FileInfo fileInfo = new(filePath);

            using FileStream lockStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            bool result = SafeIO.DeleteFile(fileInfo);

            if (OperatingSystem.IsWindows())
            {
                Assert.False(result);
                Assert.True(fileInfo.Exists);
            }
            else
            {
                Assert.True(result);
                Assert.False(File.Exists(filePath));
            }
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteFileWithResult_FileExists_DeletesAndReturnsSuccessfulResult()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "result-delete.txt");
            File.WriteAllText(filePath, "content");

            OperationResult result = SafeIO.DeleteFileWithResult(filePath);

            Assert.True(result.Success);
            Assert.False(result.HasExceptions);
            Assert.False(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DeleteFileWithResult_FileMissing_ReturnsFailureMessage()
    {
        string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing-result.txt");

        OperationResult result = SafeIO.DeleteFileWithResult(filePath);

        Assert.False(result.Success);
        Assert.Equal("The specified file does not exist.", result.Message);
        Assert.False(result.HasExceptions);
    }

    [Fact]
    public void DeleteFileWithResult_FileLocked_WindowsReturnsFailureWithExceptionOtherwiseSuccess()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "locked-result.txt");
            File.WriteAllText(filePath, "content");

            using FileStream lockStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            OperationResult result = SafeIO.DeleteFileWithResult(filePath);

            if (OperatingSystem.IsWindows())
            {
                Assert.False(result.Success);
                Assert.True(result.HasExceptions);
                Assert.NotNull(result.FirstException);
                Assert.IsType<IOException>(result.FirstException);
            }
            else
            {
                Assert.True(result.Success);
                Assert.False(result.HasExceptions);
            }
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DetermineFileFormat_FileNameIsNull_ReturnsNotSpecified()
    {
        FileFormats result = SafeIO.DetermineFileFormat(null);

        Assert.Equal(FileFormats.NotSpecified, result);
    }

    [Fact]
    public void DetermineFileFormat_FileNameHasNoExtension_ReturnsNotSpecified()
    {
        FileFormats result = SafeIO.DetermineFileFormat("filename");

        Assert.Equal(FileFormats.NotSpecified, result);
    }

    [Fact]
    public void DetermineFileFormat_KnownUpperCaseExtension_ReturnsMatchingFormat()
    {
        FileFormats result = SafeIO.DetermineFileFormat("report.TXT");

        Assert.Equal(FileFormats.TextFile, result);
    }

    [Fact]
    public void DetermineFileFormat_UnknownExtension_ReturnsNotSpecified()
    {
        FileFormats result = SafeIO.DetermineFileFormat("report.unknownext");

        Assert.Equal(FileFormats.NotSpecified, result);
    }

    [Fact]
    public void DirectoryExists_PathIsNull_ReturnsFalse()
    {
        bool result = SafeIO.DirectoryExists(null);

        Assert.False(result);
    }

    [Fact]
    public void DirectoryExists_PathExists_ReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            bool result = SafeIO.DirectoryExists(tempDirectory);

            Assert.True(result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void DirectoryExists_PathDoesNotExist_ReturnsFalse()
    {
        string missingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

        bool result = SafeIO.DirectoryExists(missingDirectory);

        Assert.False(result);
    }

    [Fact]
    public void FileExists_FileExists_ReturnsTrue()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "exists.txt");
            File.WriteAllText(filePath, "content");

            bool result = SafeIO.FileExists(filePath);

            Assert.True(result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }


    [Fact]
    public void FileExists_FileDoesNotExist_ReturnsFalse()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing.txt");

        bool result = SafeIO.FileExists(missingFilePath);

        Assert.False(result);
    }

    [Fact]
    public void FileExists_PathIsNull_ReturnsFalse()
    {
        bool result = SafeIO.FileExists((string)null!);

        Assert.False(result);
    }

    [Fact]
    public void FindUSBDrive_WhenCalled_ReturnsNullOrExistingDirectory()
    {
        DirectoryInfo? result = SafeIO.FindUSBDrive();

        if (result != null)
        {
            Assert.True(result.Exists);
        }
    }

    [Fact]
    public void FileExistsWithResult_FileExists_ReturnsSuccessfulResultWithTrueDataContent()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "exists-with-result.txt");
            File.WriteAllText(filePath, "content");

            OperationResult<bool> result = SafeIO.FileExistsWithResult(filePath);

            Assert.True(result.Success);
            Assert.False(result.HasExceptions);
            Assert.True(result.DataContent);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void FileExistsWithResult_FileDoesNotExist_ReturnsSuccessfulResultWithFalseDataContent()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing.txt");

        OperationResult<bool> result = SafeIO.FileExistsWithResult(missingFilePath);

        Assert.True(result.Success);
        Assert.False(result.HasExceptions);
        Assert.False(result.DataContent);
    }

    [Fact]
    public void GetAppPath_WhenCalled_ReturnsExistingDirectoryPath()
    {
        string? result = SafeIO.GetAppPath();

        Assert.False(string.IsNullOrWhiteSpace(result));
        Assert.True(Directory.Exists(result));
    }

    [Fact]
    public void GetDirectories_PathIsNull_ReturnsNull()
    {
        string[]? result = SafeIO.GetDirectories((string)null!);

        Assert.Null(result);
    }

    [Fact]
    public void GetDirectories_PathIsEmpty_ReturnsNull()
    {
        string[]? result = SafeIO.GetDirectories(string.Empty);

        Assert.Null(result);
    }

    [Fact]
    public void GetDirectories_PathExists_ReturnsSubdirectoryList()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        string childOne = Path.Combine(tempDirectory, "child-one");
        string childTwo = Path.Combine(tempDirectory, "child-two");
        Directory.CreateDirectory(childOne);
        Directory.CreateDirectory(childTwo);

        try
        {
            string[]? result = SafeIO.GetDirectories(tempDirectory);

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Contains(childOne, result);
            Assert.Contains(childTwo, result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void GetDirectories_PathDoesNotExist_ReturnsNull()
    {
        string missingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

        string[]? result = SafeIO.GetDirectories(missingDirectory);

        Assert.Null(result);
    }


    [Fact]
    public void GetFilesInPath_PathExists_ReturnsAllFiles()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string firstFile = Path.Combine(tempDirectory, "one.txt");
            string secondFile = Path.Combine(tempDirectory, "two.bin");
            File.WriteAllText(firstFile, "a");
            File.WriteAllText(secondFile, "b");

            string[]? result = SafeIO.GetFilesInPath(tempDirectory);

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Contains(firstFile, result);
            Assert.Contains(secondFile, result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }


    [Fact]
    public void GetFilesInPath_PathIsEmpty_ReturnsNull()
    {
        string[]? result = SafeIO.GetFilesInPath(string.Empty);

        Assert.Null(result);
    }

    [Fact]
    public void GetFilesInPath_PathDoesNotExist_ReturnsNull()
    {
        string missingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

        string[]? result = SafeIO.GetFilesInPath(missingDirectory);

        Assert.Null(result);
    }

    [Fact]
    public void GetFilesInPath_WithWildcardAndMatchingFiles_ReturnsFilteredFiles()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string textFile = Path.Combine(tempDirectory, "match.txt");
            string binFile = Path.Combine(tempDirectory, "skip.bin");
            File.WriteAllText(textFile, "a");
            File.WriteAllText(binFile, "b");

            string[]? result = SafeIO.GetFilesInPath(tempDirectory, "*.txt");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(textFile, result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void GetFilesInPath_WithWildcardNull_ReturnsNull()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string[]? result = SafeIO.GetFilesInPath(tempDirectory, null);

            Assert.Null(result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void GetFileSize_FileNameIsEmpty_ReturnsZero()
    {
        int result = SafeIO.GetFileSize(string.Empty);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetFileSize_FileDoesNotExist_ReturnsZero()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing.bin");

        int result = SafeIO.GetFileSize(missingFilePath);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetFileSize_FileExists_ReturnsLength()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "size.txt");
            File.WriteAllText(filePath, "12345");

            int result = SafeIO.GetFileSize(filePath);

            Assert.Equal(5, result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void GetFileSizeNative_FileNameIsEmpty_ReturnsNegativeOne()
    {
        long result = SafeIO.GetFileSizeNative(string.Empty);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void GetFileSizeNative_FileDoesNotExist_ReturnsNegativeOne()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing-native.bin");

        long result = SafeIO.GetFileSizeNative(missingFilePath);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void GetFileSizeNative_FileExists_WindowsReturnsLengthOtherwiseNegativeOne()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "native-size.txt");
            File.WriteAllText(filePath, "1234567");

            long result = SafeIO.GetFileSizeNative(filePath);

            if (OperatingSystem.IsWindows())
            {
                Assert.Equal(7, result);
            }
            else
            {
                Assert.Equal(-1, result);
            }
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void OpenFileForExclusiveRead_FileDoesNotExist_ReturnsNull()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing-open.txt");

        using FileStream? result = SafeIO.OpenFileForExclusiveRead(missingFilePath);

        Assert.Null(result);
    }

    [Fact]
    public void OpenFileForExclusiveRead_FileExists_ReturnsReadableStream()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "readable.txt");
            File.WriteAllText(filePath, "content");

            using FileStream? result = SafeIO.OpenFileForExclusiveRead(filePath);

            Assert.NotNull(result);
            Assert.True(result.CanRead);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void OpenFileForExclusiveRead_FileIsLocked_ReturnsNull()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "locked-read.txt");
            File.WriteAllText(filePath, "content");

            using FileStream lockStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            using FileStream? result = SafeIO.OpenFileForExclusiveRead(filePath);

            Assert.Null(result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void OpenFileForExclusiveWrite_MissingParentDirectory_ReturnsNull()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string missingDirectory = Path.Combine(tempDirectory, "missing");
            string filePath = Path.Combine(missingDirectory, "exclusive-write.txt");

            using FileStream? stream = SafeIO.OpenFileForExclusiveWrite(filePath);

            Assert.Null(stream);
            Assert.False(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void OpenFileForExclusiveWrite_FileDoesNotExist_ReturnsWritableStream()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "new-exclusive-write.txt");

            using FileStream? stream = SafeIO.OpenFileForExclusiveWrite(filePath);

            Assert.NotNull(stream);
            Assert.True(stream.CanWrite);
            Assert.True(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void OpenFileForExclusiveWrite_FileExists_DeletesAndRecreatesFile()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "existing-exclusive-write.txt");
            File.WriteAllText(filePath, "stale-content");

            using FileStream? stream = SafeIO.OpenFileForExclusiveWrite(filePath);

            Assert.NotNull(stream);
            Assert.True(File.Exists(filePath));

            FileInfo info = new(filePath);
            Assert.Equal(0, info.Length);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void ReadBytesFromFile_FileDoesNotExist_ReturnsNull()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing-bytes.bin");

        byte[]? result = SafeIO.ReadBytesFromFile(missingFilePath);

        Assert.Null(result);
    }

    [Fact]
    public void ReadBytesFromFile_FileExists_ReturnsFileContentBytes()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "bytes.bin");
            byte[] expected = [0, 1, 2, 3, 4, 5];
            File.WriteAllBytes(filePath, expected);

            byte[]? result = SafeIO.ReadBytesFromFile(filePath);

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public async Task ReadBytesFromFileAsync_FileDoesNotExist_ReturnsNull()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing-bytes-async.bin");

        byte[]? result = await SafeIO.ReadBytesFromFileAsync(missingFilePath);

        Assert.Null(result);
    }

    [Fact]
    public async Task ReadBytesFromFileAsync_FileExists_ReturnsFileContentBytes()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "bytes-async.bin");
            byte[] expected = [8, 7, 6, 5, 4, 3];
            File.WriteAllBytes(filePath, expected);

            byte[]? result = await SafeIO.ReadBytesFromFileAsync(filePath);

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void ReadBytesFromFileWithResult_FileDoesNotExist_ReturnsFailureWithMessage()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing-with-result.bin");

        OperationResult<byte[]> result = SafeIO.ReadBytesFromFileWithResult(missingFilePath);

        Assert.False(result.Success);
        Assert.Equal($"The specified file {missingFilePath} does not exist;", result.Message);
        Assert.Null(result.DataContent);
    }

    [Fact]
    public void ReadBytesFromFileWithResult_FileExists_ReturnsSuccessWithData()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "with-result.bin");
            byte[] expected = [11, 22, 33, 44];
            File.WriteAllBytes(filePath, expected);

            OperationResult<byte[]> result = SafeIO.ReadBytesFromFileWithResult(filePath);

            Assert.True(result.Success);
            Assert.NotNull(result.DataContent);
            Assert.Equal(expected, result.DataContent);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void ReadBytesFromFileWithResult_FileIsLockedForExclusiveAccess_ReturnsFailureWithException()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "locked-with-result.bin");
            File.WriteAllBytes(filePath, [99, 100, 101]);

            using FileStream lockStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            OperationResult<byte[]> result = SafeIO.ReadBytesFromFileWithResult(filePath);

            Assert.False(result.Success);
            Assert.Null(result.DataContent);
            Assert.NotNull(result.Exceptions);
            Assert.NotEmpty(result.Exceptions);
            Assert.IsType<IOException>(result.Exceptions[0]);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public async Task ReadBytesFromFileWithResultAsync_FileDoesNotExist_ReturnsFailureWithMessage()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing-with-result-async.bin");

        OperationResult<byte[]> result = await SafeIO.ReadBytesFromFileWithResultAsync(missingFilePath);

        Assert.False(result.Success);
        Assert.Equal($"The specified file {missingFilePath} does not exist;", result.Message);
        Assert.Null(result.DataContent);
    }

    [Fact]
    public async Task ReadBytesFromFileWithResultAsync_FileExists_ReturnsSuccessWithData()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "with-result-async.bin");
            byte[] expected = [1, 3, 5, 7, 9];
            File.WriteAllBytes(filePath, expected);

            OperationResult<byte[]> result = await SafeIO.ReadBytesFromFileWithResultAsync(filePath);

            Assert.True(result.Success);
            Assert.NotNull(result.DataContent);
            Assert.Equal(expected, result.DataContent);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public async Task ReadBytesFromFileWithResultAsync_FileIsLockedForExclusiveAccess_ReturnsFailureWithException()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "locked-with-result-async.bin");
            File.WriteAllBytes(filePath, [21, 22, 23]);

            using FileStream lockStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            OperationResult<byte[]> result = await SafeIO.ReadBytesFromFileWithResultAsync(filePath);

            Assert.False(result.Success);
            Assert.Null(result.DataContent);
            Assert.NotNull(result.Exceptions);
            Assert.NotEmpty(result.Exceptions);
            Assert.IsType<IOException>(result.Exceptions[0]);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void ReadBytesFromStream_SourceStreamIsNull_ReturnsNull()
    {
        byte[]? result = SafeIO.ReadBytesFromStream(null);

        Assert.Null(result);
    }

    [Fact]
    public void ReadBytesFromStream_SourceStreamContainsData_ReturnsBytesAndResetsPosition()
    {
        byte[] expected = [5, 10, 15, 20];
        using MemoryStream stream = new(expected);
        stream.Position = 2;

        byte[]? result = SafeIO.ReadBytesFromStream(stream);

        Assert.NotNull(result);
        Assert.Equal(expected, result);
        Assert.Equal(0, stream.Position);
    }

    [Fact]
    public void ReadBytesFromStream_SeekThrowsException_ReturnsNull()
    {
        Mock<Stream> streamMock = new();
        streamMock.SetupGet(x => x.CanRead).Returns(true);
        streamMock.Setup(x => x.Seek(It.IsAny<long>(), It.IsAny<SeekOrigin>())).Throws(new IOException("seek failed"));

        byte[]? result = SafeIO.ReadBytesFromStream(streamMock.Object);

        Assert.Null(result);
    }

    [Fact]
    public async Task ReadBytesFromStreamAsync_SourceStreamIsNull_ReturnsNull()
    {
        byte[]? result = await SafeIO.ReadBytesFromStreamAsync(null);

        Assert.Null(result);
    }

    [Fact]
    public async Task ReadBytesFromStreamAsync_SourceStreamCannotRead_ReturnsNull()
    {
        Mock<Stream> streamMock = new();
        streamMock.SetupGet(x => x.CanRead).Returns(false);

        byte[]? result = await SafeIO.ReadBytesFromStreamAsync(streamMock.Object);

        Assert.Null(result);
    }

    [Fact]
    public async Task ReadBytesFromStreamAsync_SourceStreamReadable_ReturnsReadBytes()
    {
        byte[] expected = [1, 2, 3, 4, 5];
        using MemoryStream stream = new(expected);

        byte[]? result = await SafeIO.ReadBytesFromStreamAsync(stream);

        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task ReadBytesFromStreamAsync_LengthAccessThrows_ReturnsNull()
    {
        Mock<Stream> streamMock = new();
        streamMock.SetupGet(x => x.CanRead).Returns(true);
        streamMock.SetupGet(x => x.Length).Throws(new IOException("length failed"));

        byte[]? result = await SafeIO.ReadBytesFromStreamAsync(streamMock.Object);

        Assert.Null(result);
    }

    [Fact]
    public void ReadTextFromFile_FileContainsAsciiBytes_ReturnsAsciiString()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "ascii.txt");
            const string expected = "ASCII text 123";
            File.WriteAllBytes(filePath, Encoding.ASCII.GetBytes(expected));

            string? result = SafeIO.ReadTextFromFile(filePath, false);

            Assert.Equal(expected, result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void ReadTextFromFile_FileContainsUnicodeBytes_ReturnsUnicodeString()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "unicode.txt");
            const string expected = "Hello Ω";
            File.WriteAllBytes(filePath, Encoding.Unicode.GetBytes(expected));

            string? result = SafeIO.ReadTextFromFile(filePath, true);

            Assert.Equal(expected, result);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void ReadTextFromFile_FileDoesNotExist_ReturnsNull()
    {
        string missingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing-text.txt");

        string? result = SafeIO.ReadTextFromFile(missingFilePath, false);

        Assert.Null(result);
    }

    [Fact]
    public void WriteBytesToFile_TargetFileDoesNotExist_WritesContentAndReturnsSuccess()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "new-bytes.bin");
            byte[] expected = [10, 20, 30, 40];

            OperationResult result = SafeIO.WriteBytesToFile(filePath, expected);

            Assert.True(result.Success);
            Assert.True(File.Exists(filePath));
            Assert.Equal(expected, File.ReadAllBytes(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void WriteBytesToFile_TargetFileExists_ReplacesContentAndReturnsSuccess()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "replace-bytes.bin");
            File.WriteAllBytes(filePath, [99, 98, 97]);
            byte[] expected = [1, 3, 5, 7];

            OperationResult result = SafeIO.WriteBytesToFile(filePath, expected);

            Assert.True(result.Success);
            Assert.Equal(expected, File.ReadAllBytes(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void WriteBytesToFile_ExistingFileIsLockedForExclusiveAccess_ReturnsFailure()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string filePath = Path.Combine(tempDirectory, "locked-write.bin");
            byte[] original = [44, 45, 46];
            File.WriteAllBytes(filePath, original);

            using FileStream lockStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            OperationResult result = SafeIO.WriteBytesToFile(filePath, [1, 2, 3]);

            Assert.False(result.Success);
            Assert.True(File.Exists(filePath));
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void WriteBytesToFile_DirectoryDoesNotExist_ReturnsFailureWithException()
    {
        string missingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        string filePath = Path.Combine(missingDirectory, "cannot-create.bin");

        OperationResult result = SafeIO.WriteBytesToFile(filePath, [7, 8, 9]);

        Assert.False(result.Success);
        Assert.NotNull(result.Exceptions);
        Assert.NotEmpty(result.Exceptions);
    }





    private static void CreateGZipFile(string filePath, string content)
    {
        using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);
        using GZipStream gzipStream = new(fileStream, CompressionMode.Compress);
        byte[] data = Encoding.UTF8.GetBytes(content);
        gzipStream.Write(data, 0, data.Length);
    }

}
