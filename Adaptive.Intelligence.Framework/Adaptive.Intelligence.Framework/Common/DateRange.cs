namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Represents a start and ending date.
    /// </summary>
    public struct DateRange
    {
        /// <summary>
        /// Gets or sets the starting date value.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the starting date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the ending date value.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the ending date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange"/> structure.
        /// </summary>
        public static DateRange NewDateRange()
        {
            return new DateRange
            {
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MaxValue
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRange"/> structure.
        /// </summary>
        /// <param name="startDate">
        /// A <see cref="DateTime"/> value representing the starting date.
        /// </param>
        /// <param name="endDate">
        /// A <see cref="DateTime"/> value representing the ending date.
        /// </param>
        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            EnsureOrder();
        }

        /// <summary>
        /// Ensures the start date occurs before the ending date.
        /// </summary>
        public void EnsureOrder()
        {
            if (StartDate > EndDate)
            {
                // Swap values via deconstruction... 
                (EndDate, StartDate) = (StartDate, EndDate);
            }
        }

        /// <summary>
        /// Sets the starting date time to midnight, and the ending date to the last second before midnight.
        /// </summary>
        public void SetDayStartAndEnd()
        {
            StartDate = StartDate.Date;
            EndDate = EndDate.Date.AddDays(1).AddSeconds(-1);
        }
    }
}