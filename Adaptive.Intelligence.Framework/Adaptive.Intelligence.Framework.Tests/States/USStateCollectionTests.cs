using Adaptive.Intelligence.States;
using Xunit;

namespace Adaptive.Intelligence.Framework.Tests;

public class USStateCollectionTests
{
    [Fact]
    public void GetStateByAbbreviation_AbbreviationIsNull_ReturnsNull()
    {
        // Arrange
        USStateCollection collection = [];

        // Act
        USState? result = collection.GetStateByAbbreviation(null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetStateByAbbreviation_NoMatchingState_ReturnsNull()
    {
        // Arrange
        using USState texas = new USState { Abbreviation = "TX", Name = "Texas" };
        USStateCollection collection = [texas];

        // Act
        USState? result = collection.GetStateByAbbreviation("CA");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetStateByAbbreviation_MatchingStateWithDifferentCase_ReturnsMatchingState()
    {
        // Arrange
        using USState expectedState = new USState { Abbreviation = "CA", Name = "California" };
        using USState texas = new USState { Abbreviation = "TX", Name = "Texas" };
        USStateCollection collection = [texas, expectedState];

        // Act
        USState? result = collection.GetStateByAbbreviation("ca");

        // Assert
        Assert.Same(expectedState, result);
    }

    [Fact]
    public void GetStateByAbbreviation_MultipleMatches_ReturnsFirstMatchingState()
    {
        // Arrange
        using USState firstMatch = new USState { Abbreviation = "WA", Name = "Washington" };
        using USState secondMatch = new USState { Abbreviation = "WA", Name = "Washington Duplicate" };
        USStateCollection collection = [firstMatch, secondMatch];

        // Act
        USState? result = collection.GetStateByAbbreviation("wa");

        // Assert
        Assert.Same(firstMatch, result);
    }

    [Fact]
    public void Clone_Called_ReturnsNewCollectionWithSameItems()
    {
        // Arrange
        using USState state = new USState { Abbreviation = "NY", Name = "New York" };
        USStateCollection original = [state];

        // Act
        USStateCollection clone = original.Clone();

        // Assert
        Assert.NotSame(original, clone);
        Assert.Single(clone);
        Assert.Same(state, clone[0]);
    }

    [Fact]
    public void Clone_CalledThroughICloneable_ReturnsCollectionClone()
    {
        // Arrange
        using USState state = new USState { Abbreviation = "FL", Name = "Florida" };
        USStateCollection original = [state];
        ICloneable cloneable = original;

        // Act
        object result = cloneable.Clone();

        // Assert
        USStateCollection typedResult = Assert.IsType<USStateCollection>(result);
        Assert.NotSame(original, typedResult);
        Assert.Single(typedResult);
        Assert.Same(state, typedResult[0]);
    }
}
