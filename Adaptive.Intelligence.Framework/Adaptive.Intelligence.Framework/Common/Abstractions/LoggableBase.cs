using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Common.Abstractions;

/// <summary>
/// Provides a base definition for a class that can log information and exceptions.
/// </summary>
public abstract class LoggableBase : DisposableObjectBase
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
        _logger?.LogCritical(message);
    }

    /// <summary>
    /// Formats and writes a critical exception.
    /// </summary>
    /// <param name="ex">
    /// The reference to the <see cref="Exception"/> instance to be logged.
    /// </param>
    protected void LogCritical(Exception ex)
    {
        _logger?.LogCritical(ex, ex.Message);
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
        _logger?.LogCritical(ex, message);
    }

    /// <summary>
    /// Formats and writes a debug message.
    /// </summary>
    /// <param name="debugMessage">
    /// A string containing the debug message.
    /// </param>
    protected void LogDebug(string debugMessage)
    {
        _logger?.LogDebug(debugMessage);
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
        _logger?.LogDebug(eventId, debugMessage);
    }

    /// <summary>
    /// Formats and writes a debug message.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> to be recorded.
    /// </param>
    protected void LogDebug(Exception ex)
    {
        _logger?.LogDebug(ex, ex.Message);
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
        _logger?.LogDebug(ex, debugMessage);
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
        _logger?.LogDebug(eventId, ex, debugMessage);
    }

    /// <summary>
    /// Formats and writes an error message.
    /// </summary>
    /// <param name="errorMessage">
    /// A string containing the error message.
    /// </param>
    protected void LogError(string errorMessage)
    {
        _logger?.LogError(errorMessage);
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
        _logger?.LogError(eventId, errorMessage);
    }

    /// <summary>
    /// Formats and writes an error message.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> to be recorded.
    /// </param>
    protected void LogError(Exception ex)
    {
        _logger?.LogError(ex, ex.Message);
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
        _logger?.LogError(ex, errorMessage);
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
        _logger?.LogError(eventId, ex, errorMessage);
    }

    /// <summary>
    /// Formats and writes an informational message.
    /// </summary>
    /// <param name="message"></param>
    protected void LogInformation(string message)
    {
        _logger?.LogInformation(message);
    }

    /// <summary>
    /// Formats and writes an warning message.
    /// </summary>
    /// <param name="message"></param>
    protected void LogWarning(string message)
    {
        _logger?.LogWarning(message);
    }
    #endregion
}

