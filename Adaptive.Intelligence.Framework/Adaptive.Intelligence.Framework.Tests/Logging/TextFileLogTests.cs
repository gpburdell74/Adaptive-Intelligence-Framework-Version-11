using Adaptive.Intelligence.Logging;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Logging;

/// <summary>
/// Provides the tests for the <see cref="TextFileLog"/> class.
/// </summary>
public class TextFileLogTests
{
    /// <summary>
    /// Tests that constructing the logger sets the file name and creates the destination log file.
    /// </summary>
    [Fact]
    public void Constructor_Sets_FileName_And_Creates_File()
    {
        string fileName = CreateTempFileName();
        try
        {
            using TextFileLog log = new(fileName);

            Assert.Equal(fileName, log.FileName);
            Assert.True(File.Exists(fileName));
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that all supported log levels are enabled by default and <see cref="LogLevel.None"/> is disabled.
    /// </summary>
    [Fact]
    public void IsEnabled_Defaults_To_True_For_All_Supported_LogLevels()
    {
        string fileName = CreateTempFileName();
        try
        {
            using TextFileLog log = new(fileName);

            Assert.True(log.IsEnabled(LogLevel.Trace));
            Assert.True(log.IsEnabled(LogLevel.Debug));
            Assert.True(log.IsEnabled(LogLevel.Information));
            Assert.True(log.IsEnabled(LogLevel.Warning));
            Assert.True(log.IsEnabled(LogLevel.Error));
            Assert.True(log.IsEnabled(LogLevel.Critical));
            Assert.False(log.IsEnabled(LogLevel.None));
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that <see cref="TextFileLog.IsEnabled(LogLevel)"/> reflects runtime changes to level-enabled flags.
    /// </summary>
    [Fact]
    public void IsEnabled_Reflects_Runtime_Toggle_Values()
    {
        string fileName = CreateTempFileName();
        try
        {
            using TextFileLog log = new(fileName)
            {
                TraceEnabled = false,
                DebugEnabled = false,
                InformationEnabled = false,
                WarningEnabled = false,
                ErrorEnabled = false,
                CriticalEnabled = false
            };

            Assert.False(log.IsEnabled(LogLevel.Trace));
            Assert.False(log.IsEnabled(LogLevel.Debug));
            Assert.False(log.IsEnabled(LogLevel.Information));
            Assert.False(log.IsEnabled(LogLevel.Warning));
            Assert.False(log.IsEnabled(LogLevel.Error));
            Assert.False(log.IsEnabled(LogLevel.Critical));
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that beginning a scope returns a <see cref="NullScope"/> instance.
    /// </summary>
    [Fact]
    public void BeginScope_Returns_NullScope_Instance()
    {
        string fileName = CreateTempFileName();
        try
        {
            using TextFileLog log = new(fileName);

            using IDisposable? scope = log.BeginScope("scope-state");

            Assert.NotNull(scope);
            Assert.IsType<NullScope>(scope);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that logging without an event identifier and without a date prefix writes the expected line.
    /// </summary>
    [Fact]
    public void Log_Without_EventId_And_Without_Date_Prefix_Writes_Expected_Line()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog log = new(fileName))
            {
                log.UseDatePrefix = false;
                log.Log(LogLevel.Information, "plain message", null, static (state, _) => state);
            }

            string[] lines = File.ReadAllLines(fileName);
            Assert.Single(lines);
            Assert.Equal("plain message", lines[0]);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that logging with an event identifier and without a date prefix writes the expected line format.
    /// </summary>
    [Fact]
    public void Log_With_EventId_And_Without_Date_Prefix_Writes_Expected_Line()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog log = new(fileName))
            {
                log.UseDatePrefix = false;
                EventId eventId = new(123);
                log.Log(LogLevel.Information, eventId, "event message", null, static (state, _) => state);
            }

            string[] lines = File.ReadAllLines(fileName);
            Assert.Single(lines);
            Assert.Equal("123: event message", lines[0]);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that logging with date prefixes enabled writes a line that ends with the message content.
    /// </summary>
    [Fact]
    public void Log_With_Date_Prefix_Writes_Line_That_Ends_With_Message()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog log = new(fileName))
            {
                log.UseDatePrefix = true;
                log.Log(LogLevel.Information, "dated message", null, static (state, _) => state);
            }

            string[] lines = File.ReadAllLines(fileName);
            Assert.Single(lines);
            Assert.EndsWith(": dated message", lines[0], StringComparison.Ordinal);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that no log output is written when the target log level is disabled.
    /// </summary>
    [Fact]
    public void Log_Does_Not_Write_When_LogLevel_Is_Disabled()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog log = new(fileName))
            {
                log.UseDatePrefix = false;
                log.WarningEnabled = false;
                log.Log(LogLevel.Warning, "should not be written", null, static (state, _) => state);
            }

            string[] lines = File.ReadAllLines(fileName);
            Assert.Empty(lines);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that no log output is written when the formatter returns <see langword="null"/>.
    /// </summary>
    [Fact]
    public void Log_Does_Not_Write_When_Formatter_Returns_Null()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog log = new(fileName))
            {
                log.UseDatePrefix = false;
                log.Log(LogLevel.Information, "state", null, static (_, _) => null!);
            }

            string[] lines = File.ReadAllLines(fileName);
            Assert.Empty(lines);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that disposing clears the file name and prevents additional writes.
    /// </summary>
    [Fact]
    public void Dispose_Clears_FileName_And_Disables_Further_Logging()
    {
        string fileName = CreateTempFileName();
        try
        {
            TextFileLog log = new(fileName);
            log.UseDatePrefix = false;
            log.Log(LogLevel.Information, "before dispose", null, static (state, _) => state);
            log.Dispose();

            Assert.Null(log.FileName);
            Assert.False(log.IsEnabled(LogLevel.Information));

            log.Log(LogLevel.Information, "after dispose", null, static (state, _) => state);

            string[] lines = File.ReadAllLines(fileName);
            Assert.Single(lines);
            Assert.Equal("before dispose", lines[0]);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that creating a new logger for an existing file appends content rather than overwriting it.
    /// </summary>
    [Fact]
    public void Constructor_Appends_To_Existing_File()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog first = new(fileName))
            {
                first.UseDatePrefix = false;
                first.Log(LogLevel.Information, "first entry", null, static (state, _) => state);
            }

            using (TextFileLog second = new(fileName))
            {
                second.UseDatePrefix = false;
                second.Log(LogLevel.Information, "second entry", null, static (state, _) => state);
            }

            string[] lines = File.ReadAllLines(fileName);
            Assert.Equal(2, lines.Length);
            Assert.Equal("first entry", lines[0]);
            Assert.Equal("second entry", lines[1]);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Creates a unique temporary file name for an isolated test log file.
    /// </summary>
    /// <returns>
    /// A string containing the full path to a unique temporary log file name.
    /// </returns>
    private static string CreateTempFileName()
    {
        return Path.Combine(Path.GetTempPath(), $"TextFileLogTests_{Guid.NewGuid():N}.log");
    }

    /// <summary>
    /// Deletes the specified file if it exists.
    /// </summary>
    /// <param name="fileName">
    /// The full path and file name to delete.
    /// </param>
    private static void DeleteFileIfExists(string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }
}
