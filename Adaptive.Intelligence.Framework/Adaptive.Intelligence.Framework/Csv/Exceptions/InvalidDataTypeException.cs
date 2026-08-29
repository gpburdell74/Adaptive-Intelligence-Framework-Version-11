namespace Adaptive.Intelligence.Csv.Exceptions;

/// <summary>
/// Represents an exception when an invalid .NET data type is specified.
/// </summary>
/// <seealso cref="Exception" />
public sealed class InvalidDataTypeException : Exception
{
    private const string ErrorMessage = "An invalid or unrecognized or unconvertable data type was specified.";

    /// <summary>
    /// Initializes a new instance of the <see cref="NullStreamException"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    /// <param name="conversionError">
    /// An optional parameter indicating whether the exception was a result of a data conversion operation.
    /// </param>
    public InvalidDataTypeException(bool conversionError = false) : this(ErrorMessage, null, null, conversionError)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidDataTypeException"/> class.
    /// </summary>
    /// <param name="specifiedType">
    /// The specified <see cref="Type"/> that caused the exception.
    /// </param>
    /// <param name="conversionError">
    /// An optional parameter indicating whether the exception was a result of a data conversion operation.
    /// </param>
    public InvalidDataTypeException(Type specifiedType, bool conversionError = false)
    {
        DataType = specifiedType;
        ConversionError = conversionError;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="NullStreamException"/> class.
    /// </summary>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    /// <param name="conversionError">
    /// An optional parameter indicating whether the exception was a result of a data conversion operation.
    /// </param>
    public InvalidDataTypeException(Exception innerException, bool conversionError = false)
        : this(ErrorMessage, null, innerException, conversionError)
    {

    }
    /// <summary>
    /// Initializes a new instance of the <see cref="NullStreamException"/> class.
    /// </summary>
    /// <param name="message">
    /// A string containing the message for the exception.
    /// </param>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    public InvalidDataTypeException(string message, Exception innerException) : this(message, null, innerException, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NullStreamException"/> class.
    /// </summary>
    /// <param name="message">
    /// A string containing the message for the exception.
    /// </param>
    /// <param name="dataType">
    /// The <see cref="Type"/> that was specified that caused the exception.
    /// </param>
    /// <param name="innerException">
    /// The <see cref="Exception"/> to be stored as the inner exception.
    /// </param>
    /// <param name="conversionError">
    /// An optional parameter indicating whether the exception was a result of a data conversion operation.
    /// </param>
    public InvalidDataTypeException(string message, Type? dataType, Exception? innerException, bool conversionError = false)
        : base(message, innerException)
    {
        DataType = dataType;
        ConversionError = conversionError;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the exception was a result of a data conversion operation
    /// </summary>
    /// <value>
    ///   <c>true</c> if the exception was thrown during a conversion operation; otherwise, <c>false</c>.
    /// </value>
    public bool ConversionError { get; init; }

    /// <summary>
    /// Gets the data type that caused the exception.
    /// </summary>
    /// <value>
    /// A <see cref="Type"/> instance, or <b>null</b>.
    /// </value>
    public Type? DataType { get; init; }

    /// <summary>
    /// Returns a string that represents the exception.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        string text = string.Empty;
        if (string.IsNullOrEmpty(Message))
        {
            if (InnerException == null)
            {
                text = ErrorMessage;
            }
            else
            {
                text = InnerException.Message;
            }
        }
        if (ConversionError)
        {
            text += ": This is the result of attempting to convert one data type to another.  See the DataType property.";
        }
        if (DataType != null)
        {
            text += ": Data Type Specified: " + DataType.ToString();
        }
        return text;
    }
}
