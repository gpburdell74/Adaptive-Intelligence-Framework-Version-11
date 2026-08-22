using Adaptive.Intelligence.Csv.Parser;

namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs because an unexpected error occured when parsing CSV content.
/// </summary>
public sealed class ParserEngineException : CsvException
{
    private const string ErrorMessage = "The CSV parser instance was null.";

    /// <summary>
    /// Initializes a new instance of the <see cref="ParserEngineException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public ParserEngineException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParserEngineException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public ParserEngineException(Exception innerException) : base(ErrorMessage, innerException)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParserEngineException"/> class.
    /// </summary>
    /// <param name="message">
    /// A string containing a description of the error.
    /// </param>
    public ParserEngineException(string message) : base(message)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParserEngineException"/> class.
    /// </summary>
    /// <param name="message">
    /// A string containing a description of the error.
    /// </param>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public ParserEngineException(string message, Exception innerException) : base(message, innerException)
    {

    }

    /// <summary>
    /// Throws a <see cref="ParserEngineException"/> if the provided <paramref name="parser"/> is null.
    /// </summary>
    /// <param name="parser"></param>
    /// <exception cref="ParserEngineException"></exception>
    public static void ThrowIfNull(ICsvParser? parser)
    {
        if (parser is null)
        {
            throw new ParserEngineException();
        }
    }
}
