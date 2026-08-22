namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs because the <see cref="Stream"/> cannot be read from.
/// </summary>
public sealed class CantReadStreamException : CsvException
{
    private const string ErrorMessage = "The stream cannot be read from.";

    /// <summary>
    /// Initializes a new instance of the <see cref="CantReadStreamException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public CantReadStreamException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CantReadStreamException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public CantReadStreamException(Exception innerException) : base(ErrorMessage, innerException)
    {

    }

    /// <summary>
    /// Throws a <see cref="CantReadStreamException"/> if the provided <see cref="Stream"/> cannot be read from.
    /// </summary>
    /// <param name="stream"></param>
    /// <exception cref="CantReadStreamException"></exception>
    public static void ThrowIfStreamCannotBeRead(Stream stream)
    {
        if (!stream.CanRead)
        {
            throw new CantReadStreamException();
        }
    }
}
