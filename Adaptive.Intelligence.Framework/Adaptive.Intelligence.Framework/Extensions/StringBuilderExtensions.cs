using Adaptive.Intelligence.Constants;
using System.Text;

namespace Adaptive.Intelligence.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="StringBuilder"/> class.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends a comma character to the end of the string.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="StringBuilder"/> instance being operated on.
        /// </param>
        public static void AppendComma(this StringBuilder builder)
        {
            builder.Append(CharacterConstants.CommaChar);
        }
        /// <summary>
        /// Appends a dot/period character to the end of the string.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="StringBuilder"/> instance being operated on.
        /// </param>
        public static void AppendDot(this StringBuilder builder)
        {
            builder.Append(CharacterConstants.DotChar);
        }
        /// <summary>
        /// Appends a space character to the end of the string.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="StringBuilder"/> instance being operated on.
        /// </param>
        public static void AppendSpace(this StringBuilder builder)
        {
            builder.Append(CharacterConstants.SpaceChar);
        }
        /// <summary>
        /// Appends a space character to the end of the string.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="StringBuilder"/> instance being operated on.
        /// </param>
        /// <param name="valueToAppend">
        /// A string containing the value to be appended.
        /// </param>
        public static void AppendWithPrecedingSpace(this StringBuilder builder, string valueToAppend)
        {
            builder.Append(CharacterConstants.SpaceChar);
            builder.Append(valueToAppend);
        }

    }
}