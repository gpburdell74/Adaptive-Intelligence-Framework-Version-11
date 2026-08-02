using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Framework.Tests.Mocks;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Abstractions
{
    /// <summary>
    /// Provides the tests for the <see cref="LoggableBase"/> abstract class.
    /// </summary>
    public class LoggableBaseTests
    {
        /// <summary>
        /// Tests that an instance can be created without a logger and logging calls are no-ops.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Can_Create_Without_Logger.
        /// </summary>
        public void Can_Create_Without_Logger()
        {
            using MockLoggableBase mock = new();
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
        /// <summary>
        /// Gets the definition for LogCritical_Overloads_Write_Expected_Entries.
        /// </summary>
        public void LogCritical_Overloads_Write_Expected_Entries()
        {
            TestLogger logger = new();
            using MockLoggableBase mock = new(logger);
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
        /// <summary>
        /// Gets the definition for LogDebug_Overloads_Write_Expected_Entries.
        /// </summary>
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
        /// <summary>
        /// Gets the definition for LogError_Overloads_Write_Expected_Entries.
        /// </summary>
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
        /// <summary>
        /// Gets the definition for LogInformation_And_LogWarning_Write_Expected_Entries.
        /// </summary>
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
        /// <summary>
        /// Gets the definition for Dispose_Clears_Logger_Reference_And_Prevents_Further_Logging.
        /// </summary>
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
        /// <summary>
        /// Gets the definition for Log_Methods_Do_Not_Write_When_Log_Levels_Are_Disabled.
        /// </summary>
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
        /// <summary>
        /// Gets the definition for Log_EventId_String_Overload_Works.
        /// </summary>
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

        /// <summary>
        /// Gets the definition for AssertLogEntry.
        /// </summary>
        private static void AssertLogEntry(TestLogEntry entry, LogLevel level, int eventId, string message, Exception? exception)
        {
            Assert.Equal(level, entry.Level);
            Assert.Equal(eventId, entry.EventId.Id);
            Assert.Equal(message, entry.Message);
            Assert.Same(exception, entry.Exception);
        }

        /// <summary>
        /// Gets the definition for TestLogEntry.
        /// </summary>
        private sealed record TestLogEntry(LogLevel Level, EventId EventId, string Message, Exception? Exception);

        /// <summary>
        /// Gets the definition for TestLogger.
        /// </summary>
        private sealed class TestLogger : ILogger
        {
            /// <summary>
            /// Gets the definition for Entries.
            /// </summary>
            public List<TestLogEntry> Entries { get; } = [];

            public IDisposable BeginScope<TState>(TState state) where TState : notnull
            {
                return NoopScope.Instance;
            }

            /// <summary>
            /// Gets the definition for IsEnabled.
            /// </summary>
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                string message = formatter(state, exception);
                Entries.Add(new TestLogEntry(logLevel, eventId, message, exception));
            }

            /// <summary>
            /// Gets the definition for NoopScope.
            /// </summary>
            private sealed class NoopScope : IDisposable
            {
                /// <summary>
                /// Gets the definition for new.
                /// </summary>
                public static NoopScope Instance { get; } = new();

                /// <summary>
                /// Gets the definition for Dispose.
                /// </summary>
                public void Dispose()
                {
                }
            }
        }

        /// <summary>
        /// Gets the definition for LevelControlledTestLogger.
        /// </summary>
        private sealed class LevelControlledTestLogger(IEnumerable<LogLevel> enabledLevels) : ILogger
        {
            /// <summary>
            /// Gets the definition for _enabledLevels.
            /// </summary>
            private readonly HashSet<LogLevel> _enabledLevels = [.. enabledLevels];

            /// <summary>
            /// Gets the definition for Entries.
            /// </summary>
            public List<TestLogEntry> Entries { get; } = [];

            public IDisposable BeginScope<TState>(TState state) where TState : notnull
            {
                return NoopScope.Instance;
            }

            /// <summary>
            /// Gets the definition for IsEnabled.
            /// </summary>
            public bool IsEnabled(LogLevel logLevel)
            {
                return _enabledLevels.Contains(logLevel);
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                string message = formatter(state, exception);
                Entries.Add(new TestLogEntry(logLevel, eventId, message, exception));
            }

            /// <summary>
            /// Gets the definition for NoopScope.
            /// </summary>
            private sealed class NoopScope : IDisposable
            {
                /// <summary>
                /// Gets the definition for new.
                /// </summary>
                public static NoopScope Instance { get; } = new();

                /// <summary>
                /// Gets the definition for Dispose.
                /// </summary>
                public void Dispose()
                {
                }
            }
        }
    }
}