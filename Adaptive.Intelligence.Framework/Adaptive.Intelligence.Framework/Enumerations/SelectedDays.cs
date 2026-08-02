namespace Adaptive.Intelligence.Enumerations
{
    /// <summary>
    /// Represents the days of the week, in a bitwise combination.
    /// </summary>
    /// <remarks>
    /// This is used to allow the "selection" of one or more days.
    /// </remarks>
    [Flags()]
    public enum SelectedDays
    {
        /// <summary>
        /// Indicates no days are selected.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates Sunday is selected.
        /// </summary>
        Sunday = 1,
        /// <summary>
        /// Indicates Monday is selected.
        /// </summary>
        Monday = 2,
        /// <summary>
        /// Indicates Tuesday is selected.
        /// </summary>
        Tuesday = 4,
        /// <summary>
        /// Indicates Wednesday is selected.
        /// </summary>
        Wednesday = 8,
        /// <summary>
        /// Indicates Thursday is selected.
        /// </summary>
        Thursday = 16,
        /// <summary>
        /// Indicates Friday is selected.
        /// </summary>
        Friday = 32,
        /// <summary>
        /// Indicates Saturday is selected.
        /// </summary>
        Saturday = 64
    }
}
