using Adaptive.Intelligence.Utility;

namespace Adaptive.Intelligence.Extensions
{
    /// <summary>
    /// Provides method extensions for <see cref="TimeSpan"/> instances.
    /// </summary>
    public static class TimeSpanExtensions
    {
        #region Private Constants
        /// <summary>
        /// The day text.
        /// </summary>
        private const string DayName = "day";
        /// <summary>
        /// The hour text.
        /// </summary>
        private const string HourName = "hour";
        /// <summary>
        /// The minute text.
        /// </summary>
        private const string MinuteName = "minute";
        /// <summary>
        /// The plural "s" value.
        /// </summary>
        private const string PluralS = "s";
        #endregion

        #region TimeSpan Extension Methods
        /// <summary>
        /// Translates the specified <see cref="TimeSpan"/> to an English string.
        /// </summary>
        /// <param name="ts">
        /// The <see cref="TimeSpan"/> being extended,
        /// </param>
        /// <returns>
        /// A string representing the value of the time span.
        /// </returns>
        public static string ToEnglishDisplayString(this TimeSpan ts)
        {
            string days = string.Empty;
            string hours = string.Empty;
            string minutes = string.Empty;

            if (ts.Days > 0)
            {
                days = GeneralUtils.EnglishPlural(ts.Days, DayName, PluralS);
            }

            if (ts.Hours > 0)
            {
                hours = GeneralUtils.EnglishPlural(ts.Hours, HourName, PluralS);
            }

            if (ts.Minutes > 0)
            {
                minutes = GeneralUtils.EnglishPlural(ts.Minutes, MinuteName, PluralS);
            }

            return GeneralUtils.EnglishStringAppend([days, hours, minutes]);
        }
        #endregion
    }
}