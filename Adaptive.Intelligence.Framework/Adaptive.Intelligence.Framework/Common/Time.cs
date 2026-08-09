using System.Globalization;
using System.Runtime.InteropServices;

namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Represents an instant in time, typically expressed as a time of day.
    /// </summary>
    /// <remarks>
    /// This is used similarly to the <see cref="DateTime"/> structure, however, it is
    /// limited to just the clock time.  The primary purpose of this structure is to provide
    /// time-data and formatting for (clock)time-only data content.
    ///
    /// The <see cref="Time"/> value type represents times with values ranging from 00:00:00 (midnight),
    /// through 11:59:59 P.M.  Time zones are not used in this reference.  Time values are measured in
    /// seconds.
    /// </remarks>
    /// <seealso cref="DateTime"/>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Time : IComparable, IComparable<Time>, IEquatable<Time>
    {
        #region Private Member Declarations

        #region Private Constants
        /// <summary>
        /// The seconds per minute.
        /// </summary>
        private const int SecondsPerMinute = 60;
        /// <summary>
        /// The seconds per hour.
        /// </summary>
        private const int SecondsPerHour = 3600;
        /// <summary>
        /// The maximum seconds per day.
        /// </summary>
        private const int MaxSecondsPerDay = 86399;
        /// <summary>
        /// The string to append to a formatted output to indicate AM.
        /// </summary>
        private const string AppendAM = " AM";
        /// <summary>
        /// The string to append to a formatted output to indicate PM.
        /// </summary>
        private const string AppendPM = " PM";
        /// <summary>
        /// The string to look for in the input value to indicate the specified time is AM.
        /// </summary>
        private const string CompareAM = "am";
        /// <summary>
        /// The string to look for in the input value to indicate the specified time is PM.
        /// </summary>
        private const string ComparePM = "pm";
        /// <summary>
        /// The format string for outputting the hour value.
        /// </summary>
        private const string FormatHour = "#0";
        /// <summary>
        /// The format string for outputting the minutes and seconds value(s).
        /// </summary>
        private const string FormatOther = "00";
        /// <summary>
        /// The time component delimiter, as used in the US.
        /// </summary>
        private const string DelimiterUS = ":";
        /// <summary>
        /// The time component delimiter, as used in the EU.
        /// </summary>
        private const string DelimiterEU = ".";
        #endregion

        /// <summary>
        /// The number of seconds being represented.  Everything else operates of this one variable.
        /// </summary>
        private readonly int _timeData;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> structure.
        /// </summary>
        /// <param name="totalNumberOfSeconds">
        /// The total number of seconds being represented.
        /// </param>
        public Time(int totalNumberOfSeconds)
        {
            if (totalNumberOfSeconds > MaxSecondsPerDay)
            {
                _timeData = MaxSecondsPerDay;
            }
            else if (totalNumberOfSeconds < 0)
            {
                _timeData = 0;
            }
            else
            {
                _timeData = totalNumberOfSeconds;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> structure.
        /// </summary>
        /// <param name="hour">The hour value (from 0 - 23).</param>
        /// <param name="minute">The minute value (from 0 - 59).</param>
        public Time(int hour, int minute)
        {
            _timeData = CalculateSeconds(hour, minute, 0);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> structure.
        /// </summary>
        /// <param name="hour">The hour value (from 0 - 23).</param>
        /// <param name="minute">The minute value (from 0 - 59).</param>
        /// <param name="seconds">The seconds value (from 0 - 59).</param>
        public Time(int hour, int minute, int seconds)
        {
            _timeData = CalculateSeconds(hour, minute, seconds);
        }
        #endregion

        #region Public Static Fields
        /// <summary>
        /// Gets the minimum possible <see cref="Time"/> value.
        /// </summary>
        public static readonly Time MinValue = new(0);
        /// <summary>
        /// Gets the maximum possible <see cref="Time"/> value.
        /// </summary>
        public static readonly Time MaxValue = new(MaxSecondsPerDay);
        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <value>
        /// A <see cref="Time"/> value indicating the current time.
        /// </value>
        public static Time Now
        {
            get
            {
                DateTime dt = DateTime.Now;
                return new Time(dt.Hour, dt.Minute, dt.Second);
            }
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the hour value of the time component.
        /// </summary>
        /// <value>
        /// The hour value, between 0 and 23.
        /// </value>
        public readonly int Hour => _timeData / SecondsPerHour;
        /// <summary>
        /// Gets or sets the minute value of the time component.
        /// </summary>
        /// <value>
        /// The minute value, between 0 and 59.
        /// </value>
        public readonly int Minute => _timeData % SecondsPerHour / SecondsPerMinute;
        /// <summary>
        /// Gets or sets the seconds value of the time component.
        /// </summary>
        /// <value>
        /// The seconds value, between 0 and 59.
        /// </value>
        public readonly int Second => _timeData % SecondsPerHour % SecondsPerMinute;
        /// <summary>
        /// Gets the total seconds currently represented by the <see cref="Time"/> structure.
        /// </summary>
        /// <value>
        /// The number of total seconds in the current <see cref="Time"/> structure.
        /// </value>
        public readonly int TotalSeconds => _timeData;
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Adds the specified number of hours to the current time value.
        /// </summary>
        /// <param name="hours">
        /// The number of hours to be added.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> structure with the new value.
        /// </returns>
        public Time AddHours(int hours)
        {
            return new Time(BoundsCheckValue(_timeData + (SecondsPerHour * hours)));
        }
        /// <summary>
        /// Adds the specified number of minutes to the current time value.
        /// </summary>
        /// <param name="minutes">
        /// The number of minutes to be added.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> structure with the new value.
        /// </returns>
        public Time AddMinutes(int minutes)
        {
            return new Time(BoundsCheckValue(_timeData + (SecondsPerMinute * minutes)));
        }
        /// <summary>
        /// Adds the specified number of seconds to the current time value.
        /// </summary>
        /// <param name="seconds">
        /// The number of seconds to be added.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> structure with the new value.
        /// </returns>
        public Time AddSeconds(int seconds)
        {
            return new Time(BoundsCheckValue(_timeData + seconds));
        }
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same
        /// position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance. </param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. If the
        /// value is less than zero, this instance precedes <i>obj</i> in the sort order.  If the value
        /// is zero, this instance occurs in the same position in the sort order as <i>obj</i>.
        /// Otherwise, if the value is greater than zero, this instance follows <i>obj</i> in
        /// the sort order.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <i>obj</i>i> is not the same type as this instance.
        /// </exception>
        public int CompareTo(object? obj)
        {
            return obj is not Time
                ? throw new ArgumentException(
                    "Specified object is not the same type as this instance.",
                    nameof(obj))
                : _timeData.CompareTo(((Time)obj).TotalSeconds);
        }
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same
        /// position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance. </param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. If the
        /// value is less than zero, this instance precedes <i>obj</i> in the sort order.  If the value
        /// is zero, this instance occurs in the same position in the sort order as <i>obj</i>.
        /// Otherwise, if the value is greater than zero, this instance follows <i>obj</i> in
        /// the sort order.
        /// </returns>
        public int CompareTo(Time other)
        {
            return _timeData.CompareTo(other.TotalSeconds);
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object" /> to compare with this instance.
        /// </param>
        /// <returns>
        /// <b>true</b> if the specified <see cref="System.Object" /> is equal to this instance;
        /// otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                return obj is Time && _timeData == ((Time)obj).TotalSeconds;
            }
        }
        /// <summary>
        /// Determines whether the specified <see cref="Time"/>, is equal to this instance.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Time" /> to compare with this instance.
        /// </param>
        /// <returns>
        /// <b>true</b> if the specified <see cref="Time" /> is equal to this instance;
        /// otherwise, <b>false</b>.
        /// </returns>
        public readonly bool Equals(Time other)
        {
            return _timeData == other.TotalSeconds;
        }
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="rightValue">The right value.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Time leftValue, Time rightValue)
        {
            return leftValue.Equals(rightValue);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="rightValue">The right value.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Time leftValue, Time rightValue)
        {
            return !leftValue.Equals(rightValue);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _timeData;
        }
        /// <summary>
        /// Converts the current value to a <see cref="DateTime"/> by combining the date
        /// component of the parameter with the current time value.
        /// </summary>
        /// <param name="originalDate">
        /// The source <see cref="DateTimeOffset"/> value containing the date component to use.
        /// </param>
        /// <returns>
        /// A new <see cref="DateTime"/> value with the date component from <i>originalDate</i> and
        /// the time component from the current <see cref="Time"/> instance.
        /// </returns>
        public DateTime ToDate(DateTimeOffset originalDate)
        {
            return ToDate(originalDate.DateTime);
        }
        /// <summary>
        /// Converts the current value to a <see cref="DateTime"/> by combining the date
        /// component of the parameter with the current time value.
        /// </summary>
        /// <param name="originalDate">
        /// The source <see cref="DateTime"/> value containing the date component to use.
        /// </param>
        /// <returns>
        /// A new <see cref="DateTime"/> value with the date component from <i>originalDate</i> and
        /// the time component from the current <see cref="Time"/> instance.
        /// </returns>
        public DateTime ToDate(DateTime originalDate)
        {
            DateTime newDate = new(originalDate.Year, originalDate.Month, originalDate.Day,
                Hour, Minute, Second);
            return newDate;
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string amPmValue;
            int displayHour;

            // Determine time of day and modify the hour value from "military" time.
            if (Hour < 12)
            {
                amPmValue = AppendAM;
                if (Hour == 0)
                {
                    displayHour = 12;
                }
                else
                {
                    displayHour = Hour;
                }
            }
            else
            {
                amPmValue = AppendPM;
                displayHour = Hour - 12;
                if (displayHour == 0)
                {
                    displayHour = 12;
                }
            }

            // Return the formatted string.
            return $"{displayHour.ToString(FormatHour, CultureInfo.CurrentCulture)}{DelimiterUS}{Minute.ToString(FormatOther, CultureInfo.CurrentCulture)}{amPmValue}";
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="withAmPm">
        /// A value indicating whether to include the "AM"/"PM" text in the out put.
        /// If <b>false</b>, the "military" time format is used.
        /// </param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToString(bool withAmPm)
        {
            return withAmPm
                ? ToString()
                : Hour.ToString(FormatHour, CultureInfo.CurrentCulture) + DelimiterUS +
                       Minute.ToString(FormatOther, CultureInfo.CurrentCulture) + DelimiterUS +
                       Second.ToString(FormatOther, CultureInfo.CurrentCulture);
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatString">
        /// A string containing a time format string (such as "hh:mm:ss tt").  If any other
        /// format string is used, unpredictable results may occur.
        /// </param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string formatString)
        {
            return ToDate(DateTime.Now).ToString(formatString, CultureInfo.CurrentCulture);
        }
        #endregion

        #region Operators
        /// <summary>
        /// Implements the less than operator.
        /// </summary>
        /// <param name="leftTime">The left-side <see cref="Time"/> value to be compared.</param>
        /// <param name="rightTime">The right-side <see cref="Time"/> value to be compared.</param>
        /// <returns>
        /// <b>true</b> if <i>leftTime</i> is less than <i>rightTime</i>; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        public static bool operator <(Time leftTime, Time rightTime)
        {
            return leftTime.TotalSeconds < rightTime.TotalSeconds;
        }
        /// <summary>
        /// Implements the less than or equal to operator.
        /// </summary>
        /// <param name="leftTime">The left-side <see cref="Time"/> value to be compared.</param>
        /// <param name="rightTime">The right-side <see cref="Time"/> value to be compared.</param>
        /// <returns>
        /// <b>true</b> if <i>leftTime</i> is less than or equal to <i>rightTime</i>; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        public static bool operator <=(Time leftTime, Time rightTime)
        {
            return leftTime.TotalSeconds <= rightTime.TotalSeconds;
        }
        /// <summary>
        /// Implements the greater than operator.
        /// </summary>
        /// <param name="leftTime">The left-side <see cref="Time"/> value to be compared.</param>
        /// <param name="rightTime">The right-side <see cref="Time"/> value to be compared.</param>
        /// <returns>
        /// <b>true</b> if <i>leftTime</i> is greater than <i>rightTime</i>; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        public static bool operator >(Time leftTime, Time rightTime)
        {
            return leftTime.TotalSeconds > rightTime.TotalSeconds;
        }
        /// <summary>
        /// Implements the greater than or equal to operator.
        /// </summary>
        /// <param name="leftTime">The left-side <see cref="Time"/> value to be compared.</param>
        /// <param name="rightTime">The right-side <see cref="Time"/> value to be compared.</param>
        /// <returns>
        /// <b>true</b> if <i>leftTime</i> is greater than or equal to <i>rightTime</i>; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        public static bool operator >=(Time leftTime, Time rightTime)
        {
            return leftTime.TotalSeconds >= rightTime.TotalSeconds;
        }
        #endregion

        #region Public Static Methods / Functions
        /// <summary>
        /// Creates and returns the <see cref="Time"/> component from the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="originalDate">
        /// The <see cref="DateTimeOffset"/> containing the time value.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> instance containing the time value from the original date structure.
        /// </returns>
        public static Time FromDate(DateTimeOffset originalDate)
        {
            return FromDate(originalDate.DateTime);
        }
        /// <summary>
        /// Creates and returns the <see cref="Time"/> component from the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="originalDate">
        /// The <see cref="DateTime"/> containing the time value.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> instance containing the time value from the original date structure.
        /// </returns>
        public static Time FromDate(DateTime originalDate)
        {
            return new Time(originalDate.Hour, originalDate.Minute, originalDate.Second);
        }
        /// <summary>
        /// Parses the specified time string into a <see cref="Time"/> value.
        /// </summary>
        /// <param name="timeString">
        /// A string containing a time to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> value containing the result of the parsed string.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <i>timeString</i> parameter is <b>null</b> or empty.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if the <i>timeString</i> cannot be parsed into a time value.
        /// </exception>
        public static Time Parse(string timeString)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(timeString, nameof(timeString));

            // Numeric-only strings, such as 2300 or 123456.
            if (IsNumericOnly(timeString))
            {
                if (timeString.Length == 4)
                {
                    return ParseFour(timeString);
                }
                else
                {
                    return timeString.Length == 6
                        ? ParseSix(timeString)
                        : throw new ArgumentException("Invalid text was supplied for parsing.", nameof(timeString));
                }
            }
            else
            {
                // Assume delimiters are present in the value.
                return ParseDelimited(timeString);
            }
        }
        /// <summary>
        /// Attempts to parse the specified string value into a <see cref="Time"/> instance.
        /// </summary>
        /// <param name="timeString">
        /// A string containing a time to convert.
        /// </param>
        /// <param name="returnValue">
        /// When this method returns, contains the <see cref="Time"/> value equivalent to the time
        /// contained in <i>timeString</i>, if the conversion succeeded, or <see cref="MinValue"/> if
        /// the conversion failed.
        /// </param>
        /// <returns>
        /// <b>true</b>if the <i>timeString</i> parameter was converted successfully;
        /// otherwise, return <b>false</b>.
        /// </returns>
        public static bool TryParse(string timeString, out Time returnValue)
        {
            bool success = false;
            try
            {
                returnValue = Parse(timeString);
                success = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                returnValue = MinValue;
            }

            return success;
        }
        /// <summary>
        /// Renders the hour value in a 12-hour format.
        /// </summary>
        /// <param name="hour">
        /// The hour value in military time (0 - 23) format.
        /// </param>
        /// <returns>
        /// A string for displaying the hour value.
        /// </returns>
        public static string RenderHourText(int hour)
        {
            string hourText;

            if (hour == 0)
            {
                hourText = "12";
            }
            else if (hour < 13)
            {
                hourText = hour.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                hourText = (hour - 12).ToString(CultureInfo.CurrentCulture);
            }

            return hourText;
        }
        /// <summary>
        /// Renders the AM or PM value based on the hour.
        /// </summary>
        /// <param name="hour">
        /// The hour value in military time (0 - 23) format.
        /// </param>
        /// <returns>
        /// A string containing the AM or PM value.
        /// </returns>
        public static string RenderAmPm(int hour)
        {
            return hour < 12 ? "AM" : "PM";
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Calculates the total number of seconds in the specified time after adjusting
        /// the specified parameters to fit within the correct boundaries.
        /// </summary>
        /// <param name="hour">The hour value (from 0 - 23).</param>
        /// <param name="minute">The minute value (from 0 - 59).</param>
        /// <param name="seconds">The seconds value (from 0 - 59).</param>
        /// <returns>
        /// The total number of seconds in the specified time.
        /// </returns>
        private static int CalculateSeconds(int hour, int minute, int seconds)
        {
            // Adjust the hour value.
            if (hour < 0)
            {
                hour = 0;
            }
            else if (hour > 23)
            {
                hour = 23;
            }

            // Adjust the minute value.
            if (minute < 0)
            {
                minute = 0;
            }
            else if (minute > 59)
            {
                minute = 59;
            }

            // Adjust the seconds value.
            if (seconds < 0)
            {
                seconds = 0;
            }
            else if (seconds > 59)
            {
                seconds = 59;
            }

            return (hour * SecondsPerHour) + (minute * SecondsPerMinute) + seconds;
        }
        /// <summary>
        /// Ensures the specified value is within the proper time range (0 - 86399).
        /// </summary>
        /// <param name="totalSeconds">The total seconds count to be checked.</param>
        /// <returns>
        /// The value of <i>totalSeconds</i> if the value is in the correct range, or
        /// 0 or 86399 if the value exceeds the bounds.
        /// </returns>
        private static int BoundsCheckValue(int totalSeconds)
        {
            if (totalSeconds > MaxSecondsPerDay)
            {
                return MaxSecondsPerDay;
            }
            else
            {
                return totalSeconds < 0 ? 0 : totalSeconds;
            }
        }
        #endregion

        #region Private Static Methods / Functions
        /// <summary>
        /// Determines whether the specified string only contains numbers.
        /// </summary>
        /// <param name="original">
        /// A string to be checked.
        /// </param>
        /// <returns>
        /// <b>true</b> if the specified string only contains numeric digits (0 - 9);
        /// otherwise, <b>false</b>.
        /// </returns>
        private static bool IsNumericOnly(string original)
        {
            bool isNumeric = true;
            int length = original.Length;
            int count = 0;
            do
            {
                if (!char.IsNumber(original[count]))
                {
                    isNumeric = false;
                }

                count++;
            } while (isNumeric && (count < length));

            return isNumeric;
        }
        /// <summary>
        /// Parses a string with four numeric digits into a time value.
        /// </summary>
        /// <remarks>
        /// This method assumes the first two digits are the hour, and the last
        /// two are the minute.
        /// </remarks>
        /// <param name="timeValue">
        /// The string containing the time value to parse.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> representation of the specified value.
        /// </returns>
        private static Time ParseFour(string timeValue)
        {
            string hours = timeValue[..2];
            string mins = timeValue[2..4];

            if (!int.TryParse(hours, out int hourValue))
            {
                hourValue = 0;
            }

            if (!int.TryParse(mins, out int minValue))
            {
                minValue = 0;
            }

            return new Time(hourValue, minValue, 0);
        }
        /// <summary>
        /// Parses a string with six numeric digits into a time value.
        /// </summary>
        /// <remarks>
        /// This method assumes the first two digits are the hour, the middle
        /// two are the minute, and the last two are the seconds value.
        /// </remarks>
        /// <param name="timeValue">
        /// The string containing the time value to parse.
        /// </param>
        /// <returns>
        /// A <see cref="Time"/> representation of the specified value.
        /// </returns>
        private static Time ParseSix(string timeValue)
        {
            string hours = timeValue[..2];
            string minutes = timeValue[2..4];
            string secs = timeValue[4..6];

            if (!int.TryParse(hours, out int hourValue))
            {
                hourValue = 0;
            }

            if (!int.TryParse(minutes, out int minValue))
            {
                minValue = 0;
            }

            if (!int.TryParse(secs, out int secsValue))
            {
                secsValue = 0;
            }

            return new Time(hourValue, minValue, secsValue);
        }
        /// <summary>
        /// Parses the delimited time string.
        /// </summary>
        /// <param name="timeValue">The time value.</param>
        /// <returns>
        /// The resulting <see cref="Time"/> instance.
        /// </returns>
        private static Time ParseDelimited(string timeValue)
        {
            bool isAm = false;
            bool isPm = false;
            Time returnTime;

            // Check for the text "AM" or "PM" in the specified string.  If these values are
            // present, they override the hour value in later parsing.  Remove these from the original
            // string, if present.
            string sample = timeValue.Trim().ToLowerInvariant();
            if (sample.Contains(CompareAM))
            {
                isAm = true;
                sample = sample.Replace(CompareAM, string.Empty).Trim();
                timeValue = timeValue[..sample.Length];
            }
            else if (sample.Contains(ComparePM))
            {
                isPm = true;
                sample = sample.Replace(ComparePM, string.Empty).Trim();
                timeValue = timeValue[..sample.Length];
            }

            // Look for the positions of the standard delimiters.
            int delimiterPositionA = FindFirstDelimiter(timeValue);
            int delimiterPositionB = timeValue.IndexOf(DelimiterUS, delimiterPositionA + 1, StringComparison.Ordinal);
            if (delimiterPositionB == -1)
            {
                delimiterPositionB = timeValue.IndexOf(DelimiterEU, delimiterPositionA + 1, StringComparison.Ordinal);
            }

            // If only one delimiter is found, parse just the hour and seconds;
            // Otherwise, parse all three positions.
            if (delimiterPositionB == -1)
            {
                returnTime = ParseHourMinute(timeValue, delimiterPositionA, isAm, isPm);
            }
            else
            {
                returnTime = ParseHourMinuteSecond(timeValue, delimiterPositionA, delimiterPositionB, isAm, isPm);
            }

            return returnTime;
        }
        /// <summary>
        /// Finds the first delimiter in the specified string.
        /// </summary>
        /// <param name="timeValue">The time value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Occurs if no delimiters are found in the string.
        /// </exception>
        private static int FindFirstDelimiter(string timeValue)
        {
            int delimiterPositionA = timeValue.IndexOf(DelimiterUS, StringComparison.Ordinal);
            if (delimiterPositionA == -1)
            {
                delimiterPositionA = timeValue.IndexOf(DelimiterEU, StringComparison.Ordinal);
            }

            // If the first delimiter is not present, the string is not parseable in this context.
            return delimiterPositionA == -1
                ? throw new ArgumentOutOfRangeException(nameof(timeValue), "Invalid text was supplied for parsing.")
                : delimiterPositionA;
        }
        /// <summary>
        /// Gets the definition for ParseHourMinute.
        /// </summary>
        private static Time ParseHourMinute(string timeValue, int delimiterPositionA, bool isAm, bool isPm)
        {
            // Parse out the text items.
            string left = timeValue[..delimiterPositionA];
            string right = timeValue[(delimiterPositionA + 1)..].Trim();

            // Translate to integers.
            int minValue = 0;
            bool translated = int.TryParse(left, out int hourValue);
            if (translated)
            {
                translated = int.TryParse(right, out minValue);
            }

            if (translated)
            {
                // Adjust the hour value based on whether the "AM" or "PM" text was in the original
                // string.
                if (isPm && hourValue < 12)
                {
                    hourValue += 12;
                }
                else if (isAm && hourValue >= 12)
                {
                    hourValue -= 12;
                }
            }

            return new Time(hourValue, minValue, 0);

        }
        /// <summary>
        /// Gets the definition for ParseHourMinuteSecond.
        /// </summary>
        private static Time ParseHourMinuteSecond(string timeValue, int delimiterPositionA, int delimiterPositionB,
            bool isAm, bool isPm)
        {
            // Parse out the text items.
            string left = timeValue[..delimiterPositionA];
            string mid = timeValue.Substring(delimiterPositionA + 1, delimiterPositionB - delimiterPositionA - 1);
            string right = timeValue[(delimiterPositionB + 1)..].Trim();

            // Translate to integers.
            int minValue = 0;
            int secValue = 0;
            bool translated = int.TryParse(left, out int hourValue);
            if (translated)
            {
                translated = int.TryParse(mid, out minValue);
                if (translated)
                {
                    translated = int.TryParse(right, out secValue);
                }
            }

            if (translated)
            {
                // Adjust the hour value based on whether the "AM" or "PM" text was in the original
                // string.
                if (isPm && hourValue < 12)
                {
                    hourValue += 12;
                }
                else if (isAm && hourValue >= 12)
                {
                    hourValue -= 12;
                }
            }

            return new Time(hourValue, minValue, secValue);
        }
        #endregion
    }
}