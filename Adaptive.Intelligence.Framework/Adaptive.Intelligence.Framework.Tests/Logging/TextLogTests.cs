using Adaptive.Intelligence.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Testing.Platform.Extensions.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Adaptive.Intelligence.Framework.Tests.Logging;

public class TextLogTests
{
    /// <summary>
    /// Tests that constructing the logger sets the file name and creates the destination log file.
    /// </summary>
    [Fact]
    public void Constructor_Sets_FileName_And_Creates_File()
    {
        MemoryStream ms = new();
        StreamWriter writer = new(ms);

        using TextLog log = new(ms, writer);
        Assert.True(log.CanWrite);
        log.Dispose();

        Assert.True(writer.BaseStream.CanWrite);
        Assert.True(ms.CanWrite);
        Assert.True(writer.BaseStream == ms);

        writer.Dispose();
        ms.Dispose();
    }

    /// <summary>
    /// Tests that all supported log levels are enabled by default and <see cref="LogLevel.None"/> is disabled.
    /// </summary>
    [Fact]
    public void IsEnabled_Defaults_To_True_For_All_Supported_LogLevels()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

            Assert.True(log.IsEnabled(LogLevel.Trace));
            Assert.True(log.IsEnabled(LogLevel.Debug));
            Assert.True(log.IsEnabled(LogLevel.Information));
            Assert.True(log.IsEnabled(LogLevel.Warning));
            Assert.True(log.IsEnabled(LogLevel.Error));
            Assert.True(log.IsEnabled(LogLevel.Critical));
            Assert.False(log.IsEnabled(LogLevel.None));
    }

    /// <summary>
    /// Tests that <see cref="TextLog.IsEnabled(LogLevel)"/> reflects runtime changes to level-enabled flags.
    /// </summary>
    [Fact]
    public void IsEnabled_Reflects_Runtime_Toggle_Values()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);
        log.TraceEnabled = false;
        log.DebugEnabled = false;
        log.InformationEnabled = false;
        log.WarningEnabled = false;
        log.ErrorEnabled = false;
        log.CriticalEnabled = false;

        Assert.False(log.IsEnabled(LogLevel.Trace));
        Assert.False(log.IsEnabled(LogLevel.Debug));
        Assert.False(log.IsEnabled(LogLevel.Information));
        Assert.False(log.IsEnabled(LogLevel.Warning));
        Assert.False(log.IsEnabled(LogLevel.Error));
        Assert.False(log.IsEnabled(LogLevel.Critical));
    }

    /// <summary>
    /// Tests that beginning a scope returns a <see cref="NullScope"/> instance.
    /// </summary>
    [Fact]
    public void BeginScope_Returns_NullScope_Instance()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        using IDisposable? scope = log.BeginScope("scope-state");

        Assert.NotNull(scope);
        Assert.IsType<NullScope>(scope);
    }

    /// <summary>
    /// Tests that logging without an event identifier and without a date prefix writes the expected line.
    /// </summary>
    [Fact]
    public void Log_Without_EventId_And_Without_Date_Prefix_Writes_Expected_Line()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);
        log.UseDatePrefix = false;
        log.Log(LogLevel.Information, "plain message", null, static (state, _) => state);

        List<string> lines = ReadMemoryStreamLines(ms);

        Assert.Single(lines);
        Assert.Equal("plain message", lines[0]);
    }

    /// <summary>
    /// Tests that logging with an event identifier and without a date prefix writes the expected line format.
    /// </summary>
    [Fact]
    public void Log_With_EventId_And_Without_Date_Prefix_Writes_Expected_Line()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.UseDatePrefix = false;
        EventId eventId = new(123);
        log.Log(LogLevel.Information, eventId, "event message", null, static (state, _) => state);

        List<string> lines = ReadMemoryStreamLines(ms);

        Assert.Single(lines);
        Assert.Equal("123: event message", lines[0]);
    }

    /// <summary>
    /// Tests that logging with date prefixes enabled writes a line that ends with the message content.
    /// </summary>
    [Fact]
    public void Log_With_Date_Prefix_Writes_Line_That_Ends_With_Message()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.UseDatePrefix = true;
        log.Log(LogLevel.Information, "dated message", null, static (state, _) => state);

        var lines = ReadMemoryStreamLines(ms);
        Assert.Single(lines);
        Assert.EndsWith(": dated message", lines[0], StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that no log output is written when the target log level is disabled.
    /// </summary>
    [Fact]
    public void Log_Does_Not_Write_When_LogLevel_Is_Disabled()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.UseDatePrefix = false;
        log.WarningEnabled = false;
        log.Log(LogLevel.Warning, "should not be written", null, static (state, _) => state);

        var lines = ReadMemoryStreamLines(ms);
        Assert.Empty(lines);
    }

    /// <summary>
    /// Tests that no log output is written when the formatter returns <see langword="null"/>.
    /// </summary>
    [Fact]
    public void Log_Does_Not_Write_When_Formatter_Returns_Null()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);
        log.UseDatePrefix = false;
        log.Log(LogLevel.Information, "state", null, static (_, _) => null!);

        var lines = ReadMemoryStreamLines(ms);
        Assert.Empty(lines);
    }

    /// <summary>
    /// Tests that disposing clears the file name and prevents additional writes.
    /// </summary>
    [Fact]
    public void Dispose_Clears_FileName_And_Disables_Further_Logging()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.UseDatePrefix = false;
        log.Log(LogLevel.Information, "before dispose", null, static (state, _) => state);
        log.Dispose();

        Assert.Null(log.FileName);
        Assert.False(log.IsEnabled(LogLevel.Information));

        log.Log(LogLevel.Information, "after dispose", null, static (state, _) => state);

        var lines = ReadMemoryStreamLines(ms);
        Assert.Single(lines);
        Assert.Equal("before dispose", lines[0]);
    }

    /// <summary>
    /// Tests that logging with <see cref="LogLevel.None"/> does not write output.
    /// </summary>
    [Fact]
    public void Log_Does_Not_Write_When_LogLevel_Is_None()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);
        log.UseDatePrefix = false;
        log.Log(LogLevel.None, "none message", null, static (state, _) => state);

        var lines = ReadMemoryStreamLines(ms);
        Assert.Empty(lines);
    }

    /// <summary>
    /// Tests that the formatter is not invoked when the selected log level is disabled.
    /// </summary>
    [Fact]
    public void Log_Does_Not_Invoke_Formatter_When_Level_Is_Disabled()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

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

    /// <summary>
    /// Tests that the event-id overload formats using the event name when a name is provided.
    /// </summary>
    [Fact]
    public void Log_With_Named_EventId_Writes_Event_Name_Prefix()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.UseDatePrefix = false;
        EventId eventId = new(22, "NamedEvent");
        log.Log(LogLevel.Information, eventId, "named event message", null, static (state, _) => state);

        var lines = ReadMemoryStreamLines(ms);
        Assert.Single(lines);
        Assert.Equal("NamedEvent: named event message", lines[0]);
    }

    /// <summary>
    /// Tests that each scope request returns a non-null, independent scope instance.
    /// </summary>
    [Fact]
    public void BeginScope_Returns_New_Scope_Instance_Each_Time()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        using IDisposable? first = log.BeginScope("scope-1");
        using IDisposable? second = log.BeginScope("scope-2");

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.IsType<NullScope>(first);
        Assert.IsType<NullScope>(second);
        Assert.NotSame(first, second);
    }

    [Fact]
    public void Critical_Property_Works()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.CriticalEnabled = false;
        Assert.False(log.CriticalEnabled);
        log.LogCritical("This should not be logged.");
        log.Dispose();

        string text = ReadStreamLogContents(ms);

        ms.Dispose();
        writer.Dispose();
        Assert.DoesNotContain("This should not be logged.", text);

        MemoryStream ms2 = new();
        StreamWriter writer2 = new(ms2);
        using TextLog log2 = new(ms2, writer2);
        log2.CriticalEnabled = true;
        Assert.True(log2.CriticalEnabled);
        log2.LogCritical("This should be logged.");
        log2.Dispose();
        
        text = ReadStreamLogContents(ms2);
        ms2.Dispose();
        writer2.Dispose();
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Debug_Property_Works()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.DebugEnabled = false;
        Assert.False(log.DebugEnabled);
        log.LogDebug("This should not be logged.");
        log.Dispose();

        string text = ReadStreamLogContents(ms);

        Assert.DoesNotContain("This should not be logged.", text);

        using MemoryStream ms2 = new();
        using StreamWriter writer2 = new(ms2);
        using TextLog log2 = new(ms2, writer2);
        log2.DebugEnabled = true;
        Assert.True(log2.DebugEnabled);
        log2.LogDebug("This should be logged.");
        log2.Dispose();

        text = ReadStreamLogContents(ms2);
        
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Error_Property_Works()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.ErrorEnabled = false;
        Assert.False(log.ErrorEnabled);
        log.LogError("This should not be logged.");
        log.Dispose();

        string text = ReadStreamLogContents(ms);

        Assert.DoesNotContain("This should not be logged.", text);

        using MemoryStream ms2 = new();
        using StreamWriter writer2 = new(ms2);
        using TextLog log2 = new(ms2, writer2);
        log2.ErrorEnabled = true;
        Assert.True(log2.ErrorEnabled);
        log2.LogError("This should be logged.");
        log2.Dispose();

        text = ReadStreamLogContents(ms2);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Information_Property_Works()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.InformationEnabled = false;
        Assert.False(log.InformationEnabled);
        log.LogInformation("This should not be logged.");
        log.Dispose();

        string text = ReadStreamLogContents(ms);

        Assert.DoesNotContain("This should not be logged.", text);

        using MemoryStream ms2 = new();
        using StreamWriter writer2 = new(ms2);
        using TextLog log2 = new(ms2, writer2);
        log2.InformationEnabled = true;
        Assert.True(log2.InformationEnabled);
        log2.LogInformation("This should be logged.");
        log2.Dispose();

        text = ReadStreamLogContents(ms2);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Trace_Property_Works()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.TraceEnabled = false;
        Assert.False(log.TraceEnabled);
        log.LogTrace("This should not be logged.");
        log.Dispose();

        string text = ReadStreamLogContents(ms);

        Assert.DoesNotContain("This should not be logged.", text);

        using MemoryStream ms2 = new();
        using StreamWriter writer2 = new(ms2);
        using TextLog log2 = new(ms2, writer2);
        log2.TraceEnabled = true;
        Assert.True(log2.TraceEnabled);
        log2.LogTrace("This should be logged.");
        log2.Dispose();

        text = ReadStreamLogContents(ms2);

        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void Warning_Property_Works()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.WarningEnabled = false;
        Assert.False(log.WarningEnabled);
        log.LogWarning("This should not be logged.");
        log.Dispose();

        string text = ReadStreamLogContents(ms);

        Assert.DoesNotContain("This should not be logged.", text);

        using MemoryStream ms2 = new();
        using StreamWriter writer2 = new(ms2);
        using TextLog log2 = new(ms2, writer2);
        log2.WarningEnabled = true;
        Assert.True(log2.WarningEnabled);
        log2.LogWarning("This should be logged.");
        log2.Dispose();

        text = ReadStreamLogContents(ms2);
        Assert.Contains("This should be logged.", text);
    }

    [Fact]
    public void DatePrefix_Property_Works()
    {
        using MemoryStream ms = new();
        using StreamWriter writer = new(ms);
        using TextLog log = new(ms, writer);

        log.UseDatePrefix = false;
        Assert.False(log.UseDatePrefix);
        log.LogWarning("This should not be preceeded by a date value.");
        log.Dispose();

        string text = ReadStreamLogContents(ms);
        int index = text.IndexOf(" ");
        string datestring = text.Substring(0, index).Trim();
        bool isADate = DateTime.TryParse(datestring, out _);
        Assert.False(isADate);
        Assert.Contains("This should not be preceeded by a date value.", text);

        using MemoryStream ms2 = new();
        using StreamWriter writer2 = new(ms2);
        using TextLog log2 = new(ms2, writer2);
        log2.UseDatePrefix = true;
        Assert.True(log2.UseDatePrefix);
        log2.LogWarning("This should be preceeded by a date value.");
        log2.Dispose();

        text = ReadStreamLogContents(ms2);
        int index2 = text.IndexOf(" ");
        string datestring2 = text.Substring(0, index2).Trim();
        bool isADate2 = DateTime.TryParse(datestring2, out _);
        Assert.True(isADate2);
        Assert.Contains("This should be preceeded by a date value.", text);

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
    private static string ReadStreamLogContents(Stream stream)
    {
        long lastPos = stream.Position;
        stream.Seek(0, SeekOrigin.Begin);
        StreamReader reader = new(stream);
        string text = reader.ReadToEnd();
        stream.Seek(lastPos, SeekOrigin.Begin);
        return text;
    }
    private static List<string> ReadMemoryStreamLines(MemoryStream ms)
    {
        long lastPos = ms.Position;

        StreamReader reader = new(ms);
        ms.Seek(0, SeekOrigin.Begin);
        List<string> lines = [];
        bool done = false;
        do
        {
            string? text = reader.ReadLine();
            if (text != null)
            {
                lines.Add(text);
            }
            else
            {
                done = true;
            }
        } while (!done);
    
        ms.Seek(lastPos, SeekOrigin.Begin);
        return lines;
    }
}