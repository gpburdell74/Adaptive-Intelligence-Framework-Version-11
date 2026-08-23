namespace Adaptive.Intelligence.Logging;

/// <summary>
/// Contains the structure for the <see cref="AdaptiveDebug"/>  structured log.
/// </summary>
internal sealed class AdaptiveDebugLogRecord
{
    /// <summary>
    /// Gets or sets the string specifying the date and time of the log entry.
    /// </summary>
    /// <value>
    /// A string containing the value to be written.
    /// </value>
    public required string DateTime { get; set; }

    /// <summary>
    /// Gets or sets the string specifying the time in ticks since the last logging call.
    /// </summary>
    /// <value>
    /// A string containing the value to be written, or <see cref="string.Empty"/> if this 
    /// field is not used.
    /// </value>
    public string ElapsedTime { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the string specifying the current debug mode, if specified.
    /// </summary>
    /// <value>
    /// A string containing the value to be written.
    /// </value>
    public required string Mode { get; set; }


    /// <summary>
    /// Gets or sets the string specifying the ID of the executing thread when the log item was written.
    /// </summary>
    /// <value>
    /// A string containing the value to be written.
    /// </value>
    public required string ThreadId { get; set; }

    /// <summary>
    /// Gets or sets the string specifying the content of the log entry.
    /// </summary>
    /// <value>
    /// A string containing the value to be written.
    /// </value>
    public required string LogText { get; set; }
}