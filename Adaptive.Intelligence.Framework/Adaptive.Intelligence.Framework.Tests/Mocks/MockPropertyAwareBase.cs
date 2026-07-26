using Adaptive.Intelligence.Common.Abstractions;

namespace Adaptive.Intelligence.Framework.Tests.Mocks;

/// <summary>
/// Provides a testable wrapper for the <see cref="PropertyAwareBase"/> abstract class.
/// </summary>
public class MockPropertyAwareBase : PropertyAwareBase
{
    /// <summary>
    /// Gets or sets the value of the property to test.
    /// </summary>
    /// <value>
    /// A string value or <b>null</b>.
    /// </value>
    public string? TestProperty
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged(nameof(TestProperty));
        }
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event for testing purposes.
    /// </summary>
    /// <param name="propertyName">
    /// A string containing the name of the property whose value was changed.
    /// </param>
    public void InvokeOnPropertyChanged(string? propertyName)
    {
        OnPropertyChanged(propertyName);
    }
}
