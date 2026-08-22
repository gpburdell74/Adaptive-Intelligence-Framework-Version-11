namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Provides the base definition for CSV-related exceptions.
/// </summary>
public class CsvException : Exception
{
    private const string ErrorGeneric = "An error occurred in the CSV processor.";

    /// <summary>
    /// Creates a new instance of the <see cref="CsvException"/> class.
    /// </summary>
    public CsvException() : base(ErrorGeneric)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CsvException"/> class.
    /// </summary>
    /// <param name="message">
    /// A string containing the message for the exception.
    /// </param>
    public CsvException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CsvException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be wrapped as the inner exception.
    /// </param>
    public CsvException(Exception innerException) : base(ErrorGeneric, innerException)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CsvException"/> class.
    /// </summary>
    /// <param name="message">
    /// A string containing the message for the exception.
    /// </param>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be wrapped as the inner exception.
    /// </param>
    public CsvException(string message, Exception innerException) : base(message, innerException)
    {
    }
}