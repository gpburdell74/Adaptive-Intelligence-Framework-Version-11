namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs because a writer for the supplied <see cref="Stream"/> instance 
/// could not be created.
/// </summary>
public sealed class CreateWriterFailedException : CsvException
{
    private const string ErrorMessage = "Failed to create the stream writer instance.";

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateWriterFailedException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public CreateWriterFailedException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateWriterFailedException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public CreateWriterFailedException(Exception innerException) : base(ErrorMessage, innerException)
    {

    }
}
