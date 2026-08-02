using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;

namespace Adaptive.Intelligence.Converters
{
    /// <summary>
    /// Provides a converter class for converting boolean values to and from various human readable
    /// string values.
    /// </summary>
    /// <seealso cref="IValueConverter{F, T}" />
    public sealed class BooleanConverter : IValueConverter<bool, string>
    {
        /// <summary>
        /// Converts the original boolean value to a formatted string value.
        /// </summary>
        /// <param name="originalValue">The original value to be converted.</param>
        /// <returns>
        /// A string for display ("Yes" or "No").
        /// </returns>
        public string Convert(bool originalValue)
        {
            return originalValue ? BooleanConstants.TrueFormatted : BooleanConstants.FalseFormatted;
        }
        /// <summary>
        /// Converts the converted value to the original representation.
        /// </summary>
        /// <param name="convertedValue">The original string value to be converted.</param>
        /// <returns>
        /// <b>true</b> or <b>false</b> based on the parsed string value.
        /// </returns>
        /// <remarks>
        /// The implementation of this method is the inverse of
        /// the <see cref="Convert(bool)" /> method.
        /// </remarks>
        public bool ConvertBack(string convertedValue)
        {
            // Directly return true if the convertedValue matches any of the truthy constants.
            return string.Equals(convertedValue, BooleanConstants.TrueValueYes, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(convertedValue, BooleanConstants.TrueValueSi, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(convertedValue, BooleanConstants.TrueValueTrue, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(convertedValue, BooleanConstants.TrueValueBT, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(convertedValue, BooleanConstants.TrueValueBY, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(convertedValue, BooleanConstants.TrueValueMinus1, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(convertedValue, BooleanConstants.TrueValueOK, StringComparison.OrdinalIgnoreCase);
        }
    }
}