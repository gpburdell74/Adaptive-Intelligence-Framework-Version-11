namespace Adaptive.Intelligence.Attributes
{
    /// <summary>
    /// Provides a marker attribute for a property indicating the value of the specified property
    /// is to be exported in an export process.
    /// </summary>
    /// <seealso cref="Attribute" />
    /// <seealso cref="ExportIgnoreAttribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportPropertyAttribute : Attribute
    {
    }
}