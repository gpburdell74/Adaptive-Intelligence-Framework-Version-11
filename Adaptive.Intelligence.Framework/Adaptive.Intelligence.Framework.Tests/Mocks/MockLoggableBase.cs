using Adaptive.Intelligence.Common.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Mocks;

/// <summary>
/// Provides a testable wrapper for the <see cref="LoggableBase"/> abstract class.
/// </summary>
public sealed class MockLoggableBase : LoggableBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MockLoggableBase"/> class.
    /// </summary>
    public MockLoggableBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MockLoggableBase"/> class with a logger instance.
    /// </summary>
    /// <param name="logger">
    /// The reference to the logger instance to use.
    /// </param>
    public MockLoggableBase(ILogger logger)
        : base(logger)
    {
    }

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogCritical(string)"/> method.
    /// </summary>
    /// <param name="message">
    /// A string containing the critical message to write.
    /// </param>
    public void InvokeLogCritical(string message) => LogCritical(message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogCritical(Exception)"/> method.
    /// </summary>
    /// <param name="ex">
    /// The reference to the <see cref="Exception"/> instance to be logged.
    /// </param>
    public void InvokeLogCritical(Exception ex) => LogCritical(ex);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogCritical(Exception, string)"/> method.
    /// </summary>
    /// <param name="ex">
    /// The reference to the <see cref="Exception"/> instance to be logged.
    /// </param>
    /// <param name="message">
    /// A string containing the critical message to write.
    /// </param>
    public void InvokeLogCritical(Exception ex, string message) => LogCritical(ex, message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogDebug(string)"/> method.
    /// </summary>
    /// <param name="message">
    /// A string containing the debug message.
    /// </param>
    public void InvokeLogDebug(string message) => LogDebug(message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogDebug(EventId, string)"/> method.
    /// </summary>
    /// <param name="eventId">
    /// An <see cref="EventId"/> structure specifying the event ID and name.
    /// </param>
    /// <param name="message">
    /// A string containing the debug message.
    /// </param>
    public void InvokeLogDebug(EventId eventId, string message) => LogDebug(eventId, message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogDebug(Exception)"/> method.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> to be recorded.
    /// </param>
    public void InvokeLogDebug(Exception ex) => LogDebug(ex);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogDebug(Exception, string)"/> method.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> to be recorded.
    /// </param>
    /// <param name="message">
    /// A string containing the debug message.
    /// </param>
    public void InvokeLogDebug(Exception ex, string message) => LogDebug(ex, message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogDebug(EventId, Exception, string)"/> method.
    /// </summary>
    /// <param name="eventId">
    /// An <see cref="EventId"/> structure specifying the event ID and name.
    /// </param>
    /// <param name="ex">
    /// The <see cref="Exception"/> to be recorded.
    /// </param>
    /// <param name="message">
    /// A string containing the debug message.
    /// </param>
    public void InvokeLogDebug(EventId eventId, Exception ex, string message) => LogDebug(eventId, ex, message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogError(string)"/> method.
    /// </summary>
    /// <param name="message">
    /// A string containing the error message.
    /// </param>
    public void InvokeLogError(string message) => LogError(message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogError(EventId, string)"/> method.
    /// </summary>
    /// <param name="eventId">
    /// An <see cref="EventId"/> structure specifying the event ID and name.
    /// </param>
    /// <param name="message">
    /// A string containing the error message.
    /// </param>
    public void InvokeLogError(EventId eventId, string message) => LogError(eventId, message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogError(Exception)"/> method.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> to be recorded.
    /// </param>
    public void InvokeLogError(Exception ex) => LogError(ex);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogError(Exception, string)"/> method.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> to be recorded.
    /// </param>
    /// <param name="message">
    /// A string containing the error message.
    /// </param>
    public void InvokeLogError(Exception ex, string message) => LogError(ex, message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogError(EventId, Exception, string)"/> method.
    /// </summary>
    /// <param name="eventId">
    /// An <see cref="EventId"/> structure specifying the event ID and name.
    /// </param>
    /// <param name="ex">
    /// The <see cref="Exception"/> to be recorded.
    /// </param>
    /// <param name="message">
    /// A string containing the error message.
    /// </param>
    public void InvokeLogError(EventId eventId, Exception ex, string message) => LogError(eventId, ex, message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogInformation(string)"/> method.
    /// </summary>
    /// <param name="message">
    /// A string containing the informational message.
    /// </param>
    public void InvokeLogInformation(string message) => LogInformation(message);

    /// <summary>
    /// Invokes the protected <see cref="LoggableBase.LogWarning(string)"/> method.
    /// </summary>
    /// <param name="message">
    /// A string containing the warning message.
    /// </param>
    public void InvokeLogWarning(string message) => LogWarning(message);
}