using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Logging;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Adaptive.Intelligence.Framework.Tests.Logging
{
    /// <summary>
    /// Provides tests for the <see cref="SimpleStructuredLog"/> class.
    /// </summary>
    public class SimpleStructuredLogTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for BeginScope_Returns_NullScope.
        /// </summary>
        public void BeginScope_Returns_NullScope()
        {
            using SimpleStructuredLog log = new();

            IDisposable? scope = log.BeginScope("test-scope");

            Assert.NotNull(scope);
            Assert.IsType<NullScope>(scope);
            scope.Dispose();
        }

        [Theory]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Error)]
        [InlineData(LogLevel.Critical)]
        [InlineData(LogLevel.None)]
        /// <summary>
        /// Gets the definition for IsEnabled_Returns_True_For_All_Log_Levels.
        /// </summary>
        public void IsEnabled_Returns_True_For_All_Log_Levels(LogLevel level)
        {
            using SimpleStructuredLog log = new();

            bool enabled = log.IsEnabled(level);

            Assert.True(enabled);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Log_With_Information_Creates_Success_Record.
        /// </summary>
        public void Log_With_Information_Creates_Success_Record()
        {
            using SimpleStructuredLog log = new();
            EventId eventId = new(10, "InfoEvent");

            log.Log(LogLevel.Information, eventId, "info-state", null, static (state, _) => state);

            StructuredLogRecordCollection records = GetRecords(log);
            Assert.Single(records);

            SimpleLogRecord record = Assert.IsType<SimpleLogRecord>(records[0]);
            Assert.True(record.IsSuccess);
            Assert.False(record.IsError);
            Assert.Equal("info-state", record.InformationMessage);
            Assert.Equal(eventId, record.LogEvent);
            Assert.True(record.LogDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Log_With_Error_And_Exception_Creates_Error_Record_With_Formatted_Message.
        /// </summary>
        public void Log_With_Error_And_Exception_Creates_Error_Record_With_Formatted_Message()
        {
            using SimpleStructuredLog log = new();
            EventId eventId = new(77, "ErrorEvent");
            InvalidOperationException ex = new("boom");

            log.Log(
                LogLevel.Error,
                eventId,
                "error-state",
                ex,
                static (state, exception) => $"{state}::{exception?.Message}");

            StructuredLogRecordCollection records = GetRecords(log);
            Assert.Single(records);

            SimpleLogRecord record = Assert.IsType<SimpleLogRecord>(records[0]);
            Assert.False(record.IsSuccess);
            Assert.True(record.IsError);
            Assert.Equal("error-state::boom", record.ErrorMessage);
            Assert.Same(ex, record.Exception);
            Assert.Equal(eventId, record.LogEvent);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Log_With_Warning_Uses_State_ToString_For_InformationMessage.
        /// </summary>
        public void Log_With_Warning_Uses_State_ToString_For_InformationMessage()
        {
            using SimpleStructuredLog log = new();
            EventId eventId = new(55, "WarnEvent");
            TestState state = new("formatted-state");

            log.Log(LogLevel.Warning, eventId, state, null, static (s, _) => s.Text);

            StructuredLogRecordCollection records = GetRecords(log);
            Assert.Single(records);

            SimpleLogRecord record = Assert.IsType<SimpleLogRecord>(records[0]);
            Assert.Equal("formatted-state", record.InformationMessage);
            Assert.True(record.IsSuccess);
            Assert.False(record.IsError);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_Clears_And_Releases_Record_Collection.
        /// </summary>
        public void Dispose_Clears_And_Releases_Record_Collection()
        {
            SimpleStructuredLog log = new();
            log.Log(LogLevel.Information, new EventId(1), "before-dispose", null, static (s, _) => s);

            StructuredLogRecordCollection recordsBeforeDispose = GetRecords(log);
            Assert.Single(recordsBeforeDispose);

            log.Dispose();

            Assert.Null(GetRecordsOrNull(log));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Log_After_Dispose_Does_Not_Throw.
        /// </summary>
        public void Log_After_Dispose_Does_Not_Throw()
        {
            SimpleStructuredLog log = new();
            log.Dispose();

            Exception? ex = Record.Exception(() =>
                log.Log(LogLevel.Information, new EventId(2), "after-dispose", null, static (s, _) => s));

            Assert.Null(ex);
        }

        /// <summary>
        /// Gets the definition for GetRecords.
        /// </summary>
        private static StructuredLogRecordCollection GetRecords(SimpleStructuredLog log)
        {
            return GetRecordsOrNull(log) ?? throw new InvalidOperationException("Record collection was null.");
        }

        /// <summary>
        /// Gets the definition for GetRecordsOrNull.
        /// </summary>
        private static StructuredLogRecordCollection? GetRecordsOrNull(SimpleStructuredLog log)
        {
            FieldInfo? field = typeof(SimpleStructuredLog).GetField("_recordCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            return (StructuredLogRecordCollection?)field?.GetValue(log);
        }

        /// <summary>
        /// Gets the definition for TestState.
        /// </summary>
        private sealed record TestState(string Text)
        {
            /// <summary>
            /// Gets the definition for ToString.
            /// </summary>
            public override string ToString()
            {
                return Text;
            }
        }
    }
}