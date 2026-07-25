using Adaptive.Intelligence.Framework.Tests.Mocks;

namespace Adaptive.Intelligence.Framework.Tests.Common;

/// <summary>
/// Provides the tests for the <see cref="PropertyAwareBase"/> abstract class.
/// </summary>
public class PropertyAwareBaseTests
{
    /// <summary>
    ///  Test to ensure the PropertyChanged event is raised correctly.
    /// </summary>
    [Fact]
    public void Property_Changed_Event_Is_Raised()
    {
        var mock = new MockPropertyAwareBase();
        var eventRaised = false;
        string propertyName = "TestProperty";

        mock.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            Assert.Equal(propertyName, args.PropertyName);
        };

        mock.InvokeOnPropertyChanged(propertyName);

        Assert.True(eventRaised, "PropertyChanged event was not raised.");
    }

    /// <summary>
    /// Test to ensure the class handles null property names without throwing exceptions 
    /// </summary>
    [Fact]
    public void Property_Changed_Event_Handles_Null_Property_Name()
    {
        var mock = new MockPropertyAwareBase();
        var eventRaised = false;

        mock.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            Assert.True(string.IsNullOrEmpty(args.PropertyName));
        };

        mock.InvokeOnPropertyChanged(null);

        Assert.False(eventRaised, "PropertyChanged event should handle null property names.");
    }

    [Fact]
    public void Property_Value_Change_Notification_Works()
    {
        var mock = new MockPropertyAwareBase();
        var eventRaised = false;
        mock.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            Assert.Equal(nameof(MockPropertyAwareBase.TestProperty), args.PropertyName);
        };

        // Simulate a property change
        mock.TestProperty = "New Value";

        Assert.True(eventRaised, "PropertyChanged event was not raised on property value change.");
    }
}
