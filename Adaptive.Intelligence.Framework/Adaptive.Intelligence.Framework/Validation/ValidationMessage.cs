using Adaptive.Intelligence.Common.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Adaptive.Intelligence.Validation;

/// <summary>
/// Represents and manages a validation message.
/// </summary>
/// <seealso cref="DisposableObjectBase" />
public class ValidationMessage : ValidationResult, IDisposable
{
    /// <summary>
    /// The default success validation instance.
    /// </summary>
    public static readonly ValidationMessage DefaultSuccess = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    public ValidationMessage() : base(string.Empty)
    {
        IsValid = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor overload assumes the level of error.
    /// </remarks>
    /// <param name="message">
    /// A string containing the validation message.
    /// </param>
    public ValidationMessage(string message) : base(message)
    {
        ErrorMessage = message;
        IsValid = false;
        Level = ValidationLevel.Error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
    /// </summary>
    /// <remarks>
    /// This overload will set the <see cref="IsValid"/> property to <b>true</b>.
    /// </remarks>
    /// <param name="message">
    /// A string containing the validation message.
    /// </param>
    /// <param name="level">
    /// A <see cref="ValidationLevel"/> enumerated value indicating the level of validation.
    /// </param>
    public ValidationMessage(string message, ValidationLevel level) : base(message)
    {
        ErrorMessage = message;
        Level = level;
        IsValid = (level != ValidationLevel.Error);        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
    /// </summary>
    /// <param name="message">
    /// A string containing the validation message.
    /// </param>
    /// <param name="level">
    /// A <see cref="ValidationLevel"/> enumerated value indicating the level of validation.
    /// </param>
    /// <param name="isValid">
    /// A value indicating whether the overall condition being investigated is to be considered valid.
    /// </param>
    public ValidationMessage(string message, ValidationLevel level, bool isValid) : base(message)
    {
        ErrorMessage = message;
        IsValid = isValid;
        Level = level;
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    public void Dispose()
    {
        Level = ValidationLevel.NoneOrNotSpecified;
        ErrorMessage = null;
        PropertyName = null;
        Tag = null;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the overall operation is to be considered valid,
    /// even if a message is attached and the <see cref="Level"/> property is not <see cref="ValidationLevel.SuccessInformational"/>
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance's related field data is valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the validation level.
    /// </summary>
    /// <value>
    /// A <see cref="ValidationLevel"/> enumerated value indicating the severity of the validation message.
    /// </value>
    public ValidationLevel Level { get; set; } = ValidationLevel.NoneOrNotSpecified;

    /// <summary>
    /// Gets or sets the name of the related property or field.
    /// </summary>
    /// <value>
    /// A string containing the name of the related field or property being evaluated, 
    /// or <b>null</b>.
    /// </value>
    public string? PropertyName { get; set; }

    /// <summary>
    /// Gets or sets a reference to another object.
    /// </summary>
    /// <value>
    /// A reference to another object instance to be used later, or <b>null</b>.
    /// </value>
    public object? Tag { get; set; }
}
