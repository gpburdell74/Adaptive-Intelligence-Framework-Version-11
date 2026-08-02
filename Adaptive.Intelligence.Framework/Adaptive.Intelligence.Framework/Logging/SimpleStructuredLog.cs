using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Logging
{
    /// <summary>
    /// Provides a logging mechanism that captures structured log data and tracks exceptions in a simple manner.
    /// </summary>
    public class SimpleStructuredLog : ExceptionTrackingBase, ILogger<SimpleStructuredLog>
    {
        #region Private Member Declarations
        /// <summary>
        /// The collection in which to store the structured log records. This collection is used to maintain a record of all structured log entries made during the execution of the application.
        /// </summary>
        private StructuredLogRecordCollection? _recordCollection;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleStructuredLog"/> class. This constructor sets up the necessary components for structured logging and exception tracking.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public SimpleStructuredLog()
        {
            _recordCollection = [];
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="SimpleStructuredLog"/> class. This method is called when the object is no longer needed, and it ensures that any resources held by the object are released properly.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _recordCollection?.Clear();
            }

            _recordCollection = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Logger Methods
        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to associate with the scope.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// A <see cref="NullScope"/> instance, since this logger does not support scopes. The returned <see cref="NullScope"/> 
        /// instance is a no-op and does not perform any operations.
        /// </returns>        
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        /// <summary>
        /// Checks if the given log level is enabled. This method is used to determine whether a specific log level is currently active and should be logged. If the log level is enabled, the logger will process and record the log entry; otherwise, it will ignore it.
        /// </summary>
        /// <param name="logLevel">The log level to check.</param>
        /// <returns>True if the log level is enabled; otherwise, false.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// Logs a structured message with the specified log level, event ID, state, exception, and formatter. This method captures the log entry and stores it in the structured log record collection for later retrieval or analysis.
        /// </summary>
        /// <typeparam name="TState">The type of the state to associate with the log entry.</typeparam>
        /// <param name="logLevel">The log level of the log entry.</param>
        /// <param name="eventId">The event ID associated with the log entry.</param>
        /// <param name="state">The state to associate with the log entry.</param>
        /// <param name="exception">The exception to associate with the log entry, if any.</param>
        /// <param name="formatter">The formatter function to format the log entry.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (_recordCollection != null)
            {
                SimpleLogRecord newRecord = new()
                {
                    LogDate = DateTime.UtcNow,
                    LogEvent = eventId,
                };
                if (exception != null)
                {
                    newRecord.Exception = exception;
                    newRecord.ErrorMessage = exception.Message;
                }

                switch (logLevel)
                {
                    case LogLevel.Critical:
                    case LogLevel.Error:
                        newRecord.IsSuccess = false;
                        newRecord.IsError = true;
                        if (exception != null)
                        {
                            newRecord.ErrorMessage = formatter(state, exception);
                        }
                        break;

                    case LogLevel.Information:
                    case LogLevel.Debug:
                    case LogLevel.Warning:
                        newRecord.IsSuccess = true;
                        newRecord.IsError = false;
                        newRecord.InformationMessage = state?.ToString() ?? string.Empty;
                        break;
                }
                _recordCollection.Add(newRecord);
            }
        }
        #endregion
    }
}