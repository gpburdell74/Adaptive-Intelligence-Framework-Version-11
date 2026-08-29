namespace Adaptive.Intelligence.Csv.Parser;

/// <summary>
/// Provides the signature definition for implementing CSV parsing classes.
/// </summary>
/// <seealso cref="IDisposable" />
public interface ICsvParser : IDisposable
{
    /// <summary>
    /// Parses the line of text into the individual data elements.
    /// </summary>
    /// <param name="originalContent">
    /// A string containing the content to be parsed.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> of <see cref="string"/> values parsed from the original.
    /// </returns>
    List<string>? ParseLine(string? originalContent);
}
