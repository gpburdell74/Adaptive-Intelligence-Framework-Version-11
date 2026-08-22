using Adaptive.Intelligence.Common;
using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Enumerations;
using Adaptive.Intelligence.Extensions;
using System.Globalization;

namespace Adaptive.Intelligence.Framework.Tests.Extensions
{
    /// <summary>
    /// Contains tests for <see cref="DateTimeExtensions"/>.
    /// </summary>
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void FirstDayOfPreviousMonth_Returns_Current_Implementation_Value()
        {
            DateTime value = new(2026, 3, 15);

            DateTime result = value.FirstDayOfPreviousMonth();

            Assert.Equal(new DateTime(2026, 1, 16), result);
        }

        [Theory]
        [InlineData(1000, true)]
        [InlineData(1754, false)]
        [InlineData(3000, false)]
        [InlineData(3001, true)]
        public void IsRidiculousDate_Returns_Expected(int year, bool expected)
        {
            DateTime value = new(year, 1, 1);

            bool result = value.IsRidiculousDate();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateBiMonthlyDates_Returns_First_And_Fifteenth()
        {
            DateTime start = new(2026, 1, 1);
            DateTime end = new(2026, 2, 15);

            List<DateTime> result = start.GenerateBiMonthlyDates(end);

            Assert.Equal(
                [
                    new DateTime(2026, 1, 1),
                    new DateTime(2026, 1, 15),
                    new DateTime(2026, 2, 1),
                    new DateTime(2026, 2, 15)
                ],
                result);
        }

        [Fact]
        public void GenerateByDaysOfWeekEachWeek_Returns_Selected_Days()
        {
            DateTime start = new(2026, 1, 4); // Sunday
            DateTime end = new(2026, 1, 10);  // Saturday

            List<DateTime> result = start.GenerateByDaysOfWeekEachWeek(
                end,
                SelectedDays.Monday | SelectedDays.Wednesday | SelectedDays.Friday);

            Assert.Equal(
                [
                    new DateTime(2026, 1, 5),
                    new DateTime(2026, 1, 7),
                    new DateTime(2026, 1, 9)
                ],
                result);
        }

        [Fact]
        public void GenerateByIntervalDaysFromEndDate_Returns_Dates_In_Range()
        {
            DateTime start = new(2026, 1, 1);
            DateTime end = new(2026, 1, 20);
            DateTime calcEnd = new(2026, 1, 20);

            List<DateTime> result = start.GenerateByIntervalDaysFromEndDate(end, calcEnd, 5);

            Assert.Equal(
                [
                    new DateTime(2026, 1, 5),
                    new DateTime(2026, 1, 10),
                    new DateTime(2026, 1, 15),
                    new DateTime(2026, 1, 20)
                ],
                result);
        }

        [Fact]
        public void GenerateByIntervalDaysFromStartDate_Returns_Dates_In_Range()
        {
            DateTime start = new(2026, 1, 1);
            DateTime end = new(2026, 1, 20);
            DateTime calcStart = new(2026, 1, 1);

            List<DateTime> result = start.GenerateByIntervalDaysFromStartDate(end, calcStart, 5);

            Assert.Equal(
                [
                    new DateTime(2026, 1, 1),
                    new DateTime(2026, 1, 6),
                    new DateTime(2026, 1, 11),
                    new DateTime(2026, 1, 16)
                ],
                result);
        }

        [Fact]
        public void GenerateFirstOfMonthDates_Returns_First_Days()
        {
            DateTime start = new(2026, 1, 15);
            DateTime end = new(2026, 4, 2);

            List<DateTime> result = start.GenerateFirstOfMonthDates(end);

            Assert.Equal(
                [
                    new DateTime(2026, 2, 1),
                    new DateTime(2026, 3, 1),
                    new DateTime(2026, 4, 1)
                ],
                result);
        }

        [Fact]
        public void GenerateOnceByIntervalDaysFromEndDate_Returns_Single_Date()
        {
            DateTime end = new(2026, 1, 20);

            List<DateTime> result = end.GenerateOnceByIntervalDaysFromEndDate(3);

            Assert.Single(result);
            Assert.Equal(new DateTime(2026, 1, 17), result[0]);
        }

        [Fact]
        public void GenerateOnceByIntervalDaysFromStartDate_Returns_Single_Date()
        {
            DateTime start = new(2026, 1, 20);

            List<DateTime> result = start.GenerateOnceByIntervalDaysFromStartDate(3);

            Assert.Single(result);
            Assert.Equal(new DateTime(2026, 1, 23), result[0]);
        }

        [Fact]
        public void GenerateRangeInstances_Returns_Inclusive_Range()
        {
            DateTime start = new(2026, 1, 1);
            DateTime end = new(2026, 1, 3);

            List<DateTime> result = start.GenerateRangeInstances(end);

            Assert.Equal(
                [
                    new DateTime(2026, 1, 1),
                    new DateTime(2026, 1, 2),
                    new DateTime(2026, 1, 3)
                ],
                result);
        }

        [Fact]
        public void GenerateSpecificDate_Returns_Single_Same_Date()
        {
            DateTime date = new(2026, 1, 20, 14, 30, 0);

            List<DateTime> result = date.GenerateSpecificDate();

            Assert.Single(result);
            Assert.Equal(date, result[0]);
        }

        [Fact]
        public void GetTime_Returns_Time_With_Matching_Components()
        {
            DateTime date = new(2026, 1, 20, 14, 30, 45);

            Time result = date.GetTime();

            Assert.Equal(14, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(45, result.Second);
        }

        [Fact]
        public void LastDayOfTheMonth_Returns_Last_Calendar_Day()
        {
            DateTime date = new(2024, 2, 10);

            DateTime result = date.LastDayOfTheMonth();

            Assert.Equal(new DateTime(2024, 2, 29), result);
        }

        [Fact]
        public void NowAsString_Returns_USFullDateFormat()
        {
            string value = DateTimeExtensions.NowAsString();

            bool parsed = DateTime.TryParseExact(
                value,
                DateConstants.USFullDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);

            Assert.True(parsed);
        }
    }
}