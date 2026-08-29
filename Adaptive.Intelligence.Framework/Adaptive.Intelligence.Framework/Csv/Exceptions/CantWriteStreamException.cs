namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs because the <see cref="Stream"/> cannot be written to.
/// </summary>
public sealed class CantWriteStreamException : CsvException
{
    private const string ErrorMessage = "The stream cannot be written to.";

    private readonly string? _fileName;

    /// <summary>
    /// Initializes a new instance of the <see cref="CantWriteStreamException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public CantWriteStreamException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CantWriteStreamException"/> class.
    /// </summary>
    /// <param name="fileName">
    /// A string containing the name of the file, if provided.
    /// </param>
    public CantWriteStreamException(string? fileName) : base(ErrorMessage)
    {
        _fileName = fileName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CantWriteStreamException"/> class.
    /// </summary>
    /// <param name="fileName">
    /// A string containing the name of the file, if provided.
    /// </param>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public CantWriteStreamException(string? fileName, Exception innerException) : base(ErrorMessage, innerException)
    {
        _fileName = fileName;
    }

    /// <summary>
    /// Gets the name of the file, if provided.
    /// </summary>
    /// <value>
    /// A string containing the name of the file, or <b>null</b>.
    /// </value>
    public string? FileName => _fileName;

    /// <summary>
    /// Throws a <see cref="CantWriteStreamException"/> if the provided <see cref="Stream"/> is <b>null</b>.
    /// </summary>
    /// <param name="destinationStream"></param>
    /// <param name="fileName"></param>
    /// <exception cref="CantWriteStreamException"></exception>
    public static void ThrowIfStreamCantWrite(Stream? destinationStream, string? fileName = null)
    {
        if (destinationStream is null)
        {
            throw new CantWriteStreamException(fileName);
        }
    }
}
