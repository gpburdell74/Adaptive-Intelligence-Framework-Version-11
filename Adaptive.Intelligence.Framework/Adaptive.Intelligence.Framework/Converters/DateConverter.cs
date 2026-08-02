using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;
using System.Globalization;

namespace Adaptive.Intelligence.Converters
{
    /// <summary>
    /// Provides a class for parsing date values in various forms.
    /// </summary>
    public sealed class DateConverter : IValueConverter<string, DateTime>
    {
        #region Private Member Declarations
        /// <summary>
        /// Gets the definition for MinYear.
        /// </summary>
        private const int MinYear = 1900;
        /// <summary>
        /// Gets the definition for MaxYear.
        /// </summary>
        private const int MaxYear = 3000;

        /// <summary>
        /// The indexes of months with 31 days.
        /// </summary>
        private static readonly int[] MonthsWith31Days = [1, 3, 5, 7, 8, 10, 12];
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Converts the original value to another value.
        /// </summary>
        /// <param name="originalValue">The original value to be converted.</param>
        /// <returns>
        /// The <see cref="DateTime"/> value or <see cref="DateTime.MinValue"/> if the
        /// value cannot be converted.
        /// </returns>
        public DateTime Convert(string originalValue)
        {
            if (string.IsNullOrEmpty(originalValue))
            {
                return new DateTime(1900, 1, 1);
            }
            else
            {
                originalValue = originalValue
                    .Replace(CharacterConstants.OpenParen, string.Empty)
                    .Replace(CharacterConstants.CloseParen, string.Empty);

                return originalValue.Contains(CharacterConstants.Slash)
                    ? ProcessWithDashes(CharacterConstants.Slash, originalValue)
                    : originalValue.Contains(CharacterConstants.Dash)
                        ? ProcessWithDashes(CharacterConstants.Dash, originalValue)
                        : originalValue.Contains(CharacterConstants.Dot)
                                            ? ProcessWithDashes(CharacterConstants.Dot, originalValue)
                                            : DateTime.TryParse(originalValue, out DateTime dt) ? dt : ProcessWithoutDashes(originalValue);
            }
        }

        /// <summary>
        /// Converts the original value to another value.
        /// </summary>
        /// <param name="originalValue">The original value to be converted.</param>
        /// <returns>
        /// The <see cref="DateTime"/> value or <see cref="DateTime.MinValue"/> if the
        /// value cannot be converted.
        /// </returns>
        public DateOnly? ConvertToDateOnly(string originalValue)
        {
            return DateToDateOnly(Convert(originalValue));
        }

        /// <summary>
        /// Converts the converted value to the original representation.
        /// </summary>
        /// <param name="convertedValue">The original value to be converted.</param>
        /// <returns>
        /// The <see cref="DateTime"/> to be converted to a string.
        /// </returns>
        /// <remarks>
        /// The implementation of this method must be the inverse of
        /// the <see cref="Convert" /> method.
        /// </remarks>
        public string ConvertBack(DateTime convertedValue)
        {
            return convertedValue.ToString(DateConstants.USDateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the <see cref="DateTime"/> to a <see cref="DateOnly"/> instance.
        /// </summary>
        /// <param name="original">
        /// The <see cref="DateTime"/> value to extract the date from.
        /// </param>
        /// <returns>
        /// A new <see cref="DateOnly"/> instance.
        /// </returns>
        public static DateOnly DateToDateOnly(DateTime original)
        {
            return new DateOnly(original.Year, original.Month, original.Day);
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Processes the date string that contains dash or slash characters.
        /// </summary>
        /// <param name="character">
        /// The character being used as the delimiter.
        /// </param>
        /// <param name="dateString">
        /// The date string to be parsed.
        /// </param>
        /// <returns>
        /// The parsed <see cref="DateTime"/> value.
        /// </returns>
        private static DateTime ProcessWithDashes(string character, string dateString)
        {
            int year = 0;
            int month = 0;
            int day = 0;

            if (!string.IsNullOrEmpty(character) && !string.IsNullOrEmpty(dateString))
            {
                int leftIndex = dateString.IndexOf(character, StringComparison.Ordinal);
                int rightIndex = dateString.IndexOf(character, leftIndex + 1, StringComparison.Ordinal);

                if ((leftIndex != -1) || (rightIndex != -1))
                {
                    string leftValue = dateString[..leftIndex];
                    if (rightIndex - leftIndex - 1 > 0)
                    {
                        string midValue = dateString.Substring(leftIndex + 1, rightIndex - leftIndex - 1);
                        string rightValue = dateString[(rightIndex + 1)..];
                        int spaceIndex = rightValue.IndexOf(CharacterConstants.Space, StringComparison.Ordinal);
                        if (spaceIndex > -1)
                        {
                            rightValue = rightValue[..spaceIndex];
                        }

                        if (leftValue.Length > 2)
                        {
                            bool canParse =
                                int.TryParse(leftValue, out year) &&
                                int.TryParse(midValue, out month) &&
                                int.TryParse(rightValue, out day);

                            if (canParse)
                            {
                                if (month > 12)
                                {
                                    day = month;
                                    month = System.Convert.ToInt32(rightValue, CultureInfo.CurrentCulture);
                                }
                                else
                                {
                                    if (!int.TryParse(rightValue, out day))
                                    {
                                        day = 0;
                                    }
                                }
                            }
                        }
                        else if (rightValue.Length > 2)
                        {
                            year = System.Convert.ToInt32(rightValue, CultureInfo.CurrentCulture);
                            month = System.Convert.ToInt32(leftValue, CultureInfo.CurrentCulture);
                            if (month > 12)
                            {
                                day = month;
                                month = System.Convert.ToInt32(midValue, CultureInfo.CurrentCulture);
                            }
                            else
                            {
                                day = System.Convert.ToInt32(midValue, CultureInfo.CurrentCulture);
                            }
                        }
                    }
                    else
                    {
                        return MakeDate(MinYear, 1, 1);
                    }
                }
            }
            return MakeDate(year, month, day);
        }
        /// <summary>
        /// Processes the date string that does not contain a delimiter character.
        /// </summary>
        /// <param name="dateString">
        /// The date string to be parsed.
        /// </param>
        /// <returns>
        /// The parsed <see cref="DateTime"/> value.
        /// </returns>
        private static DateTime ProcessWithoutDashes(string dateString)
        {
            int year = 0;
            int month = 0;
            int day = 0;

            if (!string.IsNullOrEmpty(dateString))
            {
                if (dateString.Length == 8)
                {
                    string yearC = dateString[..4];
                    int yearCandidateValue = System.Convert.ToInt32(yearC, CultureInfo.CurrentCulture);

                    if (yearCandidateValue is >= MinYear and < MaxYear)
                    {
                        month = System.Convert.ToInt32(dateString.Substring(4, 2), CultureInfo.CurrentCulture);
                        if (month <= 12)
                        {
                            day = System.Convert.ToInt32(dateString.Substring(6, 2), CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            day = month;
                            month = System.Convert.ToInt32(dateString.Substring(6, 2), CultureInfo.CurrentCulture);
                        }
                        year = yearCandidateValue;
                    }
                    else
                    {
                        year = System.Convert.ToInt32(dateString.Substring(4, 4), CultureInfo.CurrentCulture);
                        month = System.Convert.ToInt32(dateString.Substring(2, 2), CultureInfo.CurrentCulture);
                        if (month > 12)
                        {
                            day = month;
                            month = System.Convert.ToInt32(dateString[..2], CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            day = System.Convert.ToInt32(dateString[..2], CultureInfo.CurrentCulture);
                        }
                    }
                }
            }
            return MakeDate(year, month, day);
        }
        /// <summary>
        /// Creates the date/time value.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>
        /// The parsed <see cref="DateTime"/> value.
        /// </returns>
        private static DateTime MakeDate(int year, int month, int day)
        {
            DateTime returnDate = new(MinYear, 1, 1);
            if (DateIsValid(year, month, day))
            {
                try
                {
                    returnDate = new DateTime(year, month, day);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }

            return returnDate;
        }
        /// <summary>
        /// Determines if the date values are valid.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>
        /// <b>true</b> if the date values are valid; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        private static bool DateIsValid(int year, int month, int day)
        {
            bool isValid = false;
            int febMax = 28;
            bool isLeapYear = (year > 0) && DateTime.IsLeapYear(year);
            if (isLeapYear)
            {
                febMax = 29;
            }

            if (year >= MinYear && year < MaxYear &&
                month > 0 && month < 13 &&
                day > 0 && day <= 31)
            {
                if (month == 2 && day <= febMax)
                {
                    isValid = true;
                }
                else if (MonthsWith31Days.Contains(month) && day <= 31)
                {
                    isValid = true;
                }

                else if (day <= 30)
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        #endregion
    }
}