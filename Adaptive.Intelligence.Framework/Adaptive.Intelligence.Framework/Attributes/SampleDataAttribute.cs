namespace Adaptive.Intelligence.Attributes
{
    /// <summary>
    /// Provides a data attribute for a property indicating the sample data content
    /// for the property the attribute is applied to.
    /// </summary>
    /// <seealso cref="Attribute" />
    /// <seealso cref="ExportPropertyAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SampleDataAttribute : ExportPropertyAttribute
    {
        #region Private Member Declarations		
        /// <summary>
        /// The sample data text to be displayed.
        /// </summary>
        private readonly string? _data;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDataAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public SampleDataAttribute()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDataAttribute"/> class.
        /// </summary>
        /// <param name="sampleDataText">
        /// A string containing the sample data text.
        /// </param>
        public SampleDataAttribute(string? sampleDataText)
        {
            _data = sampleDataText;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the sample data text.
        /// </summary>
        /// <value>
        /// A string containing the sample data text.
        /// </value>
        public string? SampleData => _data;
        #endregion
    }
}