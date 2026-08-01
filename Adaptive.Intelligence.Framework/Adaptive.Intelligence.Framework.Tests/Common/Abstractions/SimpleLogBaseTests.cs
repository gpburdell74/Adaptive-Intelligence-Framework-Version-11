using Adaptive.Intelligence.Common.Abstractions;
using Adaptive.Intelligence.Framework.Tests.Mocks;
using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests.Common.Abstractions;

/// <summary>
/// Provides the tests for the <see cref="SimpleLogBase"/> class.
/// </summary>
public class SimpleLogBaseTests
{
    [Fact]
    public void Can_Create()
    {
        using MockSimpleLogBase log = new();
        using MockSimpleLogBase log2 = new(@"TestLog.txt");
        using MemoryStream ms = new();
        using MockSimpleLogBase log3 = new(ms);

        log.Dispose();
        log2.Dispose();
        ms.Dispose();
        log3.Dispose();
        Assert.True(File.Exists("Adaptive.Intelligence.Common.Log.log"));
        File.Delete("Adaptive.Intelligence.Common.Log.log");
        File.Delete("TestLog.txt");
    }

    [Fact]
    public void Can_Create_And_Log_To_Default()
    {
        using MockSimpleLogBase log = new();
        log.Log("Test message");
        log.LogException(new Exception("Test exception"));
        log.Dispose();

        Assert.True(File.Exists("Adaptive.Intelligence.Common.Log.log"));
        using FileStream fs = new("Adaptive.Intelligence.Common.Log.log", FileMode.Open, FileAccess.Read);
        using StreamReader reader = new(fs);

        string? lineA = reader.ReadLine();
        Assert.NotNull(lineA);
        string? lineB = reader.ReadLine();
        Assert.NotNull(lineB);
        Assert.Equal(MockSimpleLogBase.HeaderText, lineA + "\r\n" + lineB + "\r\n");

        reader.ReadLine();
        string? lineC = reader.ReadLine();
        Assert.NotNull(lineC);
        Assert.Contains("Test message", lineC);

        string? lineD = reader.ReadLine();
        Assert.NotNull(lineD);
        Assert.Contains("Exception: ['Test exception']", lineD);

        reader.Close();
        fs.Close();
        log.Dispose();
        reader.Dispose();
        fs.Dispose();
        File.Delete("Adaptive.Intelligence.Common.Log.log");
    }

    [Fact]
    public void Default_File_Name_Present()
    {
        using MockSimpleLogBase log = new MockSimpleLogBase();
        string? fn = log.FileName;
        Assert.NotNull(fn);
        Assert.Equal("Adaptive.Intelligence.Common.Log.log", fn);
        Assert.True(File.Exists("Adaptive.Intelligence.Common.Log.log"));
        log.Dispose();
        File.Delete("Adaptive.Intelligence.Common.Log.log");
    }

    [Fact]
    public void Specified_File_Name_Present()
    {
        string logFile = "TestLogFile.log";
        using MockSimpleLogBase log = new MockSimpleLogBase(logFile);
        string? fn = log.FileName;
        Assert.NotNull(fn);
        Assert.Equal(logFile, fn);
        Assert.True(File.Exists(logFile));
        log.Dispose();
        File.Delete(logFile);
    }

    [Fact]
    public void Can_Log_Exception()
    {
        File.Delete("TestLogFile.log");
        string logFile = "TestLogFile.log";
        using MockSimpleLogBase log = new MockSimpleLogBase(logFile);
        Exception ex = new Exception("This is for a test.");
        log.LogException(ex);
        log.Dispose();

        FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read);
        StreamReader r = new StreamReader(fs);
        string text = r.ReadToEnd();
        r.Dispose();
        fs.Dispose();
        File.Delete("TestLogFile.log");

        int index = text.IndexOf("Exception: ['This is for a test.']");
        Assert.True(index > 0);

        Assert.Contains("Exception: ['This is for a test.']", text);

        
    }

    /// <summary>
    /// Tests that null text values are ignored and not written to the log.
    /// </summary>
    [Fact]
    public void Log_Null_Does_Not_Write_Log_Content()
    {
        string logFile = CreateTempLogFileName();
        try
        {
            using MockSimpleLogBase log = new(logFile);
            log.Log(null);
            log.Dispose();

            string text = File.ReadAllText(logFile);
            Assert.DoesNotContain("Test marker", text, StringComparison.Ordinal);
        }
        finally
        {
            DeleteFileIfExists(logFile);
        }
    }

    /// <summary>
    /// Tests that suspended logging prevents text from being written.
    /// </summary>
    [Fact]
    public void Suspend_Prevents_Writes()
    {
        string logFile = CreateTempLogFileName();
        try
        {
            using MockSimpleLogBase log = new(logFile);
            log.Suspend();
            log.Log("blocked message");
            log.Dispose();

            string text = File.ReadAllText(logFile);
            Assert.DoesNotContain("blocked message", text, StringComparison.Ordinal);
        }
        finally
        {
            DeleteFileIfExists(logFile);
        }
    }

    /// <summary>
    /// Tests that resuming logging re-enables writes after suspension.
    /// </summary>
    [Fact]
    public void Resume_Reenables_Writes_After_Suspend()
    {
        string logFile = CreateTempLogFileName();
        try
        {
            using MockSimpleLogBase log = new(logFile);
            log.Suspend();
            log.Log("blocked message");
            log.Resume();
            log.Log("allowed message");
            log.Dispose();

            string text = File.ReadAllText(logFile);
            Assert.DoesNotContain("blocked message", text, StringComparison.Ordinal);
            Assert.Contains("allowed message", text, StringComparison.Ordinal);
        }
        finally
        {
            DeleteFileIfExists(logFile);
        }
    }

    /// <summary>
    /// Tests that the contextual exception overload writes member, file, line, and exception content.
    /// </summary>
    [Fact]
    public void LogException_With_Context_Writes_Context_And_Exception()
    {
        string logFile = CreateTempLogFileName();
        try
        {
            using MockSimpleLogBase log = new(logFile);
            Exception ex = new("Context exception");

            log.LogException(ex, "MyMember", "MyFile.cs", 123);
            log.Dispose();

            string text = File.ReadAllText(logFile);
            Assert.Contains("Member: MyMember", text, StringComparison.Ordinal);
            Assert.Contains("File: MyFile.cs", text, StringComparison.Ordinal);
            Assert.Contains("Line No: 123", text, StringComparison.Ordinal);
            Assert.Contains("Exception: ['Context exception']", text, StringComparison.Ordinal);
        }
        finally
        {
            DeleteFileIfExists(logFile);
        }
    }

    /// <summary>
    /// Tests that disposing a logger created with an external stream does not dispose the external stream.
    /// </summary>
    [Fact]
    public void Dispose_With_External_Stream_Does_Not_Close_Parent_Stream()
    {
        using MemoryStream ms = new();
        MockSimpleLogBase log = new(ms);

        log.Log("before dispose");
        log.Dispose();

        Assert.True(ms.CanRead);
        Assert.True(ms.CanWrite);

        ms.Position = ms.Length;
        byte[] marker = System.Text.Encoding.UTF8.GetBytes("external-write");
        ms.Write(marker, 0, marker.Length);
        Assert.True(ms.Length > 0);
    }

    /// <summary>
    /// Tests that file-name collisions are resolved to a unique destination file without overwriting existing files.
    /// </summary>
    [Fact]
    public void Constructor_With_Existing_File_Uses_Unique_File_Name()
    {
        string basePath = CreateTempLogFileName();
        string uniquePath = string.Empty;

        try
        {
            File.WriteAllText(basePath, "original");
            uniquePath = FileUtility.EnsureUniqueFileName(basePath);
            using MockSimpleLogBase log = new(basePath);
            log.Log("new content");
            log.Dispose();

            Assert.Equal("original", File.ReadAllText(basePath));
            Assert.True(File.Exists(uniquePath));
            string uniqueText = File.ReadAllText(uniquePath);
            Assert.Contains("new content", uniqueText, StringComparison.Ordinal);
        }
        finally
        {
            DeleteFileIfExists(basePath);
            DeleteFileIfExists(uniquePath);
        }
    }

    /// <summary>
    /// Tests that write failures raise the logger exception event.
    /// </summary>
    [Fact]
    public void Log_Raises_LoggerException_When_Write_Fails()
    {
        using MemoryStream ms = new();
        using MockSimpleLogBase log = new(ms);
        Exception? raisedException = null;

        log.LoggerException += (_, args) => raisedException = args.ExceptionObject as Exception;
        ms.Dispose();

        log.Log("write after stream disposal");

        Assert.NotNull(raisedException);
        Assert.IsType<ObjectDisposedException>(raisedException);
    }

    /// <summary>
    /// Creates a unique temporary file name for isolated log testing.
    /// </summary>
    /// <returns>
    /// The full path to a temporary log file.
    /// </returns>
    private static string CreateTempLogFileName()
    {
        return Path.Combine(Path.GetTempPath(), $"SimpleLogBaseTests_{Guid.NewGuid():N}.log");
    }

    /// <summary>
    /// Deletes the specified file if it exists.
    /// </summary>
    /// <param name="fileName">
    /// The full file path to delete.
    /// </param>
    private static void DeleteFileIfExists(string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

}