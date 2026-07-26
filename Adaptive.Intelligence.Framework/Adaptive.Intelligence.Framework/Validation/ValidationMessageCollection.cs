using Adaptive.Intelligence.Common.Abstractions;

namespace Adaptive.Intelligence.Validation;

/// <summary>
/// Contains a list of <see cref="ValidationMessage" /> instances.
/// </summary>
/// <seealso cref="ValidationMessage" />
/// <seealso cref="List{T}" />
public class ValidationMessageCollection : AdaptiveCollectionBase<ValidationMessage>
{
    /// <summary>
    /// Determines whether all the entries in the list are valid.
    /// </summary>
    /// <returns>
    /// <b>true</b> if all entries in the list are valid, or if the list is empty.
    /// </returns>
    public bool AreAllValid()
    {
        bool valid = true;
        if (Count > 0)
        {
            int length = Count;
            int index = 0;

            do
            {
                valid = this[index].IsValid;
                index++;
            } while (valid && index < length);
        }
        return valid;
    }
}
