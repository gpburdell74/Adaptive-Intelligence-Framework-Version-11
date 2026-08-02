using Adaptive.Intelligence.Common;

namespace Adaptive.Intelligence.Framework.Tests.Common
{
    /// <summary>
    /// Gets the definition for DateRangeTests.
    /// </summary>
    public class DateRangeTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for NewDateRange_ReturnsMinToMaxRange.
        /// </summary>
        public void NewDateRange_ReturnsMinToMaxRange()
        {
            DateRange range = DateRange.NewDateRange();

            Assert.Equal(DateTime.MinValue, range.StartDate);
            Assert.Equal(DateTime.MaxValue, range.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithStartBeforeEnd_PreservesOrder.
        /// </summary>
        public void Constructor_WithStartBeforeEnd_PreservesOrder()
        {
            DateTime start = new(2026, 1, 1);
            DateTime end = new(2026, 1, 10);

            DateRange range = new(start, end);

            Assert.Equal(start, range.StartDate);
            Assert.Equal(end, range.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithStartAfterEnd_SwapsValues.
        /// </summary>
        public void Constructor_WithStartAfterEnd_SwapsValues()
        {
            DateTime later = new(2026, 2, 5);
            DateTime earlier = new(2026, 2, 1);

            DateRange range = new(later, earlier);

            Assert.Equal(earlier, range.StartDate);
            Assert.Equal(later, range.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for EnsureOrder_WhenDatesAreReversed_SwapsValues.
        /// </summary>
        public void EnsureOrder_WhenDatesAreReversed_SwapsValues()
        {
            DateRange range = new()
            {
                StartDate = new DateTime(2026, 3, 20),
                EndDate = new DateTime(2026, 3, 10)
            };

            range.EnsureOrder();

            Assert.Equal(new DateTime(2026, 3, 10), range.StartDate);
            Assert.Equal(new DateTime(2026, 3, 20), range.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for EnsureOrder_WhenDatesAlreadyInOrder_DoesNotChangeValues.
        /// </summary>
        public void EnsureOrder_WhenDatesAlreadyInOrder_DoesNotChangeValues()
        {
            DateRange range = new()
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30)
            };

            range.EnsureOrder();

            Assert.Equal(new DateTime(2026, 4, 1), range.StartDate);
            Assert.Equal(new DateTime(2026, 4, 30), range.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SetDayStartAndEnd_SetsStartToMidnight.
        /// </summary>
        public void SetDayStartAndEnd_SetsStartToMidnight()
        {
            DateRange range = new()
            {
                StartDate = new DateTime(2026, 5, 15, 14, 35, 12),
                EndDate = new DateTime(2026, 5, 15)
            };

            range.SetDayStartAndEnd();

            Assert.Equal(new DateTime(2026, 5, 15, 0, 0, 0), range.StartDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SetDayStartAndEnd_WhenEndDateIsDateOnly_SetsEndToLastSecondOfDay.
        /// </summary>
        public void SetDayStartAndEnd_WhenEndDateIsDateOnly_SetsEndToLastSecondOfDay()
        {
            DateRange range = new()
            {
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 1)
            };

            range.SetDayStartAndEnd();

            Assert.Equal(new DateTime(2026, 6, 1, 23, 59, 59), range.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SetDayStartAndEnd_PreservesDateSpanAcrossMultipleDays.
        /// </summary>
        public void SetDayStartAndEnd_PreservesDateSpanAcrossMultipleDays()
        {
            DateRange range = new()
            {
                StartDate = new DateTime(2026, 7, 1, 11, 0, 0),
                EndDate = new DateTime(2026, 7, 3)
            };

            range.SetDayStartAndEnd();

            Assert.Equal(new DateTime(2026, 7, 1, 0, 0, 0), range.StartDate);
            Assert.Equal(new DateTime(2026, 7, 3, 23, 59, 59), range.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithEqualDates_PreservesSameValues.
        /// </summary>
        public void Constructor_WithEqualDates_PreservesSameValues()
        {
            DateTime value = new(2026, 8, 8, 12, 30, 45);

            DateRange range = new(value, value);

            Assert.Equal(value, range.StartDate);
            Assert.Equal(value, range.EndDate);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for NewDateRange_CalledMultipleTimes_ReturnsIndependentValues.
        /// </summary>
        public void NewDateRange_CalledMultipleTimes_ReturnsIndependentValues()
        {
            DateRange first = DateRange.NewDateRange();
            DateRange second = DateRange.NewDateRange();

            first.StartDate = new DateTime(2026, 12, 31);

            Assert.Equal(DateTime.MinValue, second.StartDate);
            Assert.Equal(DateTime.MaxValue, second.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for EnsureOrder_WhenDatesAreEqual_DoesNotChangeValues.
        /// </summary>
        public void EnsureOrder_WhenDatesAreEqual_DoesNotChangeValues()
        {
            DateTime value = new(2026, 9, 9);
            DateRange range = new()
            {
                StartDate = value,
                EndDate = value
            };

            range.EnsureOrder();

            Assert.Equal(value, range.StartDate);
            Assert.Equal(value, range.EndDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SetDayStartAndEnd_WhenEndContainsTime_SetsEndToLastSecondOfThatDay.
        /// </summary>
        public void SetDayStartAndEnd_WhenEndContainsTime_SetsEndToLastSecondOfThatDay()
        {
            DateRange range = new()
            {
                StartDate = new DateTime(2026, 10, 1, 2, 3, 4),
                EndDate = new DateTime(2026, 10, 3, 6, 7, 8)
            };

            range.SetDayStartAndEnd();

            Assert.Equal(new DateTime(2026, 10, 1, 0, 0, 0), range.StartDate);
            Assert.Equal(new DateTime(2026, 10, 3, 23, 59, 59), range.EndDate);
        }

    }
}