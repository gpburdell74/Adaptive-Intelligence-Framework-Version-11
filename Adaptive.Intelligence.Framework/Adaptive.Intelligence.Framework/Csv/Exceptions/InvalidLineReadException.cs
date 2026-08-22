namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs because a failure occurred when reading a line from a CSV file.
/// </summary>
public sealed class InvalidLineReadException : CsvException
{
    private const string ErrorMessage = "Invalid read from the CSV file.  See the inner exception for detail.";

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidLineReadException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public InvalidLineReadException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidLineReadException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public InvalidLineReadException(Exception innerException) : base(ErrorMessage, innerException)
    {
    }
}
