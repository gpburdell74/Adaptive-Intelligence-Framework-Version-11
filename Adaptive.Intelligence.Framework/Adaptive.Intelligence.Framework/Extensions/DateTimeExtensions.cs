using Adaptive.Intelligence.Common;
using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Enumerations;
using System.Globalization;

namespace Adaptive.Intelligence.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="DateTime"/> structure.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets a new <see cref="DateTime"/> for the first the day of month preceding the specified date.
        /// </summary>
        /// <param name="originalValue">
        /// The original <see cref="DateTime"/> value being examined.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> representing the first day of the preceding month in the original date value.
        /// </returns>
        public static DateTime FirstDayOfPreviousMonth(this DateTime originalValue)
        {
            return originalValue.AddMonths(-2).AddDays(1);
        }
        /// <summary>
        /// Determines whether the specified date/time specifies a point in time that is 
        /// ridiculous for business use.
        /// </summary>
        /// <param name="originalValue">
        /// The instance of the <see cref="DateTime"/> structure being examined.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the value represents a ridiculous date; otherwise, <b>false</b>.
        /// </returns>
        public static bool IsRidiculousDate(this DateTime originalValue)
        {
            return originalValue.Year is < 1754 or > 3000;
        }
        /// <summary>
        /// Generates the first and fifteenth of the month date values that fall within the specified date range.
        /// </summary>
        /// <param name="startDate">
        /// A <see cref="DateTime"/> specifying the start date value.
        /// </param>
        /// <param name="endDate">
        /// A <see cref="DateTime"/> specifying the ending date value.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> values that represent the first and fifteenth of each month
        /// within the specified date range, sequentially.
        /// </returns>
        public static List<DateTime> GenerateBiMonthlyDates(this DateTime startDate, DateTime endDate)
        {
            List<DateTime> list = [];

            if (endDate > startDate)
            {
                DateTime candidateDate = startDate;
                do
                {
                    if (candidateDate.Day is 1 or 15)
                    {
                        list.Add(candidateDate);
                    }

                    candidateDate = candidateDate.AddDays(1);

                } while (candidateDate <= endDate);
            }

            return list;
        }
        /// <summary>
        /// Calculates the dates in the specified date range that fall on the specified days of the week.
        /// </summary>
        /// <param name="startDate">
        /// A <see cref="DateTime"/> specifying the start date value.
        /// </param>
        /// <param name="endDate">
        /// A <see cref="DateTime"/> specifying the ending date value.
        /// </param>
        /// <param name="selectedDays">
        /// A <see cref="SelectedDays"/> enumerated value that indicates which days of the week to find.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> values that occur within the specified date range, sequentially.
        /// </returns>
        public static List<DateTime> GenerateByDaysOfWeekEachWeek(this DateTime startDate, DateTime endDate, SelectedDays selectedDays)
        {
            List<DateTime> list = [];

            DateTime dateValue = startDate;
            do
            {
                if (dateValue.DayOfWeek == DayOfWeek.Sunday && (selectedDays & SelectedDays.Sunday) != 0)
                {
                    list.Add(dateValue);
                }

                if (dateValue.DayOfWeek == DayOfWeek.Monday && (selectedDays & SelectedDays.Monday) != 0)
                {
                    list.Add(dateValue);
                }

                if (dateValue.DayOfWeek == DayOfWeek.Tuesday && (selectedDays & SelectedDays.Tuesday) != 0)
                {
                    list.Add(dateValue);
                }

                if (dateValue.DayOfWeek == DayOfWeek.Wednesday && (selectedDays & SelectedDays.Wednesday) != 0)
                {
                    list.Add(dateValue);
                }

                if (dateValue.DayOfWeek == DayOfWeek.Thursday && (selectedDays & SelectedDays.Thursday) != 0)
                {
                    list.Add(dateValue);
                }

                if (dateValue.DayOfWeek == DayOfWeek.Friday && (selectedDays & SelectedDays.Friday) != 0)
                {
                    list.Add(dateValue);
                }

                if (dateValue.DayOfWeek == DayOfWeek.Saturday && (selectedDays & SelectedDays.Saturday) != 0)
                {
                    list.Add(dateValue);
                }

                dateValue = dateValue.AddDays(1);
            } while (dateValue <= endDate);

            return list;
        }
        /// <summary>
        /// Calculates the dates that occur as an interval of days from before the calculated end date, within the specified
        /// date range.
        /// </summary>
        /// <remarks>
        /// This calculates the date for a value of every X number of days before the specified end date.
        /// </remarks>
        /// <param name="startDate">
        /// A <see cref="DateTime"/> specifying the start date value.
        /// </param>
        /// <param name="endDate">
        /// A <see cref="DateTime"/> specifying the ending date value.
        /// </param>
        /// <param name="calcEndDate">
        /// A <see cref="DateTime"/> specifying the date from which to begin counting backwards by <i>interval</i>.
        /// </param>
        /// <param name="interval">
        /// An integer specifying the number of days to use as the interval between dates.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> values that occur within the specified date range, sequentially.
        /// </returns>
        public static List<DateTime> GenerateByIntervalDaysFromEndDate(this DateTime startDate, DateTime endDate, DateTime calcEndDate, int interval)
        {
            List<DateTime> dateList = [];

            DateTime dateValue = calcEndDate;
            do
            {
                if (dateValue <= endDate && dateValue >= startDate)
                {
                    dateList.Insert(0, dateValue);
                }

                dateValue = interval <= 0 ? dateValue.AddDays(-1) : dateValue.AddDays(interval * -1);
            } while (dateValue >= startDate);

            return dateList;
        }
        /// <summary>
        /// Calculates the dates that occur as an interval of days from after the calculated start date, within the specified
        /// date range.
        /// </summary>
        /// <remarks>
        /// This calculates the date for a value of every X number of days after the specified start date.
        /// </remarks>
        /// <param name="startDate">
        /// A <see cref="DateTime"/> specifying the start date value.
        /// </param>
        /// <param name="endDate">
        /// A <see cref="DateTime"/> specifying the ending date value.
        /// </param>
        /// <param name="calcStartDate">
        /// A <see cref="DateTime"/> specifying the date from which to begin counting forwards by <i>interval</i> days.
        /// </param>
        /// <param name="interval">
        /// An integer specifying the number of days to use as the interval between dates.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> values that occur within the specified date range, sequentially.
        /// </returns>
        public static List<DateTime> GenerateByIntervalDaysFromStartDate(this DateTime startDate, DateTime endDate, DateTime calcStartDate, int interval)
        {
            List<DateTime> dateList = [];

            DateTime dateValue = calcStartDate;
            do
            {
                if (dateValue <= endDate && dateValue >= startDate)
                {
                    dateList.Add(dateValue);
                }

                dateValue = interval <= 0 ? dateValue.AddDays(1) : dateValue.AddDays(interval);
            } while (dateValue <= endDate);

            return dateList;
        }
        /// <summary>
        /// Generates the first of the month date values that fall within the specified date range.
        /// </summary>
        /// <param name="startDate">
        /// A <see cref="DateTime"/> specifying the start date value.
        /// </param>
        /// <param name="endDate">
        /// A <see cref="DateTime"/> specifying the ending date value.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> values that represent the first of each month
        /// within the specified date range, sequentially.
        /// </returns>
        public static List<DateTime> GenerateFirstOfMonthDates(this DateTime startDate, DateTime endDate)
        {
            List<DateTime> list = [];

            DateTime candidateDate = startDate;
            do
            {
                if (candidateDate.Day == 1)
                {
                    list.Add(candidateDate);
                }

                candidateDate = candidateDate.AddDays(1);

            } while (candidateDate <= endDate);

            return list;
        }
        /// <summary>
        /// Generates the date value that occurs <i>interval</i> number of days before the specified end date.
        /// </summary>
        /// <param name="endDate">
        /// A <see cref="DateTime"/> specifying the ending date value.
        /// </param>
        /// <param name="interval">
        /// An integer specifying the number of days to use as the interval between dates.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> containing the generated date value.
        /// </returns>
        public static List<DateTime> GenerateOnceByIntervalDaysFromEndDate(this DateTime endDate, int interval)
        {
            return [endDate.AddDays(interval * -1)];
        }
        /// <summary>
        /// Generates the date value that occurs <i>interval</i> number of days after the specified end date.
        /// </summary>
        /// <param name="startDate">
        /// A <see cref="DateTime"/> specifying the starting date value.
        /// </param>
        /// <param name="interval">
        /// An integer specifying the number of days to use as the interval between dates.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> containing the generated date value.
        /// </returns>
        public static List<DateTime> GenerateOnceByIntervalDaysFromStartDate(this DateTime startDate, int interval)
        {
            return [startDate.AddDays(interval)];
        }
        /// <summary>
        /// Generates a <see cref="DateTime"/> instance for each day in the specified date range, inclusive.
        /// </summary>
        /// <param name="startDate">
        /// The <see cref="DateTime"/> instance representing the start date.
        /// </param>
        /// <param name="endDate">
        /// The <see cref="DateTime"/> specifying the end date.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> instances, one for each day in the specified range.
        /// </returns>
        public static List<DateTime> GenerateRangeInstances(this DateTime startDate, DateTime endDate)
        {
            List<DateTime> dateList = [];

            DateTime dateValue = startDate.Date;

            do
            {
                dateList.Add(dateValue);
                dateValue = dateValue.AddDays(1);
            } while (dateValue.Date <= endDate.Date);

            return dateList;
        }
        /// <summary>
        /// Generates the specific date.
        /// </summary>
        /// <param name="dateValue">
        /// A <see cref="DateTime"/> value.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="DateTime"/> containing the generated date value.
        /// </returns>
        public static List<DateTime> GenerateSpecificDate(this DateTime dateValue)
        {
            return [dateValue];
        }
        /// <summary>
        /// Gets the time value as a <see cref="Time"/> structure.
        /// </summary>
        /// <param name="dateValue">
        /// The date value.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> structure containing the time value.
        /// </returns>
        public static Time GetTime(this DateTime dateValue)
        {
            return new Time(dateValue.Hour, dateValue.Minute, dateValue.Second);
        }
        /// <summary>
        /// Gets a new <see cref="DateTime"/> for the last the day of month for the specified date.
        /// </summary>
        /// <param name="originalValue">
        /// The original <see cref="DateTime"/> value being examined.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> representing the last day of the month in the original date value.
        /// </returns>
        public static DateTime LastDayOfTheMonth(this DateTime originalValue)
        {
            return new DateTime(
                originalValue.Year,
                originalValue.Month,
                DateTime.DaysInMonth(originalValue.Year, originalValue.Month));
        }
        /// <summary>
        /// Generates and returns the current date and time in the U.S. date format.
        /// </summary>
        /// <returns>
        /// A string containing the date and time formatted as MM/dd/yyyy hh:mm:ss tt.
        /// </returns>
        public static string NowAsString()
        {
            return DateTime.Now.ToString(DateConstants.USFullDateFormat, CultureInfo.InvariantCulture);
        }
    }

}
