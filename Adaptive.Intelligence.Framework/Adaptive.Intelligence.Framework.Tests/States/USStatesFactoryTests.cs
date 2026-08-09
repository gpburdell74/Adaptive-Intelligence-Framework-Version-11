using Adaptive.Intelligence.Enumerations;
using Adaptive.Intelligence.States;
using Xunit;

namespace Adaptive.Intelligence.Framework.Tests;

public class USStatesFactoryTests
{
    [Fact]
    public void GetState_ExactStateName_ReturnsMatchingStateCode()
    {
        // Arrange
        const string stateName = "Alabama";

        // Act
        USStates result = USStatesFactory.GetState(stateName);

        // Assert
        Assert.Equal(USStates.Alabama, result);
    }

    [Fact]
    public void States_Accessed_ReturnsCollectionContainingKnownState()
    {
        // Arrange
        const string expectedAbbreviation = "AL";

        // Act
        USStateCollection result = USStatesFactory.States;

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains(result, state => state.Abbreviation == expectedAbbreviation);
    }

    [Fact]
    public void GetStateName_KnownState_ReturnsName()
    {
        // Arrange
        const USStates state = USStates.Alabama;

        // Act
        string? result = USStatesFactory.GetStateName(state);

        // Assert
        Assert.Equal("Alabama", result);
    }

    [Fact]
    public void GetStateName_UnknownEnumValue_ReturnsEmptyString()
    {
        // Arrange
        const USStates state = (USStates)int.MaxValue;

        // Act
        string? result = USStatesFactory.GetStateName(state);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetStateAbbreviation_KnownState_ReturnsAbbreviation()
    {
        // Arrange
        const USStates state = USStates.Alabama;

        // Act
        string? result = USStatesFactory.GetStateAbbreviation(state);

        // Assert
        Assert.Equal("AL", result);
    }

    [Fact]
    public void GetStateAbbreviation_UnknownEnumValue_ReturnsEmptyString()
    {
        // Arrange
        const USStates state = (USStates)int.MaxValue;

        // Act
        string? result = USStatesFactory.GetStateAbbreviation(state);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetState_ExactAbbreviation_ReturnsMatchingStateCode()
    {
        // Arrange
        const string abbreviation = "AL";

        // Act
        USStates result = USStatesFactory.GetState(abbreviation);

        // Assert
        Assert.Equal(USStates.Alabama, result);
    }

    [Fact]
    public void GetState_LowerCaseName_ReturnsMatchingStateCode()
    {
        // Arrange
        const string stateName = "alabama";

        // Act
        USStates result = USStatesFactory.GetState(stateName);

        // Assert
        Assert.Equal(USStates.Alabama, result);
    }

    [Fact]
    public void GetState_NullInput_ReturnsUnknownOrNotSpecified()
    {
        // Arrange
        string? stateNameOrAbbrev = null;

        // Act
        USStates result = USStatesFactory.GetState(stateNameOrAbbrev!);

        // Assert
        Assert.Equal(USStates.UnknownOrNotSpecified, result);
    }

    [Fact]
    public void GetState_EmptyInput_ReturnsUnknownOrNotSpecified()
    {
        // Arrange
        const string stateNameOrAbbrev = "";

        // Act
        USStates result = USStatesFactory.GetState(stateNameOrAbbrev);

        // Assert
        Assert.Equal(USStates.UnknownOrNotSpecified, result);
    }

    [Fact]
    public void GetState_UnknownValue_ReturnsUnknownOrNotSpecified()
    {
        // Arrange
        const string stateNameOrAbbrev = "ZZZ";

        // Act
        USStates result = USStatesFactory.GetState(stateNameOrAbbrev);

        // Assert
        Assert.Equal(USStates.UnknownOrNotSpecified, result);
    }

    [Fact]
    public void GetStateInstance_ExactName_ReturnsStateInstance()
    {
        // Arrange
        const string stateName = "Alabama";

        // Act
        USState? result = USStatesFactory.GetStateInstance(stateName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(USStates.Alabama, result.StateCode);
    }

    [Fact]
    public void GetStateInstance_LowerCaseAbbreviation_ReturnsStateInstance()
    {
        // Arrange
        const string abbreviation = "al";

        // Act
        USState? result = USStatesFactory.GetStateInstance(abbreviation);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("AL", result.Abbreviation);
    }

    [Fact]
    public void GetStateInstance_NullInput_ReturnsNull()
    {
        // Arrange
        string? stateNameOrAbbrev = null;

        // Act
        USState? result = USStatesFactory.GetStateInstance(stateNameOrAbbrev!);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetStateInstance_EmptyInput_ReturnsNull()
    {
        // Arrange
        const string stateNameOrAbbrev = "";

        // Act
        USState? result = USStatesFactory.GetStateInstance(stateNameOrAbbrev);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetStateInstance_UnknownValue_ReturnsNull()
    {
        // Arrange
        const string stateNameOrAbbrev = "ZZZ";

        // Act
        USState? result = USStatesFactory.GetStateInstance(stateNameOrAbbrev);

        // Assert
        Assert.Null(result);
    }
}
