namespace Adaptive.Intelligence.Attributes
{
    /// <summary>
    /// Provides a marker attribute for a property indicating that the specified property
    /// is to be ignored in an export process.
    /// </summary>
    /// <seealso cref="Attribute" />
    /// <seealso cref="ExportPropertyAttribute"/>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class ExportIgnoreAttribute : ExportPropertyAttribute
    {
    }
}