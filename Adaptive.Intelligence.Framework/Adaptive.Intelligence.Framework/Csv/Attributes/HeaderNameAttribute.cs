namespace Adaptive.Intelligence.Csv.Attributes
{
    /// <summary>
    /// Provides an attribute to describe a CSV column's header text.
    /// </summary>
    /// <remarks>
    /// Initializes an new instance of the <see cref="HeaderNameAttribute"/> class.
    /// </remarks>
    /// <param name="headerText">
    /// A string containing the column's header text, if present.
    /// </param>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HeaderNameAttribute(string? headerText) : Attribute
    {

        /// <summary>
        /// Gets or sets the text of the header for the CSV column.
        /// </summary>
        /// <value>
        /// A string containing the text of the header for the CSV column.
        /// </value>
        public string? HeaderName { get; init; } = headerText;
    }
}
