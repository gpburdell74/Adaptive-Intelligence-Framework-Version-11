using Adaptive.Intelligence.Logging;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Logging
{
    /// <summary>
    /// Provides tests for the <see cref="SimpleLogRecord"/> class.
    /// </summary>
    public class SimpleLogRecordTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for FromJson_With_Null_Or_Whitespace_Returns_Null.
        /// </summary>
        public void FromJson_With_Null_Or_Whitespace_Returns_Null()
        {
            Assert.Null(SimpleLogRecord.FromJson(null));
            Assert.Null(SimpleLogRecord.FromJson(string.Empty));
            Assert.Null(SimpleLogRecord.FromJson("   "));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for FromJson_With_Invalid_Json_Returns_Null.
        /// </summary>
        public void FromJson_With_Invalid_Json_Returns_Null()
        {
            SimpleLogRecord? record = SimpleLogRecord.FromJson("{ not-valid-json }");

            Assert.Null(record);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ToJson_Returns_Json_For_Current_Record_State.
        /// </summary>
        public void ToJson_Returns_Json_For_Current_Record_State()
        {
            SimpleLogRecord record = new()
            {
                LogDate = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc),
                LogEvent = new EventId(25, "TestEvent"),
                IsError = false,
                IsSuccess = true,
                Level = LogLevel.Information,
                InformationMessage = "operation succeeded",
                TraceInformation = "trace-data"
            };

            string? json = record.ToJson();

            Assert.NotNull(json);
            Assert.Contains("operation succeeded", json, StringComparison.Ordinal);
            Assert.Contains("TestEvent", json, StringComparison.Ordinal);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ToJson_Then_FromJson_RoundTrips_Key_Data.
        /// </summary>
        public void ToJson_Then_FromJson_RoundTrips_Key_Data()
        {
            SimpleLogRecord source = new()
            {
                LogDate = DateTime.UtcNow,
                LogEvent = new EventId(99, "RoundTrip"),
                IsError = true,
                IsSuccess = false,
                ErrorMessage = "failure details",
                InformationMessage = "ignored for error",
                TraceInformation = "trace"
            };

            string? json = source.ToJson();
            SimpleLogRecord? result = SimpleLogRecord.FromJson(json);

            Assert.NotNull(result);
            Assert.NotNull(result.LogEvent);
            Assert.Equal(source.IsError, result.IsError);
            Assert.Equal(source.IsSuccess, result.IsSuccess);
            Assert.Equal(source.ErrorMessage, result.ErrorMessage);
            Assert.Equal(source.InformationMessage, result.InformationMessage);
            Assert.Equal(source.TraceInformation, result.TraceInformation);
        }
    }
}