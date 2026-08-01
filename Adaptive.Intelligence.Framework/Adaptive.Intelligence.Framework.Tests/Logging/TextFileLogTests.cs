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
    /// Tests that logging with <see cref="LogLevel.None"/> does not write output.
    /// </summary>
    [Fact]
    public void Log_Does_Not_Write_When_LogLevel_Is_None()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog log = new(fileName))
            {
                log.UseDatePrefix = false;
                log.Log(LogLevel.None, "none message", null, static (state, _) => state);
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
    /// Tests that the formatter is not invoked when the selected log level is disabled.
    /// </summary>
    [Fact]
    public void Log_Does_Not_Invoke_Formatter_When_Level_Is_Disabled()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog log = new(fileName))
            {
                log.UseDatePrefix = false;
                log.DebugEnabled = false;
                bool formatterInvoked = false;

                log.Log(LogLevel.Debug, "state", null, (state, _) =>
                {
                    formatterInvoked = true;
                    return state;
                });

                Assert.False(formatterInvoked);
            }
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that the event-id overload formats using the event name when a name is provided.
    /// </summary>
    [Fact]
    public void Log_With_Named_EventId_Writes_Event_Name_Prefix()
    {
        string fileName = CreateTempFileName();
        try
        {
            using (TextFileLog log = new(fileName))
            {
                log.UseDatePrefix = false;
                EventId eventId = new(22, "NamedEvent");
                log.Log(LogLevel.Information, eventId, "named event message", null, static (state, _) => state);
            }

            string[] lines = File.ReadAllLines(fileName);
            Assert.Single(lines);
            Assert.Equal("NamedEvent: named event message", lines[0]);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that each scope request returns a non-null, independent scope instance.
    /// </summary>
    [Fact]
    public void BeginScope_Returns_New_Scope_Instance_Each_Time()
    {
        string fileName = CreateTempFileName();
        try
        {
            using TextFileLog log = new(fileName);

            using IDisposable? first = log.BeginScope("scope-1");
            using IDisposable? second = log.BeginScope("scope-2");

            Assert.NotNull(first);
            Assert.NotNull(second);
            Assert.IsType<NullScope>(first);
            Assert.IsType<NullScope>(second);
            Assert.NotSame(first, second);
        }
        finally
        {
            DeleteFileIfExists(fileName);
        }
    }

    /// <summary>
    /// Tests that logger construction gracefully handles an invalid path by disabling logging.
    /// </summary>
    [Fact]
    public void Constructor_With_Invalid_Path_Disables_Logging()
    {
        string fileName = "\0invalid.log";
        using TextFileLog log = new(fileName);

        Assert.Equal(fileName, log.FileName);
        Assert.False(log.IsEnabled(LogLevel.Information));
        Assert.False(log.IsEnabled(LogLevel.Error));
    }

    [Fact]
    public void Critical_Property_Works()
    {
        string fileName = CreateTempFileName();
        using TextFileLog log = new(fileName);

        log.CriticalEnabled = false;
        Assert.False(log.CriticalEnabled);
        log.LogCritical("This should not be logged.");
        log.Dispose();

        string text = ReadLogContents(fileName);

        Assert.DoesNotContain("This should not be logged.", text);

        using TextFileLog log2 = new(fileName);
        log2.CriticalEnabled = true;
        Assert.True(log2.CriticalEnabled);
        log2.LogCritical("This should be logged.");
        log2.Dispose();

        text = ReadLogContents(fileName);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Debug_Property_Works()
    {
        string fileName = CreateTempFileName();
        using TextFileLog log = new(fileName);

        log.DebugEnabled = false;
        Assert.False(log.DebugEnabled);
        log.LogDebug("This should not be logged.");
        log.Dispose();

        string text = ReadLogContents(fileName);

        Assert.DoesNotContain("This should not be logged.", text);

        using TextFileLog log2 = new(fileName);
        log2.DebugEnabled = true;
        Assert.True(log2.DebugEnabled);
        log2.LogDebug("This should be logged.");
        log2.Dispose();

        text = ReadLogContents(fileName);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Error_Property_Works()
    {
        string fileName = CreateTempFileName();
        using TextFileLog log = new(fileName);

        log.ErrorEnabled = false;
        Assert.False(log.ErrorEnabled);
        log.LogError("This should not be logged.");
        log.Dispose();

        string text = ReadLogContents(fileName);

        Assert.DoesNotContain("This should not be logged.", text);

        using TextFileLog log2 = new(fileName);
        log2.ErrorEnabled = true;
        Assert.True(log2.ErrorEnabled);
        log2.LogError("This should be logged.");
        log2.Dispose();

        text = ReadLogContents(fileName);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Information_Property_Works()
    {
        string fileName = CreateTempFileName();
        using TextFileLog log = new(fileName);

        log.InformationEnabled = false;
        Assert.False(log.InformationEnabled);
        log.LogInformation("This should not be logged.");
        log.Dispose();

        string text = ReadLogContents(fileName);

        Assert.DoesNotContain("This should not be logged.", text);

        using TextFileLog log2 = new(fileName);
        log2.InformationEnabled = true;
        Assert.True(log2.InformationEnabled);
        log2.LogInformation("This should be logged.");
        log2.Dispose();

        text = ReadLogContents(fileName);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Trace_Property_Works()
    {
        string fileName = CreateTempFileName();
        using TextFileLog log = new(fileName);

        log.TraceEnabled = false;
        Assert.False(log.TraceEnabled);
        log.LogTrace("This should not be logged.");
        log.Dispose();

        string text = ReadLogContents(fileName);

        Assert.DoesNotContain("This should not be logged.", text);

        using TextFileLog log2 = new(fileName);
        log2.TraceEnabled = true;
        Assert.True(log2.TraceEnabled);
        log2.LogTrace("This should be logged.");
        log2.Dispose();

        text = ReadLogContents(fileName);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Warning_Property_Works()
    {
        string fileName = CreateTempFileName();
        using TextFileLog log = new(fileName);

        log.WarningEnabled = false;
        Assert.False(log.WarningEnabled);   
        log.LogWarning("This should not be logged.");
        log.Dispose();

        string text = ReadLogContents(fileName);

        Assert.DoesNotContain("This should not be logged.", text);

        using TextFileLog log2 = new(fileName);
        log2.WarningEnabled = true;
        Assert.True(log2.WarningEnabled);
        log2.LogWarning("This should be logged.");
        log2.Dispose();

        text = ReadLogContents(fileName);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void DatePrefix_Property_Works()
    {
        string fileName = CreateTempFileName();
        using TextFileLog log = new(fileName);

        log.UseDatePrefix = false;
        Assert.False(log.UseDatePrefix);
        log.LogWarning("This should not be preceeded by a date value.");
        log.Dispose();

        string text = ReadLogContents(fileName);
        int index = text.IndexOf(" ");
        string datestring = text.Substring( 0, index).Trim();
        bool isADate = DateTime.TryParse(datestring, out _);
        Assert.False(isADate);
        Assert.Contains("This should not be preceeded by a date value.", text);

        using TextFileLog log2 = new(fileName);
        log2.UseDatePrefix = true;
        Assert.True(log2.UseDatePrefix);
        log2.LogWarning("This should be preceeded by a date value.");
        log2.Dispose();

        text = ReadLogContents(fileName);
        int index2 = text.IndexOf(" ");
        string datestring2 = text.Substring(0, index2).Trim();
        bool isADate2 = DateTime.TryParse(datestring2, out _);
        Assert.True(isADate2);
        Assert.Contains("This should be preceeded by a date value.", text);

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

    private static string ReadLogContents(string fileName)
    {
        FileStream fs = new(fileName, FileMode.Open, FileAccess.Read);
        StreamReader r = new(fs);
        string text = r.ReadToEnd();
        r.Dispose();
        fs.Dispose();
        File.Delete(fileName);
        return text;
    }
}
