namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs when the data in a CSV file could not be parsed and/or the
/// parsing process caused an error.
/// </summary>
public sealed class InvalidCsvDataException : CsvException
{
    private const string ErrorMessage = "Could not parse the data content as a CSV line of text.";

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCsvDataException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public InvalidCsvDataException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCsvDataException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public InvalidCsvDataException(Exception innerException) : base(ErrorMessage, innerException)
    {

    }
}
