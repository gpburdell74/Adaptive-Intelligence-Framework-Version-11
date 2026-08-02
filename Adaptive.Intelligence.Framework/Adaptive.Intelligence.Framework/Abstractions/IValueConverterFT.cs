namespace Adaptive.Intelligence.Abstractions
{
    /// <summary>
    /// Provides a signature definition for types that convert one type to another.
    /// </summary>
    /// <remarks>
    /// This is generally used to convert enumerations to strings and back.
    /// </remarks>
    public interface IValueConverter<TFromType, TToType>
    {
        /// <summary>
        /// Converts the original value to another value.
        /// </summary>
        /// <param name="originalValue">
        /// The original value to be converted.
        /// </param>
        /// <returns>
        /// The <typeparamref name="TToType"/> converted value.
        /// </returns>
        TToType Convert(TFromType originalValue);

        /// <summary>
        /// Converts the converted value to the original representation.
        /// </summary>
        /// <remarks>
        /// The implementation of this method must be the inverse of the <see cref="Convert(TFromType)"/> method.
        /// </remarks>
        /// <param name="convertedValue">
        /// The original value to be converted.
        /// </param>
        /// <returns>
        /// The <typeparamref name="TFromType"/> value.
        /// </returns>
        TFromType ConvertBack(TToType convertedValue);
    }
}