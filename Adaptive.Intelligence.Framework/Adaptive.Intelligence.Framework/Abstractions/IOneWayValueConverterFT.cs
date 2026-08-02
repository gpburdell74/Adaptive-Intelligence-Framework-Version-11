namespace Adaptive.Intelligence.Abstractions
{
    /// <summary>
    /// Provides a signature definition for types that convert one type to another, in
    /// a one-way direction.
    /// </summary>
    /// <remarks>
    /// This is generally used to convert enumerations to strings, with no reverse
    /// conversion.
    /// </remarks>
    public interface IOneWayValueConverter<in TFromType, out TToType>
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
    }
}
