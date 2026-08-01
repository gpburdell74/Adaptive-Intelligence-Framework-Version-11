using Adaptive.Intelligence.Common.Abstractions.Logging;
using System.Text.Json;

namespace Adaptive.Intelligence.Logging;

/// <summary>
/// Provides an implementaton for a simple log record.
/// </summary>
public record SimpleLogRecord : StructuredLogRecordBase
{
    /// <summary>
    /// Creates a new instance of the <see cref="SimpleLogRecord"/> class.
    /// </summary>
    /// <param name="jsonText">
    /// A string containing the text representation of a log record in JSON format.
    /// </param>
    /// <returns>
    /// The deserialized <see cref="SimpleLogRecord"/> instance, or null if the input is null or whitespace, or if deserialization fails.
    /// </returns>
    public static SimpleLogRecord? FromJson(string? jsonText)
    {
        SimpleLogRecord? record = null;

        if (!string.IsNullOrWhiteSpace(jsonText))
        {
            try
            {
                record = JsonSerializer.Deserialize<SimpleLogRecord>(jsonText);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Deserialization of a log record failed.", ex);
            }
        }
        return record;
    }

    /// <summary>
    /// Converts the current log record to a JSON string representation.
    /// </summary>
    /// <returns>
    /// A string containing the JSON representation of the log record, or null if the conversion fails.
    /// </returns>
    public string? ToJson()
    {
        string? logData = null;
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IndentSize = 4
        };
        try
        {
            logData = JsonSerializer.Serialize(this, options);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError("Serialization of a log record failed.", ex);
        }
        return logData;
    }
}
