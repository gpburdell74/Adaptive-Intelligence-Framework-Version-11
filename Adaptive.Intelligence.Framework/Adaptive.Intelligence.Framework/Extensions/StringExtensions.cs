using Adaptive.Intelligence.Utility;
using System.Globalization;

namespace Adaptive.Intelligence.Extensions
{
    /// <summary>
    /// Provides extension methods for string values.
    /// </summary>
    public static class StringExtensions
    {
        #region Private Member Declarations
        /// <summary>
        /// The dollar sign as a string.
        /// </summary>
        private const string DollarSign = "$";
        /// <summary>
        /// The plural ending for singular words that end in Y.
        /// </summary>
        private const string EndingsY = "ies";
        /// <summary>
        /// The plural ending for some words.
        /// </summary>
        private const string EndingsES = "es";
        /// <summary>
        /// The plural ending for other words.
        /// </summary>
        private const string EndingsS = "s";
        /// <summary>
        /// The singular ending for words that end in "SS".
        /// </summary>
        private const string EndingsSS = "ss";
        /// <summary>
        /// The endings exception strings.
        /// </summary>
        private const string EndingsExceptionAles = "ales";
        /// <summary>
        /// The endings exception strings.
        /// </summary>
        private const string EndingsExceptionIles = "iles";
        /// <summary>
        /// The endings exception strings.
        /// </summary>
        private const string EndingsExceptionUles = "ules";
        /// <summary>
        /// The endings exception strings.
        /// </summary>
        private const string EndingsExceptionRoutes = "routes";
        /// <summary>
        /// The endings exception strings.
        /// </summary>
        private const string EndingsExceptionTypes = "types";
        /// <summary>
        /// The endings exception strings.
        /// </summary>
        private const string EndingsExceptionPages = "pages";
        /// <summary>
        /// The endings exception strings.
        /// </summary>
        private const string EndingsExceptionPlates = "plates";
        #endregion

        #region Public Static String Extension Methods / Functions
        /// <summary>
        /// Provides a string extension method for removing US dollar signs from
        /// the string instance.
        /// </summary>
        /// <param name="s">
        /// The string being extended.
        /// </param>
        /// <returns>
        /// The modified string value.
        /// </returns>
        public static string CleanUpDollarText(this string s)
        {
            return s.Replace(DollarSign, string.Empty);
        }
        /// <summary>
        /// Finds the first non-numeric character in the string.
        /// </summary>
        /// <param name="original">
        /// The string instance.
        /// </param>
        /// <param name="isFloatingPoint">
        /// A value indicating whether to allow for a floating-point number.
        /// </param>
        /// <returns>
        /// An integer indicating the first position of a non-numeric character in the string, or 
        /// -1 if there are no characters to examine or a non-numeric character could not be found.
        /// </returns>
        public static int FindFirstNonNumericCharacter(this string original, bool isFloatingPoint)
        {
            // Allow one period for floating point numbers.
            bool dotFound = !isFloatingPoint;
            int index = 0;
            int position = -1;
            int length = original.Length;

            while (index < length && position == -1)
            {
                char charToExamine = original[index];

                // If the character is not a digit...
                if (!char.IsDigit(charToExamine))
                {
                    // If we have not yet encountered a period, treat the period as a number -
                    // unless <i>isFloatingPoint</i> is <b>false</b>.
                    if (charToExamine == '.' && !dotFound)
                    {
                        dotFound = true;
                    }
                    else
                    {
                        position = index;
                    }
                }
                index++;
            }

            return position;
        }
        /// <summary>
        /// Modifies the plural word back to its singular form.
        /// </summary>
        /// <param name="originalValue">The original value to be modified.</param>
        /// <returns>
        /// The English singular form of the provided word.
        /// </returns>
        public static string Singularize(this string originalValue)
        {
            string returnValue = originalValue;

            if (!string.IsNullOrEmpty(originalValue))
            {
                // Create the comparison string.
                string comparisonValue = originalValue.ToLower(CultureInfo.CurrentCulture).Trim();
                originalValue = originalValue.Trim();

                if (comparisonValue.EndsWith(EndingsY, StringComparison.CurrentCulture))
                {
                    // Ends with "ies".
                    returnValue = string.Concat(originalValue.AsSpan(0, comparisonValue.Length - 3), "y");
                }
                else if (comparisonValue.EndsWith(EndingsES, StringComparison.CurrentCulture))
                {
                    // Exclude the known exceptions.
                    if (comparisonValue.EndsWith(EndingsExceptionTypes, StringComparison.CurrentCulture) ||
                        comparisonValue.EndsWith(EndingsExceptionPages, StringComparison.CurrentCulture) ||
                        comparisonValue.EndsWith(EndingsExceptionRoutes, StringComparison.CurrentCulture) ||
                        comparisonValue.EndsWith(EndingsExceptionAles, StringComparison.CurrentCulture) ||
                        comparisonValue.EndsWith(EndingsExceptionIles, StringComparison.CurrentCulture) ||
                        comparisonValue.EndsWith(EndingsExceptionUles, StringComparison.CurrentCulture) ||
                        comparisonValue.EndsWith(EndingsExceptionPlates, StringComparison.CurrentCulture))
                    // In this case, just remove the "s".
                    {
                        returnValue = originalValue[..(comparisonValue.Length - 1)];
                    }
                    else
                    {
                        if (!originalValue.EndsWith(EndingsSS, StringComparison.CurrentCulture))
                        {
                            // Otherwise, remove the "es".
                            returnValue = originalValue[..(comparisonValue.Length - 2)];
                        }
                        else
                        {
                            returnValue = originalValue;
                        }
                    }
                }
                else if (!comparisonValue.EndsWith(EndingsSS, StringComparison.CurrentCulture) &&
                         comparisonValue.EndsWith(EndingsS, StringComparison.CurrentCulture))
                // Standard word - remove the "s"
                {
                    returnValue = originalValue[..(comparisonValue.Length - 1)];
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Modifies the plural word back to its singular form.
        /// </summary>
        /// <param name="originalValue">The original value to be modified.</param>
        /// <returns>
        /// The English singular form of the provided word.
        /// </returns>
        public static string Pluralize(this string originalValue)
        {
            string returnValue = originalValue;

            if (!string.IsNullOrEmpty(originalValue))
            {
                // Create the comparison string.
                string comparisonValue = originalValue.ToLowerInvariant().Trim();
                originalValue = originalValue.Trim();

                if (comparisonValue.EndsWith('y'))
                {
                    // Determine if just adding "s" is needed...
                    bool endsInS = YWordEndsInS(comparisonValue);
                    if (endsInS)
                    {
                        returnValue = originalValue + EndingsS;
                    }
                    else
                    {
                        // Remove Y and ends with "ies".
                        returnValue = string.Concat(originalValue.AsSpan(0, originalValue.Length - 1), EndingsY);
                    }
                }
                else if (comparisonValue.EndsWith('e'))
                {
                    returnValue = originalValue + EndingsS;
                }
                else
                {
                    returnValue = originalValue + EndingsS;
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Capitalizes the First Letter of Each Word.
        /// </summary>
        /// <param name="s">
        /// The reference to the string to be modified.
        /// </param>
        /// <returns>
        /// The modified string with each work capitalized.
        /// </returns>
        public static string Properize(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLowerInvariant());
        }
        /// <summary>
        /// Surrounds a string with double quotes on each side.
        /// </summary>
        /// <param name="originalValue">The original value to be modified.</param>
        /// <returns>
        /// The original value with double-quote characters pre-pended and appended to the string.
        /// </returns>
        public static string SurroundWithQuotes(this string originalValue)
        {
            return $"\"{originalValue}\"";
        }
        /// <summary>
        /// Converts the current string to a <see cref="MemoryStream"/> instance.
        /// </summary>
        /// <param name="originalValue">The original value to be modified.</param>
        /// <returns>
        /// A <see cref="MemoryStream"/> containing the contents of the current string.
        /// </returns>
        public static MemoryStream ToStream(this string originalValue)
        {
            return GeneralUtils.ToStream(originalValue);
        }
        #endregion

        #region Private Methods / Functions		
        /// <summary>
        /// Determines whether the specified the word ending Y is pluralized by adding "s".
        /// </summary>
        /// <param name="testString">
        /// The test string to be checked.
        /// </param>
        /// <returns>
        /// <b>true</b> if the word is pluralized by adding "s"; otherwise, the "y" is removed
        /// and "ies" is added.
        /// </returns>
        private static bool YWordEndsInS(string testString)
        {
            bool endsInS = false;

            if (testString.Length >= 2)
            {
                char vowelCheck = testString[^2];
                endsInS =
                    vowelCheck is 'a' or
                    'e' or
                    'i' or
                    'o' or
                    'u';
            }
            return endsInS;
        }
        #endregion
    }
}
