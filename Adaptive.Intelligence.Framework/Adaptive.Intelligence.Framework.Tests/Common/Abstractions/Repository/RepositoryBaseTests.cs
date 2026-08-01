using Adaptive.Intelligence.Framework.Tests.Mocks;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Common.Abstractions.Repository;

/// <summary>
/// Provides tests for the <see cref="Adaptive.Intelligence.Common.Abstractions.Repository.RepositoryBase"/> abstract class.
/// </summary>
public class RepositoryBaseTests
{
    [Fact]
    public void Initial_State_Contains_Default_Property_Values()
    {
        MockRepositoryBase mock = new();

        Assert.False(mock.LastOperationSuccess);
        Assert.Null(mock.LastOperationError);
        Assert.Equal(0, mock.QueriesRunning);
        Assert.False(mock.HasExceptions);
    }

    [Fact]
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
    public void OnAsyncQueryCompleted_Does_Not_Allow_Negative_Query_Count()
    {
        MockRepositoryBase mock = new();

        mock.InvokeOnAsyncQueryCompleted("SaveAsync");

        Assert.Equal(0, mock.QueriesRunning);
    }

    [Fact]
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
    public void CancelToken_Throws_ObjectDisposedException_After_Dispose()
    {
        MockRepositoryBase mock = new();

        mock.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = mock.CancelToken);
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
}
