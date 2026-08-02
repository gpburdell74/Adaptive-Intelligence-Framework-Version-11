namespace Adaptive.Intelligence.Attributes
{
    /// <summary>
    /// Provides a property decoration attribute to allow an import process
    /// to ignore a specified property when performing an import.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ImportIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportIgnoreAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public ImportIgnoreAttribute()
        {
        }
    }
}