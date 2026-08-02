using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Abstractions.Logging
{
    /// <summary>
    /// Provides a base definition for a class that can log information and exceptions.
    /// </summary>
    public abstract class LoggableBase : PropertyAwareBase
    {
        #region Private Member Declarations
        /// <summary>
        /// The logger instance to use.
        /// </summary>
        private ILogger? _logger;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableBase"/> class.
        /// </summary>
        protected LoggableBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableBase"/> class with a logger.
        /// </summary>
        /// <param name="logger">
        /// The reference to the logger instance to use.
        /// </param>
        protected LoggableBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="LoggableBase"/> class.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether the method is being called from a Dispose method (true) or from a finalizer (false).
        /// </param>
        protected override void Dispose(bool disposing)
        {
            _logger = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Log Methods
        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="message">
        /// A string containing the critical message to write.
        /// </param>
        protected void LogCritical(string message)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical("{ExceptionMessage}", message);
            }
        }

        /// <summary>
        /// Formats and writes a critical exception.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> instance to be logged.
        /// </param>
        protected void LogCritical(Exception ex)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(ex, "{ExceptionMessage}", ex.Message);
            }
        }

        /// <summary>
        /// Formats and writes a critical message and exception.
        /// </summary>
        /// <param name="message">
        /// A string containing the critical message to write.
        /// </param>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> instance to be logged.
        /// </param>
        protected void LogCritical(Exception ex, string message)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(ex, "{Message}", message);
            }
        }

        /// <summary>
        /// Formats and writes a debug message.
        /// </summary>
        /// <param name="debugMessage">
        /// A string containing the debug message.
        /// </param>
        protected void LogDebug(string debugMessage)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("{DebugMessage}", debugMessage);
            }
        }

        /// <summary>
        /// Formats and writes a debug message.
        /// </summary>
        /// <param name="eventId">
        /// An <see cref="EventId"/> structure specifying the event ID and name.
        /// </param>
        /// <param name="debugMessage">
        /// A string containing the debug message.
        /// </param>
        protected void LogDebug(EventId eventId, string debugMessage)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Debug))
            {
                _logger?.LogDebug(eventId, "{DebugMessage}", debugMessage);
            }
        }

        /// <summary>
        /// Formats and writes a debug message.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to be recorded.
        /// </param>
        protected void LogDebug(Exception ex)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Debug))
            {
                _logger?.LogDebug(ex, "{ExceptionMessage}", ex.Message);
            }
        }

        /// <summary>
        /// Formats and writes a debug message.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to be recorded.
        /// </param>
        /// <param name="debugMessage">
        /// A string containing the debug message.
        /// </param>
        protected void LogDebug(Exception ex, string debugMessage)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Debug))
            {
                _logger?.LogDebug(ex, "{DebugMessage}", debugMessage);
            }
        }

        /// <summary>
        /// Formats and writes a debug message.
        /// </summary>
        /// <param name="eventId">
        /// An <see cref="EventId"/> structure specifying the event ID and name.
        /// </param>
        /// <param name="ex">
        /// The <see cref="Exception"/> to be recorded.
        /// </param>
        /// <param name="debugMessage">
        /// A string containing the debug message.
        /// </param>
        protected void LogDebug(EventId eventId, Exception ex, string debugMessage)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Debug))
            {
                _logger?.LogDebug(eventId, ex, "{DebugMessage}", debugMessage);
            }
        }

        /// <summary>
        /// Formats and writes an error message.
        /// </summary>
        /// <param name="errorMessage">
        /// A string containing the error message.
        /// </param>
        protected void LogError(string errorMessage)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Error))
            {
                _logger?.LogError("{ErrorMessage}", errorMessage);
            }
        }

        /// <summary>
        /// Formats and writes an error message.
        /// </summary>
        /// <param name="eventId">
        /// An <see cref="EventId"/> structure specifying the event ID and name.
        /// </param>
        /// <param name="errorMessage">
        /// A string containing the error message.
        /// </param>
        protected void LogError(EventId eventId, string errorMessage)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Error))
            {
                _logger?.LogError(eventId, "{ErrorMessage}", errorMessage);
            }
        }

        /// <summary>
        /// Formats and writes an error message.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to be recorded.
        /// </param>
        protected void LogError(Exception ex)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Error))
            {
                _logger?.LogError(ex, "{ExceptionMessage}", ex.Message);
            }
        }

        /// <summary>
        /// Formats and writes an error message.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to be recorded.
        /// </param>
        /// <param name="errorMessage">
        /// A string containing the error message.
        /// </param>
        protected void LogError(Exception ex, string errorMessage)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
            }
        }

        /// <summary>
        /// Formats and writes an error message.
        /// </summary>
        /// <param name="eventId">
        /// An <see cref="EventId"/> structure specifying the event ID and name.
        /// </param>
        /// <param name="ex">
        /// The <see cref="Exception"/> to be recorded.
        /// </param>
        /// <param name="errorMessage">
        /// A string containing the error message.
        /// </param>
        protected void LogError(EventId eventId, Exception ex, string errorMessage)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(eventId, ex, "{ErrorMessage}", errorMessage);
            }
        }

        /// <summary>
        /// Formats and writes an informational message.
        /// </summary>
        /// <param name="message">
        /// A string containing the informational message.
        /// </param>
        protected void LogInformation(string message)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Information))
            {
                _logger?.LogInformation("{Message}", message);
            }
        }

        /// <summary>
        /// Formats and writes an warning message.
        /// </summary>
        /// <param name="message">
        /// A string containing the warning message.
        /// </param>
        protected void LogWarning(string message)
        {
            if (_logger != null && _logger.IsEnabled(LogLevel.Warning))
            {
                _logger?.LogWarning("{Message}", message);
            }
        }
        #endregion
    }
}