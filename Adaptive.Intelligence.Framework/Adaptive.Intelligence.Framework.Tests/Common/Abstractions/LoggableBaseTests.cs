using Adaptive.Intelligence.Common.Abstractions.Logging;
using Adaptive.Intelligence.Framework.Tests.Mocks;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Common.Abstractions;

/// <summary>
/// Provides the tests for the <see cref="LoggableBase"/> abstract class.
/// </summary>
public class LoggableBaseTests
{
    /// <summary>
    /// Tests that an instance can be created without a logger and logging calls are no-ops.
    /// </summary>
    [Fact]
    public void Can_Create_Without_Logger()
    {
        MockLoggableBase mock = new();
        Assert.NotNull(mock);

        mock.InvokeLogInformation("No logger set.");
        mock.InvokeLogWarning("No logger set.");
        mock.InvokeLogError("No logger set.");
        mock.InvokeLogDebug("No logger set.");
        mock.InvokeLogCritical("No logger set.");
    }

    /// <summary>
    /// Tests that critical logging overloads write entries with expected values.
    /// </summary>
    [Fact]
    public void LogCritical_Overloads_Write_Expected_Entries()
    {
        TestLogger logger = new();
        MockLoggableBase mock = new(logger);
        InvalidOperationException ex = new("critical exception");

        mock.InvokeLogCritical("critical message");
        mock.InvokeLogCritical(ex);
        mock.InvokeLogCritical(ex, "critical message with exception");

        Assert.Collection(
            logger.Entries,
            entry => AssertLogEntry(entry, LogLevel.Critical, 0, "critical message", null),
            entry => AssertLogEntry(entry, LogLevel.Critical, 0, "critical exception", ex),
            entry => AssertLogEntry(entry, LogLevel.Critical, 0, "critical message with exception", ex));
    }

    /// <summary>
    /// Tests that debug logging overloads write entries with expected values.
    /// </summary>
    [Fact]
    public void LogDebug_Overloads_Write_Expected_Entries()
    {
        TestLogger logger = new();
        MockLoggableBase mock = new(logger);
        EventId eventId = new(101, "DebugEvent");
        ApplicationException ex = new("debug exception");

        mock.InvokeLogDebug("debug message");
        mock.InvokeLogDebug(eventId, "debug event message");
        mock.InvokeLogDebug(ex);
        mock.InvokeLogDebug(ex, "debug with exception");
        mock.InvokeLogDebug(eventId, ex, "debug event with exception");

        Assert.Collection(
            logger.Entries,
            entry => AssertLogEntry(entry, LogLevel.Debug, 0, "debug message", null),
            entry => AssertLogEntry(entry, LogLevel.Debug, 101, "debug event message", null),
            entry => AssertLogEntry(entry, LogLevel.Debug, 0, "debug exception", ex),
            entry => AssertLogEntry(entry, LogLevel.Debug, 0, "debug with exception", ex),
            entry => AssertLogEntry(entry, LogLevel.Debug, 101, "debug event with exception", ex));
    }

    /// <summary>
    /// Tests that error logging overloads write entries with expected values.
    /// </summary>
    [Fact]
    public void LogError_Overloads_Write_Expected_Entries()
    {
        TestLogger logger = new();
        MockLoggableBase mock = new(logger);
        EventId eventId = new(202, "ErrorEvent");
        ArgumentException ex = new("error exception");

        mock.InvokeLogError("error message");
        mock.InvokeLogError(eventId, "error event message");
        mock.InvokeLogError(ex);
        mock.InvokeLogError(ex, "error with exception");
        mock.InvokeLogError(eventId, ex, "error event with exception");

        Assert.Collection(
            logger.Entries,
            entry => AssertLogEntry(entry, LogLevel.Error, 0, "error message", null),
            entry => AssertLogEntry(entry, LogLevel.Error, 202, "error event message", null),
            entry => AssertLogEntry(entry, LogLevel.Error, 0, "error exception", ex),
            entry => AssertLogEntry(entry, LogLevel.Error, 0, "error with exception", ex),
            entry => AssertLogEntry(entry, LogLevel.Error, 202, "error event with exception", ex));
    }

    /// <summary>
    /// Tests that informational and warning messages are logged with expected values.
    /// </summary>
    [Fact]
    public void LogInformation_And_LogWarning_Write_Expected_Entries()
    {
        TestLogger logger = new();
        MockLoggableBase mock = new(logger);

        mock.InvokeLogInformation("info message");
        mock.InvokeLogWarning("warning message");

        Assert.Collection(
            logger.Entries,
            entry => AssertLogEntry(entry, LogLevel.Information, 0, "info message", null),
            entry => AssertLogEntry(entry, LogLevel.Warning, 0, "warning message", null));
    }

    /// <summary>
    /// Tests that disposing clears the logger reference and prevents subsequent log writes.
    /// </summary>
    [Fact]
    public void Dispose_Clears_Logger_Reference_And_Prevents_Further_Logging()
    {
        TestLogger logger = new();
        MockLoggableBase mock = new(logger);

        mock.InvokeLogInformation("before dispose");
        Assert.Single(logger.Entries);

        mock.Dispose();
        mock.InvokeLogInformation("after dispose");
        mock.InvokeLogWarning("after dispose");

        Assert.Single(logger.Entries);
        AssertLogEntry(logger.Entries[0], LogLevel.Information, 0, "before dispose", null);
    }

    /// <summary>
    /// Tests that no entries are written when all log levels are disabled.
    /// </summary>
    [Fact]
    public void Log_Methods_Do_Not_Write_When_Log_Levels_Are_Disabled()
    {
        LevelControlledTestLogger logger = new(
            enabledLevels:
            [
                LogLevel.None
            ]);

        MockLoggableBase mock = new(logger);
        EventId eventId = new(55, "DisabledEvent");
        InvalidOperationException ex = new("disabled");

        mock.InvokeLogCritical("critical");
        mock.InvokeLogCritical(ex);
        mock.InvokeLogCritical(ex, "critical-ex");

        mock.InvokeLogDebug("debug");
        mock.InvokeLogDebug(eventId, "debug-event");
        mock.InvokeLogDebug(ex);
        mock.InvokeLogDebug(ex, "debug-ex");
        mock.InvokeLogDebug(eventId, ex, "debug-event-ex");

        mock.InvokeLogError("error");
        mock.InvokeLogError(eventId, "error-event");
        mock.InvokeLogError(ex);
        mock.InvokeLogError(ex, "error-ex");
        mock.InvokeLogError(eventId, ex, "error-event-ex");

        mock.InvokeLogInformation("info");
        mock.InvokeLogWarning("warn");

        Assert.Empty(logger.Entries);
    }
    [Fact]
    public void Log_EventId_String_Overload_Works()
    {
        LevelControlledTestLogger logger = new(
            enabledLevels:
            [
                LogLevel.Debug
            ]);

        MockLoggableBase mock = new(logger);
        EventId eventId = new(42, "TestEvent");
        mock.InvokeLogDebug(eventId, "This is an debug log test.");
        Assert.NotEmpty(logger.Entries);
        Assert.Equal(42, logger.Entries[0].EventId.Id);
        Assert.Equal(LogLevel.Debug, logger.Entries[0].Level);

    }

    private static void AssertLogEntry(TestLogEntry entry, LogLevel level, int eventId, string message, Exception? exception)
    {
        Assert.Equal(level, entry.Level);
        Assert.Equal(eventId, entry.EventId.Id);
        Assert.Equal(message, entry.Message);
        Assert.Same(exception, entry.Exception);
    }

    private sealed record TestLogEntry(LogLevel Level, EventId EventId, string Message, Exception? Exception);

    private sealed class TestLogger : ILogger
    {
        public List<TestLogEntry> Entries { get; } = [];

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return NoopScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = formatter(state, exception);
            Entries.Add(new TestLogEntry(logLevel, eventId, message, exception));
        }

        private sealed class NoopScope : IDisposable
        {
            public static NoopScope Instance { get; } = new();

            public void Dispose()
            {
            }
        }
    }

    private sealed class LevelControlledTestLogger(IEnumerable<LogLevel> enabledLevels) : ILogger
    {
        private readonly HashSet<LogLevel> _enabledLevels = [.. enabledLevels];

        public List<TestLogEntry> Entries { get; } = [];

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return NoopScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _enabledLevels.Contains(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string message = formatter(state, exception);
            Entries.Add(new TestLogEntry(logLevel, eventId, message, exception));
        }

        private sealed class NoopScope : IDisposable
        {
            public static NoopScope Instance { get; } = new();

            public void Dispose()
            {
            }
        }
    }
}
