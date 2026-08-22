namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs because a reader for the supplied <see cref="Stream"/> instance 
/// could not be created.
/// </summary>
public sealed class CreateReaderFailedException : CsvException
{
    private const string ErrorMessage = "Failed to create the stream reader instance.";

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateReaderFailedException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public CreateReaderFailedException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateReaderFailedException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public CreateReaderFailedException(Exception innerException) : base(ErrorMessage, innerException)
    {

    }
}
