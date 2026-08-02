using Adaptive.Intelligence.Common;

namespace Adaptive.Intelligence.Framework.Tests.Common
{
    /// <summary>
    /// Gets the definition for TimeTests.
    /// </summary>
    public class TimeTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for ConstructorTests.
        /// </summary>
        public void ConstructorTests()
        {
            Time? t = new Time();
            Assert.NotNull(t);
            Assert.Equal(0, t.Value.TotalSeconds);
            Assert.Equal(0, t.Value.Second);
            Assert.Equal(0, t.Value.Minute);
            Assert.Equal(0, t.Value.Hour);

            t = new Time(50);
            Assert.NotNull(t);
            Assert.Equal(50, t.Value.TotalSeconds);
            Assert.Equal(50, t.Value.Second);
            Assert.Equal(0, t.Value.Minute);
            Assert.Equal(0, t.Value.Hour);

            t = new Time(-9700);
            Assert.NotNull(t);
            Assert.Equal(0, t.Value.TotalSeconds);
            Assert.Equal(0, t.Value.Second);
            Assert.Equal(0, t.Value.Minute);
            Assert.Equal(0, t.Value.Hour);

            t = new Time(Int32.MaxValue);
            Assert.NotNull(t);
            Assert.Equal(86399, t.Value.TotalSeconds);
            Assert.Equal(59, t.Value.Second);
            Assert.Equal(59, t.Value.Minute);
            Assert.Equal(23, t.Value.Hour);

        }

        [Fact]

        /// <summary>
        /// Gets the definition for Constructor_TotalSeconds_Valid.
        /// </summary>
        public void Constructor_TotalSeconds_Valid()
        {
            var time = new Time(3600);
            Assert.Equal(1, time.Hour);
            Assert.Equal(0, time.Minute);
            Assert.Equal(0, time.Second);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_HourMinute_Valid.
        /// </summary>
        public void Constructor_HourMinute_Valid()
        {
            var time = new Time(1, 30);
            Assert.Equal(1, time.Hour);
            Assert.Equal(30, time.Minute);
            Assert.Equal(0, time.Second);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_HourMinuteSecond_Valid.
        /// </summary>
        public void Constructor_HourMinuteSecond_Valid()
        {
            var time = new Time(1, 30, 45);
            Assert.Equal(1, time.Hour);
            Assert.Equal(30, time.Minute);
            Assert.Equal(45, time.Second);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_AddHours_Valid.
        /// </summary>
        public void Method_AddHours_Valid()
        {
            var time = new Time(1, 0, 0);
            var newTime = time.AddHours(2);
            Assert.Equal(3, newTime.Hour);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_AddMinutes_Valid.
        /// </summary>
        public void Method_AddMinutes_Valid()
        {
            var time = new Time(1, 0, 0);
            var newTime = time.AddMinutes(30);
            Assert.Equal(1, newTime.Hour);
            Assert.Equal(30, newTime.Minute);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_AddSeconds_Valid.
        /// </summary>
        public void Method_AddSeconds_Valid()
        {
            var time = new Time(1, 0, 0);
            var newTime = time.AddSeconds(45);
            Assert.Equal(1, newTime.Hour);
            Assert.Equal(0, newTime.Minute);
            Assert.Equal(45, newTime.Second);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_ToString_Valid.
        /// </summary>
        public void Method_ToString_Valid()
        {
            var time = new Time(13, 30, 0);
            Assert.Equal("1:30 PM", time.ToString());
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_ToString_WithAmPm.
        /// </summary>
        public void Method_ToString_WithAmPm()
        {
            var time = new Time(1, 30, 0);
            Assert.Equal("1:30:00", time.ToString(false));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_Parse_Valid.
        /// </summary>
        public void Method_Parse_Valid()
        {
            var time = Time.Parse("13:30:00");
            Assert.Equal(13, time.Hour);
            Assert.Equal(30, time.Minute);
            Assert.Equal(0, time.Second);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_TryParse_Valid.
        /// </summary>
        public void Method_TryParse_Valid()
        {
            bool success = Time.TryParse("13:30:00", out Time time);
            Assert.True(success);
            Assert.Equal(13, time.Hour);
            Assert.Equal(30, time.Minute);
            Assert.Equal(0, time.Second);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_Equals_Valid.
        /// </summary>
        public void Method_Equals_Valid()
        {
            var time1 = new Time(1, 30, 0);
            var time2 = new Time(1, 30, 0);
            Assert.True(time1.Equals(time2));
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Method_CompareTo_Valid.
        /// </summary>
        public void Method_CompareTo_Valid()
        {
            var time1 = new Time(1, 30, 0);
            var time2 = new Time(2, 0, 0);
            Assert.True(time1.CompareTo(time2) < 0);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for MinValueTest.
        /// </summary>
        public void MinValueTest()
        {
            Time? instance = Time.MinValue;

            Assert.NotNull(instance);
            Assert.Equal(0, instance.Value.Hour);
            Assert.Equal(0, instance.Value.Minute);
            Assert.Equal(0, instance.Value.Second);
            Assert.Equal(0, instance.Value.TotalSeconds);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for MaxValueTest.
        /// </summary>
        public void MaxValueTest()
        {
            Time? instance = Time.MaxValue;

            Assert.NotNull(instance);
            Assert.Equal(23, instance.Value.Hour);
            Assert.Equal(59, instance.Value.Minute);
            Assert.Equal(59, instance.Value.Second);
            Assert.Equal(86399, instance.Value.TotalSeconds);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for NowTest.
        /// </summary>
        public void NowTest()
        {
            Time? instance = Time.Now;
            DateTime dt = DateTime.Now;

            Assert.NotNull(instance);
            Assert.Equal(dt.Hour, instance.Value.Hour);
            Assert.Equal(dt.Minute, instance.Value.Minute);
            Assert.Equal(dt.Second, instance.Value.Second);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CompareToInvalidObjectTest.
        /// </summary>
        public void CompareToInvalidObjectTest()
        {
            Time t = Time.Now;
            DateTime dt = DateTime.Now;

            bool isGood = false;
            try
            {
                t.CompareTo(dt);
            }
            catch (ArgumentException)
            {
                isGood = true;
            }

            Assert.True(isGood);
        }

        [Fact]
        public void Constructor_TotalSeconds_ExceedsMax_ClampsToMaxValue()
        {
            // Arrange
            int totalSeconds = 86400;

            // Act
            Time time = new Time(totalSeconds);

            // Assert
            Assert.Equal(23, time.Hour);
            Assert.Equal(59, time.Minute);
            Assert.Equal(59, time.Second);
            Assert.Equal(86399, time.TotalSeconds);
        }

        [Fact]
        public void Constructor_TotalSeconds_Negative_ClampsToZero()
        {
            // Arrange
            int totalSeconds = -1;

            // Act
            Time time = new Time(totalSeconds);

            // Assert
            Assert.Equal(0, time.Hour);
            Assert.Equal(0, time.Minute);
            Assert.Equal(0, time.Second);
            Assert.Equal(0, time.TotalSeconds);
        }

        [Fact]
        public void Constructor_TotalSeconds_MaxBoundary_UsesProvidedValue()
        {
            // Arrange
            int totalSeconds = 86399;

            // Act
            Time time = new Time(totalSeconds);

            // Assert
            Assert.Equal(23, time.Hour);
            Assert.Equal(59, time.Minute);
            Assert.Equal(59, time.Second);
            Assert.Equal(86399, time.TotalSeconds);
        }

        [Fact]
        public void Constructor_HourMinute_ValuesOutOfRange_ClampsHourAndMinute()
        {
            // Arrange
            int hour = 99;
            int minute = -3;

            // Act
            Time time = new Time(hour, minute);

            // Assert
            Assert.Equal(23, time.Hour);
            Assert.Equal(0, time.Minute);
            Assert.Equal(0, time.Second);
            Assert.Equal(82800, time.TotalSeconds);
        }

        [Fact]
        public void Constructor_HourMinuteSecond_ValuesOutOfRange_ClampsAllComponents()
        {
            // Arrange
            int hour = -5;
            int minute = 100;
            int second = 65;

            // Act
            Time time = new Time(hour, minute, second);

            // Assert
            Assert.Equal(0, time.Hour);
            Assert.Equal(59, time.Minute);
            Assert.Equal(59, time.Second);
            Assert.Equal(3599, time.TotalSeconds);
        }

        [Fact]
        public void Property_Now_Called_ReturnsTimeWithinCapturedRange()
        {
            // Arrange
            DateTime before = DateTime.Now;

            // Act
            Time actual = Time.Now;

            // Assert
            DateTime after = DateTime.Now;
            int beforeSeconds = (before.Hour * 3600) + (before.Minute * 60) + before.Second;
            int afterSeconds = (after.Hour * 3600) + (after.Minute * 60) + after.Second;

            if (afterSeconds >= beforeSeconds)
            {
                Assert.InRange(actual.TotalSeconds, beforeSeconds, afterSeconds);
            }
            else
            {
                Assert.True(actual.TotalSeconds >= beforeSeconds || actual.TotalSeconds <= afterSeconds);
            }
        }

        [Fact]
        public void Property_Hour_TotalSecondsHasRemainder_ReturnsWholeHourComponent()
        {
            // Arrange
            int totalSeconds = 7199;

            // Act
            Time time = new Time(totalSeconds);

            // Assert
            Assert.Equal(1, time.Hour);
        }



        [Fact]
        public void Minute_TotalSecondsContainsHoursAndMinutes_ReturnsMinuteComponent()
        {
            // Arrange
            Time time = new Time(2, 45, 10);

            // Act
            int minute = time.Minute;

            // Assert
            Assert.Equal(45, minute);
        }


        [Fact]
        public void Second_TotalSecondsContainsHoursMinutesAndSeconds_ReturnsSecondComponent()
        {
            // Arrange
            Time time = new Time(2, 45, 10);

            // Act
            int second = time.Second;

            // Assert
            Assert.Equal(10, second);
        }

        [Fact]
        public void TotalSeconds_ConstructedFromComponents_ReturnsExpectedTotal()
        {
            // Arrange
            Time time = new Time(3, 4, 5);

            // Act
            int totalSeconds = time.TotalSeconds;

            // Assert
            Assert.Equal(11045, totalSeconds);
        }

        [Fact]
        public void AddHours_ResultExceedsMaxValue_ClampsToMaxValue()
        {
            // Arrange
            Time time = new Time(22, 0, 0);

            // Act
            Time result = time.AddHours(5);

            // Assert
            Assert.Equal(23, result.Hour);
            Assert.Equal(59, result.Minute);
            Assert.Equal(59, result.Second);
            Assert.Equal(86399, result.TotalSeconds);
        }

        [Fact]
        public void AddHours_ResultBelowZero_ClampsToMinValue()
        {
            // Arrange
            Time time = new Time(1, 0, 0);

            // Act
            Time result = time.AddHours(-5);

            // Assert
            Assert.Equal(0, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);
            Assert.Equal(0, result.TotalSeconds);
        }

        [Fact]
        public void AddMinutes_ResultExceedsMaxValue_ClampsToMaxValue()
        {
            // Arrange
            Time time = new Time(23, 50, 0);

            // Act
            Time result = time.AddMinutes(30);

            // Assert
            Assert.Equal(23, result.Hour);
            Assert.Equal(59, result.Minute);
            Assert.Equal(59, result.Second);
            Assert.Equal(86399, result.TotalSeconds);
        }

        [Fact]
        public void AddMinutes_ResultBelowZero_ClampsToMinValue()
        {
            // Arrange
            Time time = new Time(0, 10, 0);

            // Act
            Time result = time.AddMinutes(-30);

            // Assert
            Assert.Equal(0, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);
            Assert.Equal(0, result.TotalSeconds);
        }


        [Fact]
        public void CompareTo_ObjectIsNull_ThrowsArgumentException()
        {
            // Arrange
            Time time = new Time(1, 0, 0);

            // Act
            Action action = () => time.CompareTo((object?)null);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("obj", exception.ParamName);
        }


        [Fact]
        public void AddSeconds_ResultExceedsMaxValue_ClampsToMaxValue()
        {
            // Arrange
            Time time = new Time(23, 59, 50);

            // Act
            Time result = time.AddSeconds(15);

            // Assert
            Assert.Equal(23, result.Hour);
            Assert.Equal(59, result.Minute);
            Assert.Equal(59, result.Second);
            Assert.Equal(86399, result.TotalSeconds);
        }

        [Fact]
        public void AddSeconds_ResultBelowZero_ClampsToMinValue()
        {
            // Arrange
            Time time = new Time(0, 0, 10);

            // Act
            Time result = time.AddSeconds(-20);

            // Assert
            Assert.Equal(0, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);
            Assert.Equal(0, result.TotalSeconds);
        }

        [Fact]
        public void CompareTo_ObjectIsTimeWithHigherValue_ReturnsNegativeNumber()
        {
            // Arrange
            Time current = new Time(1, 0, 0);
            object other = new Time(2, 0, 0);

            // Act
            int result = current.CompareTo(other);

            // Assert
            Assert.True(result < 0);
        }

        [Fact]
        public void CompareTo_ObjectIsTimeWithSameValue_ReturnsZero()
        {
            // Arrange
            Time current = new Time(1, 15, 30);
            object other = new Time(1, 15, 30);

            // Act
            int result = current.CompareTo(other);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CompareTo_TimeIsLowerThanCurrent_ReturnsPositiveNumber()
        {
            // Arrange
            Time current = new Time(3, 0, 0);
            Time other = new Time(2, 59, 59);

            // Act
            int result = current.CompareTo(other);

            // Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void CompareTo_TimeIsEqualToCurrent_ReturnsZero()
        {
            // Arrange
            Time current = new Time(3, 5, 10);
            Time other = new Time(3, 5, 10);

            // Act
            int result = current.CompareTo(other);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Equals_ObjectIsNull_ReturnsFalse()
        {
            // Arrange
            Time current = new Time(1, 0, 0);

            // Act
            bool result = current.Equals((object?)null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ObjectIsDifferentType_ReturnsFalse()
        {
            // Arrange
            Time current = new Time(1, 0, 0);

            // Act
            bool result = current.Equals("01:00:00");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ObjectIsTimeWithDifferentValue_ReturnsFalse()
        {
            // Arrange
            Time current = new Time(1, 0, 0);
            object other = new Time(1, 0, 1);

            // Act
            bool result = current.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ObjectIsTimeWithSameValue_ReturnsTrue()
        {
            // Arrange
            Time current = new Time(4, 20, 10);
            object other = new Time(4, 20, 10);

            // Act
            bool result = current.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_TimeHasDifferentValue_ReturnsFalse()
        {
            // Arrange
            Time current = new Time(5, 10, 15);
            Time other = new Time(5, 10, 16);

            // Act
            bool result = current.Equals(other);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void GetHashCode_SameTimeValue_ReturnsTotalSeconds()
        {
            // Arrange
            Time time = new Time(5, 10, 15);

            // Act
            int hashCode = time.GetHashCode();

            // Assert
            Assert.Equal(time.TotalSeconds, hashCode);
        }

        [Fact]
        public void ToDate_DateTimeInput_ReturnsDateWithCurrentTimeComponents()
        {
            // Arrange
            Time time = new Time(14, 25, 30);
            DateTime originalDate = new DateTime(2024, 12, 31, 1, 2, 3);

            // Act
            DateTime result = time.ToDate(originalDate);

            // Assert
            Assert.Equal(2024, result.Year);
            Assert.Equal(12, result.Month);
            Assert.Equal(31, result.Day);
            Assert.Equal(14, result.Hour);
            Assert.Equal(25, result.Minute);
            Assert.Equal(30, result.Second);
        }

        [Fact]
        public void ToDate_DateTimeOffsetInput_ReturnsDateWithCurrentTimeComponents()
        {
            // Arrange
            Time time = new Time(6, 7, 8);
            DateTimeOffset originalDate = new DateTimeOffset(2025, 1, 15, 22, 45, 50, TimeSpan.FromHours(-5));

            // Act
            DateTime result = time.ToDate(originalDate);

            // Assert
            Assert.Equal(2025, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(15, result.Day);
            Assert.Equal(6, result.Hour);
            Assert.Equal(7, result.Minute);
            Assert.Equal(8, result.Second);
        }

        [Fact]
        public void ToString_HourIsZero_ReturnsMidnightAmFormat()
        {
            // Arrange
            Time time = new Time(0, 5, 0);

            // Act
            string result = time.ToString();

            // Assert
            Assert.Equal("12:05 AM", result);
        }

        [Fact]
        public void ToString_HourIsMorningNonZero_ReturnsAmFormat()
        {
            // Arrange
            Time time = new Time(9, 8, 0);

            // Act
            string result = time.ToString();

            // Assert
            Assert.Equal("9:08 AM", result);
        }

        [Fact]
        public void ToString_HourIsTwelve_ReturnsNoonPmFormat()
        {
            // Arrange
            Time time = new Time(12, 0, 0);

            // Act
            string result = time.ToString();

            // Assert
            Assert.Equal("12:00 PM", result);
        }

        [Fact]
        public void ToString_WithAmPmTrue_ReturnsSameAsDefaultToString()
        {
            // Arrange
            Time time = new Time(15, 45, 9);

            // Act
            string result = time.ToString(true);

            // Assert
            Assert.Equal(time.ToString(), result);
        }



        [Fact]
        public void ToString_FormatStringProvided_ReturnsFormattedTimeText()
        {
            // Arrange
            Time time = new Time(13, 5, 7);

            // Act
            string result = time.ToString("HH:mm:ss");

            // Assert
            Assert.Equal("13:05:07", result);
        }


        [Fact]
        public void FromDate_DateTimeOffsetProvided_ReturnsTimeWithMatchingComponents()
        {
            // Arrange
            DateTimeOffset source = new DateTimeOffset(2026, 2, 3, 21, 22, 23, TimeSpan.FromHours(4));

            // Act
            Time result = Time.FromDate(source);

            // Assert
            Assert.Equal(21, result.Hour);
            Assert.Equal(22, result.Minute);
            Assert.Equal(23, result.Second);
        }

        [Fact]
        public void FromDate_DateTimeProvided_ReturnsTimeWithMatchingComponents()
        {
            // Arrange
            DateTime source = new DateTime(2026, 8, 9, 4, 5, 6);

            // Act
            Time result = Time.FromDate(source);

            // Assert
            Assert.Equal(4, result.Hour);
            Assert.Equal(5, result.Minute);
            Assert.Equal(6, result.Second);
        }

        [Fact]
        public void Parse_NumericFourDigits_ReturnsHourMinuteWithZeroSeconds()
        {
            // Arrange
            string timeText = "2301";

            // Act
            Time result = Time.Parse(timeText);

            // Assert
            Assert.Equal(23, result.Hour);
            Assert.Equal(1, result.Minute);
            Assert.Equal(0, result.Second);
        }

        [Fact]
        public void Parse_NumericSixDigits_ReturnsHourMinuteSecond()
        {
            // Arrange
            string timeText = "112233";

            // Act
            Time result = Time.Parse(timeText);

            // Assert
            Assert.Equal(11, result.Hour);
            Assert.Equal(22, result.Minute);
            Assert.Equal(33, result.Second);
        }

        [Fact]
        public void Parse_NumericLengthNotFourOrSix_ThrowsArgumentException()
        {
            // Arrange
            string timeText = "12345";

            // Act
            Action action = () => Time.Parse(timeText);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("timeString", exception.ParamName);
        }

        [Fact]
        public void Parse_NullInput_ThrowsArgumentNullException()
        {
            // Arrange
            string? timeText = null;

            // Act
            Action action = () => Time.Parse(timeText!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(action);
            Assert.Equal("timeString", exception.ParamName);
        }

        [Fact]
        public void Parse_EmptyInput_ThrowsArgumentException()
        {
            // Arrange
            string timeText = string.Empty;

            // Act
            Action action = () => Time.Parse(timeText);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("timeString", exception.ParamName);
        }

        [Fact]
        public void Parse_DelimitedHourMinuteWithPm_Returns24HourTime()
        {
            // Arrange
            string timeText = "9:45 PM";

            // Act
            Time result = Time.Parse(timeText);

            // Assert
            Assert.Equal(21, result.Hour);
            Assert.Equal(45, result.Minute);
            Assert.Equal(0, result.Second);
        }

        [Fact]
        public void TryParse_InvalidInput_ReturnsFalseAndMinValue()
        {
            // Arrange
            string timeText = "invalid";

            // Act
            bool success = Time.TryParse(timeText, out Time result);

            // Assert
            Assert.False(success);
            Assert.Equal(Time.MinValue, result);
        }


        [Fact]
        public void RenderHourText_HourIsZero_ReturnsTwelve()
        {
            // Arrange
            int hour = 0;

            // Act
            string result = Time.RenderHourText(hour);

            // Assert
            Assert.Equal("12", result);
        }


        [Fact]
        public void RenderHourText_HourIsTwelve_ReturnsHourText()
        {
            // Arrange
            int hour = 12;

            // Act
            string result = Time.RenderHourText(hour);

            // Assert
            Assert.Equal("12", result);
        }

        [Fact]
        public void RenderHourText_HourIsGreaterThanTwelve_ReturnsAdjustedHourText()
        {
            // Arrange
            int hour = 13;

            // Act
            string result = Time.RenderHourText(hour);

            // Assert
            Assert.Equal("1", result);
        }

        [Fact]
        public void RenderAmPm_HourIsLessThanTwelve_ReturnsAm()
        {
            // Arrange
            int hour = 11;

            // Act
            string result = Time.RenderAmPm(hour);

            // Assert
            Assert.Equal("AM", result);
        }

        [Fact]
        public void RenderAmPm_HourIsTwelve_ReturnsPm()
        {
            // Arrange
            int hour = 12;

            // Act
            string result = Time.RenderAmPm(hour);

            // Assert
            Assert.Equal("PM", result);
        }

    }
}