using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Abstractions.Logging
{
    /// <summary>
    /// Provides the sigature definiton for basic classes that are used in structured logging.
    /// </summary>
    public interface ILogRecord
    {
        /// <summary>
        /// Gets or sets the date/time the record was created.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the date/time the record was created.
        /// </value>
        DateTime LogDate { get; set; }

        /// <summary>
        /// Gets or sets the event ID for the record.
        /// </summary>
        /// <value>
        /// A <see cref="EventId"/> value that represents the event ID for the record.
        /// </value>
        EventId? LogEvent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the record is an error.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> value that indicates whether the record is an error.
        /// </value>
        bool IsError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the record is an error.
        /// </summary>
        /// <value>
        /// A <see cref="bool"/> value that indicates whether the record is an error.
        /// </value>
        bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the logging level indicator for the record.
        /// </summary>
        /// <value>
        /// A <see cref="LogLevel"/> value that represents the logging level indicator for the record.
        /// </value>
        LogLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the error message for the record.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> value that represents the error message for the record.
        /// </value>
        string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the reference to the <see cref="Exception"/> for the record, if provided.
        /// </summary>
        /// <value>
        /// A <see cref="Exception"/> instance, or <b>null</b>.
        /// </value>
        Exception? Exception { get; set; }

        /// <summary>
        /// Gets or sets the information message for the record.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> value that represents the information message for the record, if provided.
        /// </value>
        string? InformationMessage { get; set; }

        /// <summary>
        /// Gets or sets the trace information for the record.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> value that represents the trace information for the record, if provided.
        /// </value>
        string? TraceInformation { get; set; }
    }
}