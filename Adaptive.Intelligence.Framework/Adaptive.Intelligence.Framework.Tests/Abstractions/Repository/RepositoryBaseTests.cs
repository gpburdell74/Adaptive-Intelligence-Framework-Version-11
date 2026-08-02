using Adaptive.Intelligence.Framework.Tests.Mocks;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Abstractions.Repository
{
    /// <summary>
    /// Provides tests for the <see cref="Intelligence.Abstractions.Repository.RepositoryBase"/> abstract class.
    /// </summary>
    public class RepositoryBaseTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Initial_State_Contains_Default_Property_Values.
        /// </summary>
        public void Initial_State_Contains_Default_Property_Values()
        {
            MockRepositoryBase mock = new();

            Assert.False(mock.LastOperationSuccess);
            Assert.Null(mock.LastOperationError);
            Assert.Equal(0, mock.QueriesRunning);
            Assert.False(mock.HasExceptions);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for OnAsyncQueryStarted_Increments_Count_And_Raises_Event_With_Method_Name.
        /// </summary>
        public void OnAsyncQueryStarted_Increments_Count_And_Raises_Event_With_Method_Name()
        {
            MockRepositoryBase mock = new();
            string? methodName = null;

            mock.AsyncQueryStarted += (_, e) => methodName = e.Content;

            mock.InvokeOnAsyncQueryStarted("DoWorkAsync");

            Assert.Equal(1, mock.QueriesRunning);
            Assert.Equal("DoWorkAsync", methodName);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for OnAsyncQueryCompleted_Decrements_Count_And_Raises_Event_With_Method_Name.
        /// </summary>
        public void OnAsyncQueryCompleted_Decrements_Count_And_Raises_Event_With_Method_Name()
        {
            MockRepositoryBase mock = new();
            string? methodName = null;

            mock.AsyncQueryStarted += (_, _) => { };
            mock.AsyncQueryCompleted += (_, e) => methodName = e.Content;

            mock.InvokeOnAsyncQueryStarted("LoadAsync");
            mock.InvokeOnAsyncQueryCompleted("LoadAsync");

            Assert.Equal(0, mock.QueriesRunning);
            Assert.Equal("LoadAsync", methodName);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for OnAsyncQueryCompleted_Does_Not_Allow_Negative_Query_Count.
        /// </summary>
        public void OnAsyncQueryCompleted_Does_Not_Allow_Negative_Query_Count()
        {
            MockRepositoryBase mock = new();

            mock.InvokeOnAsyncQueryCompleted("SaveAsync");

            Assert.Equal(0, mock.QueriesRunning);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for OnAsyncQueryStarted_When_Handler_Throws_Logs_Error_And_Does_Not_Throw.
        /// </summary>
        public void OnAsyncQueryStarted_When_Handler_Throws_Logs_Error_And_Does_Not_Throw()
        {
            TestLogger logger = new();
            MockRepositoryBase mock = new(logger);

            mock.AsyncQueryStarted += (_, _) => throw new InvalidOperationException("handler failed");

            Exception? thrown = Record.Exception(() => mock.InvokeOnAsyncQueryStarted("GetData"));

            Assert.Null(thrown);
            Assert.Contains(logger.Entries, entry =>
                entry.Level == LogLevel.Error &&
                entry.Message.Contains("AsyncQueryStarted", StringComparison.Ordinal));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for OnAsyncQueryCompleted_When_Handler_Throws_Logs_Error_And_Does_Not_Throw.
        /// </summary>
        public void OnAsyncQueryCompleted_When_Handler_Throws_Logs_Error_And_Does_Not_Throw()
        {
            TestLogger logger = new();
            MockRepositoryBase mock = new(logger);

            mock.AsyncQueryCompleted += (_, _) => throw new InvalidOperationException("handler failed");

            Exception? thrown = Record.Exception(() => mock.InvokeOnAsyncQueryCompleted("GetData"));

            Assert.Null(thrown);
            Assert.Contains(logger.Entries, entry =>
                entry.Level == LogLevel.Error &&
                entry.Message.Contains("AsyncQueryCompleted", StringComparison.Ordinal));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for RecordException_Adds_Exception_Sets_LastOperationError_And_Logs.
        /// </summary>
        public void RecordException_Adds_Exception_Sets_LastOperationError_And_Logs()
        {
            TestLogger logger = new();
            MockRepositoryBase mock = new(logger);
            InvalidOperationException ex = new("repository failure");

            mock.RecordException(ex);

            Assert.True(mock.HasExceptions);
            Assert.Same(ex, mock.FirstException);
            Assert.Equal("repository failure", mock.LastOperationError);
            Assert.Contains(logger.Entries, entry =>
                entry.Level == LogLevel.Error &&
                entry.Exception == ex &&
                entry.Message == "repository failure");
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CancelToken_Throws_ObjectDisposedException_After_Dispose.
        /// </summary>
        public void CancelToken_Throws_ObjectDisposedException_After_Dispose()
        {
            MockRepositoryBase mock = new();

            mock.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _ = mock.CancelToken);
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
    }
}