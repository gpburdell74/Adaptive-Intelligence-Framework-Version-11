namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception that occurs because a <b>null</b> <see cref="Stream"/> instance was specified.
/// </summary>
public sealed class NullStreamException : CsvException
{
    private const string ErrorMessage = "No stream was supplied to read from.";
    private const string NullStreamMessage = "The provided stream reference was null.";

    /// <summary>
    /// Initializes a new instance of the <see cref="NullStreamException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public NullStreamException() : base(ErrorMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NullStreamException"/> class.
    /// </summary>
    /// <param name="message">
    /// A string containing the error description.
    /// </param>
    public NullStreamException(string message) : base(message)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NullStreamException"/> class.
    /// </summary>
    /// <param name="parameterName">
    /// A string containing the name of the relevant parameter that was null.
    /// </param>
    /// <param name="message">
    /// A string containing the error description.
    /// </param>
    public NullStreamException(string parameterName, string message) : base(message)
    {
        ParameterName = parameterName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NullStreamException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public NullStreamException(Exception innerException) : base(ErrorMessage, innerException)
    {

    }

    /// <summary>
    /// Gets the name of the parameter that was null, if provided.
    /// </summary>
    /// <value>
    /// A string containing the parameter name, if present; otherwise, <b>null</b>.
    /// </value>
    public string? ParameterName { get; init; }

    /// <summary>
    /// Provides a static ThrowIfNull operation for the instance.
    /// </summary>
    /// <param name="stream">
    /// The variable to be evaluated for nullity.
    /// </param>
    /// <param name="parameterName">
    /// A string containing the relevant parameter name.
    /// </param>
    /// <param name="message">
    /// A string containing the exception message text.
    /// </param>
    /// <exception cref="NullStreamException">
    /// Thrown if the <paramref name="stream"/> is <b>null</b>.
    /// </exception>
    public static void ThrowIfNull(Stream? stream, string? parameterName = null, string? message = null)
    {
        if (stream is null)
        {
            if (parameterName is null && message is null)
            {
                throw new NullStreamException(NullStreamMessage);
            }
            else if (parameterName is null && (message is not null))
            {
                throw new NullStreamException(message);
            }
            else
            {
                throw new NullStreamException(parameterName!, message!);
            }
        }
    }
}
