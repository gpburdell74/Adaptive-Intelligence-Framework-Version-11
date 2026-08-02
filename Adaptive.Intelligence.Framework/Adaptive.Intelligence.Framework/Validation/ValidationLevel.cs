namespace Adaptive.Intelligence.Validation
{
    /// <summary>
    /// Lists the validation levels that are currently supported.
    /// </summary>
    public enum ValidationLevel
    {
        /// <summary>
        /// Indicates no level has been specified.
        /// </summary>
        NoneOrNotSpecified = 0,
        /// <summary>
        /// Indicates success with an attached message.
        /// </summary>
        SuccessInformational = 1,
        /// <summary>
        /// Indicates informational only level validation messages.
        /// </summary>
        Informational = 2,
        /// <summary>
        /// Indicates a warning message.
        /// </summary>
        Warning = 3,
        /// <summary>
        /// Indicates a validation error.
        /// </summary>
        Error = 4
    }
}