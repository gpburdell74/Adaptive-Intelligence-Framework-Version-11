namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs because the header data for a CSV file could
/// not be read successfully.
/// </summary>
public sealed class ReadHeaderException : CsvException
{
    private const string ErrorMessage = "Could not read the header data from the CSV file.";

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadHeaderException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public ReadHeaderException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadHeaderException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public ReadHeaderException(Exception innerException) : base(ErrorMessage, innerException)
    {

    }
}
